using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.Dragging
{
    internal class StashesDragHandler : Base.DragHandler<StashObject>
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
            base.OnDragEnd(sender, args);
            if (DropSink.OverStashGroup != null)
            {
                foreach (StashObject stash in DropSink.OrderedList)
                {
                    stash.GroupID = DropSink.OverStashGroup.ID;
                }
                Global.Stashes.ResetOrder(DropSink.OrderedList);
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
