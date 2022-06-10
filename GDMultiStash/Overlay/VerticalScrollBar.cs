using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Elements
{
    public class VerticalScrollBar : ScrollBarElement
    {
        protected override float ScrollAreaWidth => 9f;
        protected override float ScrollAreaHeight => 567f;
        protected override float ScrollBarMinWidth => 9f;
        protected override float ScrollBarMinHeight => 30f;
        protected override int ScrollAreaWidthUnits => 0;
        protected override int ScrollAreaHeightUnits => 20;

        public static D3DHook.Hook.Common.IImageResource _ScrollBarResource;
        public static D3DHook.Hook.Common.IImageResource _ScrollBarTopResource;
        public static D3DHook.Hook.Common.IImageResource _ScrollBarBottomResource;

        public VerticalScrollBar() : base()
        {
            AnchorPoint = Anchor.TopRight;
            X = -4;
            Y = 15;
            Width = ScrollAreaWidth;
            Height = ScrollAreaHeight;

            ScrollBar.AddChild(new ImageElement()
            {
                AutoSize = false,
                Resource = _ScrollBarResource,
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.TopLeft,

                Height = -20,
                Y = 10,
            });

            ScrollBar.AddChild(new ImageElement()
            {
                AutoSize = false,
                Resource = _ScrollBarTopResource,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.TopLeft,
            });

            ScrollBar.AddChild(new ImageElement()
            {
                AutoSize = false,
                Resource = _ScrollBarBottomResource,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.BottomLeft,
            });
        }

    }
}
