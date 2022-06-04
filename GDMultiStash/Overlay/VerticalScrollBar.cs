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

        protected override bool ScrollHorizontal => false;
        protected override bool ScrollVertical => true;
        protected override float ScrollAreaWidth => 15f - 6f;
        protected override float ScrollAreaHeight => 565f - 6f;
        protected override int ScrollAreaWidthUnits => 0;
        protected override int ScrollAreaHeightUnits => 20;

        public static D3DHook.Hook.Common.IImageResource _ScrollBarResource;

        public VerticalScrollBar() : base()
        {
            Width = ScrollAreaWidth;
            Height = ScrollAreaHeight;

            AnchorPoint = Anchor.TopLeft;
            ScrollBar.AddChild(new ImageElement()
            {
                AutoSize = false,
                Resource = _ScrollBarResource,
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.TopLeft, // its faster than center
                ParentPoint = Anchor.TopLeft, // its faster than center
            });
            ScrollBar.Alpha = 0.5f;
            ScrollBar.MouseEnter += delegate { ScrollBar.Alpha = 1f; };
            ScrollBar.MouseLeave += delegate { if (!IsScrolling) ScrollBar.Alpha = 0.5f; };
            ScrollingEnd += delegate { if (!ScrollBar.MouseOver) ScrollBar.Alpha = 0.5f; };
        }




    }
}
