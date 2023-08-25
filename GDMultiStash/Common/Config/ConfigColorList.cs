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
    [XmlType("Colors")]
    public class ConfigColorList
    {
        [XmlElement("Color")] public List<ConfigColor> Items;
        public ConfigColorList()
        {
            Items = new List<ConfigColor>();
        }
    }
}
