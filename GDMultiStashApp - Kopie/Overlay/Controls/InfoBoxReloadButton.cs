using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;
using GrimDawnLib;

namespace GDMultiStash.Overlay.Controls
{
    public class InfoBoxReloadButton : ButtonElement
    {
        protected override D3DHook.Hook.Common.IImageResource UpResource => StaticResources.ReloadButtonSmallUp;
        protected override D3DHook.Hook.Common.IImageResource DownResource => StaticResources.ReloadButtonSmallDown;
        protected override D3DHook.Hook.Common.IImageResource OverResource => StaticResources.ReloadButtonSmallOver;

        public InfoBoxReloadButton()
        {
            Width = 27;
            Height = 22;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
