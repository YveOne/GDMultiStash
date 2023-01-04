using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

namespace GDMultiStash.GlobalHandlers
{
    internal class StashesHandler
    {

        private readonly Dictionary<int, StashObject> _stashes;
        private readonly Dictionary<int, StashGroupObject> _stashGroups;

        public StashesHandler()
        {
            _stashes = new Dictionary<int, StashObject>();
            _stashGroups = new Dictionary<int, StashGroupObject>();
        }

        #region Base

        private void UpdateOrder(IList<IBaseObject> list)
        {
            List<int> orders = new List<int>();
            foreach (IBaseObject item in list)
            {
                orders.Add(item.Order);
            }
            orders.Sort();
            foreach (IBaseObject item in list)
            {
                item.Order = orders[0];
                orders.RemoveAt(0);
            }
        }

        public void UpdateOrder(List<StashObject> list)
        {
            UpdateOrder(list.ConvertAll(i => (IBaseObject)i));
        }

        public void UpdateOrder(List<StashGroupObject> list)
        {
            UpdateOrder(list.ConvertAll(i => (IBaseObject)i));
        }

        #endregion

        #region Stashes

        public void LoadStashes()
        {
            Console.WriteLine($"Loading Stashes:");
            foreach (Common.Config.ConfigStash cfgStash in Global.Configuration.Stashes)
            {
                StashObject stash = new StashObject(cfgStash);
                _stashes.Add(cfgStash.ID, stash);
                if (stash.LoadTransferFile())
                    Console.WriteLine($"   #{cfgStash.ID} {cfgStash.Name}");
                else
                    Console.WriteLine($"   #{cfgStash.ID} {cfgStash.Name} (FAILED)");
            }
        }

        public StashObject[] GetAllStashes()
        {
            //return _stashes.Values.ToList().OrderBy(s => s.Order).ToArray();
            return _stashes.Values.ToArray();
        }
        
        public StashObject[] GetStashesForExpansion(GrimDawnGameExpansion exp)
        {
            return Array.FindAll(GetAllStashes(), delegate (StashObject stash) {
                return (exp == stash.Expansion);
            });
        }

        public StashObject GetStash(int stashID)
        {
            if (_stashes.TryGetValue(stashID, out StashObject stash))
                return stash;
            return null;
        }

        public bool StashExists(int stashID)
        {
            return _stashes.ContainsKey(stashID);
        }

        public bool ImportOverwriteStash(string src, StashObject stash)
        {
            if (Common.TransferFile.ValidateFile(src, out Common.TransferFile transfer))
            {
                if (Global.FileSystem.ImportStashTransferFile(stash.ID, src, out bool changed))
                {
                    stash.SetTransferFile(transfer);
                    return true;
                }
            }
            else
            {
                // TODO: WARNING
            }
            return false;
        }

        public StashObject CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode, int tabsCount = -1)
        {
            StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
            Global.FileSystem.CreateStashTransferFile(stash.ID, expansion, tabsCount);
            stash.LoadTransferFile();
            _stashes.Add(stash.ID, stash);
            return stash;
        }

        public void DeleteStash(int stashID)
        {
            _stashes.Remove(stashID);
            Global.Configuration.DeleteStash(stashID);
        }

        public List<StashObject> DeleteStashes(IEnumerable<StashObject> list)
        {
            List<StashObject> deletedItems = new List<StashObject>();
            foreach (StashObject toDelete in list)
            {
                if (Global.Configuration.IsMainStashID(toDelete.ID))
                {
                    Console.Warning(Global.L.CannotDeleteStashMessage(toDelete.Name, Global.L.StashIsMainMessage()));
                    continue;
                }
                if (Global.Configuration.IsCurrentStashID(toDelete.ID))
                {
                    Console.Warning(Global.L.CannotDeleteStashMessage(toDelete.Name, Global.L.StashIsActiveMessage()));
                    continue;
                }
                DeleteStash(toDelete.ID);
                deletedItems.Add(toDelete);
            }
            return deletedItems;
        }

        public void CleanupBackups()
        {
            int deletedFiles = 0;
            foreach (var stash in GetAllStashes())
            {
                deletedFiles += Global.FileSystem.BackupCleanupStashTransferFile(stash.ID);
            }
            Console.Alert(Global.L.BackupsCleanedUpMessage(deletedFiles));
        }

        public string[] GetBackupFiles(int stashID)
        {
            List<string> files = new List<string>();
            int backupIndex = 1;
            while (true)
            {
                string backupFile = Global.FileSystem.GetStashTransferFile(stashID, backupIndex);
                if (!File.Exists(backupFile)) break;
                files.Add(backupFile);
                backupIndex += 1;
            }
            return files.ToArray();
        }

        public void RestoreTransferFile(int stashID, string srcFile)
        {
            if (Global.FileSystem.RestoreStashTransferFile(stashID, srcFile))
            {
                ExportStash(stashID);
            }
        }

        public StashObject CreateImportStash(string src, string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            if (Common.TransferFile.ValidateFile(src, out Common.TransferFile transfer))
            {
                StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
                if (Global.FileSystem.ImportStashTransferFile(stash.ID, src, out bool changed))
                {
                    stash.SetTransferFile(transfer);
                    _stashes.Add(stash.ID, stash);
                    return stash;
                }
            }
            else
            {
                // TODO: WARNING
            }
            return null;
        }

