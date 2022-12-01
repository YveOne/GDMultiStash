using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GDMultiStash.Common.Config
{
    [Serializable]
    public class ConfigStashCategory
    {
        [XmlAttribute] public int ID { get; set; }
        [XmlAttribute] public string Name { get; set; }

		public ConfigStashCategory()
		{
			ID = -1;
			Name = null;
		}

	}
}
