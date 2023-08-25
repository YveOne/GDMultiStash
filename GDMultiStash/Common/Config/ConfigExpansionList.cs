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
    [XmlType("Expansions")]
    public class ConfigExpansionList
    {
        [XmlElement("Expansion")] public List<ConfigExpansion> Items;
        public ConfigExpansionList()
        {
            Items = new List<ConfigExpansion>();
        }
    }
}
