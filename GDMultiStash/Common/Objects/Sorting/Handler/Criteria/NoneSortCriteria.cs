using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects.Sorting.Handler.Criteria
{
    internal class NoneSortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString();
        public override uint GetKey(Global.Database.ItemInfo itemInfo)
        {
            return 1;
        }
        public override string GetText(uint k)
        {
            return G.L.SortByNone();
        }
    }
}
