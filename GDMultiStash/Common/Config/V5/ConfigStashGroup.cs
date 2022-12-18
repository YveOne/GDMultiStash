using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDMultiStash.Common.Config.V5
{
    [Serializable]
    public class ConfigStashGroup
    {
        [XmlAttribute] public int ID { get; set; }
        [XmlAttribute] public string Name { get; set; }
		[XmlAttribute] public int Order { get; set; }

		public ConfigStashGroup()
		{
			ID = -1;
			Name = null;
			Order = -1;
		}

	}
}
