using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{
    internal class NoneSortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString();
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            return 1;
        }
        public override string GetText(uint k)
        {
            return Global.L.SortByNone();
        }
    }
}
