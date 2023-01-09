
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GDMultiStash.Common.Overlay.Animations
{
    public partial class Animator : AnimatorEventHandler
    {

        private float _duration;
        private float _durationPos = 0f;
        private float _durationTar = 0f;
        private float _curValue = 0f;

        private float _delay = 0f;
        private float _delayed = 0f;

        private bool _running = false;

        public Animator(float duration)
        {
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

        public float Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
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


        public void Reset(float value)
        {
            _durationTar = _duration * value;
            _durationPos = _durationTar;
            _curValue = _durationPos / _duration;
            _delayed = 0f;
            OnAnimate(_curValue);
        }

        public void Reset()
        {
            Reset(_curValue);
        }

        public virtual void OnAnimate(float p)
        {
            AnimationAnimate?.Invoke(this, new AnimatorAnimateEventArgs(_curValue));
        }

        public bool Animate(float elapsed)
        {
            float missing = _durationTar - _durationPos;
            if ((int)(missing * 1000000) == 0) missing = 0;
            if (missing == 0)
            {
                if (_running)
                {
                    _running = false;
                    AnimationEnd?.Invoke(this, EventArgs.Empty);
                }
                return false;
            }
            if (_delayed <= _delay)
            {
                _delayed += elapsed;
                return false;

            }
            if (!_running)
            {
                _running = true;
                AnimationStart?.Invoke(this, EventArgs.Empty);
            }

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
            OnAnimate(_curValue);
            return true;
        }

    }
}
