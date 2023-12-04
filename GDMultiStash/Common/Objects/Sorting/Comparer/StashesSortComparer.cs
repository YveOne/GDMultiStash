using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects.Sorting.Comparer
{
    internal class StashesSortComparer : Utils.UniversalComparer<StashObject>
    {
        public delegate bool CustomDelegate(StashObject x, StashObject y, out int ret);

        private CustomDelegate _custom = null;

        public StashesSortComparer()
        {
        }

        public StashesSortComparer(CustomDelegate custom)
        {
            _custom = custom;
        }

        public override int Compare(StashObject x, StashObject y)
        {
            if (x is StashDummyObject) return +1;
            if (y is StashDummyObject) return -1;
            bool s1m = G.Configuration.IsMainStashID(x.ID);
            bool s2m = G.Configuration.IsMainStashID(y.ID);
            if (s1m && s2m) return x.Order.CompareTo(y.Order);
            if (s1m) return -1; // keep main stashgrp always at top
            if (s2m) return +1;
            if (_custom != null && _custom(x, y, out int ret))
                return ret;
            return x.Order.CompareTo(y.Order);
        }

    }
}
