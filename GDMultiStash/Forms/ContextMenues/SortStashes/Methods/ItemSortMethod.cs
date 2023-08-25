using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal abstract class ItemSortMethod<T>
    {
        public abstract string GroupText { get; }
        public abstract T GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo);
        public abstract T[] IgnoreKeys { get; }
        public abstract string GetText(T key);
    }
}
