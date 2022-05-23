using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay.Animations
{
    public class FadeAnimation : Animation
    {

        private readonly Element _element;

        private float _minAlpha = 0f;
        private float _maxAlpha = 1f;
        private float _range = 1f;

        public FadeAnimation(Element element, Utils.Easing.EaserDelegate easer, float duration) : base(easer, duration)
        {
            _element = element;
        }

        public float MinAlpha
        {
            get { return _minAlpha; }
            set {
                _minAlpha = value;
                if (_minAlpha < 0f) _minAlpha = 0f;
                _range = _maxAlpha - _minAlpha;
            }
        }

        public float MaxAlpha
        {
            get { return _maxAlpha; }
            set { 
                _maxAlpha = value;
                if (_maxAlpha > 1f) _maxAlpha = 1f;
                _range = _maxAlpha - _minAlpha;
            }
        }

        public override void OnAnimate(float p)
        {
            _element.Alpha = _minAlpha + (_range * p);
        }

    }
}
