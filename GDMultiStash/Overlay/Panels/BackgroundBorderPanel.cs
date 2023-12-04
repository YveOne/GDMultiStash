
using D3DHook.Hook.Common;
using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay.Panels
{
    internal class BackgroundBorderPanel : PanelElement
    {
        protected override IImageResource T => StaticResources.WindowBorderTop;
        protected override IImageResource TR => StaticResources.WindowBorderTopRight;
        protected override IImageResource R => StaticResources.WindowBorderRight;
        protected override IImageResource B => StaticResources.WindowBorderBottom;
        protected override IImageResource BR => StaticResources.WindowBorderBottomRight;

        public BackgroundBorderPanel() : base()
        {
            BorderTop = 11;
            BorderLeft = 0;
            BorderRight = 11;
            BorderBottom = 11;
        }
    }
}
