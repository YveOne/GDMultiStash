using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace GDMultiStash.Common.Config
{
    [Serializable]
    public class ConfigSettingList
    {
        [XmlElement("LastToolVersion")] public string LastToolVersion = "";
        
        [XmlElement("WindowWidth")] public int WindowWidth = 650;
        [XmlElement("WindowHeight")] public int WindowHeight = 500;

        [XmlElement("Language")] public string Language = "enUS";
        [XmlElement("GamePath")] public string GamePath = "";

        //[XmlIgnore] public int ShowExpansion = 0;
        [XmlElement("ShowSoftcoreState")] public int ShowSoftcoreState = -1;
        [XmlElement("ShowHardcoreState")] public int ShowHardcoreState = -1;

        [XmlElement("MaxBackups")] public int MaxBackups = 10;

        [XmlElement("OverlayWidth")] public int OverlayWidth = 450;
        [XmlElement("OverlayScale")] public int OverlayScale = 100;
        [XmlElement("OverlayTransparency")] public int OverlayTransparency = 90;
        [XmlElement("OverlayStashesCount")] public int OverlayStashesCount = 20;
        [XmlElement("OverlayShowWorkload")] public bool OverlayShowWorkload = true;

        [XmlElement("ConfirmClosing")] public bool ConfirmClosing = true;
        [XmlElement("CloseWithGrimDawn")] public bool CloseWithGrimDawn = true;
        [XmlElement("ConfirmStashDelete")] public bool ConfirmStashDelete = true; // TODO: rename, this is used for ALL deletions
        [XmlElement("AutoBackToMain")] public bool AutoBackToMain = true;
        [XmlElement("AutoSelectFirstStashInGroup")] public bool AutoSelectFirstStashInGroup = true;
        [XmlElement("SaveOverwritesLocked")] public bool SaveOverwritesLocked = false;
        [XmlElement("SaveExternalChanges")] public bool SaveExternalChanges = true;

        [XmlElement("AutoStartGame")] public bool AutoStartGame = false;
        [XmlElement("StartGameCommand")] public string StartGameCommand = "";
        [XmlElement("StartGameArguments")] public string StartGameArguments = "";

        [XmlElement("CheckForNewVersion")] public bool CheckForNewVersion = true;

        [XmlElement("ShowIDColumn")] public bool ShowIDColumn = true;
        [XmlElement("ShowLastChangeColumn")] public bool ShowLastChangeColumn = true;

        [XmlElement("ExperimentalFeatures")] public bool ExperimentalFeatures = false;

        public ConfigSettingList Copy()
        {
            return new ConfigSettingList
            {
                LastToolVersion = LastToolVersion,

                WindowWidth = WindowWidth,
                WindowHeight = WindowHeight,

                Language = Language,
                GamePath = GamePath,

                ShowSoftcoreState = ShowSoftcoreState,
                ShowHardcoreState = ShowHardcoreState,

                MaxBackups = MaxBackups,

                OverlayWidth = OverlayWidth,
                OverlayScale = OverlayScale,
                OverlayTransparency = OverlayTransparency,
                OverlayStashesCount = OverlayStashesCount,
                OverlayShowWorkload = OverlayShowWorkload,

                ConfirmClosing = ConfirmClosing,
                CloseWithGrimDawn = CloseWithGrimDawn,
                ConfirmStashDelete = ConfirmStashDelete,
                AutoBackToMain = AutoBackToMain,
                AutoSelectFirstStashInGroup = AutoSelectFirstStashInGroup,
                SaveOverwritesLocked = SaveOverwritesLocked,
                SaveExternalChanges = SaveExternalChanges,

                AutoStartGame = AutoStartGame,
                StartGameCommand = StartGameCommand,
                StartGameArguments = StartGameArguments,

                CheckForNewVersion = CheckForNewVersion,

                ShowIDColumn = ShowIDColumn,
                ShowLastChangeColumn = ShowLastChangeColumn,

                ExperimentalFeatures = ExperimentalFeatures,
            };
        }

        public void Set(ConfigSettingList s)
        {
            LastToolVersion = s.LastToolVersion;

            WindowWidth = s.WindowWidth;
            WindowHeight = s.WindowHeight;

            Language = s.Language;
            GamePath = s.GamePath;

            ShowSoftcoreState = s.ShowSoftcoreState;
            ShowHardcoreState = s.ShowHardcoreState;

            MaxBackups = s.MaxBackups;

            OverlayWidth = s.OverlayWidth;
            OverlayScale = s.OverlayScale;
            OverlayTransparency = s.OverlayTransparency;
            OverlayStashesCount = s.OverlayStashesCount;
            OverlayShowWorkload = s.OverlayShowWorkload;

            ConfirmClosing = s.ConfirmClosing;
            CloseWithGrimDawn = s.CloseWithGrimDawn;
            ConfirmStashDelete = s.ConfirmStashDelete;
            AutoBackToMain = s.AutoBackToMain;
            AutoSelectFirstStashInGroup = s.AutoSelectFirstStashInGroup;
            SaveOverwritesLocked = s.SaveOverwritesLocked;
            SaveExternalChanges = s.SaveExternalChanges;

            AutoStartGame = s.AutoStartGame;
            StartGameCommand = s.StartGameCommand;
            StartGameArguments = s.StartGameArguments;

            CheckForNewVersion = s.CheckForNewVersion;

            ShowIDColumn = s.ShowIDColumn;
            ShowLastChangeColumn = s.ShowLastChangeColumn;

            ExperimentalFeatures = s.ExperimentalFeatures;
        }

    }
}
