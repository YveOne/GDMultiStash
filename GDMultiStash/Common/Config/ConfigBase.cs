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


        [XmlIgnore] public const int LatestVersion = 4;

        [XmlElement("Version")] public int Version { get; set; }


        [XmlIgnore]
        public bool IsNew { get; private set; }

        public ConfigBase(bool isNew)
        {
            Version = LatestVersion;
            IsNew = isNew;
        }

        public ConfigBase()
        {
            Version = LatestVersion;
            IsNew = false;
        }

    }
}
