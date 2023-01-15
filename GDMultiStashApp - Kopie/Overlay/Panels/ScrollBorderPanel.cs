
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Panels
{
    internal class ScrollBorderPanel : PanelElement
    {
        protected override D3DHook.Hook.Common.IImageResource T => StaticResources.ScrollAreaTop;
        protected override D3DHook.Hook.Common.IImageResource M => StaticResources.ScrollAreaMiddle;
        protected override D3DHook.Hook.Common.IImageResource B => StaticResources.ScrollAreaBottom;

        public ScrollBorderPanel() : base()
        {
            BorderTop = 15;
            BorderLeft = 0;
            BorderRight = 0;
            BorderBottom = 15;
        }
    }
}
