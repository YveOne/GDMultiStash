using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

using GrimDawnLib;

namespace GDMultiStash.Common.Config.V1
{
    [Serializable]
    public class ConfigSettingList
    {

        [XmlElement("Language")] public string Language = "en";
        [XmlElement("GamePath")] public string GamePath = "";

        [XmlElement("MaxBackups")] public int MaxBackups = 10;

        [XmlElement("ConfirmClosing")] public bool ConfirmClosing = true;
        [XmlElement("CloseWithGrimDawn")] public bool CloseWithGrimDawn = true;
        [XmlElement("ConfirmStashDelete")] public bool ConfirmStashDelete = true;

        [XmlElement("AutoStartGD")] public bool AutoStartGD = false;
        [XmlElement("AutoStartGDCommand")] public string AutoStartGDCommand = "";
        [XmlElement("AutoStartGDArguments")] public string AutoStartGDArguments = "";

        [XmlAnyElement("Comment1")] public XmlComment Comment1 = new XmlDocument().CreateComment("Change the following only if you know what you are doing!");

        [XmlElement("LastID")] public int LastID = 0; // the first stash will get id 1

        [XmlElement("MainSCID")] public int MainSCID = -1;
        [XmlElement("MainHCID")] public int MainHCID = -1;
        [XmlElement("CurSCID")] public int CurSCID = -1;
        [XmlElement("CurHCID")] public int CurHCID = -1;

        public ConfigSettingList Copy()
        {
            return new ConfigSettingList
            {
                Language = Language,
                GamePath = GamePath,

                MaxBackups = MaxBackups,

                ConfirmClosing = ConfirmClosing,
                CloseWithGrimDawn = CloseWithGrimDawn,
                ConfirmStashDelete = ConfirmStashDelete,

                AutoStartGD = AutoStartGD,
                AutoStartGDCommand = AutoStartGDCommand,
                AutoStartGDArguments = AutoStartGDArguments,

                Comment1 = Comment1,

                LastID = LastID,

                MainSCID = MainSCID,
                MainHCID = MainHCID,
                CurSCID = CurSCID,
                CurHCID = CurHCID,

            };
        }

        public void Set(ConfigSettingList s)
        {
            Language = s.Language;
            GamePath = s.GamePath;

            MaxBackups = s.MaxBackups;

            ConfirmClosing = s.ConfirmClosing;
            CloseWithGrimDawn = s.CloseWithGrimDawn;
            ConfirmStashDelete = s.ConfirmStashDelete;

            AutoStartGD = s.AutoStartGD;
            AutoStartGDCommand = s.AutoStartGDCommand;
            AutoStartGDArguments = s.AutoStartGDArguments;

            Comment1 = s.Comment1;

            LastID = s.LastID;

            MainSCID = s.MainSCID;
            MainHCID = s.MainHCID;
            CurSCID = s.CurSCID;
            CurHCID = s.CurHCID;

        }

    }
}
