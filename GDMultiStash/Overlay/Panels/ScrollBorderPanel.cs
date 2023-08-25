
using D3DHook.Overlay;
using D3DHook.Hook.Common;

namespace GDMultiStash.Overlay.Panels
{
    internal class ScrollBorderPanel : PanelElement
    {
        protected override IImageResource T => StaticResources.ScrollAreaTop;
        protected override IImageResource M => StaticResources.ScrollAreaMiddle;
        protected override IImageResource B => StaticResources.ScrollAreaBottom;

        public ScrollBorderPanel() : base()
        {
            BorderTop = 15;
            BorderLeft = 0;
            BorderRight = 0;
            BorderBottom = 15;
        }
    }
}
