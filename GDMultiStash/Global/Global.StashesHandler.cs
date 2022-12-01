using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common;

namespace GDMultiStash.GlobalHandlers
{
    internal class StashesHandler
    {

        private readonly Dictionary<int, StashObject> _stashes;

        private readonly List<StashCategoryObject> _categories;

        public StashesHandler()
        {
            _stashes = new Dictionary<int, StashObject>();
            _categories = new List<StashCategoryObject>();
        }

        #region Stashes

        private string GetMainStashName(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            string stashNameExp = ((int)expansion).ToString();
            string stashNameMode = mode == GrimDawnGameMode.SC ? "sc" : "hc";
            string stashNameKey = $"mainStash_{stashNameExp}{stashNameMode}";
            string stashName = Global.Localization.GetString(stashNameKey);
            return stashName;
        }

        private int CreateMainStash(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            string stashName = GetMainStashName(expansion, mode);
            Console.WriteLine($"Creating Main Stash: {stashName}");
            StashObject stash = new StashObject(Global.Configuration.CreateStash(stashName, expansion, mode));
            string filePath = GrimDawn.GetTransferFilePath(expansion, mode);
            if (File.Exists(filePath))
            {
                Console.WriteLine($"- import from: {filePath}");
                Global.FileSystem.ImportStashTransferFile(stash.ID, filePath);
            }
            else
            {
                Console.WriteLine($"- export to: {filePath}");
                Global.FileSystem.CreateStashTransferFile(stash.ID, expansion);
                Global.FileSystem.ExportStashTransferFile(stash.ID, filePath);
            }
            return stash.ID;
        }

