using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects.Sorting.Handler.Criteria
{

    internal abstract class SortCriteria
    {
        public abstract string FormatKey(uint k);
        public abstract uint GetKey(Global.Database.ItemInfo itemInfo);
        public abstract string GetText(uint k);
    }

}
