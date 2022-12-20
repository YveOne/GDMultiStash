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

        public int Width { get; private set; }
        public int Height { get; private set; }
        internal byte[] ImageData { get; private set; }
        internal bool ReCreate { get; private set; }

        public ImageResource(System.Drawing.Image bitmap, System.Drawing.Imaging.ImageFormat format)
        {
            SetImage(bitmap, format);
        }

        public ImageResource(byte[] imageData, int width, int height)
        {
            SetImage(imageData, width, height);
        }

        public void SetImage(System.Drawing.Image bitmap, System.Drawing.Imaging.ImageFormat format)
        {
            ImageData = bitmap.ToByteArray(format);
            Width = bitmap.Width;
            Height = bitmap.Height;
            //bitmap.Dispose();
            ReCreate = true;
        }

        public void SetImage(byte[] imageData, int width, int height)
        {
            ImageData = imageData;
            Width = width;
            Height = height;
            ReCreate = true;
        }

        internal System.Drawing.Bitmap Bitmap
        {
            get
            {
                if (ImageData == null) return null;
                ReCreate = false;
                return ImageData.ToBitmap();
                //System.Drawing.Bitmap bmp = ImageData.ToBitmap();
                //ImageData = null;
                //return bmp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ImageData = null;
        }
    }
}