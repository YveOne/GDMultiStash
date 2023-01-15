using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public partial class Element
    {

        public delegate void MouseEnterEventHandler(object sender, EventArgs e);
        public event MouseEnterEventHandler MouseEnter;

        public delegate void MouseLeaveEventHandler(object sender, EventArgs e);
        public event MouseLeaveEventHandler MouseLeave;

        public delegate void MouseDownEventHandler(object sender, EventArgs e);
        public event MouseDownEventHandler MouseDown;

        public delegate void MouseUpEventHandler(object sender, EventArgs e);
        public event MouseUpEventHandler MouseUp;

        public delegate void MouseClickEventHandler(object sender, EventArgs e);
        public event MouseClickEventHandler MouseClick;

        public delegate void MouseWheelEventHandler(object sender, MouseWheelEventArgs e);
        public event MouseWheelEventHandler MouseWheel;

        public class MouseWheelEventArgs : EventArgs
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Delta { get; private set; }
            public MouseWheelEventArgs(int x, int y, int d) : base()
            {
                this.X = x;
                this.Y = y;
                this.Delta = d;
            }
        }

        private bool _isMouseOver = false;

        public bool IsMouseOver => _isMouseOver;

        public bool MouseCheckChildren { get; set; } = true;
        public bool MouseCheckNeedBaseHit { get; set; } = false;

        public bool EventsEnabled = true;

        public virtual bool CheckMouseMove(int x, int y)
        {
            if (!_visible) return false;
            if (!EventsEnabled) return false;
            bool hit = CheckHitRect(x, y);
            bool _wasOver = false;
            if (_isMouseOver)
            {
                if (!hit)
                {
                    _wasOver = true;
                    _isMouseOver = false;
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (hit)
                {
                    _isMouseOver = true;
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                }
            }
            if (MouseCheckChildren && (hit || !MouseCheckNeedBaseHit || _wasOver))
                foreach (Element child in Children)
                    hit |= child.CheckMouseMove(x, y);
            return hit;
        }

        private const long MouseClickTickCount = 500;
        private long _mouseClickStartTickCount = 0;

        public virtual bool CheckMouseDown(int x, int y)
        {
            if (!_visible) return false;
            if (!EventsEnabled) return false;
            bool hit = CheckHitRect(x, y);
            if (hit)
            {
                _mouseClickStartTickCount = Environment.TickCount;
                MouseDown?.Invoke(this, EventArgs.Empty);
            }
            if (MouseCheckChildren && (hit || !MouseCheckNeedBaseHit))
                foreach (Element child in Children)
                    hit |= child.CheckMouseDown(x, y);
            return hit;
        }

        public virtual bool CheckMouseUp(int x, int y)
        {
            if (!_visible) return false;
            if (!EventsEnabled) return false;
            bool hit = CheckHitRect(x, y);
            if (hit)
            {
                MouseUp?.Invoke(this, EventArgs.Empty);
                if (Environment.TickCount - _mouseClickStartTickCount <= MouseClickTickCount)
                {
                    MouseClick?.Invoke(this, EventArgs.Empty);
                }
            }
            if (MouseCheckChildren && (hit || !MouseCheckNeedBaseHit))
                foreach (Element child in Children)
                    hit |= child.CheckMouseUp(x, y);
            return hit;
        }

        public virtual bool OnMouseWheel(int x, int y, int delta)
        {
            if (!_visible) return false;
            if (!EventsEnabled) return false;
            bool hit = CheckHitRect(x, y);
            if (hit)
            {
                MouseWheel?.Invoke(this, new MouseWheelEventArgs(x,y,delta));
            }
            if (MouseCheckChildren && (hit || !MouseCheckNeedBaseHit))
                foreach (Element child in Children)
                    hit |= child.OnMouseWheel(x, y, delta);
            return hit;
        }




        protected bool CheckHitRect(int x, int y)
        {
            float thisL = _xTotal;
            float thisT = _yTotal;
            float thisR = thisL + _widthTotal;
            float thisB = thisT + _heightTotal;
            if (x < thisL) return false;
            if (x > thisR) return false;
            if (y < thisT) return false;
            if (y > thisB) return false;
            return true;
        }

    }
}