        public StashObject CreateStashCopy(StashObject toCopy)
        {
            GrimDawnGameMode mode = GrimDawnGameMode.None;
            if (toCopy.SC) mode |= GrimDawnGameMode.SC;
            if (toCopy.HC) mode |= GrimDawnGameMode.HC;
            Common.Config.ConfigStash cfgStash = Global.Configuration.CreateStashCopy(toCopy.ID);
            StashObject copied = new StashObject(cfgStash);
            copied.LoadTransferFile();
            _stashes.Add(copied.ID, copied);
            return copied;
        }


        public bool ImportStash(int stashID, GrimDawnGameExpansion exp, GrimDawnGameMode mode, bool ignoreLock = false)
        {
            string externalFile = GrimDawn.GetTransferFilePath(exp, mode);
            Console.WriteLine($"Importing Stash #{stashID}");
            Console.WriteLine($"  exp : {exp}");
            Console.WriteLine($"  mode: {mode}");
            Console.WriteLine($"  gdms: {Global.FileSystem.GetStashTransferFile(stashID)}");
            Console.WriteLine($"  game: {externalFile}");
            StashObject stash = GetStash(stashID);
            if (stash.Locked && !ignoreLock)
            {
                Console.WriteLine($"  LOCKED !!!");
                return true;
            }
            if (Global.FileSystem.ImportStashTransferFile(stashID, externalFile, out bool changed))
            {
                if (changed)
                {
                    _stashes[stashID].LoadTransferFile();
                    Global.Runtime.NotifyStashesImported(_stashes[stashID]);
                }
                return true;
            }
            return false;
        }

        public bool ImportStash(int stashID, bool ignoreLock = false)
        {
            return ImportStash(stashID, Global.Runtime.CurrentExpansion, Global.Runtime.CurrentMode, ignoreLock);
        }

        public void ExportStash(int stashID, GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            string externalFile = GrimDawn.GetTransferFilePath(exp, mode);
            Console.WriteLine($"Exporting Stash #{stashID}");
            Console.WriteLine($"  exp : {exp}");
            Console.WriteLine($"  mode: {mode}");
            Console.WriteLine($"  gdms: {Global.FileSystem.GetStashTransferFile(stashID)}");
            Console.WriteLine($"  game: {externalFile}");
            if (Global.FileSystem.ExportStashTransferFile(stashID, externalFile))
            {
                Global.Runtime.NotifyStashesExported(_stashes[stashID]);
            }
            else
            {
                Console.WriteLine($"EXPORT FAILED");
            }
        }

        public void ExportStash(int stashID)
        {
            // usually there should only one environment (expansion+mode combo) for one stash
            // but just to be on safe side use foreach loop
            foreach(var env in Global.Configuration.GetStashEnvironments(stashID))
                ExportStash(stashID, env.Expansion, env.Mode);
        }

        public bool SwitchToStash(int toStashID)
        {
            if (Global.Runtime.CurrentMode == GrimDawnGameMode.None) return false; // not loaded?
            if (Global.Runtime.CurrentExpansion == GrimDawnGameExpansion.Unknown) return false; // not loaded?

            Console.WriteLine($"Switching to stash #{toStashID}");
            if (!ImportStash(Global.Runtime.ActiveStashID)) return false;
            Global.Runtime.ActiveStashID = toStashID;
            ExportStash(toStashID);
            Global.Configuration.Save();
            return true;
        }

        #endregion

        #region Stash Groups

        public void LoadStashGroups()
        {
            Console.WriteLine($"Loading Stash Groups:");
            foreach (Common.Config.ConfigStashGroup cfgStashGroup in Global.Configuration.StashGroups)
            {
                StashGroupObject grp = new StashGroupObject(cfgStashGroup);
                _stashGroups.Add(grp.ID, grp);
                Console.WriteLine($"   #{grp.ID} {grp.Name}");
            }
        }

        public StashGroupObject GetStashGroup(int grpId)
        {
            if (_stashGroups.TryGetValue(grpId, out StashGroupObject grp))
                return grp;
            return null;
        }

        public StashGroupObject[] GetAllStashGroups()
        {
            return _stashGroups.Values.ToList().OrderBy(s => s.Order).ToArray();
        }

        public StashGroupObject[] GetSortedStashGroups()
        {
            List<StashGroupObject> l = _stashGroups.Values.ToList();
            l.Sort(new GroupsSortComparer());
            return l.ToArray();
        }

        public StashGroupObject CreateStashGroup(string name)
        {
            StashGroupObject group = new StashGroupObject(Global.Configuration.CreateStashGroup(name));
            _stashGroups.Add(group.ID, group);
            return group;
        }

        public void DeleteStashGroup(int groupID)
        {
            _stashGroups.Remove(groupID);
            Global.Configuration.DeleteStashGroup(groupID);
            foreach(StashObject stash in _stashes.Values)
                if (stash.GroupID == groupID)
                    stash.GroupID = 0;
            Global.Runtime.NotifyStashesRebuild();
        }

        public List<StashGroupObject> DeleteStashGroups(IEnumerable<StashGroupObject> list)
        {
            List<StashGroupObject> deletedItems = new List<StashGroupObject>();
            foreach (StashGroupObject toDelete in list)
            {
                if (Global.Configuration.IsMainStashGroupID(toDelete.ID))
                {
                    Console.Warning(Global.L.CannotDeleteStashGroupMessage(toDelete.Name, Global.L.StashGroupIsMainMessage()));
                    continue;
                }
                DeleteStashGroup(toDelete.ID);
                deletedItems.Add(toDelete);
            }
            return deletedItems;
        }





        #endregion





        //ActiveGroupID

    }
}
