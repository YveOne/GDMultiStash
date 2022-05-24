﻿using System;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms
{

    class StashesDropSink : SimpleDropSink
    {
        private StashesDragHandler _handler;

        public StashesDropSink(StashesDragHandler handler)
        {
            _handler = handler;
            base.CanDropBetween = false;
            base.CanDropOnBackground = false;
            base.CanDropOnItem = true;
            base.AcceptExternal = false;
            base.EnableFeedback = false;
        }








        private int lastTargetIndex = -1;

        public int TargetIndex => lastTargetIndex;

        protected override void OnModelCanDrop(ModelDropEventArgs args)
        {
            base.OnModelCanDrop(args);
            if (args.Handled) return;

            if (lastTargetIndex == args.DropTargetIndex) return;
            lastTargetIndex = args.DropTargetIndex;

            if (args.DropTargetItem == null || args.DropTargetIndex == -1 || _handler.DragSource.IsDraggingMainStash)
            {
                args.Effect = DragDropEffects.None;
            }
            else
            {
                Common.Stash stash = (Common.Stash)args.DropTargetItem.RowObject;
                if (stash == null || Core.Stashes.IsMainStash(stash))
                {
                    args.Effect = DragDropEffects.None;
                }
                else
                {
                    args.Effect = DragDropEffects.Move;
                    RearrangeModels(args);
                }
            }
        }

        public virtual void RearrangeModels(ModelDropEventArgs args)
        {
            if (args.DropTargetLocation != DropTargetLocation.Item) return;
            ListView.MoveObjects(args.DropTargetIndex + 1, args.SourceModels);
        }

    }

}
