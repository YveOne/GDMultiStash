
using System.Collections;
using System.Collections.Generic;

namespace GDMultiStash.Common.Objects.Sorting
{

    // universal class for generic and non-generic
    // so its just a wrapper to combines both
    // accepts delegates or custom comparer as arg
    internal abstract class ObjectSortComparer<U, T> : IComparer<U>, IComparer
    {
        public delegate int CompareDelegate(T x, T y);
        private readonly CompareDelegate comp;

        public ObjectSortComparer()
        {
            this.comp = delegate (T x, T y) {
                return 0;
            };
        }

        public ObjectSortComparer(CompareDelegate comp)
        {
            this.comp = comp;
        }

        public ObjectSortComparer(IComparer<T> comp)
        {
            this.comp = delegate (T x, T y) {
                return comp.Compare(x, y);
            };
        }

        public ObjectSortComparer(IComparer comp)
        {
            this.comp = delegate (T x, T y) {
                return comp.Compare(x, y);
            };
        }

        public virtual int Compare(T x, T y)
        {
            return comp(x, y);
        }

        public abstract int Compare(object x, object y);

        public abstract int Compare(U x, U y);

    }

}
