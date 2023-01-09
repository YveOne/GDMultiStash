using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

namespace GDMultiStash.Forms.Dragging
{

    internal class GroupsDragSource : BaseDragSource<StashGroupObject>
    {

        public GroupsDragSource() : base(new GroupsSortComparer())
        {
        }

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            if (button != MouseButtons.Left) return null;

            int draggingCount = 0;
            foreach (OLVListItem i in olv.SelectedItems)
            {
                StashGroupObject group = (StashGroupObject)i.RowObject;
                base.AddItem(group);
                draggingCount += 1;
            }
            if (draggingCount == 0) return null;
            return base.StartDrag(olv, button, item);
        }

        public override void EndDrag(object obj, DragDropEffects effect)
        {
            base.EndDrag(obj, effect);
        }

    }

}
