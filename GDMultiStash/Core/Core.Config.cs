using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using XMLHelper;

namespace GDMultiStash
{
    internal static partial class Core
    {
        public static class Config
        {

            private static Common.Config.Config _config;

            private static int _previousVersion = 0;
            public static int PreviousVersion => _previousVersion;

            private static Common.Config.Config LoadFromFile(string filePath)
            {
                Common.Config.ConfigBase _configBase = XmlIO.ReadXmlText<Common.Config.ConfigBase>(File.ReadAllText(filePath));
                Console.WriteLine("Version of config file: " + _configBase.Version);

                _previousVersion = _configBase.Version;
                while (_configBase.Version < Common.Config.ConfigBase.LatestVersion)
                {
                    switch (_configBase.Version)
                    {

                        case 1:
                            Console.WriteLine("Updating Config to V2...");
                            _configBase.Version += 1;

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

                            Console.WriteLine("... Done");
                            WriteToFile(configNew, filePath);
                            break;

                    }
                }
                //V1.ConfigV1 _configV1 = (V1.ConfigV1)new XmlSerializer(typeof(V1.ConfigV1)).Deserialize(xmlReader);
                Console.WriteLine("Config is up to date");
                return XmlIO.ReadXmlText<Common.Config.Config>(File.ReadAllText(filePath));
            }

            public static Common.Config.Config LoadOrCreate(string filePath)
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














            public static void Load()
            {
                Console.WriteLine("Loading config");
                _config = LoadOrCreate(Files.DataConfigFilePath);
            }

            public static void Save()
            {
                Console.WriteLine("Saving config");
                WriteToFile(_config, Files.DataConfigFilePath);
            }














            public static Common.Config.ConfigStash GetStashByID(int id)
            {
                return _config.GetStashByID(id);
            }

            public static Common.Config.ConfigStash[] GetStashes()
            {
                return _config.GetStashes();
            }

            public static Common.Config.ConfigStash CreateStash(string name, GrimDawnGameExpansion expansion, GrimDawnGameMode mode = GrimDawnGameMode.None)
            {
                Console.WriteLine(string.Format(@"Adding Config Stash: {0} (expansion: {1}, mode: {2})", name, expansion.ToString(), mode.ToString()));
                Common.Config.ConfigStash stash = _config.CreateStash(name, expansion, mode);
                Console.WriteLine("- id: " + stash.ID);
                Files.CreateStashDir(stash.ID);
                return stash;
            }

            public static bool DeleteStash(int stashID)
            {
                Console.WriteLine(string.Format(@"Deleting Config Stash with id {0}", stashID));
                Files.DeleteStashDir(stashID);
                int index = _config.GetStashIndex(stashID);
                if (index != -1)
                {
                    Console.WriteLine(string.Format(@"- index: {0}", index));
                    _config.DeleteStashAt(index);
                    return true;
                }
                else
                {
                    Console.WriteLine(string.Format(@"- No Config Stash found with that id"));
                    return false;
                }
            }




            public static bool IsMainStashID(int stashID)
            {
                return (stashID == Main0SCID
                    || stashID == Main0HCID
                    || stashID == Main1SCID
                    || stashID == Main1HCID
                    || stashID == Main2SCID
                    || stashID == Main2HCID);
            }

            public static bool IsCurStashID(int stashID)
            {
                return (stashID == Cur0SCID
                    || stashID == Cur0HCID
                    || stashID == Cur1SCID
                    || stashID == Cur1HCID
                    || stashID == Cur2SCID
                    || stashID == Cur2HCID);
            }





            public static Common.Config.ConfigSettingList GetSettings()
            {
                return _config.GetSettings();
            }

            public static void SetSettings(Common.Config.ConfigSettingList settings)
            {
                Common.Config.ConfigSettingList previous = GetSettings();
                _config.SetSettings(settings);

                if (previous.Language != Language)
                {
                    Localization.LoadLanguage(Language);
                    LanguageChanged?.Invoke(null, EventArgs.Empty);
                }
                if (previous.GamePath != GamePath) GamePathChanged?.Invoke(null, EventArgs.Empty);
                if (previous.OverlayScale != OverlayScale
                    || previous.OverlayWidth != OverlayWidth) AppearanceChanged?.Invoke(null, EventArgs.Empty);

            }

            #region Events

