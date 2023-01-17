using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace D3DHook.Overlay
{
    public class ImageElement : Element
    {

        private Hook.Common.ImageElement _image = null;
        private Hook.Common.IImageResource _imageResource;

        private bool _setSizeToBitmap = false;

        public ImageElement() : base()
        {
        }

        public override List<Hook.Common.IOverlayElement> GetImagesRecursive()
        {
            List<Hook.Common.IOverlayElement> imglist = new List<Hook.Common.IOverlayElement>();
                imglist.Add(_image);
            imglist.AddRange(base.GetImagesRecursive());
            return imglist;
        }

        public Hook.Common.IImageResource Resource
        {
            get { return _imageResource; }
            set {
                _imageResource = value;
                if (_image != null)
                {
                    if (value != null)
                    {
                        _image.ResourceUID = value.UID;
                        if (_setSizeToBitmap)
                        {
                            Width = value.Width;
                            Height = value.Height;
                        }
                    }
                    Redraw();
                }
            }
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_image == null)
            {
                _image = new Hook.Common.ImageElement();
                Resource = _imageResource;
            }
        }

        protected override void OnDrawEnd()
        {
            base.OnDrawEnd();
            if (ResetX) _image.X = TotalX;
            if (ResetY) _image.Y = TotalY;
            if (ResetWidth) _image.Width = TotalWidth;
            if (ResetHeight) _image.Height = TotalHeight;
            if (ResetAlpha) _image.Tint = Color.FromArgb((int)(255f * TotalAlpha), 255, 255, 255);
            if (ResetVisible) _image.Hidden = !TotalVisible;
        }

        #region properties

        public bool AutoSize
        {
            get { return _setSizeToBitmap; }
            set { _setSizeToBitmap = value; }
        }

        public float ZIndex
        {
            get { return _image.ZIndex; }
            set
            {
                _image.ZIndex = value;
                Redraw();
            }
        }

        #endregion

    }
}
