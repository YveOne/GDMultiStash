
using System.Drawing;

using D3DHook.Hook.Common;
using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay.Controls.Base
{
    public class SmallButton : ButtonElement
    {
        protected override IImageResource UpResource => StaticResources.ButtonSmallUp;
        protected override IImageResource OverResource => StaticResources.ButtonSmallOver;
        protected override IImageResource DownResource => StaticResources.ButtonSmallDown;

        public SmallButton() : base(StaticResources.SmallButtonFontHandler)
        {
            Width = 120;
            Height = 25;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
