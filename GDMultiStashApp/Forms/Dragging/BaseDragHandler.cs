using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Dragging
{

    internal abstract class BaseDragHandler<T>
    {
        public ObjectListView ListView { get; private set; }
        public bool IsDragging { get; private set; } = false;
        public virtual BaseDragSource<T> DragSource
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
        private BaseDragSource<T> _dragSource = null;

        public virtual BaseDropSink<T> DropSink
        {
            get => _dropSink;
            protected set
            {
                ListView.DropSink = value;
                _dropSink = value;
            }
        }
        private BaseDropSink<T> _dropSink = null;

        public BaseDragHandler(ObjectListView olv)
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
