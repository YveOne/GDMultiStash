
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Panels
{
    internal class BackgroundShadowPanel : PanelElement
    {
        protected override D3DHook.Hook.Common.IImageResource TL => StaticResources.ShadowTopLeft;
        protected override D3DHook.Hook.Common.IImageResource T => StaticResources.ShadowTop;
        protected override D3DHook.Hook.Common.IImageResource TR => StaticResources.ShadowTopRight;
        protected override D3DHook.Hook.Common.IImageResource L => StaticResources.ShadowLeft;
        protected override D3DHook.Hook.Common.IImageResource M => StaticResources.ShadowMiddle;
        protected override D3DHook.Hook.Common.IImageResource R => StaticResources.ShadowRight;
        protected override D3DHook.Hook.Common.IImageResource BL => StaticResources.ShadowBottomLeft;
        protected override D3DHook.Hook.Common.IImageResource B => StaticResources.ShadowBottom;
        protected override D3DHook.Hook.Common.IImageResource BR => StaticResources.ShadowBottomRight;

        public BackgroundShadowPanel() : base()
        {
            BorderTop = 20;
            BorderLeft = 20;
            BorderRight = 20;
            BorderBottom = 20;
        }
    }
}
