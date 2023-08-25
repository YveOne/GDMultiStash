using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GrimDawnLib;

namespace GDMultiStash.Common.Config.V4
{

    [Serializable]
    [XmlRoot("Config", IsNullable = false)]
    public partial class Config : ConfigBase
    {

        [XmlElement("Settings")]
        public ConfigSettingList Settings;

        [XmlElement("Stashes")]
        public ConfigStashList Stashes;

        public Config() : base()
        {
            Stashes = new ConfigStashList();
            Settings = new ConfigSettingList();
        }

    }

}
