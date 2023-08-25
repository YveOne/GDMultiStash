using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay
{

    [Flags]
    public enum ScrollOrientation
    {
        None,
        Vertical,
        Horizontal,
        Both
    }

    public class ScrollHandler
    {

        ScrollOrientation Orientation => _orientation;
        private readonly ScrollOrientation _orientation;

        public ScrollHandler(ScrollOrientation orientation)
        {
            _orientation = orientation;
        }

        #region VisibleUnits

        public event EventHandler<EventArgs> VisibleUnitsXChanged;
        public event EventHandler<EventArgs> VisibleUnitsYChanged;

        private int _visibleUnitsX = 0;
        private int _visibleUnitsY = 0;

        public int VisibleUnitsX
        {
            get => _visibleUnitsX;
            set
            {
                bool changed = value != _visibleUnitsX;
                _visibleUnitsX = value;
                if (changed)
                {
                    VisibleUnitsXChanged?.Invoke(this, EventArgs.Empty);
                    ScrollPositionX = _scrollPositionX;
                }
            }
        }

        public int VisibleUnitsY
        {
            get => _visibleUnitsY;
            set
            {
                bool changed = value != _visibleUnitsY;
                _visibleUnitsY = value;
                if (changed)
                {
                    VisibleUnitsYChanged?.Invoke(this, EventArgs.Empty);
                    ScrollPositionY = _scrollPositionY;
                }
            }
        }

        #endregion

        #region TotalUnits

        public event EventHandler<EventArgs> TotalUnitsXChanged;
        public event EventHandler<EventArgs> TotalUnitsYChanged;

        private int _totalUnitsX = 0;
        private int _totalUnitsY = 0;

        public int TotalUnitsX
        {
            get => _totalUnitsX;
            set
            {
                bool changed = value != _totalUnitsX;
                _totalUnitsX = value;
                if (changed)
                {
                    TotalUnitsXChanged?.Invoke(this, EventArgs.Empty);
                    ScrollPositionX = _scrollPositionX;
                }
            }
        }

        public int TotalUnitsY
        {
            get => _totalUnitsY;
            set
            {
                bool changed = value != _totalUnitsY;
                _totalUnitsY = value;
                if (changed)
                {
                    TotalUnitsYChanged?.Invoke(this, EventArgs.Empty);
                    ScrollPositionY = _scrollPositionY;
                }
            }
        }

        #endregion

        #region ScrollPosition

        public event EventHandler<EventArgs> ScrollPositionXChanged;
        public event EventHandler<EventArgs> ScrollPositionYChanged;

        private int _scrollPositionX = 0;
        private int _scrollPositionY = 0;

        public int ScrollPositionX
        {
            get => _scrollPositionX;
            set
            {
                int max = _totalUnitsX - _visibleUnitsX;
                if (value > max) value = max;
                if (value < 0) value = 0;
                if (_visibleUnitsX >= _totalUnitsX) value = 0;
                bool changed = value != _scrollPositionX;
                _scrollPositionX = value;
                if (changed) ScrollPositionXChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int ScrollPositionY
        {
            get => _scrollPositionY;
            set
            {
                int max = _totalUnitsY - _visibleUnitsY;
                if (value > max) value = max;
                if (value < 0) value = 0;
                if (_visibleUnitsY >= _totalUnitsY) value = 0;
                bool changed = value != _scrollPositionY;
                _scrollPositionY = value;
                if (changed) ScrollPositionYChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

    }

    public interface IScrollable
    {
        ScrollHandler ScrollHandler { get; }
    }

    internal class ScrollManager
    {

        private readonly IScrollable _target;
        private readonly IEnumerable<IScrollable> _bars;

        public ScrollManager(IScrollable target, IEnumerable<IScrollable> bars)
        {
            _target = target;
            _bars = bars;
            BindElements();
        }

        public ScrollManager(IScrollable target, params IScrollable[] bars)
        {
            _target = target;
            _bars = bars;
            BindElements();
        }

        private void BindElements()
        {
            _target.ScrollHandler.TotalUnitsXChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.TotalUnitsX = _target.ScrollHandler.TotalUnitsX;
            };
            _target.ScrollHandler.TotalUnitsYChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.TotalUnitsY = _target.ScrollHandler.TotalUnitsY;
            };
            _target.ScrollHandler.ScrollPositionXChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.ScrollPositionX = _target.ScrollHandler.ScrollPositionX;
            };
            _target.ScrollHandler.ScrollPositionYChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.ScrollPositionY = _target.ScrollHandler.ScrollPositionY;
            };
            _target.ScrollHandler.VisibleUnitsXChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.VisibleUnitsX = _target.ScrollHandler.VisibleUnitsX;
            };
            _target.ScrollHandler.VisibleUnitsYChanged += delegate {
                foreach (var b in _bars)
                    b.ScrollHandler.VisibleUnitsY = _target.ScrollHandler.VisibleUnitsY;
            };
            foreach (var b in _bars)
            {
                b.ScrollHandler.ScrollPositionXChanged += delegate {
                    _target.ScrollHandler.ScrollPositionX = b.ScrollHandler.ScrollPositionX;
                };
                b.ScrollHandler.ScrollPositionYChanged += delegate {
                    _target.ScrollHandler.ScrollPositionY = b.ScrollHandler.ScrollPositionY;
                };
            }
        }

    }
}