            public delegate void SettingChangedDelegate(object sender, EventArgs e);
            public static event SettingChangedDelegate LanguageChanged;
            public static event SettingChangedDelegate GamePathChanged;
            public static event SettingChangedDelegate AppearanceChanged;

            #endregion

            #region Properties

            public static bool IsNew
            {
                get { return _config.IsNew; }
            }

            public static string Language
            {
                get { return _config.Settings.Language; }
            }

            public static string GamePath
            {
                get { return _config.Settings.GamePath; }
            }

            public static int MaxBackups
            {
                get { return _config.Settings.MaxBackups; }
            }

            public static int Main0SCID
            {
                get { return _config.Settings.Main0SCID; }
                set { _config.Settings.Main0SCID = value; }
            }

            public static int Main0HCID
            {
                get { return _config.Settings.Main0HCID; }
                set { _config.Settings.Main0HCID = value; }
            }

            public static int Cur0SCID
            {
                get { return _config.Settings.Cur0SCID; }
                set { _config.Settings.Cur0SCID = value; }
            }

            public static int Cur0HCID
            {
                get { return _config.Settings.Cur0HCID; }
                set { _config.Settings.Cur0HCID = value; }
            }

            public static int Main1SCID
            {
                get { return _config.Settings.Main1SCID; }
                set { _config.Settings.Main1SCID = value; }
            }

            public static int Main1HCID
            {
                get { return _config.Settings.Main1HCID; }
                set { _config.Settings.Main1HCID = value; }
            }

            public static int Cur1SCID
            {
                get { return _config.Settings.Cur1SCID; }
                set { _config.Settings.Cur1SCID = value; }
            }

            public static int Cur1HCID
            {
                get { return _config.Settings.Cur1HCID; }
                set { _config.Settings.Cur1HCID = value; }
            }

            public static int Main2SCID
            {
                get { return _config.Settings.Main2SCID; }
                set { _config.Settings.Main2SCID = value; }
            }

            public static int Main2HCID
            {
                get { return _config.Settings.Main2HCID; }
                set { _config.Settings.Main2HCID = value; }
            }

            public static int Cur2SCID
            {
                get { return _config.Settings.Cur2SCID; }
                set { _config.Settings.Cur2SCID = value; }
            }

            public static int Cur2HCID
            {
                get { return _config.Settings.Cur2HCID; }
                set { _config.Settings.Cur2HCID = value; }
            }

            public static int ShowExpansion
            {
                get => _config.Settings.ShowExpansion;
                set => _config.Settings.ShowExpansion = value;
            }

            public static bool ShowSoftcore
            {
                get => _config.Settings.ShowSoftcore;
                set => _config.Settings.ShowSoftcore = value;
            }

            public static bool ShowHardcore
            {
                get => _config.Settings.ShowHardcore;
                set => _config.Settings.ShowHardcore = value;
            }

            public static bool ShowCloseConfirm
            {
                get { return _config.Settings.ConfirmClosing; }
            }

            public static bool CloseWithGrimDawn
            {
                get { return _config.Settings.CloseWithGrimDawn; }
            }

            public static bool ConfirmStashDelete
            {
                get { return _config.Settings.ConfirmStashDelete; }
            }

            public static bool AutoBackToMain
            {
                get { return _config.Settings.AutoBackToMain; }
            }

            public static bool AutoStartGD
            {
                get { return _config.Settings.AutoStartGD; }
            }

            public static string AutoStartGDCommand
            {
                get { return _config.Settings.AutoStartGDCommand; }
            }

            public static string AutoStartGDArguments
            {
                get { return _config.Settings.AutoStartGDArguments; }
            }

            public static int OverlayWidth
            {
                get { return _config.Settings.OverlayWidth; }
            }

            public static float OverlayScale
            {
                get { return _config.Settings.OverlayScale / 100f; }
            }

            public static long LastVersionCheck
            {
                get => _config.Settings.LastVersionCheck;
                set => _config.Settings.LastVersionCheck = value;
            }

            public static bool CheckForNewVersion
            {
                get => _config.Settings.CheckForNewVersion;
            }

            public static bool AutoUpdate
            {
                get => _config.Settings.AutoUpdate;
            }

            public static int DefaultStashMode
            {
                get => _config.Settings.DefaultStashMode;
            }

            #endregion

        }
    }
}
