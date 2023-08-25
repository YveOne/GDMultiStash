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
    internal partial class StashesHandler : Base.ObjectsHandler<StashObject>
    {

        public void LoadStashes()
        {
            Console.WriteLine($"Loading Stashes:");
            foreach (Common.Config.ConfigStash cfgStash in Global.Configuration.Stashes)
            {
                StashObject stash = new StashObject(cfgStash);
                Items.Add(cfgStash.ID, stash);
                if (stash.LoadTransferFile())
                    Console.WriteLine($"   #{cfgStash.ID} {cfgStash.Name}");
                else
                    Console.WriteLine($"   #{cfgStash.ID} {cfgStash.Name} (FAILED)");
            }
        }

        public StashObject[] GetAllStashes()
        {
            return Items.Values.ToArray();
        }

        public StashObject[] GetStashesForExpansion(GrimDawnGameExpansion exp)
        {
            return Array.FindAll(GetAllStashes(), delegate (StashObject stash) {
                return (exp == stash.Expansion);
            });
        }

        public StashObject[] GetStashesForGroup(int groupId)
        {
            return Array.FindAll(GetAllStashes(), delegate (StashObject stash) {
                return stash.GroupID == groupId;
            });
        }

        public bool TryGetStash(int stashID, out StashObject stash)
        {
            return Items.TryGetValue(stashID, out stash);
        }

        public StashObject GetStash(int stashID)
        {
            if (TryGetStash(stashID, out StashObject grp))
                return grp;
            return null;
        }

        public StashObject CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode, int tabsCount = -1, bool warnZerotabs = true)
        {
            StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
            Global.FileSystem.CreateStashTransferFile(stash.ID, expansion, tabsCount, warnZerotabs);
            stash.LoadTransferFile();
            Items.Add(stash.ID, stash);
            return stash;
        }

        public StashObject CreateStashCopy(StashObject toCopy)
        {
            Common.Config.ConfigStash cfgStash = Global.Configuration.CreateStashCopy(toCopy.ID);
            StashObject copied = new StashObject(cfgStash);
            copied.LoadTransferFile();
            Items.Add(copied.ID, copied);
            return copied;
        }

        public bool ImportStash(int stashID, GrimDawnGameEnvironment env, bool ignoreLock = false)
        {
            Console.WriteLine($"Importing Stash #{stashID}");
            if (!TryGetStash(stashID, out StashObject stash))
            {
                Console.WriteLine($"- Error: stash not existing");
                return false;
            }
            if (stash.Locked && !ignoreLock)
            {
                Console.WriteLine($"- Skipped (stash is locked)");
                return true;
            }
            string externalFile = GrimDawn.GetTransferFilePath(env.GameExpansion, env.GameMode);
            Console.WriteLine($"- exp : {env.GameExpansion}");
            Console.WriteLine($"- mode: {env.GameMode}");
            Console.WriteLine($"- gdms: {Global.FileSystem.GetStashTransferFile(stashID).Substring(Global.FileSystem.StashesDirectory.Length)}");
            Console.WriteLine($"- game: {externalFile.Substring(GrimDawn.DocumentsPath.Length)}");
            if (Global.FileSystem.ImportStashTransferFile(stash.ID, externalFile, out bool changed))
            {
                if (changed)
                {
                    stash.LoadTransferFile();
                    Global.Runtime.InvokeStashesContentChanged(stash, false);
                }
                return true;
            }
            return false;
        }

        public bool ImportStash(int stashID, bool ignoreLock = false)
        {
            var env = new GrimDawnGameEnvironment(Global.Runtime.ActiveExpansion, Global.Runtime.ActiveMode);
            return ImportStash(stashID, env, ignoreLock);
        }

        public StashObject ImportCreateStash(string src, string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            if (Common.TransferFile.ValidateFile(src, out Common.TransferFile transfer))
            {
                StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
                if (Global.FileSystem.ImportStashTransferFile(stash.ID, src, out bool changed))
                {
                    stash.SetTransferFile(transfer);
                    Items.Add(stash.ID, stash);
                    return stash;
                }
            }
            else
            {
                // TODO: WARNING
            }
            return null;
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

        public bool DeleteStash(StashObject stash)
        {
            if (Global.Configuration.IsMainStashID(stash.ID))
            {
                Console.Warning(Global.L.CannotDeleteStashMessage(stash.Name, Global.L.StashIsMainMessage()));
                return false;
            }
            if (Global.Configuration.IsCurrentStashID(stash.ID))
            {
                Console.Warning(Global.L.CannotDeleteStashMessage(stash.Name, Global.L.StashIsActiveMessage()));
                return false;
            }
            Items.Remove(stash.ID);
            Global.Configuration.DeleteStash(stash.ID);
            return true;
        }

        public List<StashObject> DeleteStashes(IEnumerable<StashObject> list, bool onlyEmpty = false)
        {
            List<StashObject> deletedItems = new List<StashObject>();
            foreach (StashObject stash in list.Where(s => !onlyEmpty || s.TotalUsage == 0))
            {
                if (DeleteStash(stash))
                    deletedItems.Add(stash);
            }
            return deletedItems;
        }

        public void ExportStash(int stashID, GrimDawnGameEnvironment env)
        {
            string externalFile = GrimDawn.GetTransferFilePath(env.GameExpansion, env.GameMode);
            Console.WriteLine($"Exporting Stash #{stashID}");
            Console.WriteLine($"  exp : {env.GameExpansion}");
            Console.WriteLine($"  mode: {env.GameMode}");
            Console.WriteLine($"  gdms: {Global.FileSystem.GetStashTransferFile(stashID).Substring(Global.FileSystem.StashesDirectory.Length)}");
            Console.WriteLine($"  game: {externalFile.Substring(GrimDawn.DocumentsPath.Length)}");
            if (Global.FileSystem.ExportStashTransferFile(stashID, externalFile))
            {
            }
            else
            {
                Console.WriteLine($"EXPORT FAILED");
            }
        }

        public void ExportStash(int stashID)
        {
            foreach (var env in Global.Configuration.GetStashEnvironments(stashID))
                ExportStash(stashID, env);
        }

        public bool SwitchToStash(int toStashID)
        {
            if (!Global.Runtime.GameInitialized) return false;

            Console.WriteLine($"Switching to stash #{toStashID}");
            if (!ImportStash(Global.Runtime.ActiveStashID)) return false;
            Global.Runtime.ActiveStashID = toStashID;
            ExportStash(toStashID);
            Global.Configuration.Save();
            return true;
        }

        public bool SwitchToMainStash()
        {
            int mainStashID = Global.Configuration.GetMainStashID(Global.Runtime.ActiveExpansion, Global.Runtime.ActiveMode);
            return SwitchToStash(mainStashID);
        }

        #region backups

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

        #endregion

    }
}
