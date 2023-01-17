
using D3DHook.Overlay;
using D3DHook.Hook.Common;

namespace GDMultiStash.Overlay.Panels
{
    internal class BackgroundShadowPanel : PanelElement
    {
        protected override IImageResource TL => StaticResources.ShadowTopLeft;
        protected override IImageResource T => StaticResources.ShadowTop;
        protected override IImageResource TR => StaticResources.ShadowTopRight;
        protected override IImageResource L => StaticResources.ShadowLeft;
        protected override IImageResource M => StaticResources.ShadowMiddle;
        protected override IImageResource R => StaticResources.ShadowRight;
        protected override IImageResource BL => StaticResources.ShadowBottomLeft;
        protected override IImageResource B => StaticResources.ShadowBottom;
        protected override IImageResource BR => StaticResources.ShadowBottomRight;

        public BackgroundShadowPanel() : base()
        {
            BorderTop = 20;
            BorderLeft = 20;
            BorderRight = 20;
            BorderBottom = 20;
        }
    }
}
