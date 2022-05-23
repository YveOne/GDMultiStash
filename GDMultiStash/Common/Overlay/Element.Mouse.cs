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



        public bool MouseOver => _mouseOver;
        private bool _mouseOver = false;

        public bool MouseCheckChildren
        {
            get => _checkChildren;
            set => _checkChildren = value;
        }
        private bool _checkChildren = true;

        public bool MouseCheckNeedBaseHit
        {
            get => _needBaseHit;
            set => _needBaseHit = value;
        }
        private bool _needBaseHit = true;



        public virtual bool CheckMouseMove(int x, int y)
        {
            if (!_visible) return false;
            bool hit = CheckHitRect(x, y);
            bool _wasOver = false;
            if (_mouseOver)
            {
                if (!hit)
                {
                    _wasOver = true;
                    _mouseOver = false;
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (hit)
                {
                    _mouseOver = true;
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                }
            }
            if (_checkChildren && (hit || !_needBaseHit || _wasOver))
                foreach (Element child in _children)
                    hit |= child.CheckMouseMove(x, y);
            return hit;
        }

        private const long MouseClickTickCount = 500;
        private long _mouseClickStartTickCount = 0;

        public virtual bool CheckMouseDown(int x, int y)
        {
            if (!_visible) return false;
            bool hit = CheckHitRect(x, y);
            if (hit)
            {
                _mouseClickStartTickCount = Environment.TickCount;
                MouseDown?.Invoke(this, EventArgs.Empty);
            }
            if (_checkChildren && (hit || !_needBaseHit))
                foreach (Element child in _children)
                    hit |= child.CheckMouseDown(x, y);
            return hit;
        }

        public virtual bool CheckMouseUp(int x, int y)
        {
            if (!_visible) return false;
            bool hit = CheckHitRect(x, y);
            if (hit)
            {
                MouseUp?.Invoke(this, EventArgs.Empty);
                if (Environment.TickCount - _mouseClickStartTickCount <= MouseClickTickCount)
                {
                    MouseClick?.Invoke(this, EventArgs.Empty);
                }
            }
            if (_checkChildren && (hit || !_needBaseHit))
                foreach (Element child in _children)
                    hit |= child.CheckMouseUp(x, y);
            return hit;
        }




        protected bool CheckHitRect(int x, int y)
        {
            float thisL = _xTotal - 1;
            float thisT = _yTotal - 1;
            float thisR = thisL + _widthTotal + 1;
            float thisB = thisT + _heightTotal + 1;
            if (x < thisL) return false;
            if (x > thisR) return false;
            if (y < thisT) return false;
            if (y > thisB) return false;
            return true;
        }

    }
}
