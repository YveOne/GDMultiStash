using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay
{
    public class PanelElement : Element
    {

        protected virtual Hook.Common.IImageResource TL { get; }
        protected virtual Hook.Common.IImageResource T { get; }
        protected virtual Hook.Common.IImageResource TR { get; }
        protected virtual Hook.Common.IImageResource L { get; }
        protected virtual Hook.Common.IImageResource M { get; }
        protected virtual Hook.Common.IImageResource R { get; }
        protected virtual Hook.Common.IImageResource BL { get; }
        protected virtual Hook.Common.IImageResource B { get; }
        protected virtual Hook.Common.IImageResource BR { get; }

        private ImageElement _imgTL;
        private ImageElement _imgT;
        private ImageElement _imgTR;
        private ImageElement _imgL;
        private ImageElement _imgM;
        private ImageElement _imgR;
        private ImageElement _imgBL;
        private ImageElement _imgB;
        private ImageElement _imgBR;

        public PanelElement() : base()
        {
            //ScaleWithParent = false;

            _imgTL = new ImageElement()
            {
                Resource = TL,
            };
            _imgT = new ImageElement()
            {
                Resource = T,
            };
            _imgTR = new ImageElement()
            {
                Resource = TR,
            };
            _imgL = new ImageElement()
            {
                Resource = L,
            };
            _imgM = new ImageElement()
            {
                Resource = M,
            };
            _imgR = new ImageElement()
            {
                Resource = R,
            };
            _imgBL = new ImageElement()
            {
                Resource = BL,
            };
            _imgB = new ImageElement() { Resource = B };
            _imgBR = new ImageElement() { Resource = BR };
            AddChild(_imgM);
            AddChild(_imgTL);
            AddChild(_imgT);
            AddChild(_imgTR);
            AddChild(_imgL);
            AddChild(_imgR);
            AddChild(_imgBL);
            AddChild(_imgB);
            AddChild(_imgBR);
        }

        private float _borderT = 0;
        private float _borderL = 0;
        private float _borderR = 0;
        private float _borderB = 0;

        private float _insetT = 0;
        private float _insetL = 0;
        private float _insetR = 0;
        private float _insetB = 0;

        private bool _update = false;

        protected override void OnDraw()
        {
            base.OnDraw();
            if (Parent != null)
            {
                _update |= Parent.ResetScale;
                _update |= Parent.ResetWidth || ResetScale;
                _update |= Parent.ResetHeight || ResetScale;
            }
            _update |= ResetWidth | ResetHeight;
            if (_update)
            {
                int finalWidth = (int)(TotalWidth);
                int finalHeight = (int)(TotalHeight);

                int scaledBorderT = (int)(_borderT * TotalScale);
                int scaledBorderL = (int)(_borderL * TotalScale);
                int scaledBorderR = (int)(_borderR * TotalScale);
                int scaledBorderB = (int)(_borderB * TotalScale);

                int scaledInsetT = (int)(_insetT * TotalScale);
                int scaledInsetL = (int)(_insetL * TotalScale);
                int scaledInsetR = (int)(_insetR * TotalScale);
                int scaledInsetB = (int)(_insetB * TotalScale);

                float outBorderT = (float)scaledBorderT / TotalScale;
                float outBorderL = (float)scaledBorderL / TotalScale;
                float outBorderR = (float)scaledBorderR / TotalScale;
                float outBorderB = (float)scaledBorderB / TotalScale;

                float outWidthM = (float)(finalWidth - scaledBorderL - scaledBorderR) / TotalScale;
                float outHeightM = (float)(finalHeight - scaledBorderT - scaledBorderB) / TotalScale;



                _imgTL.Height = outBorderT;
                _imgT.Height = outBorderT;
                _imgTR.Height = outBorderT;

                _imgTL.Width = outBorderL;
                _imgL.Width = outBorderL;
                _imgBL.Width = outBorderL;

                _imgTR.Width = outBorderR;
                _imgR.Width = outBorderR;
                _imgBR.Width = outBorderR;

                _imgBL.Height = outBorderB;
                _imgB.Height = outBorderB;
                _imgBR.Height = outBorderB;

                _imgL.Y = outBorderT;
                _imgR.Y = outBorderT;
                _imgT.X = outBorderL;
                _imgB.X = outBorderL;

                _imgTL.X = 0;
                _imgTL.Y = 0;
                _imgT.Y = 0;
                _imgTR.Y = 0;
                _imgL.X = 0;
                _imgBL.X = 0;

                _imgT.Width = outWidthM;
                _imgB.Width = outWidthM;

                _imgL.Height = outHeightM;
                _imgR.Height = outHeightM;

                _imgM.Width = (finalWidth - scaledBorderL - scaledBorderR + scaledInsetL + scaledInsetR) / TotalScale;
                _imgM.Height = (finalHeight - scaledBorderT - scaledBorderB + scaledInsetT + scaledInsetB) / TotalScale;







                _imgTR.X = (finalWidth - scaledBorderR) / TotalScale;
                _imgR.X = (finalWidth - scaledBorderR) / TotalScale;
                _imgBR.X = (finalWidth - scaledBorderR) / TotalScale;

                _imgM.X = (scaledBorderL - scaledInsetL) / TotalScale;

                _imgM.Y = (scaledBorderT - scaledInsetT) / TotalScale;

                _imgBL.Y = (finalHeight - scaledBorderB) / TotalScale;
                _imgB.Y = (finalHeight - scaledBorderB) / TotalScale;
                _imgBR.Y = (finalHeight - scaledBorderB) / TotalScale;


            }







        }





        public float BorderTop
        {
            get => _borderT;
            set
            {
                _borderT = value;
                _update = true;
            }
        }

        public float BorderLeft
        {
            get => _borderL;
            set
            {
                _borderL = value;
                _update = true;
            }
        }

        public float BorderRight
        {
            get => _borderR;
            set
            {
                _borderR = value;
                _update = true;
            }
        }

        public float BorderBottom
        {
            get => _borderB;
            set
            {
                _borderB = value;
                _update = true;
            }
        }

        public float InsetTop
        {
            get => _insetT;
            set
            {
                _insetT = value;
                _update = true;
            }
        }

        public float InsetLeft
        {
            get => _insetL;
            set
            {
                _insetL = value;
                _update = true;
            }
        }

        public float InsetRight
        {
            get => _insetR;
            set
            {
                _insetR = value;
                _update = true;
            }
        }

        public float InsetBottom
        {
            get => _insetB;
            set
            {
                _insetB = value;
                _update = true;
            }
        }

    }
}
