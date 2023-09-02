using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GrimDawnLib;

namespace GDMultiStash.Common.Config
{

    [Serializable]
    [XmlRoot("Config", IsNullable = false)]
    public partial class Config : ConfigBase
    {

        [XmlElement("LastToolVersion")]
        public string LastToolVersion = "";

        [XmlAttribute("UpdateRevision")]
        public int UpdateRevision = 0;

        [XmlElement("Settings")]
        public ConfigSettingList Settings;

        [XmlElement("Colors")]
        public ConfigColorList Colors;

        [XmlElement("SortPatterns")]
        public ConfigSortPatternList SortPatterns;

        [XmlElement("Expansions")]
        public ConfigExpansionList Expansions;

        [XmlElement("Stashes")]
        public ConfigStashList Stashes;

        [XmlElement("Groups")]
        public ConfigStashGroupList StashGroups;

        public Config() : base()
        {
            Settings = new ConfigSettingList();
            Colors = new ConfigColorList();
            SortPatterns = new ConfigSortPatternList();
            Expansions = new ConfigExpansionList();
            Stashes = new ConfigStashList();
            StashGroups = new ConfigStashGroupList();
        }

    }

}
