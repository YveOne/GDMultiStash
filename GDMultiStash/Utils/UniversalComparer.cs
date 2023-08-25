
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    // universal class for generic and non-generic
    // so its just a wrapper to combines both
    internal abstract class UniversalComparer<T> : IComparer<T>, IComparer
    {
        public int Compare(object x, object y) => Compare((T)x, (T)y);
        public abstract int Compare(T x, T y);
    }
}
