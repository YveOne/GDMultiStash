using System;
using System.Collections;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms
{

    class StashesSortComparer : IComparer
    {

        public StashesSortComparer()
        {
        }

        public int Compare(object x, object y)
        {
            Common.Stash s1 = ((OLVListItem)x).RowObject as Common.Stash;
            Common.Stash s2 = ((OLVListItem)y).RowObject as Common.Stash;

            bool s1m = Core.Stashes.IsMainStash(s1);
            bool s2m = Core.Stashes.IsMainStash(s2);
            if (s1m && s2m) return s1.Expansion.CompareTo(s2.Expansion);
            if (s1m) return -1;
            if (s2m) return +1;
            return s1.Order.CompareTo(s2.Order);

        }
    }

}
