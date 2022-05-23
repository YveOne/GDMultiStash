using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public class ScrollBarElement : Element
    {

        protected virtual bool ScrollHorizontal => false;
        protected virtual bool ScrollVertical => false;
        protected virtual float ScrollAreaWidth => 0f;
        protected virtual float ScrollAreaHeight => 0f;
        protected virtual int ScrollAreaWidthUnits => 0;
        protected virtual int ScrollAreaHeightUnits => 0;

        private int _scrollUnitsX = 0;
        private int _scrollUnitsY = 0;

        private int _scrollWidthUnits = 0;
        private int _scrollHeightUnits = 0;

        private float _scrollUnitsAspectX = 0;
        private float _scrollUnitsAspectY = 0;

        private readonly Element _scrollBar;

        private float _calcWidthPerUnitX = 0;
        private float _calcHeightPerUnitY = 0;

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
                int max = _scrollWidthUnits - ScrollAreaWidthUnits;
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
                int max = _scrollHeightUnits - ScrollAreaHeightUnits;
                if (value > max) value = max;
                if (value < 0) value = 0;
                _scrollUnitsY = value;
                _rearrange = true;
            }
        }

        public int ScrollWidthUnits
        {
            get => _scrollWidthUnits;
            set
            {
                _scrollWidthUnits = value;

                _calcWidthPerUnitX = ScrollAreaWidthUnits != 0 ? ScrollAreaWidth / value : 0;
                _scrollUnitsAspectX = value != 0 ? (float)ScrollAreaWidthUnits / (float)value : 0;
                if (_scrollUnitsAspectX > 1) _scrollUnitsAspectX = 1;

                _rearrange = true;
            }
        }

        public int ScrollHeightUnits
        {
            get => _scrollHeightUnits;
            set
            {
                _scrollHeightUnits = value;

                _calcHeightPerUnitY = value != 0 ? ScrollAreaHeight / value : 0;
                _scrollUnitsAspectY = value != 0 ? (float)ScrollAreaHeightUnits / (float)value : 0;
                if (_scrollUnitsAspectY > 1) _scrollUnitsAspectY = 1;

                _rearrange = true;
            }
        }





        public void Rearrange()
        {
            if (ScrollHorizontal)
            {
                _scrollBar.Width = ScrollAreaWidth * _scrollUnitsAspectX;
                _scrollBar.X = _calcWidthPerUnitX * _scrollUnitsX;
            }
            else
            {
                _scrollBar.Width = ScrollAreaWidth;
                _scrollBar.X = 0;
            }
            if (ScrollVertical)
            {
                _scrollBar.Height = ScrollAreaHeight * _scrollUnitsAspectY;
                _scrollBar.Y = _calcHeightPerUnitY * _scrollUnitsY;
            }
            else
            {
                _scrollBar.Height = ScrollAreaHeight;
                _scrollBar.Y = 0;
            }
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

                int movedUnitsX = (int)(diffX / _calcWidthPerUnitX);
                int movedUnitsY = (int)(diffY / _calcHeightPerUnitY);

                bool movedX = ScrollHorizontal && movedUnitsX != _tempUnitsX;
                bool movedY = ScrollVertical && movedUnitsY != _tempUnitsY;

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
