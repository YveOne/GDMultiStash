using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;

namespace GDMultiStash
{
    internal static partial class Core
    {
        public static partial class Stashes
        {

            private static readonly Dictionary<int, Common.Stash> _stashes = new Dictionary<int, Common.Stash>();

            private static int CreateMainStash(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
            {
                string stashNameFormat = "main_stash_{0}{1}";
                string stashNameExp = ((int)expansion).ToString();
                string stashNameMode = mode == GrimDawnGameMode.SC ? "sc" : "hc";
                string stashName = Localization.GetString(string.Format(stashNameFormat, stashNameExp, stashNameMode));
                Console.WriteLine("Creating Main Stash: " + stashName);
                Common.Stash stash = new Common.Stash(Config.CreateStash(stashName, expansion, mode));
                string filePath = GrimDawn.GetTransferFilePath(expansion, mode);
                if (File.Exists(filePath))
                {
                    Console.WriteLine("- import from: " + filePath);
                    Files.ImportTransferFile(stash.ID, filePath);
                }
                else
                {
                    Console.WriteLine("- export to: " + filePath);
                    Files.CreateTransferFile(stash.ID, expansion);
                    Files.ExportTransferFile(stash.ID, filePath);
                }
                return stash.ID;
            }

            public static void LoadStashes()
            {
                int stashID;
                if (Config.IsNew)
                {

                    stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.SC);
                    Config.Main0SCID = stashID;
                    Config.Cur0SCID = stashID;

                    stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.HC);
                    Config.Main0HCID = stashID;
                    Config.Cur0HCID = stashID;

                    stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.SC);
                    Config.Main1SCID = stashID;
                    Config.Cur1SCID = stashID;

                    stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.HC);
                    Config.Main1HCID = stashID;
                    Config.Cur1HCID = stashID;

                    stashID = CreateMainStash(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.SC);
                    Config.Main2SCID = stashID;
                    Config.Cur2SCID = stashID;

                    stashID = CreateMainStash(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.HC);
                    Config.Main2HCID = stashID;
                    Config.Cur2HCID = stashID;

                    Config.Save();
                }
                else
                {
                    if (Config.PreviousVersion == 1)
                    {
                        Config.GetStashByID(Config.Main2SCID).Name = Localization.GetString("main_stash_2sc");
                        Config.GetStashByID(Config.Main2HCID).Name = Localization.GetString("main_stash_2hc");

                        // create missing main stashes for earlier expansions

                        stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.SC);
                        Config.Main0SCID = stashID;
                        Config.Cur0SCID = stashID;

                        stashID = CreateMainStash(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.HC);
                        Config.Main0HCID = stashID;
                        Config.Cur0HCID = stashID;

                        stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.SC);
                        Config.Main1SCID = stashID;
                        Config.Cur1SCID = stashID;

                        stashID = CreateMainStash(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.HC);
                        Config.Main1HCID = stashID;
                        Config.Cur1HCID = stashID;

