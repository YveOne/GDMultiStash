using System;
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

        public StashesDragHandler(ObjectListView olv)
        {
            _olv = olv;
            _dragSource = new StashesDragSource(this);
            _dragSource.DragStart += delegate {
            };
            _dragSource.DragEnd += delegate {
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
            olv.FormatRow += delegate (object sender, FormatRowEventArgs e)
            {
                Common.Stash stash = (Common.Stash)e.Model;
                if (_dragSource.DraggingStashes.Contains(stash)
                 && !_dragSource.IsDraggingMainStash)
                {
                    e.Item.BackColor = Color.Teal;
                    e.Item.ForeColor = Color.White;
                }
            };
            _dropSink = new StashesDropSink(this);
            olv.DragSource = _dragSource;
            olv.DropSink = _dropSink;
            olv.DragLeave += delegate {

                Console.WriteLine(_dragSource.OriginalIndex);
                Console.WriteLine(_dragSource.DraggingStashes.Count);
                olv.MoveObjects(_dragSource.OriginalIndex + 1, _dragSource.DraggingStashes);

            };
        }

    }
}
