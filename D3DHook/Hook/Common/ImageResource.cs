using D3DHook.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3DHook.Hook.Common
{
    [Serializable]
    public class ImageResource : Resource, IImageResource
    {

        private readonly int _width = 0;
        private readonly int _height = 0;

        public ImageResource(System.Drawing.Image bitmap, System.Drawing.Imaging.ImageFormat format)
        {
            ImageData = bitmap.ToByteArray(format);
            _width = bitmap.Width;
            _height = bitmap.Height;
            bitmap.Dispose();
        }

        public ImageResource(byte[] imageData, int width, int height)
        {
            ImageData = imageData;
            _width = width;
            _height = height;
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        internal byte[] ImageData { get; private set; }

        internal System.Drawing.Bitmap Bitmap {
            get {
                if (ImageData == null) return null; // it must have been initialized already
                System.Drawing.Bitmap bmp = ImageData.ToBitmap();
                //ImageData = null;
                return bmp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ImageData = null;
        }
    }
}