                        Config.Save();
                    }
                }

                Console.WriteLine("Loading Stashes:");
                foreach (Common.Config.ConfigStash cfgStash in Config.GetStashes())
                {
                    Console.WriteLine(string.Format(@"#{0} {1}", cfgStash.ID, cfgStash.Name));
                    Common.Stash stash = new Common.Stash(cfgStash);
                    _stashes.Add(cfgStash.ID, stash);
                    stash.LoadTransferFile();
                }
            }

            public static Common.Stash[] GetAllStashes()
            {
                return _stashes.Values.ToArray();
            }

            public static Common.Stash[] GetShownStashes(int exp, bool sc, bool hc)
            {
                return Array.FindAll(GetAllStashes(), delegate (Common.Stash stash) {
                    return (exp == -1 || exp == (int)stash.Expansion) && ((!sc && !hc) || (sc == stash.SC && hc == stash.HC));
                });
            }

            public static Common.Stash[] GetStashesForExpansion(GrimDawnGameExpansion exp)
            {
                return Array.FindAll(GetAllStashes(), delegate (Common.Stash stash) {
                    return (exp == stash.Expansion);
                });
            }

            public static Common.Stash[] GetActiveStashes()
            {
                return Array.FindAll(GetAllStashes(), delegate (Common.Stash stash) {
                    return (Runtime.CurrentMode == GrimDawnGameMode.SC && stash.SC)
                    || (Runtime.CurrentMode == GrimDawnGameMode.HC && stash.HC);
                });
            }

            public static bool IsMainStash(Common.Stash stash)
            {
                return (stash.ID == Config.Main0SCID
                    || stash.ID == Config.Main0HCID
                    || stash.ID == Config.Main1SCID
                    || stash.ID == Config.Main1HCID
                    || stash.ID == Config.Main2SCID
                    || stash.ID == Config.Main2HCID);
            }

            public static Common.Stash[] GetMainStashes()
            {
                return new Common.Stash[] {
                    GetStash(Config.Main0SCID),
                    GetStash(Config.Main0HCID),
                    GetStash(Config.Main1SCID),
                    GetStash(Config.Main1HCID),
                    GetStash(Config.Main2SCID),
                    GetStash(Config.Main2HCID),
                };
            }

            public static Common.Stash GetStash(int stashID)
            {
                if (_stashes.TryGetValue(stashID, out Common.Stash stash))
                {
                    return stash;
                }
                return null;
            }

            public static bool StashExists(int stashID)
            {
                return _stashes.ContainsKey(stashID);
            }

            public static Common.Stash ImportOverwriteStash(string src, int stashID)
            {
                Common.Stash stash = GetStash(stashID);
                if (stash == null) return null;

                Files.ImportTransferFile(stash.ID, src, true);

                stash.LoadTransferFile();
                return stash;
            }

            public static Common.Stash ImportStash(string src, string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
            {
                Common.Stash stash = new Common.Stash(Config.CreateStash(name, expansion, mode));

                Files.ImportTransferFile(stash.ID, src, true);

                stash.LoadTransferFile();
                _stashes.Add(stash.ID, stash);
                return stash;
            }

            public static Common.Stash CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
            {
                Common.Stash stash = new Common.Stash(Config.CreateStash(name, expansion, mode));

                Files.CreateTransferFile(stash.ID, expansion);

                stash.LoadTransferFile();
                _stashes.Add(stash.ID, stash);
                return stash;
            }

            public static void DeleteStash(int stashID)
            {
                _stashes.Remove(stashID);
                Config.DeleteStash(stashID);
            }

            public static string[] GetBackupFiles(int stashID)
            {
                List<string> files = new List<string>();
                int backupIndex = 1;
                while (true)
                {
                    string backupFile = Files.GetStashFilePath(stashID, backupIndex);
                    if (!File.Exists(backupFile)) break;
                    files.Add(backupFile);
                    backupIndex += 1;
                }
                return files.ToArray();
            }

            public static bool RestoreTransferFile(int stashID, string srcFile)
            {
                Console.WriteLine("Restoring stash: {0} -> {1}", stashID, srcFile);
                File.Move(srcFile, srcFile + ".tmp");
                if (Files.ImportTransferFile(stashID, srcFile + ".tmp", true))
                {
                    File.Delete(srcFile + ".tmp");
                    if (Config.Cur0SCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.SC));
                    if (Config.Cur0HCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.BaseGame, GrimDawnGameMode.HC));
                    if (Config.Cur1SCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.SC));
                    if (Config.Cur1HCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.AshesOfMalmouth, GrimDawnGameMode.HC));
                    if (Config.Cur2SCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.SC));
                    if (Config.Cur2HCID == stashID) Files.ExportTransferFile(stashID, GrimDawn.GetTransferFilePath(GrimDawnGameExpansion.ForgottenGods, GrimDawnGameMode.HC));
                    return true;
                }
                else
                {
                    File.Move(srcFile + ".tmp", srcFile);
                    return false;
                }
            }

            public static void ImportStash(int stashID)
            {
                string externalFile = GrimDawn.GetTransferFilePath(Runtime.CurrentExpansion, Runtime.CurrentMode);
                if (Files.ImportTransferFile(Runtime.ActiveStashID, externalFile))
                {
                    _stashes[Runtime.ActiveStashID].LoadTransferFile();
                }
                else
                {
                    Console.WriteLine("IMPORT FAILED: ");
                    Console.WriteLine("  mode: " + Runtime.CurrentMode.ToString());
                    Console.WriteLine("  file: " + externalFile);
                }
            }

            public static void ExportStash(int stashID)
            {
                string externalFile = GrimDawn.GetTransferFilePath(Runtime.CurrentExpansion, Runtime.CurrentMode);
                if (!Files.ExportTransferFile(Runtime.ActiveStashID, externalFile))
                {
                    Console.WriteLine("EXPORT FAILED: ");
                    Console.WriteLine("  mode: " + Runtime.CurrentMode.ToString());
                    Console.WriteLine("  file: " + externalFile);
                }
            }

            public static void SwitchToStash(int toStashID)
            {
                if (toStashID == Runtime.ActiveStashID) return;
                if (Runtime.CurrentMode == GrimDawnGameMode.None) return; // not loaded?

                ImportStash(Runtime.ActiveStashID);
                Runtime.ActiveStashID = toStashID;
                ExportStash(Runtime.ActiveStashID);

                Config.Save();
            }

        }
    }
}
