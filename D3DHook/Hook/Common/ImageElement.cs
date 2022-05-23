using D3DHook.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3DHook.Hook.Common
{
    [Serializable]
    public class ImageElement : Element
    {
        private int _resourceUID = -1;

        public ImageElement()
        {
            Tint = System.Drawing.Color.White;
            ZIndex = 1f;
            Angle = 0f;
            Width = 0;
            Height = 0;
        }

        public ImageElement(int resourceUID)
        {
            _resourceUID = resourceUID;
            Tint = System.Drawing.Color.White;
            ZIndex = 1f;
            Angle = 0f;
            Width = 0;
            Height = 0;
        }

        public int ResourceUID
        {
            get { return _resourceUID; }
            set { _resourceUID = value; }
        }

        /// <summary>
        /// This value is multiplied with the source color (e.g. White will result in same color as source image)
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="System.Drawing.Color.White"/>.
        /// </remarks>
        public virtual System.Drawing.Color Tint { get; set; } = System.Drawing.Color.White;

        public virtual float X { get; set; }
        public virtual float Y { get; set; }

        public virtual float Angle { get; set; }

        public virtual float ZIndex { get; set; }

        public virtual float Width { get; set; }

        public virtual float Height { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
            }
        }
    }
}