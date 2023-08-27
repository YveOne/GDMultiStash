using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{

    internal abstract class SortCriteria
    {
        public abstract string FormatKey(uint k);
        public abstract uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo);
        public abstract string GetText(uint k);
    }

}
