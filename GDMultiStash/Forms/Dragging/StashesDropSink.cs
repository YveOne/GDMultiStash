using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.Dragging
{

    internal class StashesDropSink : Base.DropSink<StashObject>
    {
        public new StashesDragHandler DragHandler => (StashesDragHandler)base.DragHandler;

        public StashGroupObject OverStashGroup { get; private set; } = null;

        public StashesDropSink(StashesDragHandler handler) : base(handler)
        {
        }

        public override void Reset()
        {
            base.Reset();
            OverStashGroup = null;
        }

        protected override void OnCanDrop(OlvDropEventArgs e)
        {
            base.OnCanDrop(e);
            if (e.Handled) return;
            e.Handled = true;

            e.Effect = DragDropEffects.None;
            if (e.DragEventArgs.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        protected override void OnModelCanDrop(ModelDropEventArgs args)
        {
            base.OnModelCanDrop(args);
            if (args.Handled) return;
            args.Handled = true;

            OLVGroup targetGroup = null;

            // we are not dragging over item
            // but maybe cursor is over group header?
            if (args.DropTargetItem == null)
            {
                targetGroup = (DragHandler.ListView).HitTestGroup(DragHandler.ListView.PointToClient(Cursor.Position), out bool _);
                if (targetGroup == null)
                {
                    args.Effect = DragDropEffects.None;
                    return;
                }
            }
            // we are dragging over stash item
            else
            {
                targetGroup = DragHandler.ListView.OLVGroups[DragHandler.ListView.Groups.IndexOf(args.DropTargetItem.Group)];
            }

            if (targetGroup == null)
            {
                args.Effect = DragDropEffects.None;
                return;
            }
            args.Effect = DragDropEffects.Move;

            StashGroupObject targetStashGroup = (StashGroupObject)targetGroup.Key;

            bool changedStashGroup = OverStashGroup == null || OverStashGroup.ID != targetStashGroup.ID;
            if (changedStashGroup)
            {
                OverStashGroup = targetStashGroup;
                //Console.WriteLine($"----------- CHANGED OverStashGroup: {OverStashGroup.ID}");
            }

            int targetIndex = 0;
            if (args.DropTargetItem != null)
            { // over stash item

                int overPosOffsetY = ((OLVListItem)DragHandler.ListView.Items[args.DropTargetIndex]).Bounds.Y;
                int overHeight = ((OLVListItem)DragHandler.ListView.Items[args.DropTargetIndex]).Bounds.Height;
                int indexOffset = ((args.MouseLocation.Y - overPosOffsetY) / (float)overHeight >= 0.75f) ? 1 : 0;

                targetIndex = targetGroup.Items.IndexOf(args.DropTargetItem);
                //if (targetIndex == -1) continue;
                targetIndex += indexOffset;
            }
            else
            { // over group header
                targetIndex = targetGroup.Items.Count - 1;
            }

            if (changedStashGroup)
            {
                base.Reset();
                foreach (OLVGroup olvGrp in DragHandler.ListView.OLVGroups)
                {
                    StashGroupObject stashGroup = (StashGroupObject)olvGrp.Key;
                    if (stashGroup.ID == OverStashGroup.ID)
                    {
                        foreach (OLVListItem item in olvGrp.Items)
                        {
                            if (item.RowObject is StashDummyObject) continue; // do not add dummies ...
                            OrderedList.Add((StashObject)item.RowObject);
                        }
                        OrderedList.Sort(new Common.Objects.Sorting.StashesSortComparer());
                    }
                }
            }

            SetOverIndex(targetIndex);
         }

    }

}
