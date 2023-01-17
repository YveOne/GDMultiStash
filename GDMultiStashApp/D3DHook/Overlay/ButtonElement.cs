using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace D3DHook.Overlay
{
    public class ButtonElement : Element
    {

        private TextElement _text;
        private ImageElement _back;

        protected virtual Hook.Common.IImageResource UpResource { get; }
        protected virtual Hook.Common.IImageResource DownResource { get; }
        protected virtual Hook.Common.IImageResource OverResource { get; }

        public ButtonElement(FontHandler fontHandler)
        {

            _back = new ImageElement()
            {
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.Center,
            };
            AddChild(_back);

            _text = new TextElement(fontHandler)
            {
                WidthToParent = true,
                HeightToParent = true,
                AnchorPoint = Anchor.Center,
                Align = StringAlignment.Center,

                Height = -2,
                Y = 1,
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

        public string Text
        {
            get { return _text.Text; }
            set { _text.Text = value; }
        }

        public StringAlignment TextAlign
        {
            get { return _text.Align; }
            set { _text.Align = value; }
        }

        public Anchor TextAnchor
        {
            get { return _text.AnchorPoint; }
            set { _text.AnchorPoint = value; }
        }

        public float TextWidth
        {
            get { return _text.Width; }
            set { _text.Width = value; }
        }

        public float TextX
        {
            get { return _text.X; }
            set { _text.X = value; }
        }






    }
}
