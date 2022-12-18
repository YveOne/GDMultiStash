using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay.Animations
{
    public class AnimationValue
    {
        private Easing.EaserDelegate _easer = Easing.Linear();
        private float _min = 0f;
        private float _max = 1f;
        private float _range = 1f;

        public AnimationValue() : base()
        {
        }

        public float Min
        {
            get { return _min; }
            set
            {
                _min = value;
                _range = _max - _min;
            }
        }

        public float Max
        {
            get { return _max; }
            set
            {
                _max = value;
                _range = _max - _min;
            }
        }

        public Easing.EaserDelegate Easer
        {
            get { return _easer; }
            set
            {
                _easer = value;
            }
        }

        public float Convert(float p)
        {
            return _min + (_range * _easer(p));
        }

    }
}
