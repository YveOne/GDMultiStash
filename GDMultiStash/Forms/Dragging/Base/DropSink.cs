using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Dragging.Base
{

    internal abstract class DropSink<T> : SimpleDropSink
    {

        protected DragHandler<T> DragHandler { get; private set; }

        public List<T> OrderedList { get; private set; } = new List<T>();

        public int OverIndex { get; private set; } = -1;

        public DropSink(DragHandler<T> handler) : base()
        {
            DragHandler = handler;
            base.CanDropBetween = true;
            base.CanDropOnBackground = true;
            base.CanDropOnItem = true;
            base.AcceptExternal = false;
            base.EnableFeedback = false;
            base.FeedbackColor = Color.Teal;
        }

        public virtual void Reset()
        {
            OverIndex = -1;
            OrderedList.Clear();
        }

        protected void SetOverIndex(int index)
        {
            if (index == OverIndex) return;
            OverIndex = index;

            //Console.WriteLine($"---- OverIndex: {OverIndex} OrderedList.Count: {OrderedList.Count}");
            foreach (T t in DragHandler.DragSource.Items)
                OrderedList.Remove(t);
            OrderedList.InsertRange(Math.Min(OverIndex, OrderedList.Count), DragHandler.DragSource.Items);

            // UpdateObjects(); // DONT UPDATE THE WHOLE LIST!! its laggy!!!
            // we need to update (and not refresh) to update the listviewitem position in the listview
            DragHandler.ListView.UpdateObjects((ICollection)DragHandler.DragSource.Items);
        }

    }

}
