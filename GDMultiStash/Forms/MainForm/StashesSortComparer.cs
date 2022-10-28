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
            GlobalHandlers.StashObject s1 = ((OLVListItem)x).RowObject as GlobalHandlers.StashObject;
            GlobalHandlers.StashObject s2 = ((OLVListItem)y).RowObject as GlobalHandlers.StashObject;

            bool s1m = Global.Configuration.IsMainStashID(s1.ID);
            bool s2m = Global.Configuration.IsMainStashID(s2.ID);
            if (s1m && s2m) return s1.Expansion.CompareTo(s2.Expansion);
            if (s1m) return -1;
            //if (s2m) return -1;
            return s1.Order.CompareTo(s2.Order);

        }
    }

}
