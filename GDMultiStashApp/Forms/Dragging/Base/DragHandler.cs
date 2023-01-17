using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Dragging.Base
{

    internal abstract class DragHandler<T>
    {
        public ObjectListView ListView { get; private set; }
        public bool IsDragging { get; private set; } = false;
        public virtual DragSource<T> DragSource
        {
            get => _dragSource;
            protected set
            {
                ListView.DragSource = value;
                _dragSource = value;
                _dragSource.DragStart += OnDragStart;
                _dragSource.DragEnd += OnDragEnd;
            }
        }
        private DragSource<T> _dragSource = null;

        public virtual DropSink<T> DropSink
        {
            get => _dropSink;
            protected set
            {
                ListView.DropSink = value;
                _dropSink = value;
            }
        }
        private DropSink<T> _dropSink = null;

        public DragHandler(ObjectListView olv)
        {
            ListView = olv;
        }

        protected virtual void OnDragStart(object sender, EventArgs args)
        {
            IsDragging = true;
        }

        protected virtual void OnDragEnd(object sender, EventArgs args)
        {
            IsDragging = false;
        }

    }

}
