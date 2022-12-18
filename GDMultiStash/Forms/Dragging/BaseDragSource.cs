using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Dragging
{

    internal abstract class BaseDragSource<T> : SimpleDragSource
    {
        public event EventHandler<EventArgs> DragStart;
        public event EventHandler<EventArgs> DragEnd;

        public IList<T> Items => _items.AsReadOnly();
        private readonly List<T> _items = new List<T>();

        protected virtual void AddItem(T item)
        {
            _items.Add(item);
        }

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            DragStart?.Invoke(this, EventArgs.Empty);
            return (DataObject)base.StartDrag(olv, button, item);
        }

        public override void EndDrag(object obj, DragDropEffects effect)
        {
            _items.Clear();
            base.EndDrag(obj, effect);
            DragEnd?.Invoke(this, EventArgs.Empty);
        }

    }

}
