using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using D3DHook.Hook.Common;
using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay.Controls
{
    public class InfoBoxReloadButton : ButtonElement
    {
        protected override IImageResource UpResource => StaticResources.ReloadButtonSmallUp;
        protected override IImageResource DownResource => StaticResources.ReloadButtonSmallDown;
        protected override IImageResource OverResource => StaticResources.ReloadButtonSmallOver;

        public InfoBoxReloadButton() : base(null)
        {
            Width = 31;
            Height = 25;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
