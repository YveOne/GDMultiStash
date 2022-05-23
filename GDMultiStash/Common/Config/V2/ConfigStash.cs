using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDMultiStash.Common.Config
{
    [Serializable]
	public class ConfigStash
    {

		[XmlAttribute] public int ID { get; set; }
		[XmlAttribute] public int Order { get; set; }
		[XmlAttribute] public bool SC { get; set; }
		[XmlAttribute] public bool HC { get; set; }
		[XmlAttribute] public int Expansion { get; set; }
		[XmlAttribute] public string Name { get; set; }

		public ConfigStash()
		{
			ID = -1;
			Order = -1;
			SC = false;
			HC = false;
			Name = null;
		}

	}
}
