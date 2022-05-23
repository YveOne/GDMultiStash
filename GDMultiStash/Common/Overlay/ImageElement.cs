using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public class ImageElement : Element
    {

        private readonly D3DHook.Hook.Common.ImageElement _image;
        private D3DHook.Hook.Common.IImageResource _imageResource;

        private bool _setSizeToBitmap = true;

        public ImageElement() : base()
        {
            _image = new D3DHook.Hook.Common.ImageElement();
        }

        public override List<D3DHook.Hook.Common.IOverlayElement> GetImagesRecursive()
        {
            List<D3DHook.Hook.Common.IOverlayElement> imglist = base.GetImagesRecursive();
            imglist.Add(_image);
            return imglist;
        }

        public D3DHook.Hook.Common.IImageResource Resource
        {
            get { return _imageResource; }
            set {
                _imageResource = value;
                _image.ResourceUID = value.UID;
                if (_setSizeToBitmap)
                {
                    Width = value.Width;
                    Height = value.Height;
                }
                Redraw();
            }
        }


        public override void End()
        {
            base.End();
            if (ResetVisible) _image.Hidden = !TotalVisible;
            if (ResetAlpha) _image.Tint = Color.FromArgb((int)(255f * TotalAlpha), 255, 255, 255);
            if (ResetWidth) _image.Width = TotalWidth + 0.5f;
            if (ResetHeight) _image.Height = TotalHeight + 0.5f;
            if (ResetX) _image.X = TotalX + 0.5f;
            if (ResetY) _image.Y = TotalY + 0.5f;
        }


        public bool AutoSize
        {
            get { return _setSizeToBitmap; }
            set { _setSizeToBitmap = value; }
        }



        public float ZIndex
        {
            get { return _image.ZIndex; }
            set {
                _image.ZIndex = value;
                Redraw();
            }
        }

    }
}
