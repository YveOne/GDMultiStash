using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Easing
    {

        //https://github.com/d3/d3-ease

        public delegate float EaserDelegate(float t);

        private const float PI = (float)Math.PI;
        private const float PIH = PI / 2;
        private const float PI2 = PI * 2;

        private delegate float Delegate1(float v);

        private static readonly Delegate1 tpmt = (float v) => {
            return ((float)Math.Pow(2f, -10f * v) - 0.0009765625f) * 1.0009775171065494f;
        }; 

        #region Linear

        public static EaserDelegate Linear()
        {
            return new EaserDelegate((t) => {
                return t;
            });
        }

        #endregion

        #region Poly

        public static EaserDelegate PolyIn(float e = 3)
        {
            return new EaserDelegate((t) => {
                return (float)Math.Pow(t, e);
            });
        }

        public static EaserDelegate PolyOut(float e = 3)
        {
            return new EaserDelegate((t) => {
                return 1f - (float)Math.Pow(1f - t, e);
            });
        }

        public static EaserDelegate PolyInOut(float e = 3)
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return (float)Math.Pow(t, e) / 2;
                }
                return (2f - (float)Math.Pow(2f - t, e)) / 2f;
            });
        }

        #endregion

        #region Quad

        public static EaserDelegate QuadIn()
        {
            return new EaserDelegate((t) => {
                return t * t;
            });
        }

        public static EaserDelegate QuadOut()
        {
            return new EaserDelegate((t) => {
                return t * (2f - t);
            });
        }

        public static EaserDelegate QuadInOut()
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return (t * t) / 2f;
                }
                t -= 1f;
                return (t * (2f - t) + 1f) / 2f;
            });
        }

        #endregion

        #region Cubic

        public static EaserDelegate CubicIn()
        {
            return new EaserDelegate((t) => {
                return t * t * t;
            });
        }

        public static EaserDelegate CubicOut()
        {
            return new EaserDelegate((t) => {
                t -= 1f;
                return t * t * t + 1f;
            });
        }

        public static EaserDelegate CubicInOut()
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return (t * t * t) / 2f;
                }
                t -= 2f;
                return (t * t * t + 2f) / 2f;
            });
        }

        #endregion

        #region Sin

        public static EaserDelegate SinIn()
        {
            return new EaserDelegate((t) => {
                if (t == 1f)
                {
                    return t;
                }
                return 1f - (float)Math.Cos(t * PIH);
            });
        }

        public static EaserDelegate SinOut()
        {
            return new EaserDelegate((t) => {
                return (float)Math.Sin(t * PIH);
            });
        }

        public static EaserDelegate SinInOut()
        {
            return new EaserDelegate((t) => {
                return (1f - (float)Math.Cos(PI * t)) / 2f;
            });
        }

        #endregion

        #region Exp

        public static EaserDelegate ExpIn()
        {
            return new EaserDelegate((t) => {
                return tpmt(1f - t);
            });
        }

        public static EaserDelegate ExpOut()
        {
            return new EaserDelegate((t) => {
                return 1f - tpmt(t);
            });
        }

        public static EaserDelegate ExpInOut()
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return tpmt(1f - t) / 2f;
                }
                return (2f - tpmt(t - 1f)) / 2f;
            });
        }

        #endregion

        #region Circle

        public static EaserDelegate CircleIn()
        {
            return new EaserDelegate((t) => {
                return 1f - (float)Math.Sqrt(1f - t * t);
            });
        }

        public static EaserDelegate CircleOut()
        {
            return new EaserDelegate((t) => {
                t -= 1f;
                return (float)Math.Sqrt(1f - t * t);
            });
        }

        public static EaserDelegate CircleInOut()
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return (1f - (float)Math.Sqrt(1f - t * t)) / 2f;
                }
                t -= 2f;
                return ((float)Math.Sqrt(1f - t * t) + 1f) / 2f;
            });
        }

        #endregion

        #region Elastic

        public static EaserDelegate ElasticIn(float amplitude = 1, float period = 0.3f)
        {
            amplitude = Math.Max(1f, amplitude);
            period /= PI2;
            float s = (float)Math.Asin(1f / amplitude) * period;
            return new EaserDelegate((t) => {
                return amplitude * tpmt(-(--t)) * (float)Math.Sin((s - t) / period);
            });
        }

        public static EaserDelegate ElasticOut(float amplitude = 1, float period = 0.3f)
        {
            amplitude = Math.Max(1, amplitude);
            period /= PI2;
            float s = (float)Math.Asin(1f / amplitude) * period;
            return new EaserDelegate((t) => {
                return 1f - amplitude * tpmt(t) * (float)Math.Sin((t + s) / period);
            });
        }

        public static EaserDelegate ElasticInOut(float amplitude = 1, float period = 0.3f)
        {
            amplitude = Math.Max(1f, amplitude);
            period /= PI2;
            float s = (float)Math.Asin(1 / amplitude) * period;
            return new EaserDelegate((t) => {
                t = t * 2f - 1f;
                if (t < 0)
                {
                    return (amplitude * tpmt(-t) * (float)Math.Sin((s - t) / period)) / 2f;
                }
                return (2f - amplitude * tpmt(t) * (float)Math.Sin((s + t) / period)) / 2f;
            });
        }

        #endregion

        #region Back

        public static EaserDelegate BackIn(float overshoot = 1.70158f)
        {
            return new EaserDelegate((t) => {
                return t * t * (overshoot * (t - 1f) + t);
            });
        }

        public static EaserDelegate BackOut(float overshoot = 1.70158f)
        {
            return new EaserDelegate((t) => {
                t -= 1f;
                return t * t * ((t + 1f) * overshoot + t) + 1f;
            });
        }

        public static EaserDelegate BackInOut(float overshoot = 1.70158f)
        {
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t < 1f)
                {
                    return (t * t * ((overshoot + 1f) * t - overshoot)) / 2f;
                }
                t -= 2f;
                return (t * t * ((overshoot + 1f) * t + overshoot) + 2f) / 2f;
            });
        }

        #endregion

        #region Bounce

        private const float
            b1 = 4f / 11f,
            b2 = 6f / 11f,
            b3 = 8f / 11f,
            b4 = 3f / 4f,
            b5 = 9f / 11f,
            b6 = 10f / 11f,
            b7 = 15f / 16f,
            b8 = 21f / 22f,
            b9 = 63f / 64f,
            b0 = 1f / b1 / b1;

        public static EaserDelegate BounceIn()
        {
            EaserDelegate bounceOut = BounceOut();
            return new EaserDelegate((t) => {
                return 1f - bounceOut(1f - t);
            });
        }

        public static EaserDelegate BounceOut()
        {
            return new EaserDelegate((t) => {
                if (t < b1)
                {
                    return b0 * t * t;
                }
                if (t < b3)
                {
                    t -= b2;
                    return b0 * t * t + b4;
                }
                if (t < b6)
                {
                    t -= b5;
                    return b0 * t * t + b7;
                }
                t -= b8;
                return b0 * t * t + b9;
            });
        }

        public static EaserDelegate BounceInOut()
        {
            EaserDelegate bounceOut = BounceOut();
            return new EaserDelegate((t) => {
                t *= 2f;
                if (t <= 1f)
                {
                    return (1f - bounceOut(1f - t)) / 2f;
                }
                return (bounceOut(t - 1f) + 1f) / 2f;
            });
        }

        #endregion

    }
}
