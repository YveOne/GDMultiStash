using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Elements
{
    public class StashListChild : PseudoScrollChild
    {
        public static D3DHook.Hook.Common.IImageResource _Radio0Resource;
        public static D3DHook.Hook.Common.IImageResource _Radio1Resource;

        private readonly TextElement _textElement;
        private readonly ImageElement _radioButton;
        private readonly int _textMarginLeft = 30;

        private bool _active = false;

        public StashListChild()
        {

            _textElement = new TextElement()
            {
                Align = StringAlignment.Near,
                AnchorPoint = Anchor.Left,
                X = _textMarginLeft,
                WidthToParent = true,
                Width = -_textMarginLeft,
            };
            AddChild(_textElement);

            _radioButton = new ImageElement() {
                Resource = _Radio0Resource,
                AnchorPoint = Anchor.Left,
                X = 10,
                Y = -2,
                Scale = 0.8f,
            };
            AddChild(_radioButton);
        }

        public string Text
        {
            get => _textElement.Text;
            set => _textElement.Text = value;
        }

        public override float Alpha
        {
            get { return base.Alpha; }
            set { base.Alpha = value; _radioButton.Alpha = value; }
        }

        public override float Height
        {
            get { return base.Height; }
            set { base.Height = value; _textElement.Height = value; }
        }

        public Font Font
        {
            get => _textElement.Font;
            set => _textElement.Font = value;
        }

        public Color Color
        {
            get => _textElement.Color;
            set => _textElement.Color = value;
        }

        public bool Active
        {
            get { return _active; }
            set {
                _active = value;
                _radioButton.Resource = value ? _Radio1Resource : _Radio0Resource;
            }
        }







    }
}
