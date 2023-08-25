
using System.Collections;
using System.Collections.Generic;
using BrightIdeasSoftware;

namespace GDMultiStash.Common.Objects.Sorting
{

    internal class ObjectListViewSortComparer<T> : Utils.UniversalComparer<OLVListItem>
    {
        private readonly IComparer<T> comparer;

        public ObjectListViewSortComparer(IComparer<T> comparer) : base()
        {
            this.comparer = comparer;
        }

        public override int Compare(OLVListItem x, OLVListItem y)
        {
            return this.comparer.Compare((T)x.RowObject, (T)y.RowObject);
        }
    }

}
