using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Controls.Base
{
    public class VerticalScrollBar : ScrollBarElement
    {
        public override System.Drawing.Color DebugColor => System.Drawing.Color.FromArgb(128, 0, 0, 255);

        public override float ScrollBarMinWidth => 9f;
        public override float ScrollBarMinHeight => 30f;

        public VerticalScrollBar() : base()
        {
            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = StaticResources.ScrollBar,
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.TopLeft,

            });

            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = StaticResources.ScrollBarTop,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.TopLeft,
            });

            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = StaticResources.ScrollBarBottom,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.BottomLeft,
            });
        }

    }
}
