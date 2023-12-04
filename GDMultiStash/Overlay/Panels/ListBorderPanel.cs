
using D3DHook.Hook.Common;
using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay.Panels
{
    internal class ListBorderPanel : PanelElement
    {
        protected override IImageResource TL => StaticResources.StashListBorderTL;
        protected override IImageResource T => StaticResources.StashListBorderT;
        protected override IImageResource TR => StaticResources.StashListBorderTR;
        protected override IImageResource L => StaticResources.StashListBorderL;
        protected override IImageResource M => StaticResources.StashListBackground;
        protected override IImageResource R => StaticResources.StashListBorderR;
        protected override IImageResource BL => StaticResources.StashListBorderBL;
        protected override IImageResource B => StaticResources.StashListBorderB;
        protected override IImageResource BR => StaticResources.StashListBorderBR;

        public ListBorderPanel() : base()
        {
            BorderTop = 25;
            BorderLeft = 25;
            BorderRight = 25;
            BorderBottom = 25;
            InsetTop = 21;
            InsetLeft = 21;
            InsetRight = 21;
            InsetBottom = 21;
        }
    }
}
