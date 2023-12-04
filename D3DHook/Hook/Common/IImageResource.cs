using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace D3DHook.Hook.Common
{
    public interface IImageResource : IResource
    {
        int Width { get; }
        int Height { get; }
        void SetImage(System.Drawing.Image bitmap, System.Drawing.Imaging.ImageFormat format);
    }
}