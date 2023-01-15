using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{

    public partial class Element
    {

        #region Visible

        private bool _visible = true;
        private bool _visibleTotal = true;

        public virtual bool TotalVisible => _visibleTotal;

        public virtual bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                bool _visibleTotalLast = _visibleTotal;
                _visibleTotal = value && (Parent == null || Parent.TotalVisible);
                ResetVisible |= _visibleTotal != _visibleTotalLast;
            }
        }

        #endregion

        #region Alpha

        private float _alpha = 1f;
        private float _alphaTotal = 1f;

        public virtual float TotalAlpha => _alphaTotal;

        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;

                float _alphaTotalLast = _alphaTotal;
                _alphaTotal = value * (Parent != null ? Parent._alphaTotal : 1);
                ResetAlpha |= _alphaTotal != _alphaTotalLast;
            }
        }

        #endregion

        #region Scale

        private float _scale = 1f;
        private float _scaleTotal = 1f;
        private bool _scaleWithParent = true;

        public float TotalScale => _scaleTotal;

        public bool ScaleWithParent
        {
            get { return _scaleWithParent; }
            set
            {
                _scaleWithParent = value;
                Scale = _scale; // trigger update
            }
        }

        public virtual float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;

                float _scaleTotalLast = _scaleTotal;
                _scaleTotal = value * (Parent != null && _scaleWithParent ? Parent._scaleTotal : 1);
                ResetScale |= _scaleTotal != _scaleTotalLast;
            }
        }

        #endregion

        #region Width

        private float _width = 0f;
        private float _widthTotal = 0f;

        public float TotalWidth => _widthTotal;

        public virtual float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                float _widthTotalLast = _widthTotal;

                _widthTotal = value * _scaleTotal;
                if (_widthToParent && Parent != null)
                    _widthTotal = Parent._widthTotal + _widthTotal;

                if (_widthTotal < 0) _widthTotal = 0;
                ResetWidth |= _widthTotal != _widthTotalLast;
            }
        }

        #endregion

        #region Height

        private float _height = 0f;
        private float _heightTotal = 0f;

        public float TotalHeight => _heightTotal;

        public virtual float Height
        {
            get { return _height; }
            set
            {
                _height = value;
                float _heightTotalLast = _heightTotal;

                _heightTotal = value * _scaleTotal;
                if (_heightToParent && Parent != null)
                    _heightTotal = Parent._heightTotal + _heightTotal;

                if (_heightTotal < 0) _heightTotal = 0;
                ResetHeight |= _heightTotal != _heightTotalLast;
            }
        }

        #endregion

        #region WidthToParent

        private bool _widthToParent = false;

        public virtual bool WidthToParent
        {
            get { return _widthToParent; }
            set
            {
                _widthToParent = value;
                Width = _width; // just trigger
            }
        }

        #endregion

        #region HeightToParent

        private bool _heightToParent = false;

        public virtual bool HeightToParent
        {
            get { return _heightToParent; }
            set
            {
                _heightToParent = value;
                Height = _height; // just trigger
            }
        }

        #endregion

        #region X

        private float _x = 0f;
        private float _xTotal = 0f;

        public float TotalX => _xTotal;

        public virtual float X
        {
            get { return _x; }
            set
            {
                _x = value;

                float _xTotalLast = _xTotal;
                _xTotal = _x;
                if (Parent != null)
                {
                    _xTotal *= Parent._scaleTotal;

                    float addAnchorPointX = 0;
                    float addParentPointX = 0;
                    switch (_anchorPointX)
                    {
                        case Anchor.Left:
                            addAnchorPointX = -0;
                            break;
                        case Anchor.Center:
                            addAnchorPointX = -_widthTotal / 2;
                            break;
                        case Anchor.Right:
                            addAnchorPointX = -_widthTotal;
                            break;
                    }
                    switch (_parentPointX)
                    {
                        case Anchor.Left:
                            addParentPointX = 0;
                            break;
                        case Anchor.Center:
                            addParentPointX = Parent._widthTotal / 2;
                            break;
                        case Anchor.Right:
                            addParentPointX = Parent._widthTotal;
                            break;
                    }
                    _xTotal += Parent._xTotal + addParentPointX + addAnchorPointX;
                }
                ResetX |= _xTotal != _xTotalLast;
            }
        }

        #endregion

        #region Y

        private float _y = 0f;
        private float _yTotal = 0f;

        public float TotalY => _yTotal;

        public virtual float Y
        {
            get { return _y; }
            set
            {
                _y = value;

                float _yTotalLast = _yTotal;
                _yTotal = _y;
                if (Parent != null)
                {
                    _yTotal *= Parent._scaleTotal;

                    float addAnchorPointY = 0;
                    float addParentPointY = 0;
                    switch (_anchorPointY)
                    {
                        case Anchor.Top:
                            addAnchorPointY = -0;
                            break;
                        case Anchor.Center:
                            addAnchorPointY = -_heightTotal / 2;
                            break;
                        case Anchor.Bottom:
                            addAnchorPointY = -_heightTotal;
                            break;
                    }
                    switch (_parentPointY)
                    {
                        case Anchor.Top:
                            addParentPointY = 0;
                            break;
                        case Anchor.Center:
                            addParentPointY = Parent._heightTotal / 2;
                            break;
                        case Anchor.Bottom:
                            addParentPointY = Parent._heightTotal;
                            break;
                    }
                    _yTotal += Parent._yTotal + addParentPointY + addAnchorPointY;
                }
                ResetY |= _yTotal != _yTotalLast;
            }
        }

        #endregion

        #region Resetting (protected, private set)

        public bool ResetVisible { get; private set; } = true;

        public bool ResetAlpha { get; private set; } = true;

        public bool ResetScale { get; private set; } = true;

        public bool ResetWidth { get; private set; } = true;

        public bool ResetHeight { get; private set; } = true;

        public bool ResetX { get; private set; } = true;

        public bool ResetY { get; private set; } = true;

        #endregion

    }
}
