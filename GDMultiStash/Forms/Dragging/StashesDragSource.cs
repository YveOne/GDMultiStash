using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting.Comparer;

namespace GDMultiStash.Forms.Dragging
{
    internal class StashesDragSource : Base.DragSource<StashObject>
    {

        public StashesDragSource() : base(new StashesSortComparer())
        {
        }

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            if (button != MouseButtons.Left) return null;

            int draggingCount = 0;
            foreach (OLVListItem i in olv.SelectedItems)
            {
                if (i.RowObject is StashDummyObject) continue;

                StashObject stash = (StashObject)i.RowObject;
                if (!G.Configuration.IsMainStashID(stash.ID)) // dont add main stash to list
                    base.AddItem(stash);
                draggingCount += 1; // but increase count, even if its main stash, so dragging wont be canceled
            }

            if (draggingCount == 0)
                return null; // cancel when only dragging dummy





            return base.StartDrag(olv, button, item);
        }

        public override void EndDrag(object obj, DragDropEffects effect)
        {
            base.EndDrag(obj, effect);
        }

    }

}
