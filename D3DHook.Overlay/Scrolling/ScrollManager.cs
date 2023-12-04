using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay.Scrolling
{

    public class ScrollManager
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
