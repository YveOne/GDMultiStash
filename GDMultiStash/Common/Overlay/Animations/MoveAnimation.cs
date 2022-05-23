using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay.Animations
{
    public class MoveAnimation : Animation
    {

        private readonly Element _element;

        private float _minX = 0f;
        private float _maxX = 0f;
        private float _rangeX = 0f;

        private float _minY = 0f;
        private float _maxY = 0f;
        private float _rangeY = 0f;

        public MoveAnimation(Element element, Utils.Easing.EaserDelegate easer, float duration) : base(easer, duration)
        {
            _element = element;
        }

        public float MinX
        {
            get { return _minX; }
            set
            {
                _minX = value;
                _rangeX = _maxX - _minX;
            }
        }

        public float MaxX
        {
            get { return _maxX; }
            set
            {
                _maxX = value;
                _rangeX = _maxX - _minX;
            }
        }

        public float MinY
        {
            get { return _minY; }
            set
            {
                _minY = value;
                _rangeY = _maxY - _minY;
            }
        }

        public float MaxY
        {
            get { return _maxY; }
            set
            {
                _maxY = value;
                _rangeY = _maxY - _minY;
            }
        }

        public override void OnAnimate(float p)
        {
            _element.X = _minX + (_rangeX * p);
            _element.Y = _minY + (_rangeY * p);
        }

    }
}
