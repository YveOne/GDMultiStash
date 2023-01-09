
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Panels
{
    internal class ListBorderPanel : PanelElement
    {
        protected override D3DHook.Hook.Common.IImageResource TL => _TL;
        public static D3DHook.Hook.Common.IImageResource _TL;
        protected override D3DHook.Hook.Common.IImageResource T => _T;
        public static D3DHook.Hook.Common.IImageResource _T;
        protected override D3DHook.Hook.Common.IImageResource TR => _TR;
        public static D3DHook.Hook.Common.IImageResource _TR;
        protected override D3DHook.Hook.Common.IImageResource L => _L;
        public static D3DHook.Hook.Common.IImageResource _L;
        protected override D3DHook.Hook.Common.IImageResource M => _M;
        public static D3DHook.Hook.Common.IImageResource _M;
        protected override D3DHook.Hook.Common.IImageResource R => _R;
        public static D3DHook.Hook.Common.IImageResource _R;
        protected override D3DHook.Hook.Common.IImageResource BL => _BL;
        public static D3DHook.Hook.Common.IImageResource _BL;
        protected override D3DHook.Hook.Common.IImageResource B => _B;
        public static D3DHook.Hook.Common.IImageResource _B;
        protected override D3DHook.Hook.Common.IImageResource BR => _BR;
        public static D3DHook.Hook.Common.IImageResource _BR;
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
