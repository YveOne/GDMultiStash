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
    public class ConfigExpansionMode
    {
        [XmlAttribute("MainID")] public int MainID = -1;
        [XmlAttribute("CurrentID")] public int CurrentID = -1;
    }
}
