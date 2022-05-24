using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;
using GrimDawnLib;

namespace GDMultiStash.Overlay.Elements
{
    public class InfoButton : ButtonElement
    {

        public static Font _Font;
        public static D3DHook.Hook.Common.IImageResource _UpResource;
        public static D3DHook.Hook.Common.IImageResource _DownResource;
        public static D3DHook.Hook.Common.IImageResource _OverResource;

        protected override D3DHook.Hook.Common.IImageResource UpResource => _UpResource;
        protected override D3DHook.Hook.Common.IImageResource DownResource => _DownResource;
        protected override D3DHook.Hook.Common.IImageResource OverResource => _OverResource;

        public InfoButton()
        {
            Font = _Font;
            Width = 120;
            Height = 22;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
