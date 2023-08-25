using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal class ItemLevelSortMethod : ItemSortMethod<uint>
    {
        public override string GroupText => Global.L.GroupSortByLevelName();

        public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            return itemInfo.ShownLevelRequirement;
        }

        public override uint[] IgnoreKeys => new uint[] { };

        public override string GetText(uint key)
        {
            return $"{key}";
        }
    }
}
