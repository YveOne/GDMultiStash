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
    public class ConfigSortPattern
    {
        [XmlAttribute("Value")] public string Value;
        [XmlText] public string Name;
    }
}
