using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal class ItemNoneSortMethod : ItemSortMethod<int>
    {
        public override string GroupText => Global.L.GroupSortByNoneName();

        public override int GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            return 0;
        }

        public override int[] IgnoreKeys => new int[] { };

        public override string GetText(int key)
        {
            return Global.L.StashSortByNoneName();
        }
    }
}
