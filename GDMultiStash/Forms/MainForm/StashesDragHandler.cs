﻿using System;
using System.Collections.Generic;
using System.Drawing;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms
{
    class StashesDragHandler
    {

        private readonly ObjectListView _olv;
        private readonly StashesDragSource _dragSource;
        private readonly StashesDropSink _dropSink;

        public ObjectListView ListView => _olv;
        public StashesDragSource DragSource => _dragSource;

        private bool _isDragging;
        public bool IsDragging => _isDragging;

        public StashesDragHandler(ObjectListView olv)
        {
            _olv = olv;
            _dragSource = new StashesDragSource(this);
            _dragSource.DragStart += delegate {
                _isDragging = true;
            };
            _dragSource.DragEnd += delegate {
                _isDragging = false;
                List<int> orders = new List<int>();
                foreach (OLVListItem item in olv.Items)
                {
                    Common.Stash stash = (Common.Stash)item.RowObject;
                    orders.Add(stash.Order);
                }
                orders.Sort();
                foreach (OLVListItem item in olv.Items)
                {
                    Common.Stash stash = (Common.Stash)item.RowObject;
                    stash.Order = orders[0];
                    orders.RemoveAt(0);
                }
            };
            _dropSink = new StashesDropSink(this);
            olv.DragSource = _dragSource;
            olv.DropSink = _dropSink;
            olv.DragLeave += delegate {
                ResetDragPositions();
            };
        }

        public void ResetDragPositions()
        {
            ListView.MoveObjects(0, _dragSource.OriginalOrderedModels);
        }

    }
}
