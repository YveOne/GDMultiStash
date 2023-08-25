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
    public class ConfigExpansion
    {
        [XmlAttribute("ID")] public int ID;
        [XmlAttribute("Name")] public string Name;
        [XmlElement("SC")] public ConfigExpansionMode SC;
        [XmlElement("HC")] public ConfigExpansionMode HC;

        public ConfigExpansion()
        {
            SC = new ConfigExpansionMode();
            HC = new ConfigExpansionMode();
        }
    }
}
