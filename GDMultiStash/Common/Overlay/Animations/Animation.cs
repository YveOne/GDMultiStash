using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay.Animations
{
    public class Animation
    {

        private readonly Utils.Easing.EaserDelegate _easer;
        private float _duration;
        private float _durationPos = 0f;
        private float _durationTar = 0f;
        private float _curValue = 0f;

        private float _delay = 0f;
        private float _delayed = 0f;

        public Animation(Utils.Easing.EaserDelegate easer, float duration)
        {
            _easer = easer;
            _duration = duration;
        }

        public float Duration
        {
            get { return _duration; }
            set
            {
                _durationPos = _durationPos / _duration * value;
                _durationTar = _durationTar / _duration * value;
                _duration = value;
            }
        }

        public float Value
        {
            get { return _curValue; }
            set
            {
                _durationTar = _duration * value;
                _delayed = 0f;
            }
        }

        public float Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
            }
        }

        public void Reset(float value)
        {
            _durationTar = _duration * value;
            _durationPos = _durationTar;
            _curValue = _durationPos / _duration;
            _delayed = 0f;
            OnAnimate(_easer(_curValue));
        }

        public void Reset()
        {
            Reset(_curValue);
        }

        public virtual void OnAnimate(float p)
        {
        }

        public bool Animate(float elapsed)
        {
            float missing = _durationTar - _durationPos;
            if (missing == 0) return false;

            if (_delayed >= _delay)
            {

                float add = elapsed;
                if (missing > 0)
                {
                    // anim in
                    if (add > missing) add = missing;
                }
                else
                {
                    // anim out
                    add = -add;
                    if (add < missing) add = missing;
                }
                _durationPos += add;
                _curValue = _durationPos / _duration;

            }

            OnAnimate(_easer(_curValue));
            _delayed += elapsed;
            return true;
        }

    }
}
