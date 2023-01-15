using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public class ScrollBarElement : Element, IScrollable
    {

        public virtual float ScrollBarMinWidth => 0f;
        public virtual float ScrollBarMinHeight => 0f;
        public virtual ScrollOrientation Orientation => ScrollOrientation.None;

        private readonly Element _scrollBar;
        private bool _rearrange = true;

        public Element ScrollBar => _scrollBar;

        public ScrollHandler ScrollHandler { get; private set; }

        public ScrollBarElement()
        {
            _scrollBar = new Element()
            {
                ScaleWithParent = true,
            };
            AddChild(_scrollBar);
            ScrollHandler = new ScrollHandler(Orientation);
            ScrollHandler.ScrollPositionXChanged += delegate { _rearrange = true; };
            ScrollHandler.ScrollPositionYChanged += delegate { _rearrange = true; };
            ScrollHandler.TotalUnitsXChanged += delegate { _rearrange = true; };
            ScrollHandler.TotalUnitsYChanged += delegate { _rearrange = true; };
            ScrollHandler.VisibleUnitsXChanged += delegate { _rearrange = true; };
            ScrollHandler.VisibleUnitsYChanged += delegate { _rearrange = true; };
        }

        private float _pixelPerUnitX;
        private float _pixelPerUnitY;

        public override float Width
        {
            get => base.Width;
            set
            {
                _rearrange |= base.Width != value;
                base.Width = value;
            }
        }

        public override float Height
        {
            get => base.Height;
            set
            {
                _rearrange |= base.Height != value;
                base.Height = value;
            }
        }

        private void TriggerScrollingEvent()
        {
            /*
            Scrolling?.Invoke(this, new ScrollingEventArgs
            {
                X = ScrollHandler.ScrollPositionX,
                Y = ScrollHandler.ScrollPositionY,
            });
            */
        }

        public void Rearrange()
        {
            bool hide = true;
            float _unscaledWidth = TotalWidth / TotalScale;
            float _unscaledHeight = TotalHeight / TotalScale;
            if (ScrollHandler.VisibleUnitsX > 0 && ScrollHandler.TotalUnitsX > ScrollHandler.VisibleUnitsX)
            {
                float _scrollUnitsAspectX = (float)ScrollHandler.VisibleUnitsX / ScrollHandler.TotalUnitsX;
                _scrollBar.Width = Math.Max(ScrollBarMinWidth, _unscaledWidth * _scrollUnitsAspectX);
                _pixelPerUnitX = (_unscaledWidth - _scrollBar.Width) / (ScrollHandler.TotalUnitsX - ScrollHandler.VisibleUnitsX);
                _scrollBar.X = _pixelPerUnitX * ScrollHandler.ScrollPositionX;
                hide = false;
            }
            else
            {
                _scrollBar.Width = _unscaledWidth;
                _scrollBar.X = 0;
            }
            if (ScrollHandler.VisibleUnitsY > 0 && ScrollHandler.TotalUnitsY > ScrollHandler.VisibleUnitsY)
            {
                float _scrollUnitsAspectY = (float)ScrollHandler.VisibleUnitsY / ScrollHandler.TotalUnitsY;
                _scrollBar.Height = Math.Max(ScrollBarMinHeight, _unscaledHeight * _scrollUnitsAspectY);
                _pixelPerUnitY = (_unscaledHeight - _scrollBar.Height) / (ScrollHandler.TotalUnitsY - ScrollHandler.VisibleUnitsY);
                _scrollBar.Y = _pixelPerUnitY * ScrollHandler.ScrollPositionY;
                hide = false;
            }
            else
            {
                _scrollBar.Height = _unscaledHeight;
                _scrollBar.Y = 0;
            }
            _scrollBar.Visible = !hide;
            TriggerScrollingEvent();
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_rearrange)
            {
                _rearrange = false;
                Rearrange();
            }
        }

        #region Mouse

        public class ScrollingEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        public event EventHandler<EventArgs> ScrollingStart;
        public event EventHandler<EventArgs> ScrollingEnd;

        //public delegate void ScrollingEventHandler(object sender, ScrollingEventArgs e);
        //public event ScrollingEventHandler Scrolling;

        private bool _mouseDown = false;

        private float _tempMouseStartX = 0;
        private float _tempMouseStartY = 0;

        private int _tempUnitsStartX = 0;
        private int _tempUnitsStartY = 0;

        private int _tempUnitsX = 0;
        private int _tempUnitsY = 0;

        public bool IsScrolling => _mouseDown;

        public override bool CheckMouseDown(int x, int y)
        {
            bool hit = base.CheckMouseDown(x, y);
            if (!hit) return false;

            _mouseDown = true;
            ScrollingStart?.Invoke(this, EventArgs.Empty);
            _tempMouseStartX = x;
            _tempMouseStartY = y;

            _tempUnitsStartX = ScrollHandler.ScrollPositionX;
            _tempUnitsStartY = ScrollHandler.ScrollPositionY;

            _tempUnitsX = 0;
            _tempUnitsY = 0;

            return true;
        }

        public override bool CheckMouseUp(int x, int y)
        {
            if (_mouseDown)
            {
                _mouseDown = false;
                ScrollingEnd?.Invoke(this, EventArgs.Empty);
            }
            return base.CheckMouseUp(x, y);
        }

        public override bool CheckMouseMove(int x, int y)
        {
            if (_mouseDown)
            {

                float diffX = x - _tempMouseStartX;
                float diffY = y - _tempMouseStartY;

                int movedUnitsX = (int)(diffX / _pixelPerUnitX);
                int movedUnitsY = (int)(diffY / _pixelPerUnitY);

                bool movedX = ScrollHandler.VisibleUnitsX > 0 && movedUnitsX != _tempUnitsX;
                bool movedY = ScrollHandler.VisibleUnitsY > 0 && movedUnitsY != _tempUnitsY;

                _tempUnitsX = movedUnitsX;
                _tempUnitsY = movedUnitsY;

                if (movedX || movedY)
                {
                    if (movedX)
                        ScrollHandler.ScrollPositionX = _tempUnitsStartX + movedUnitsX;

                    if (movedY)
                        ScrollHandler.ScrollPositionY = _tempUnitsStartY + movedUnitsY;

                    TriggerScrollingEvent();
                }
            }
            return base.CheckMouseMove(x, y);
        }

        #endregion

    }
}
