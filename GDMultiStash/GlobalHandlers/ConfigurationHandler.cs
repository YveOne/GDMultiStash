using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using Utils.XML;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class ConfigurationHandler : Base.BaseHandler
    {
        public Common.Config.ConfigSettingList Settings => _config.Settings;
        public List<Common.Config.ConfigStash> Stashes => _config.Stashes.Items;
        public List<Common.Config.ConfigColor> Colors => _config.Colors.Items;
        public List<Common.Config.ConfigSortPattern> SortPatterns => _config.SortPatterns.Items;
        public List<Common.Config.ConfigStashGroup> StashGroups => _config.StashGroups.Items;
        public List<Common.Config.ConfigExpansion> Expansions => _config.Expansions.Items;

        public bool IsNewConfiguration { get; private set; }
        public bool AppVersionUpdated { get; private set; }

        private Common.Config.Config _config;

        public string GetStartGameCommand(string cmd = null)
        {
            var command = cmd != null ? cmd : Settings.StartGameCommand;

            if (command == "steam" || command.StartsWith("steam://"))
                return "steam";

            if (command == "gog" || command.EndsWith("GalaxyClient.exe"))
                return "gog";

            if (command == "grimdawn" || command.EndsWith(Path.Combine("x64", "Grim Dawn.exe")))
                return "grimdawn";

            if (command == "griminternals" || command.EndsWith("GrimInternals64.exe"))
                return "griminternals";

            if (command == "grimcam" || command.EndsWith("GrimCam.exe"))
                return "grimcam";

            return command;
        }

        public int CheckStateToInt(System.Windows.Forms.CheckState state)
        {
            switch (state)
            {
                case System.Windows.Forms.CheckState.Checked: return 1;
                case System.Windows.Forms.CheckState.Unchecked: return 0;
            }
            return -1;
        }

        public System.Windows.Forms.CheckState IntToCheckState(int state)
        {
            switch (state)
            {
                case 0: return System.Windows.Forms.CheckState.Unchecked;
                case 1: return System.Windows.Forms.CheckState.Checked;
            }
            return System.Windows.Forms.CheckState.Indeterminate;
        }

        #region Load/Save Methods

        private Common.Config.Config LoadFromFile(string filePath)
        {
            Common.Config.ConfigBase _configBase = XmlIO.ReadXmlText<Common.Config.ConfigBase>(File.ReadAllText(filePath));
            Console.WriteLine("Version of config file: " + _configBase.Version);

            while (_configBase.Version++ < Common.Config.ConfigBase.LatestVersion)
            {
                Console.WriteLine(string.Format("Updating Config to v{0}...", _configBase.Version.ToString()));
                switch (_configBase.Version)
                {
                    case 6:
                        {
                            Common.Config.V5.Config configOld = XmlIO.ReadXmlText<Common.Config.V5.Config>(File.ReadAllText(filePath));
                            Common.Config.Config configNew = XmlIO.ReadXmlText<Common.Config.Config>(File.ReadAllText(filePath));
                            configNew.Version = _configBase.Version;

                            configNew.Stashes.Items.Clear();
                            foreach (Common.Config.V5.ConfigStash stash in configOld.Stashes.Items)
                            {
                                configNew.Stashes.Items.Add(new Common.Config.ConfigStash()
                                {
                                    ID = stash.ID,
                                    Order = stash.Order,
                                    SC = stash.SC,
                                    HC = stash.HC,
                                    Expansion = stash.Expansion,
                                    GroupID = stash.GroupID,
                                    Name = stash.Name,
                                    Color = stash.Color,
                                    Locked = stash.Locked,
                                });
                            }

                            configNew.StashGroups.Items.Clear();
                            foreach (Common.Config.V5.ConfigStashGroup cat in configOld.StashGroups.Items)
                            {
                                configNew.StashGroups.Items.Add(new Common.Config.ConfigStashGroup()
                                {
                                    ID = cat.ID,
                                    Name = cat.Name,
                                    Order = cat.Order,
                                });
                            }

                            configNew.Expansions.Items.Clear();
                            foreach (Common.Config.V5.ConfigExpansion exp in configOld.Expansions.Items)
                            {
                                configNew.Expansions.Items.Add(new Common.Config.ConfigExpansion()
                                {
                                    ID = exp.ID,
                                    NameCommentValue = exp.Name,
                                    SC = exp.SC,
                                    HC = exp.HC,
                                });
                            }

                            WriteToFile(configNew, filePath);
                        }
                        break;

                }
                Console.WriteLine("... Done");
            }

            Console.WriteLine("Config is up to date");
            return XmlIO.ReadXmlText<Common.Config.Config>(File.ReadAllText(filePath));
        }

        public static void WriteToFile<T>(T cfg, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            XmlIO.WriteXmlFile(cfg, filePath);
        }

        public void Load()
        {
            string filePath = Global.FileSystem.ConfigFile;
            Console.WriteLine($"Loading config");
            if (File.Exists(filePath))
            {
                _config = LoadFromFile(filePath);
                IsNewConfiguration = false;
            }
            else
            {
                Console.WriteLine("- File not found -> Create new config");
                _config = new Common.Config.Config();
                IsNewConfiguration = true;
            }
        }

        public void Save()
        {
            Console.WriteLine("Saving config");
            WriteToFile(_config, Global.FileSystem.ConfigFile);
        }

        public void SaveBackup()
        {
            if (File.Exists(Global.FileSystem.ConfigFile))
            {
                File.Copy(Global.FileSystem.ConfigFile, Global.FileSystem.ConfigFile + ".backup", true);
            }
        }

        public void RestoreBackup()
        {
            if (File.Exists(Global.FileSystem.ConfigFile + ".backup"))
            {
                File.Delete(Global.FileSystem.ConfigFile);
                File.Move(Global.FileSystem.ConfigFile + ".backup", Global.FileSystem.ConfigFile);
            }
        }

        public void DeleteBackup()
        {
            if (File.Exists(Global.FileSystem.ConfigFile + ".backup"))
            {
                File.Delete(Global.FileSystem.ConfigFile + ".backup");
            }
        }

        public void UpdateAndCleanup()
        {
            var needSaveAfterUpdate = false;
            Utils.AssemblyInfo ai = new Utils.AssemblyInfo();
            if (ai.Version != _config.LastToolVersion)
            {
                AppVersionUpdated = _config.LastToolVersion != "";
                _config.LastToolVersion = ai.Version;
                needSaveAfterUpdate = true;
            }

            #region check expansion list
            {
                foreach (GrimDawnGameExpansion exp in GrimDawn.ExpansionList)
                {
                    if (exp == GrimDawnGameExpansion.Unknown) continue;
                    Common.Config.ConfigExpansion cfgExp = _config.Expansions.Items.Find(ex => { return ex.ID == (int)exp; });
                    if (cfgExp == null)
                    {
                        CreateExpansion(exp);
                        needSaveAfterUpdate = true;
                    }
                    else
                    {
                        // update the name in the comment
                        cfgExp.NameCommentValue = GrimDawn.ExpansionNames[exp];
                    }
                }
                _config.Expansions.Items.Sort((x, y) => x.ID.CompareTo(y.ID));
            }
            #endregion

            #region check stashes and groups
            {
                List<int> existingStashGroupIds = new List<int>();
                List<Common.Config.ConfigStashGroup> stashGroupsToDelete = new List<Common.Config.ConfigStashGroup>();
                foreach (Common.Config.ConfigStashGroup cat in StashGroups)
                {
                    if (existingStashGroupIds.Contains(cat.ID))
                        stashGroupsToDelete.Add(cat);
                    else
                        existingStashGroupIds.Add(cat.ID);
                }
                if (stashGroupsToDelete.Count > 0)
                {
                    foreach (Common.Config.ConfigStashGroup cat in stashGroupsToDelete)
                        StashGroups.Remove(cat);
                    needSaveAfterUpdate = true;
                }
                if (!existingStashGroupIds.Contains(0))
                {
                    CreateMainGroup(Global.L.MainGroupName());
                    existingStashGroupIds.Add(0);
                    needSaveAfterUpdate = true;
                }
                _config.StashGroups.Items.Sort((x, y) => x.ID.CompareTo(y.ID));

                foreach (Common.Config.ConfigStash stash in Stashes)
                {
                    if (!existingStashGroupIds.Contains(stash.GroupID))
                    {
                        stash.GroupID = 0;
                        needSaveAfterUpdate = true;
                    }
                }
            }
            #endregion

            var revisionUpdates = new SortedDictionary<int, Action>();
            revisionUpdates[1] = () => {
                if (_config.Colors.Items.Count == 0)
                {
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#ebdec3", Name = Global.L.DefaultColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#34eb58", Name = Global.L.GreenColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#5ecfff", Name = Global.L.BlueColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#af69ff", Name = Global.L.PurpleColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#ffcc00", Name = Global.L.GoldColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#aaaaaa", Name = Global.L.GrayColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#f0f0f0", Name = Global.L.WhiteColor() });
                    _config.Colors.Items.Add(new Common.Config.ConfigColor() { Value = "#f765ad", Name = Global.L.RoseColor() });
                }
            };
            revisionUpdates[2] = () => {
                if (_config.SortPatterns.Items.Count == 0)
                {
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByNone(),
                        Value = "{none}/{none}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByLevel(),
                        Value = Global.L.SortByLevel() + " / {level}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByQuality(),
                        Value = Global.L.SortByQuality() + " / {quality}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByClass(),
                        Value = Global.L.SortByClass() + " / {class}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByType(),
                        Value = Global.L.SortByType() + " / {type}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortBySet(),
                        Value = "{quality} / [{level}] {set}",
                    });
                    _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                    {
                        Name = Global.L.SortByAIO(),
                        Value = "{quality}/[{level}] {set}\n{type} - {quality}/{class} - lvl {level}",
                    });
                }
            };
            revisionUpdates[3] = () => {
                _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                {
                    Name = Global.L.SortByRarity(),
                    Value = Global.L.SortByRarity() + " / {rarity}",
                });
                _config.SortPatterns.Items.Add(new Common.Config.ConfigSortPattern()
                {
                    Name = Global.L.SortByAIO() + " (2)",
                    Value = Global.L.SortByRarity() + "/{rarity}\n" + Global.L.SortBySet() + " - {quality}/[{level}] {set}\n{type} - {quality}/{class} - lvl {level}",
                });
            };
            foreach (var revUptdKvp in revisionUpdates)
            {
                if (_config.UpdateRevision < revUptdKvp.Key)
                {
                    _config.UpdateRevision = revUptdKvp.Key;
                    revUptdKvp.Value();
                    needSaveAfterUpdate = true;
                }
            }

            if (needSaveAfterUpdate)
                Global.Configuration.Save();
        }

        public void CreateExpansion(GrimDawnGameExpansion exp)
        {
            _config.Expansions.Items.Add(new Common.Config.ConfigExpansion()
            {
                ID = (int)exp,
                NameCommentValue = GrimDawn.ExpansionNames[exp],
            });

            int index = _config.Expansions.Items.Count - 1;
            int stashID;

            stashID = CreateMainStash(exp, GrimDawnGameMode.SC);
            _config.Expansions.Items[index].SC.MainID = stashID;
            _config.Expansions.Items[index].SC.CurrentID = stashID;

            stashID = CreateMainStash(exp, GrimDawnGameMode.HC);
            _config.Expansions.Items[index].HC.MainID = stashID;
            _config.Expansions.Items[index].HC.CurrentID = stashID;
        }

        #endregion

        #region Settings Methods

        public Common.Config.ConfigSettingList GetSettings()
        {
            return Settings.Copy();
        }

        public void SetSettings(Common.Config.ConfigSettingList settings)
        {
            Common.Config.ConfigSettingList previous = Settings.Copy();
            _config.Settings.Set(settings);
            if (previous.Language != Settings.Language)
            {
                Global.Localization.LoadLanguage(Settings.Language);
                InvokeLanguageChanged();
            }
            if (previous.MaxBackups != Settings.MaxBackups)
            {
                Global.Stashes.CleanupBackups();
            }
            if (previous.GamePath != Settings.GamePath)
            {
                InvokeGamePathChanged();
            }
            if (previous.OverlayScale != Settings.OverlayScale
                || previous.OverlayWidth != Settings.OverlayWidth
                || previous.OverlayTransparency != Settings.OverlayTransparency
                || previous.OverlayStashesCount != Settings.OverlayStashesCount
                || previous.OverlayShowWorkload != Settings.OverlayShowWorkload)
            {
                InvokeOverlayDesignChanged();
            }
        }

        #endregion

        #region Stash Methods

        private string GetMainStashName(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            string stashNameExp = ((int)expansion).ToString();
            string stashNameMode = mode == GrimDawnGameMode.SC ? "sc" : "hc";
            // UNSECURE!
            return Global.L._.List[$"main_stash_name_{stashNameExp}{stashNameMode}"];
        }

        private int CreateMainStash(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            string stashName = GetMainStashName(expansion, mode);
            Console.WriteLine($"Creating Main Stash: {stashName}");
            Common.Config.ConfigStash stash = Global.Configuration.CreateStash(stashName, expansion, mode);
            string filePath = GrimDawn.GetTransferFilePath(expansion, mode);
            if (File.Exists(filePath))
            {
                Console.WriteLine($"- import from: {filePath}");
                Global.FileSystem.ImportStashTransferFile(stash.ID, filePath, out bool changed);
            }
            else
            {
                Console.WriteLine($"- export to: {filePath}");
                Global.FileSystem.CreateStashTransferFile(stash.ID, expansion, 1);
                Global.FileSystem.ExportStashTransferFile(stash.ID, filePath);
            }
            return stash.ID;
        }

        private Common.Config.ConfigExpansionMode GetExpansionMode(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            if (exp == GrimDawnGameExpansion.Unknown) return null;
            Common.Config.ConfigExpansion cExp = Expansions[(int)exp];
            if (mode == GrimDawnGameMode.SC) return cExp.SC;
            if (mode == GrimDawnGameMode.HC) return cExp.HC;
            return null;
        }

        public int GetMainStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            return GetExpansionMode(exp, mode).MainID;
        }

        public int GetCurrentStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            return GetExpansionMode(exp, mode).CurrentID;
        }

        public int GetCurrentStashID(GrimDawnGameEnvironment env)
        {
            return GetCurrentStashID(env.GameExpansion, env.GameMode);
        }

        public void SetCurrentStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode, int stashID)
        {
            GetExpansionMode(exp, mode).CurrentID = stashID;
        }

        public void SetCurrentStashID(GrimDawnGameEnvironment env, int stashID)
        {
            SetCurrentStashID(env.GameExpansion, env.GameMode, stashID);
        }

        public Common.Config.ConfigStash GetStashByID(int stashID)
        {
            return Stashes.Find(s => { return s.ID == stashID; });
        }

        public int GetStashIndex(int stashID)
        {
            return Stashes.FindIndex((stash) => { return stash.ID == stashID; });
        }

        public Common.Config.ConfigStash CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            Console.WriteLine($"Adding Config Stash: {name} (expansion: {expansion}, mode: {mode})");

            _config.Stashes.LastID += 1;
            Common.Config.ConfigStash stash = new Common.Config.ConfigStash
            {
                Name = name,
                ID = _config.Stashes.LastID,
                Order = _config.Stashes.LastID,
                Expansion = (int)expansion,
                SC = mode.HasFlag(GrimDawnGameMode.SC),
                HC = mode.HasFlag(GrimDawnGameMode.HC),
            };
            Stashes.Add(stash);

            Console.WriteLine("- id: " + stash.ID);
            Global.FileSystem.CreateStashDirectory(stash.ID);
            return stash;
        }

        public Common.Config.ConfigStash CreateStashCopy(int stashID)
        {
            Common.Config.ConfigStash toCopy = GetStashByID(stashID);
            Console.WriteLine($"Copying Config Stash: #{stashID} {toCopy.Name}");
            _config.Stashes.LastID += 1;
            Common.Config.ConfigStash stash = toCopy.Copy();
            stash.ID = _config.Stashes.LastID;
            Stashes.Add(stash);
            Console.WriteLine("- new id: " + stash.ID);
            Global.FileSystem.CopyStashDirectory(stashID, stash.ID);
            return stash;
        }

        public bool DeleteStash(int stashID)
        {
            Console.WriteLine($"Deleting Config Stash with id {stashID}");
            Global.FileSystem.DeleteStashDirectory(stashID);
            int index = GetStashIndex(stashID);
            if (index != -1)
            {
                Stashes.RemoveAt(index);
                return true;
            }
            else
            {
                Console.WriteLine(string.Format("- Not found"));
                return false;
            }
        }

        public bool IsMainStashID(int stashID)
        {
            foreach (Common.Config.ConfigExpansion cexp in Expansions)
            {
                if (cexp.SC.MainID == stashID) return true;
                if (cexp.HC.MainID == stashID) return true;
            }
            return false;
        }

        public bool IsMainStashID(int stashID, GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            Common.Config.ConfigExpansion cexp = Expansions[(int)exp];
            if (mode.HasFlag(GrimDawnGameMode.SC) && cexp.SC.MainID == stashID) return true;
            if (mode.HasFlag(GrimDawnGameMode.HC) && cexp.HC.MainID == stashID) return true;
            return false;
        }

        public bool IsCurrentStashID(int stashID)
        {
            foreach (Common.Config.ConfigExpansion cexp in Expansions)
            {
                if (cexp.SC.CurrentID == stashID) return true;
                if (cexp.HC.CurrentID == stashID) return true;
            }
            return false;
        }

        public bool IsCurrentStashID(int stashID, GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            Common.Config.ConfigExpansion cexp = Expansions[(int)exp];
            if (mode.HasFlag(GrimDawnGameMode.SC) && cexp.SC.CurrentID == stashID) return true;
            if (mode.HasFlag(GrimDawnGameMode.HC) && cexp.HC.CurrentID == stashID) return true;
            return false;
        }

        public GrimDawnGameEnvironment[] GetStashEnvironments(int stashId)
        {
            List<GrimDawnGameEnvironment> l = new List<GrimDawnGameEnvironment>();
            foreach (var cfgExp in Expansions)
            {
                if (stashId == cfgExp.SC.CurrentID)
                    l.Add(new GrimDawnGameEnvironment((GrimDawnGameExpansion)cfgExp.ID, GrimDawnGameMode.SC));
                if (stashId == cfgExp.HC.CurrentID)
                    l.Add(new GrimDawnGameEnvironment((GrimDawnGameExpansion)cfgExp.ID, GrimDawnGameMode.HC));
            }
            return l.ToArray();
        }

        #endregion

        #region Stash Group Methods

        public Common.Config.ConfigStashGroup GetGroupByID(int grpID)
        {
            return StashGroups.Find(c => { return c.ID == grpID; });
        }

        public Common.Config.ConfigStashGroup CreateMainGroup(string name)
        {
            int id = 0;
            Console.WriteLine($"Config: Adding Main Group: #{id} {name}");
            Common.Config.ConfigStashGroup grp = new Common.Config.ConfigStashGroup
            {
                Name = name,
                ID = id,
                Order = id,
            };
            StashGroups.Add(grp);
            return grp;
        }



        public Common.Config.ConfigStashGroup CreateStashGroup(string name)
        {
            _config.StashGroups.LastID += 1;
            int id = _config.StashGroups.LastID;
            Console.WriteLine($"Config: Adding Group: #{id} {name}");
            Common.Config.ConfigStashGroup grp = new Common.Config.ConfigStashGroup
            {
                Name = name,
                ID = id,
                Order = id,
            };
            StashGroups.Add(grp);
            return grp;
        }

        public bool IsMainStashGroupID(int groupID)
        {
            // well ...
            return groupID == 0;
        }

        public bool DeleteStashGroup(int groupID)
        {
            Console.WriteLine($"Deleting Config Stash Group with id {groupID}");

            int index = GetStashGroupIndex(groupID);
            if (index != -1)
            {
                StashGroups.RemoveAt(index);
                return true;
            }
            else
            {
                Console.WriteLine(string.Format("- Not found"));
                return false;
            }
        }

        public int GetStashGroupIndex(int groupID)
        {
            return StashGroups.FindIndex((group) => { return group.ID == groupID; });
        }


        #endregion

    }
}
