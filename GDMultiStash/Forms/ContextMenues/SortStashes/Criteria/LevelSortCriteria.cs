using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{
    internal class LevelSortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString().PadLeft(3, '0');
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            return itemInfo.ShownLevelRequirement+1; // +1 to prevent returning 0 (ignored key)
        }
        public override string GetText(uint k)
        {
            return (k-1).ToString();
        }
    }
}
