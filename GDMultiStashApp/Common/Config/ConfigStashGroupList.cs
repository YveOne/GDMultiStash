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
    [XmlType("Groups")]
    public class ConfigStashGroupList
    {
        [XmlAttribute("LastID")] public int LastID = 0; // the first stash will get id 1
        [XmlElement("Group")] public List<ConfigStashGroup> Items;

        public ConfigStashGroupList()
        {
            Items = new List<ConfigStashGroup>();
        }

    }
}
