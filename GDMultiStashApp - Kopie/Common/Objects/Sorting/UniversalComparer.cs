
using System.Collections;
using System.Collections.Generic;

namespace GDMultiStash.Common.Objects.Sorting
{

    // universal class for generic and non-generic
    // so its just a wrapper to combines both
    // accepts delegates or custom comparer as arg
    internal abstract class UniversalComparer<T> : IComparer<T>, IComparer
    {

        public UniversalComparer()
        {
        }

        public int Compare(object x, object y)
        {
            return Compare((T)x, (T)y);
        }

        public abstract int Compare(T x, T y);

    }

}
