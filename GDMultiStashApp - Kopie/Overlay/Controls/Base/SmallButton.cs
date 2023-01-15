
using System.Drawing;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Controls.Base
{
    public class SmallButton : ButtonElement
    {
        protected override D3DHook.Hook.Common.IImageResource UpResource => StaticResources.ButtonSmallUp;
        protected override D3DHook.Hook.Common.IImageResource OverResource => StaticResources.ButtonSmallOver;
        protected override D3DHook.Hook.Common.IImageResource DownResource => StaticResources.ButtonSmallDown;

        public SmallButton()
        {
            Font = StaticResources.SmallButtonFont;
            Width = 120;
            Height = 22;
            Color = Color.FromArgb(255, 209, 191, 153);
        }

    }
}
