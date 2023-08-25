using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace GDMultiStash.Common.Config.V5
{
    [Serializable]
    public class ConfigSettingList
    {

        [XmlIgnore] private static readonly int _LastRevision = 4;
        [XmlIgnore] public int LastRevision => _LastRevision;
        [XmlElement("Revision")] public int Revision = _LastRevision;
        [XmlElement("LastToolVersion")] public string LastToolVersion = "";
        
        [XmlElement("WindowWidth")] public int WindowWidth = 650;
        [XmlElement("WindowHeight")] public int WindowHeight = 500;

        [XmlElement("Language")] public string Language = "enUS";
        [XmlElement("GamePath")] public string GamePath = "";

        //[XmlIgnore] public int ShowExpansion = 0;
        [XmlElement("ShowSoftcore")] public bool ShowSoftcore = false;
        [XmlElement("ShowHardcore")] public bool ShowHardcore = false;

        [XmlElement("MaxBackups")] public int MaxBackups = 10;

        [XmlElement("OverlayWidth")] public int OverlayWidth = 450;
        [XmlElement("OverlayScale")] public int OverlayScale = 100;
        [XmlElement("OverlayTransparency")] public int OverlayTransparency = 90;

        [XmlElement("ConfirmClosing")] public bool ConfirmClosing = true;
        [XmlElement("CloseWithGrimDawn")] public bool CloseWithGrimDawn = true;
        [XmlElement("ConfirmStashDelete")] public bool ConfirmStashDelete = true;
        [XmlElement("AutoBackToMain")] public bool AutoBackToMain = true;
        [XmlElement("SaveOverwritesLocked")] public bool SaveOverwritesLocked = false;

        [XmlElement("AutoStartGame")] public bool AutoStartGame = false;
        [XmlElement("StartGameCommand")] public string StartGameCommand = "";
        [XmlElement("StartGameArguments")] public string StartGameArguments = "";

        [XmlElement("CheckForNewVersion")] public bool CheckForNewVersion = true;

        [XmlElement("ShowIDColumn")] public bool ShowIDColumn = true;
        [XmlElement("ShowLastChangeColumn")] public bool ShowLastChangeColumn = true;

        public ConfigSettingList Copy()
        {
            return new ConfigSettingList
            {
                Revision = Revision,
                LastToolVersion = LastToolVersion,

                WindowWidth = WindowWidth,
                WindowHeight = WindowHeight,

                Language = Language,
                GamePath = GamePath,

                ShowSoftcore = ShowSoftcore,
                ShowHardcore = ShowHardcore,

                MaxBackups = MaxBackups,

                OverlayWidth = OverlayWidth,
                OverlayScale = OverlayScale,
                OverlayTransparency = OverlayTransparency,

                ConfirmClosing = ConfirmClosing,
                CloseWithGrimDawn = CloseWithGrimDawn,
                ConfirmStashDelete = ConfirmStashDelete,
                AutoBackToMain = AutoBackToMain,
                SaveOverwritesLocked = SaveOverwritesLocked,

                AutoStartGame = AutoStartGame,
                StartGameCommand = StartGameCommand,
                StartGameArguments = StartGameArguments,

                CheckForNewVersion = CheckForNewVersion,

                ShowIDColumn = ShowIDColumn,
                ShowLastChangeColumn = ShowLastChangeColumn,

            };
        }

        public void Set(ConfigSettingList s)
        {
            Revision = s.Revision;
            LastToolVersion = s.LastToolVersion;

            WindowWidth = s.WindowWidth;
            WindowHeight = s.WindowHeight;

            Language = s.Language;
            GamePath = s.GamePath;

            ShowSoftcore = s.ShowSoftcore;
            ShowHardcore = s.ShowHardcore;

            MaxBackups = s.MaxBackups;

            OverlayWidth = s.OverlayWidth;
            OverlayScale = s.OverlayScale;
            OverlayTransparency = s.OverlayTransparency;

            ConfirmClosing = s.ConfirmClosing;
            CloseWithGrimDawn = s.CloseWithGrimDawn;
            ConfirmStashDelete = s.ConfirmStashDelete;
            AutoBackToMain = s.AutoBackToMain;
            SaveOverwritesLocked = s.SaveOverwritesLocked;

            AutoStartGame = s.AutoStartGame;
            StartGameCommand = s.StartGameCommand;
            StartGameArguments = s.StartGameArguments;

            CheckForNewVersion = s.CheckForNewVersion;

            ShowIDColumn = s.ShowIDColumn;
            ShowLastChangeColumn = s.ShowLastChangeColumn;

        }

    }
}
