
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Panels
{
    internal class BackgroundBorderPanel : PanelElement
    {
        protected override D3DHook.Hook.Common.IImageResource T => StaticResources.WindowBorderTop;
        protected override D3DHook.Hook.Common.IImageResource TR => StaticResources.WindowBorderTopRight;
        protected override D3DHook.Hook.Common.IImageResource R => StaticResources.WindowBorderRight;
        protected override D3DHook.Hook.Common.IImageResource B => StaticResources.WindowBorderBottom;
        protected override D3DHook.Hook.Common.IImageResource BR => StaticResources.WindowBorderBottomRight;

        public BackgroundBorderPanel() : base()
        {
            BorderTop = 11;
            BorderLeft = 0;
            BorderRight = 11;
            BorderBottom = 11;
        }
    }
}
