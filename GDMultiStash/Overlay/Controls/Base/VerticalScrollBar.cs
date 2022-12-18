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

        public static D3DHook.Hook.Common.IImageResource _ScrollBarResource;
        public static D3DHook.Hook.Common.IImageResource _ScrollBarTopResource;
        public static D3DHook.Hook.Common.IImageResource _ScrollBarBottomResource;


        public VerticalScrollBar() : base()
        {
            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = _ScrollBarResource,
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.TopLeft,

            });

            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = _ScrollBarTopResource,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.TopLeft,
            });

            ScrollBar.AddChild(new ImageElement()
            {
                DebugColor = System.Drawing.Color.FromArgb(128, 0, 255, 255),

                Resource = _ScrollBarBottomResource,
                WidthToParent = true,
                HeightToParent = false,
                Height = 10,
                AnchorPoint = Anchor.BottomLeft,
            });
        }

    }
}
