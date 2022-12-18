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
    public class ConfigExpansion
    {
        [XmlAttribute("ID")] public int ID;
        [XmlAnyElement("Comment1")] public XmlComment NameComment = new XmlDocument().CreateComment("");
        [XmlIgnore] public string NameCommentValue { get => NameComment.Value; set { NameComment.Value = $" {value.Trim()} "; } }
        [XmlElement("SC")] public ConfigExpansionMode SC;
        [XmlElement("HC")] public ConfigExpansionMode HC;

        public ConfigExpansion()
        {
            SC = new ConfigExpansionMode();
            HC = new ConfigExpansionMode();
        }
    }
}
