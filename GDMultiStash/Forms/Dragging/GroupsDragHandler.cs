using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.Dragging
{
    internal class GroupsDragHandler : BaseDragHandler<StashGroupObject>
    {
        public new ObjectListView ListView => base.ListView;
        public new GroupsDragSource DragSource => (GroupsDragSource)base.DragSource;
        public new GroupsDropSink DropSink => (GroupsDropSink)base.DropSink;

        public GroupsDragHandler(ObjectListView olv) : base(olv)
        {
            base.DragSource = new GroupsDragSource();
            base.DropSink = new GroupsDropSink(this);
        }

        protected override void OnDragEnd(object sender, EventArgs args)
        {
            if (DropSink.OverIndex != -1)
            {
                List<int> orders = new List<int>();
                foreach (StashGroupObject group in DropSink.OrderedList)
                {
                    orders.Add(group.Order);
                }
                orders.Sort();
                foreach (StashGroupObject group in DropSink.OrderedList)
                {
                    group.Order = orders[0];
                    orders.RemoveAt(0);
                }
            }
            DropSink.Reset();
        }

        public bool IsDraggingGroup(StashGroupObject group)
        {
            if (!IsDragging) return false;
            return DragSource.Items.Contains(group);
        }

    }
}
