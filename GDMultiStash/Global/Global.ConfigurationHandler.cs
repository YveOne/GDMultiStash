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
    internal class ConfigurationHandler
    {

        private Common.Config.Config _config;

        public Common.Config.ConfigSettingList Settings => _config.Settings;

        public bool IsNew => _config.IsNew;

        public int PreviousVersion => _previousVersion;
        private int _previousVersion = 0;

        public delegate void SettingChangedDelegate(object sender, EventArgs e);
        public event SettingChangedDelegate LanguageChanged;
        public event SettingChangedDelegate GamePathChanged;
        public event SettingChangedDelegate AppearanceChanged;

        #region Load/Save Methods

        private Common.Config.Config LoadFromFile(string filePath)
        {
            Common.Config.ConfigBase _configBase = XmlIO.ReadXmlText<Common.Config.ConfigBase>(File.ReadAllText(filePath));
            Console.WriteLine("Version of config file: " + _configBase.Version);

            _previousVersion = _configBase.Version;
            while (_configBase.Version < Common.Config.ConfigBase.LatestVersion)
            {
                _configBase.Version += 1;
                Console.WriteLine(string.Format("Updating Config to v{0}...", _configBase.Version.ToString()));
                switch (_configBase.Version)
                {

                    case 2:
                        {
                            Common.Config.V1.Config configOld = XmlIO.ReadXmlText<Common.Config.V1.Config>(File.ReadAllText(filePath));
                            Common.Config.Config configNew = new Common.Config.Config();

                            foreach (var stash in configOld.Stashes)
                                stash.Expansion = 2;
                            configNew.Version = _configBase.Version;
                            configNew.Stashes = configOld.Stashes;
                            configNew.Settings.Language = configOld.Settings.Language;
                            configNew.Settings.GamePath = configOld.Settings.GamePath;
                            configNew.Settings.MaxBackups = configOld.Settings.MaxBackups;
                            configNew.Settings.ConfirmClosing = configOld.Settings.ConfirmClosing;
                            configNew.Settings.CloseWithGrimDawn = configOld.Settings.CloseWithGrimDawn;
                            configNew.Settings.ConfirmStashDelete = configOld.Settings.ConfirmStashDelete;
                            configNew.Settings.AutoStartGD = configOld.Settings.AutoStartGD;
                            configNew.Settings.AutoStartGDCommand = configOld.Settings.AutoStartGDCommand;
                            configNew.Settings.AutoStartGDArguments = configOld.Settings.AutoStartGDArguments;
                            configNew.Settings.LastID = configOld.Settings.LastID;
                            configNew.Settings.Main2SCID = configOld.Settings.MainSCID;
                            configNew.Settings.Main2HCID = configOld.Settings.MainHCID;
                            configNew.Settings.Cur2SCID = configOld.Settings.CurSCID;
                            configNew.Settings.Cur2HCID = configOld.Settings.CurHCID;

                            WriteToFile(configNew, filePath);
                        }
                        break;

                    case 3:
                        {
                            Common.Config.Config cfg = XmlIO.ReadXmlText<Common.Config.Config>(File.ReadAllText(filePath));
                            cfg.Version = _configBase.Version;
                            switch (cfg.Settings.Language)
                            {
                                case "de": cfg.Settings.Language = "deDE"; break;
                                case "en": cfg.Settings.Language = "enUS"; break;
                                case "zh": cfg.Settings.Language = "zhCN"; break;
                                default: cfg.Settings.Language = "enUS"; break;
                            }

                            WriteToFile(cfg, filePath);
                        }
                        break;

                    case 4:
                        {
                            string langDir = Path.Combine(Global.FileSystem.DataDirectory, "Locales");
                            if (Directory.Exists(langDir))
                                Directory.Delete(langDir, true);
                        }
                        break;

                }
                Console.WriteLine("... Done");
            }

            Console.WriteLine("Config is up to date");
            return XmlIO.ReadXmlText<Common.Config.Config>(File.ReadAllText(filePath));
        }

        public Common.Config.Config LoadOrCreate(string filePath)
        {
            Console.WriteLine("Loading config from file: " + filePath);
            if (File.Exists(filePath))
            {
                return LoadFromFile(filePath);
            }
            else
            {
                Console.WriteLine("- File not found -> Create new config");
                return new Common.Config.Config(true);
            }
        }

        public static void WriteToFile(Common.Config.Config cfg, string filePath)
        {
            Console.WriteLine("Writing config to file: " + filePath);
            if (File.Exists(filePath))
            {
                Console.WriteLine("- Deleting old file");
                File.Delete(filePath);
            }
            Console.WriteLine("- Writing new file");
            XmlIO.WriteXmlFile(cfg, filePath);
        }

        public void Load()
        {
            Console.WriteLine("Loading config");
            _config = LoadOrCreate(Global.FileSystem.ConfigFile);
        }

        public void Save()
        {
            Console.WriteLine("Saving config");
            WriteToFile(_config, Global.FileSystem.ConfigFile);
        }

        #endregion

        #region Settings Methods

        public void SetSettings(Common.Config.ConfigSettingList settings)
        {
            Common.Config.ConfigSettingList previous = Settings.Copy();
            _config.Settings.Set(settings);
            if (previous.Language != Settings.Language)
            {
                Global.Localization.LoadLanguage(Settings.Language);
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
            if (previous.GamePath != Settings.GamePath)
            {
                GamePathChanged?.Invoke(null, EventArgs.Empty);
            }
            if (previous.OverlayScale != Settings.OverlayScale || previous.OverlayWidth != Settings.OverlayWidth || previous.OverlayTransparency != Settings.OverlayTransparency)
            {
                AppearanceChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion

        #region Stash Methods

        public int GetMainStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            switch (exp)
            {
                case GrimDawnGameExpansion.BaseGame:
                    if (mode == GrimDawnGameMode.SC) return Settings.Main0SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Main0HCID;
                    break;
                case GrimDawnGameExpansion.AshesOfMalmouth:
                    if (mode == GrimDawnGameMode.SC) return Settings.Main1SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Main1HCID;
                    break;
                case GrimDawnGameExpansion.ForgottenGods:
                    if (mode == GrimDawnGameMode.SC) return Settings.Main2SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Main2HCID;
                    break;
            }
            return -1;
        }

        public int GetCurrentStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            switch (exp)
            {
                case GrimDawnGameExpansion.BaseGame:
                    if (mode == GrimDawnGameMode.SC) return Settings.Cur0SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Cur0HCID;
                    break;
                case GrimDawnGameExpansion.AshesOfMalmouth:
                    if (mode == GrimDawnGameMode.SC) return Settings.Cur1SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Cur1HCID;
                    break;
                case GrimDawnGameExpansion.ForgottenGods:
                    if (mode == GrimDawnGameMode.SC) return Settings.Cur2SCID;
                    if (mode == GrimDawnGameMode.HC) return Settings.Cur2HCID;
                    break;
            }
            return -1;
        }

        public void SetCurrentStashID(GrimDawnGameExpansion exp, GrimDawnGameMode mode, int stashID)
        {
            switch (exp)
            {
                case GrimDawnGameExpansion.BaseGame:
                    if (mode == GrimDawnGameMode.SC) Settings.Cur0SCID = stashID;
                    if (mode == GrimDawnGameMode.HC) Settings.Cur0HCID = stashID;
                    break;
                case GrimDawnGameExpansion.AshesOfMalmouth:
                    if (mode == GrimDawnGameMode.SC) Settings.Cur1SCID = stashID;
                    if (mode == GrimDawnGameMode.HC) Settings.Cur1HCID = stashID;
                    break;
                case GrimDawnGameExpansion.ForgottenGods:
                    if (mode == GrimDawnGameMode.SC) Settings.Cur2SCID = stashID;
                    if (mode == GrimDawnGameMode.HC) Settings.Cur2HCID = stashID;
                    break;
            }
        }

        public Common.Config.ConfigStash GetStashByID(int stashID)
        {
            return _config.Stashes.Find(s => { return s.ID == stashID; });
        }

        public int GetStashIndex(int stashID)
        {
            return _config.Stashes.FindIndex((stash) => { return stash.ID == stashID; });
        }

        public IEnumerable<Common.Config.ConfigStash> GetStashes()
        {
            return _config.Stashes;
        }

        public Common.Config.ConfigStash CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            Console.WriteLine(string.Format(@"Adding Config Stash: {0} (expansion: {1}, mode: {2})", name, expansion.ToString(), mode.ToString()));

            _config.Settings.LastID += 1;
            Common.Config.ConfigStash stash = new Common.Config.ConfigStash
            {
                Name = name,
                ID = _config.Settings.LastID,
                Order = _config.Settings.LastID,
                Expansion = (int)expansion,
                SC = mode.HasFlag(GrimDawnGameMode.SC),
                HC = mode.HasFlag(GrimDawnGameMode.HC),
            };
            _config.Stashes.Add(stash);

            Console.WriteLine("- id: " + stash.ID);
            Global.FileSystem.CreateStashDirectory(stash.ID);
            return stash;
        }

        public bool DeleteStash(int stashID)
        {
            Console.WriteLine(string.Format(@"Deleting Config Stash with id {0}", stashID));
            Global.FileSystem.DeleteStashDirectory(stashID);
            int index = GetStashIndex(stashID);
            if (index != -1)
            {
                Console.WriteLine(string.Format(@"- index: {0}", index));
                _config.Stashes.RemoveAt(index);
                return true;
            }
            else
            {
                Console.WriteLine(string.Format(@"- No Config Stash found with that id"));
                return false;
            }
        }

        public bool IsMainStashID(int stashID)
        {
            return (stashID == Settings.Main0SCID
                || stashID == Settings.Main0HCID
                || stashID == Settings.Main1SCID
                || stashID == Settings.Main1HCID
                || stashID == Settings.Main2SCID
                || stashID == Settings.Main2HCID);
        }

        public bool IsCurrentStashID(int stashID)
        {
            return (stashID == Settings.Cur0SCID
                || stashID == Settings.Cur0HCID
                || stashID == Settings.Cur1SCID
                || stashID == Settings.Cur1HCID
                || stashID == Settings.Cur2SCID
                || stashID == Settings.Cur2HCID);
        }

        #endregion

        #region Stash Category Methods

        public IEnumerable<Common.Config.ConfigStashCategory> GetCategories()
        {
            return _config.StashCategories;
        }

        public Common.Config.ConfigStashCategory GetCategoryByID(int catID)
        {
            return _config.StashCategories.Find(c => { return c.ID == catID; });
        }

        public Common.Config.ConfigStashCategory CreateStashCategory(string name, int id = -1)
        {
            Common.Config.ConfigStashCategory cat = null;
            // generate new id
            if (id == -1)
            {
                // find next ID to be used
                id = 1 + _config.StashCategories.Aggregate(0, (acc, x) => {
                    return Math.Max(acc, x.ID);
                });
            }
            // explicite id
            else
            {
                // check if ID already exists
                cat = GetCategoryByID(id);
                if (cat != null) return cat;
            }

            Console.WriteLine($"Adding Config Stash Category: #{id} {name}");
            cat = new Common.Config.ConfigStashCategory
            {
                Name = name,
                ID = id,
            };
            _config.StashCategories.Add(cat);
            return cat;
        }

        #endregion

    }
}
