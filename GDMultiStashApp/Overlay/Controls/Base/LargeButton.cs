
using System.Drawing;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Controls.Base
{
    public class LargeButton : ButtonElement
    {
        protected override D3DHook.Hook.Common.IImageResource UpResource => StaticResources.ButtonLargeUp;
        protected override D3DHook.Hook.Common.IImageResource DownResource => StaticResources.ButtonLargeDown;
        protected override D3DHook.Hook.Common.IImageResource OverResource => StaticResources.ButtonLargeOver;

        public LargeButton()
        {
            Font = StaticResources.LargeButtonFont;
            Width = 120;
            Height = 30;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
