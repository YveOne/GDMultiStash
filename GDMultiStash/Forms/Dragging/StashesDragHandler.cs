using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.Dragging
{
    internal class StashesDragHandler : BaseDragHandler<StashObject>
    {
        public new Controls.OLVGroupFeatures ListView => (Controls.OLVGroupFeatures)base.ListView;
        public new StashesDragSource DragSource => (StashesDragSource)base.DragSource;
        public new StashesDropSink DropSink => (StashesDropSink)base.DropSink;

        public StashesDragHandler(ObjectListView olv) : base(olv)
        {
            base.DragSource = new StashesDragSource();
            base.DropSink = new StashesDropSink(this);
        }

        protected override void OnDragEnd(object sender, EventArgs args)
        {
            if (DropSink.OverStashGroup != null)
            {
                List<int> orders = new List<int>();
                foreach (StashObject stash in DropSink.OrderedList)
                {
                    stash.GroupID = DropSink.OverStashGroup.ID;
                    orders.Add(stash.Order);
                }
                orders.Sort();
                foreach (StashObject stash in DropSink.OrderedList)
                {
                    stash.Order = orders[0];
                    orders.RemoveAt(0);
                }
            }
            DropSink.Reset();
        }

        public bool IsDraggingStash(StashObject stash)
        {
            if (!IsDragging) return false;
            return DragSource.Items.Contains(stash);
        }

    }
}