        public void CreateMainStashes()
        {
            int stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.SC);
            Global.Configuration.Settings.Main0SCID = stashID;
            Global.Configuration.Settings.Cur0SCID = stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.HC);
            Global.Configuration.Settings.Main0HCID = stashID;
            Global.Configuration.Settings.Cur0HCID = stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.SC);
            Global.Configuration.Settings.Main1SCID = stashID;
            Global.Configuration.Settings.Cur1SCID = stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.HC);
            Global.Configuration.Settings.Main1HCID = stashID;
            Global.Configuration.Settings.Cur1HCID = stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.SC);
            Global.Configuration.Settings.Main2SCID = stashID;
            Global.Configuration.Settings.Cur2SCID = stashID;

            stashID = CreateMainStash(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.HC);
            Global.Configuration.Settings.Main2HCID = stashID;
            Global.Configuration.Settings.Cur2HCID = stashID;

            Global.Configuration.Save();
        }

        public void LoadStashes()
        {
            foreach (Common.Config.ConfigStash cfgStash in Global.Configuration.GetStashes())
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
            return _stashes.Values.ToArray();
        }

        public StashObject[] GetShownStashes(int exp, bool sc, bool hc)
        {
            return Array.FindAll(GetAllStashes(), delegate (StashObject stash) {
                return (exp == -1 || exp == (int)stash.Expansion) && ((!sc && !hc) || (sc == stash.SC && hc == stash.HC));
            });
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
            {
                return stash;
            }
            return null;
        }

        public bool StashExists(int stashID)
        {
            return _stashes.ContainsKey(stashID);
        }

        public StashObject ImportOverwriteStash(string src, int stashID)
        {
            StashObject stash = GetStash(stashID);
            if (stash == null) return null;
            if (Global.FileSystem.ImportStashTransferFile(stash.ID, src, true))
            {
                stash.LoadTransferFile();
                return stash;
            }
            return null;
        }

        public StashObject ImportStash(string src, string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
            if (Global.FileSystem.ImportStashTransferFile(stash.ID, src, true))
            {
                stash.LoadTransferFile();
                _stashes.Add(stash.ID, stash);
                return stash;
            }
            return null;
        }

        public StashObject CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            StashObject stash = new StashObject(Global.Configuration.CreateStash(name, expansion, mode));
            Global.FileSystem.CreateStashTransferFile(stash.ID, expansion);
            stash.LoadTransferFile();
            _stashes.Add(stash.ID, stash);
            return stash;
        }

        public void DeleteStash(int stashID)
        {
            _stashes.Remove(stashID);
            Global.Configuration.DeleteStash(stashID);
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

        public bool ImportStash(int stashID)
        {
            string externalFile = GrimDawn.GetTransferFilePath(Global.Runtime.CurrentExpansion, Global.Runtime.CurrentMode);
            Console.WriteLine($"Importing Stash #{stashID}");
            Console.WriteLine($"  mode: {Global.Runtime.CurrentMode}");
            Console.WriteLine($"  file: {externalFile}");
            StashObject stash = GetStash(stashID);
            if (stash.Locked)
            {
                Console.WriteLine($"  LOCKED !!!");
                return true;
            }
            if (Global.FileSystem.ImportStashTransferFile(Global.Runtime.ActiveStashID, externalFile))
            {
                _stashes[Global.Runtime.ActiveStashID].LoadTransferFile();
                return true;
            }
            return false;
        }

        public bool ExportStash(int stashID)
        {
            string externalFile = GrimDawn.GetTransferFilePath(Global.Runtime.CurrentExpansion, Global.Runtime.CurrentMode);
            Console.WriteLine($"Exporting Stash #{stashID}");
            Console.WriteLine($"  mode: {Global.Runtime.CurrentMode}");
            Console.WriteLine($"  file: {externalFile}");
            if (!Global.FileSystem.ExportStashTransferFile(Global.Runtime.ActiveStashID, externalFile))
            {
                Console.WriteLine($"EXPORT FAILED");
                return false;
            }
            return ExportSharedModeStash(stashID);
        }

        public bool ExportSharedModeStash(int stashID)
        {
            StashObject stash = GetStash(stashID);
            if (!stash.SC || !stash.HC) return true; // stash is not shared mode

            GrimDawnGameMode oppositeMode = Global.Runtime.CurrentMode == GrimDawnGameMode.SC
                ? GrimDawnGameMode.HC
                : GrimDawnGameMode.SC;

            // opposite mode got different stash selected
            if (stashID != Global.Configuration.GetMainStashID(Global.Runtime.CurrentExpansion, oppositeMode)) return true;

            string externalFile = GrimDawn.GetTransferFile(Global.Runtime.CurrentExpansion, oppositeMode);
            Console.WriteLine($"Exporting shared mode transfer file:");
            Console.WriteLine($"  stash id: {stashID}");
            Console.WriteLine($"  mode: {Global.Runtime.CurrentMode} -> {oppositeMode}");
            Console.WriteLine($"  file: {externalFile}");
            if (!Global.FileSystem.ExportStashTransferFile(stashID, externalFile))
            {
                Console.WriteLine($"EXPORT (SHARED) FAILED");
                return false;
            }
            return true;
        }

        public bool SwitchToStash(int toStashID)
        {
            if (Global.Runtime.CurrentMode == GrimDawnGameMode.None) return false; // not loaded?
            if (Global.Runtime.CurrentExpansion == GrimDawnGameExpansion.Unknown) return false; // not loaded?

            Console.WriteLine($"Switching to stash #{toStashID}");
            if (!ImportStash(Global.Runtime.ActiveStashID)) return false;
            Global.Runtime.ActiveStashID = toStashID;
            if (!ExportStash(toStashID)) return false;
            Global.Configuration.Save();
            return true;
        }

        #endregion

        #region Categories

        public void LoadCategories()
        {
            uint order = 0;
            foreach (Common.Config.ConfigStashCategory cfgCat in Global.Configuration.GetCategories())
            {
                StashCategoryObject cat = new StashCategoryObject(cfgCat, order++);
                _categories.Add(cat);
                Console.WriteLine($"   #{cat.ID} {cat.Name}");
            }
            // ensure main category exists




            // ensure categories of stashes exist


        }








        #endregion

    }
}
