using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public partial class Element
    {

        private Anchor _anchorPoint = Anchor.TopLeft;
        private Anchor _anchorPointX = Anchor.Left;
        private Anchor _anchorPointY = Anchor.Top;

        private Anchor _parentPoint = Anchor.TopLeft;
        private Anchor _parentPointX = Anchor.Left;
        private Anchor _parentPointY = Anchor.Top;

        public Anchor AnchorPoint
        {
            get { return _anchorPoint; }
            set
            {
                _anchorPoint = value;
                ParentPoint = value;
                switch (value)
                {
                    case Anchor.TopLeft:
                    case Anchor.Left:
                    case Anchor.BottomLeft:
                        _anchorPointX = Anchor.Left;
                        break;
                    case Anchor.Top:
                    case Anchor.Center:
                    case Anchor.Bottom:
                        _anchorPointX = Anchor.Center;
                        break;
                    case Anchor.TopRight:
                    case Anchor.Right:
                    case Anchor.BottomRight:
                        _anchorPointX = Anchor.Right;
                        break;
                }
                switch (value)
                {
                    case Anchor.TopLeft:
                    case Anchor.Top:
                    case Anchor.TopRight:
                        _anchorPointY = Anchor.Top;
                        break;
                    case Anchor.Left:
                    case Anchor.Center:
                    case Anchor.Right:
                        _anchorPointY = Anchor.Center;
                        break;
                    case Anchor.BottomLeft:
                    case Anchor.Bottom:
                    case Anchor.BottomRight:
                        _anchorPointY = Anchor.Bottom;
                        break;
                }
                _resetX = true;
                _resetY = true;
            }
        }

        public Anchor ParentPoint
        {
            get { return _parentPoint; }
            set
            {
                _parentPoint = value;
                switch (value)
                {
                    case Anchor.TopLeft:
                    case Anchor.Left:
                    case Anchor.BottomLeft:
                        _parentPointX = Anchor.Left;
                        break;
                    case Anchor.Top:
                    case Anchor.Center:
                    case Anchor.Bottom:
                        _parentPointX = Anchor.Center;
                        break;
                    case Anchor.TopRight:
                    case Anchor.Right:
                    case Anchor.BottomRight:
                        _parentPointX = Anchor.Right;
                        break;
                }
                switch (value)
                {
                    case Anchor.TopLeft:
                    case Anchor.Top:
                    case Anchor.TopRight:
                        _parentPointY = Anchor.Top;
                        break;
                    case Anchor.Left:
                    case Anchor.Center:
                    case Anchor.Right:
                        _parentPointY = Anchor.Center;
                        break;
                    case Anchor.BottomLeft:
                    case Anchor.Bottom:
                    case Anchor.BottomRight:
                        _parentPointY = Anchor.Bottom;
                        break;
                }
                _resetX = true;
                _resetY = true;
            }
        }

    }
}
