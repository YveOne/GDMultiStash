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

        [XmlElement("WindowWidth")] public int WindowWidth = 650;
        [XmlElement("WindowHeight")] public int WindowHeight = 500;

        [XmlElement("Language")] public string Language = "enUS";
        [XmlElement("GamePath")] public string GamePath = "";

        [XmlElement("ShowExpansion")] public int ShowExpansion = -1;
        [XmlElement("ShowSoftcore")] public bool ShowSoftcore = false;
        [XmlElement("ShowHardcore")] public bool ShowHardcore = false;

        [XmlElement("MaxBackups")] public int MaxBackups = 10;

        [XmlElement("OverlayWidth")] public int OverlayWidth = 450;
        [XmlElement("OverlayScale")] public int OverlayScale = 100;

        [XmlElement("ConfirmClosing")] public bool ConfirmClosing = true;
        [XmlElement("CloseWithGrimDawn")] public bool CloseWithGrimDawn = true;
        [XmlElement("ConfirmStashDelete")] public bool ConfirmStashDelete = true;
        [XmlElement("AutoBackToMain")] public bool AutoBackToMain = true;

        [XmlElement("AutoStartGD")] public bool AutoStartGD = false;
        [XmlElement("AutoStartGDCommand")] public string AutoStartGDCommand = "";
        [XmlElement("AutoStartGDArguments")] public string AutoStartGDArguments = "";

        [XmlElement("CheckForNewVersion")] public bool CheckForNewVersion = true;
        [XmlElement("CheckForNewVersionDelay")] public int CheckForNewVersionDelay = 3600; // in seconds
        [XmlElement("AutoUpdate")] public bool AutoUpdate = false;
        [XmlElement("LastVersionCheck")] public long LastVersionCheck = 0L;

        [XmlElement("DefaultStashMode")] public int DefaultStashMode = 0;

        [XmlElement("ShowColorColumn")] public bool ShowColorColumn = true;
        [XmlElement("ShowExpansionColumn")] public bool ShowExpansionColumn = true;
        [XmlElement("ShowIDColumn")] public bool ShowIDColumn = true;
        [XmlElement("ShowLastChangeColumn")] public bool ShowLastChangeColumn = true;

        [XmlAnyElement("Comment1")] public XmlComment Comment1 = new XmlDocument().CreateComment("Change the following only if you know what you are doing!");

        [XmlElement("LastID")] public int LastID = 0; // the first stash will get id 1

        [XmlAnyElement("Comment2")] public XmlComment Comment2 = new XmlDocument().CreateComment("Grim Dawn Base Game");

        [XmlElement("Main0SCID")] public int Main0SCID = -1;
        [XmlElement("Main0HCID")] public int Main0HCID = -1;
        [XmlElement("Cur0SCID")] public int Cur0SCID = -1;
        [XmlElement("Cur0HCID")] public int Cur0HCID = -1;

        [XmlAnyElement("Comment3")] public XmlComment Comment3 = new XmlDocument().CreateComment("Grim Dawn: AoM");

        [XmlElement("Main1SCID")] public int Main1SCID = -1;
        [XmlElement("Main1HCID")] public int Main1HCID = -1;
        [XmlElement("Cur1SCID")] public int Cur1SCID = -1;
        [XmlElement("Cur1HCID")] public int Cur1HCID = -1;

        [XmlAnyElement("Comment4")] public XmlComment Comment4 = new XmlDocument().CreateComment("Grim Dawn: FG");

        [XmlElement("Main2SCID")] public int Main2SCID = -1;
        [XmlElement("Main2HCID")] public int Main2HCID = -1;
        [XmlElement("Cur2SCID")] public int Cur2SCID = -1;
        [XmlElement("Cur2HCID")] public int Cur2HCID = -1;


        public ConfigSettingList Copy()
        {
            return new ConfigSettingList
            {
                WindowWidth = WindowWidth,
                WindowHeight = WindowHeight,

                Language = Language,
                GamePath = GamePath,

                ShowExpansion = ShowExpansion,
                ShowSoftcore = ShowSoftcore,
                ShowHardcore = ShowHardcore,

                MaxBackups = MaxBackups,

                OverlayWidth = OverlayWidth,
                OverlayScale = OverlayScale,

                ConfirmClosing = ConfirmClosing,
                CloseWithGrimDawn = CloseWithGrimDawn,
                ConfirmStashDelete = ConfirmStashDelete,
                AutoBackToMain = AutoBackToMain,

                AutoStartGD = AutoStartGD,
                AutoStartGDCommand = AutoStartGDCommand,
                AutoStartGDArguments = AutoStartGDArguments,

                CheckForNewVersion = CheckForNewVersion,
                CheckForNewVersionDelay = CheckForNewVersionDelay,
                AutoUpdate = AutoUpdate,
                LastVersionCheck = LastVersionCheck,

                DefaultStashMode = DefaultStashMode,

                ShowColorColumn = ShowColorColumn,
                ShowExpansionColumn = ShowExpansionColumn,
                ShowIDColumn = ShowIDColumn,
                ShowLastChangeColumn = ShowLastChangeColumn,

                LastID = LastID,

                Main0SCID = Main0SCID,
                Main0HCID = Main0HCID,
                Cur0SCID = Cur0SCID,
                Cur0HCID = Cur0HCID,

                Main1SCID = Main1SCID,
                Main1HCID = Main1HCID,
                Cur1SCID = Cur1SCID,
                Cur1HCID = Cur1HCID,

                Main2SCID = Main2SCID,
                Main2HCID = Main2HCID,
                Cur2SCID = Cur2SCID,
                Cur2HCID = Cur2HCID,

            };
        }

        public void Set(ConfigSettingList s)
        {
            WindowWidth = s.WindowWidth;
            WindowHeight = s.WindowHeight;

            Language = s.Language;
            GamePath = s.GamePath;

            ShowExpansion = s.ShowExpansion;
            ShowSoftcore = s.ShowSoftcore;
            ShowHardcore = s.ShowHardcore;

            MaxBackups = s.MaxBackups;

            OverlayWidth = s.OverlayWidth;
            OverlayScale = s.OverlayScale;

            ConfirmClosing = s.ConfirmClosing;
            CloseWithGrimDawn = s.CloseWithGrimDawn;
            ConfirmStashDelete = s.ConfirmStashDelete;
            AutoBackToMain = s.AutoBackToMain;

            AutoStartGD = s.AutoStartGD;
            AutoStartGDCommand = s.AutoStartGDCommand;
            AutoStartGDArguments = s.AutoStartGDArguments;

            CheckForNewVersion = s.CheckForNewVersion;
            CheckForNewVersionDelay = s.CheckForNewVersionDelay;
            AutoUpdate = s.AutoUpdate;
            LastVersionCheck = s.LastVersionCheck;

            DefaultStashMode = s.DefaultStashMode;

            ShowColorColumn = s.ShowColorColumn;
            ShowExpansionColumn = s.ShowExpansionColumn;
            ShowIDColumn = s.ShowIDColumn;
            ShowLastChangeColumn = s.ShowLastChangeColumn;

            LastID = s.LastID;

            Main0SCID = s.Main0SCID;
            Main0HCID = s.Main0HCID;
            Cur0SCID = s.Cur0SCID;
            Cur0HCID = s.Cur0HCID;

            Main1SCID = s.Main1SCID;
            Main1HCID = s.Main1HCID;
            Cur1SCID = s.Cur1SCID;
            Cur1HCID = s.Cur1HCID;

            Main2SCID = s.Main2SCID;
            Main2HCID = s.Main2HCID;
            Cur2SCID = s.Cur2SCID;
            Cur2HCID = s.Cur2HCID;

        }

    }
}
