
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Panels
{
    internal class ListBorderPanel : PanelElement
    {
        protected override D3DHook.Hook.Common.IImageResource TL => StaticResources.StashListBorderTL;
        protected override D3DHook.Hook.Common.IImageResource T => StaticResources.StashListBorderT;
        protected override D3DHook.Hook.Common.IImageResource TR => StaticResources.StashListBorderTR;
        protected override D3DHook.Hook.Common.IImageResource L => StaticResources.StashListBorderL;
        protected override D3DHook.Hook.Common.IImageResource M => StaticResources.StashListBackground;
        protected override D3DHook.Hook.Common.IImageResource R => StaticResources.StashListBorderR;
        protected override D3DHook.Hook.Common.IImageResource BL => StaticResources.StashListBorderBL;
        protected override D3DHook.Hook.Common.IImageResource B => StaticResources.StashListBorderB;
        protected override D3DHook.Hook.Common.IImageResource BR => StaticResources.StashListBorderBR;

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
