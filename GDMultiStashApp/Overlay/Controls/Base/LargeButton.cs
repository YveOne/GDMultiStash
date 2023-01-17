
using System.Drawing;

using D3DHook.Overlay;
using D3DHook.Hook.Common;

namespace GDMultiStash.Overlay.Controls.Base
{
    public class LargeButton : ButtonElement
    {
        protected override IImageResource UpResource => StaticResources.ButtonLargeUp;
        protected override IImageResource DownResource => StaticResources.ButtonLargeDown;
        protected override IImageResource OverResource => StaticResources.ButtonLargeOver;

        public LargeButton() : base(StaticResources.LargeButtonFontHandler)
        {
            Width = 120;
            Height = 30;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
