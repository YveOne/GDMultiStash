using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public class ScrollBarElement : Element
    {

        protected virtual float ScrollAreaWidth => 0f;
        protected virtual float ScrollAreaHeight => 0f;
        protected virtual float ScrollBarMinWidth => 0f;
        protected virtual float ScrollBarMinHeight => 0f;
        protected virtual int ScrollAreaWidthUnits => 0;
        protected virtual int ScrollAreaHeightUnits => 0;

        private int _scrollUnitsX = 0;
        private int _scrollUnitsY = 0;

        private int _unitsX = 0;
        private int _unitsY = 0;

        private float _scrollUnitsAspectX = 0;
        private float _scrollUnitsAspectY = 0;

        private readonly Element _scrollBar;

        private float _pixelPerUnitX = 0;
        private float _pixelPerUnitY = 0;

        private bool _rearrange = true;

        public ScrollBarElement()
        {
            _scrollBar = new Element();
            AddChild(_scrollBar);
        }

        public Element ScrollBar => _scrollBar;





        public int ScrollUnitsX
        {
            get => _scrollUnitsX;
            set
            {
                int max = _unitsX - ScrollAreaWidthUnits;
                if (value > max) value = max;
                if (value < 0) value = 0;
                _scrollUnitsX = value;
                _rearrange = true;
            }
        }

        public int ScrollUnitsY
        {
            get => _scrollUnitsY;
            set
            {
                int max = _unitsY - ScrollAreaHeightUnits;
                if (value > max) value = max;
                if (value < 0) value = 0;
                _scrollUnitsY = value;
                _rearrange = true;
            }
        }

        public int UnitsX
        {
            get => _unitsX;
            set
            {
                _unitsX = value;
                _rearrange = true;
            }
        }

        public int UnitsY
        {
            get => _unitsY;
            set
            {
                _unitsY = value;
                _rearrange = true;
            }
        }

        public void Rearrange()
        {
            bool hide = true;
            if (ScrollAreaWidthUnits > 0 && _unitsX > ScrollAreaWidthUnits)
            {
                _scrollUnitsAspectX = (float)ScrollAreaWidthUnits / _unitsX;
                _scrollBar.Width = Math.Max(ScrollBarMinWidth, ScrollAreaWidth * _scrollUnitsAspectX);
                _pixelPerUnitX = (ScrollAreaWidth - _scrollBar.Width) / (_unitsX - ScrollAreaWidthUnits);
                _scrollBar.X = _pixelPerUnitX * _scrollUnitsX;
                hide = false;
            }
            else
            {
                _scrollBar.Width = ScrollAreaWidth;
                _scrollBar.X = 0;
            }
            if (ScrollAreaHeightUnits > 0 && _unitsY > ScrollAreaHeightUnits)
            {

                _scrollUnitsAspectY = (float)ScrollAreaHeightUnits / _unitsY;
                _scrollBar.Height = Math.Max(ScrollBarMinHeight, ScrollAreaHeight * _scrollUnitsAspectY);
                _pixelPerUnitY = (ScrollAreaHeight - _scrollBar.Height) / (_unitsY - ScrollAreaHeightUnits);
                _scrollBar.Y = _pixelPerUnitY * _scrollUnitsY;
                hide = false;
            }
            else
            {
                _scrollBar.Height = ScrollAreaHeight;
                _scrollBar.Y = 0;
            }
            _scrollBar.Visible = !hide;
        }

        public override void Draw(float elapsed)
        {
            base.Draw(elapsed);
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

        public delegate void ScrollingStartEventHandler(object sender, EventArgs e);
        public event ScrollingStartEventHandler ScrollingStart;

        public delegate void ScrollingEndEventHandler(object sender, EventArgs e);
        public event ScrollingEndEventHandler ScrollingEnd;

        public delegate void ScrollingEventHandler(object sender, ScrollingEventArgs e);
        public event ScrollingEventHandler Scrolling;

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

            _tempUnitsStartX = ScrollUnitsX;
            _tempUnitsStartY = ScrollUnitsY;

            _tempUnitsX = 0;
            _tempUnitsY = 0;

            return true;
        }

        public override bool CheckMouseUp(int x, int y)
        {
            _mouseDown = false;
            ScrollingEnd?.Invoke(this, EventArgs.Empty);
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

                bool movedX = ScrollAreaWidthUnits > 0 && movedUnitsX != _tempUnitsX;
                bool movedY = ScrollAreaHeightUnits > 0 && movedUnitsY != _tempUnitsY;

                _tempUnitsX = movedUnitsX;
                _tempUnitsY = movedUnitsY;

                if (movedX || movedY)
                {
                    if (movedX)
                        ScrollUnitsX = _tempUnitsStartX + movedUnitsX;

                    if (movedY)
                        ScrollUnitsY = _tempUnitsStartY + movedUnitsY;

                    Scrolling?.Invoke(this, new ScrollingEventArgs
                    {
                        X = -ScrollUnitsX,
                        Y = -ScrollUnitsY,
                    });
                }
            }
            return base.CheckMouseMove(x, y);
        }

        #endregion



    }
}
