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

        [XmlElement("Settings")]
        public ConfigSettingList Settings;

        [XmlArray("Stashes")]
        [XmlArrayItem("Stash")]
        public ConfigStashList Stashes;

        [XmlArray("StashCategories")]
        [XmlArrayItem("Category")]
        public ConfigStashCategoryList StashCategories;

        public Config(bool IsNew) : base(IsNew)
        {
            Stashes = new ConfigStashList();
            Settings = new ConfigSettingList();
        }

        public Config() : base()
        {
            Stashes = new ConfigStashList();
            Settings = new ConfigSettingList();
        }

    }

}
