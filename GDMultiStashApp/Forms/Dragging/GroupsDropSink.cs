using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.Dragging
{

    internal class GroupsDropSink : BaseDropSink<StashGroupObject>
    {
        public new GroupsDragHandler DragHandler => (GroupsDragHandler)base.DragHandler;

        public GroupsDropSink(GroupsDragHandler handler) : base(handler)
        {
        }

        public override void Reset()
        {
            base.Reset();
        }

        protected override void OnCanDrop(OlvDropEventArgs e)
        {
            base.OnCanDrop(e);
            if (e.Handled) return;
            e.Handled = true;

            e.Effect = DragDropEffects.None;
        }

        protected override void OnModelCanDrop(ModelDropEventArgs args)
        {
            base.OnModelCanDrop(args);
            if (args.Handled) return;
            args.Handled = true;

            // we are not dragging over item
            if (args.DropTargetItem == null)
            {
                args.Effect = DragDropEffects.None;
                return;
            }

            if (OverIndex == -1)
            {
                List<OLVListItem> lol = new List<OLVListItem>();
                foreach (OLVListItem item in DragHandler.ListView.Items)
                {
                    lol.Add(item);
                }
                lol.Sort((x, y) => x.Position.Y.CompareTo(y.Position.Y));
                foreach (OLVListItem item in lol)
                    OrderedList.Add((StashGroupObject)item.RowObject);
            }









            args.Effect = DragDropEffects.Move;
            SetOverIndex(args.DropTargetIndex);
        }

    }

}
