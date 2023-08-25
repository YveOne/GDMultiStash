using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects
{
    internal interface IBaseObject
    {
        int ID { get; }
        string Name { get; set; }
        int Order { get; set; }
    }
}
