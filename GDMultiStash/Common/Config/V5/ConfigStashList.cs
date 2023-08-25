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
    [XmlType("Stashes")]
    public class ConfigStashList
    {
        [XmlAttribute("LastID")] public int LastID = 0; // the first stash will get id 1
        [XmlElement("Stash")] public List<ConfigStash> Items;

        public ConfigStashList()
        {
            Items = new List<ConfigStash>();
        }
    }
}
