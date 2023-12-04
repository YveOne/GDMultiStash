using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global.Base
{
    public class ObjectsEventArgs<T> : EventArgs
    {
        public T[] Items { get; private set; }
        public ObjectsEventArgs(IEnumerable<T> list)
        {
            Items = list.ToArray();
        }
        public ObjectsEventArgs(T obj)
        {
            Items = new T[] { obj };
        }
    }

}
