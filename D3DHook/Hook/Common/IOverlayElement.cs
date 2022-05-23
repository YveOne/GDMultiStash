using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace D3DHook.Hook.Common
{
    public interface IOverlayElement : ICloneable
    {
        bool Hidden { get; set; }

        void Frame();
    }
}