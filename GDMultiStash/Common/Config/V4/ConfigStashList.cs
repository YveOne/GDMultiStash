using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace GDMultiStash.Common.Config.V4
{
    [Serializable]
    public class ConfigStashList : List<ConfigStash>
    {
        public ConfigStashList() : base()
        {
        }
    }
}
