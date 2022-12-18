
using System.Collections;
using System.Collections.Generic;
using BrightIdeasSoftware;

namespace GDMultiStash.Common.Objects.Sorting
{

    internal class ObjectListViewSortComparer<T> : ObjectSortComparer<OLVListItem, T>
    {
        public ObjectListViewSortComparer() : base()
        {
        }

        public ObjectListViewSortComparer(CompareDelegate comp) : base(comp)
        {
        }

        public ObjectListViewSortComparer(IComparer<T> comp) : base(comp)
        {
        }

        public ObjectListViewSortComparer(IComparer comp) : base(comp)
        {
        }

        public override int Compare(object x, object y)
        {
            return Compare((OLVListItem)x, (OLVListItem)y);
        }

        public override int Compare(OLVListItem x, OLVListItem y)
        {
            return Compare((T)x.RowObject, (T)y.RowObject);
        }
    }

}
