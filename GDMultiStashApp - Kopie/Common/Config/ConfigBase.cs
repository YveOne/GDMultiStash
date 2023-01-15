using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDMultiStash.Common.Config
{
    [Serializable]
    [XmlRoot("Config", IsNullable = false)]
    public class ConfigBase
    {

        [XmlIgnore] public const int LatestVersion = 6;

        [XmlElement("Version")] public int OldVersion = 0; // deprecated. TODO: remove me in future release
        [XmlAttribute("Version")] public int Version = LatestVersion;

        public ConfigBase()
        {
        }

    }
}
