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
    }
}