using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public class ButtonElement : Element
    {

        private TextElement _text;
        private ImageElement _back;

        protected virtual D3DHook.Hook.Common.IImageResource UpResource { get; }
        protected virtual D3DHook.Hook.Common.IImageResource DownResource { get; }
        protected virtual D3DHook.Hook.Common.IImageResource OverResource { get; }

        public ButtonElement()
        {

            _back = new ImageElement()
            {
                AutoSize = false,
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.Center,
            };
            AddChild(_back);

            _text = new TextElement()
            {
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.Center,
                Align = StringAlignment.Center,
            };
            AddChild(_text);

            _back.Resource = UpResource;
            MouseEnter += delegate { _back.Resource = OverResource; };
            MouseDown += delegate { _back.Resource = DownResource; };
            MouseUp += delegate { _back.Resource = OverResource; };
            MouseLeave += delegate { _back.Resource = UpResource; };

        }

        public Color Color
        {
            get { return _text.Color; }
            set { _text.Color = value; }
        }

        public Font Font
        {
            get { return _text.Font; }
            set { _text.Font = value; }
        }

        public string Text
        {
            get { return _text.Text; }
            set { _text.Text = value; }
        }






    }
}
