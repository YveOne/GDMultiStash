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
    [XmlType("SortPatterns")]
    public class ConfigSortPatternList
    {
        [XmlElement("SortPattern")] public List<ConfigSortPattern> Items;
        public ConfigSortPatternList()
        {
            Items = new List<ConfigSortPattern>();
        }
    }
}
