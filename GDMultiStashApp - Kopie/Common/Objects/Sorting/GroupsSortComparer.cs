
using System.Collections.Generic;

namespace GDMultiStash.Common.Objects.Sorting
{

    internal class GroupsSortComparer : IComparer<StashGroupObject>
    {
        public delegate bool CustomDelegate(StashGroupObject x, StashGroupObject y, out int ret);

        private CustomDelegate _custom = null;

        public GroupsSortComparer()
        {
        }

        public GroupsSortComparer(CustomDelegate custom)
        {
            _custom = custom;
        }

        public int Compare(StashGroupObject x, StashGroupObject y)
        {
            if (x.ID == 0) return -1; // keep main stashgrp always at top
            if (y.ID == 0) return +1;
            if (_custom != null && _custom(x, y, out int ret))
                return ret;
            return x.Order.CompareTo(y.Order);
        }
    }

}
