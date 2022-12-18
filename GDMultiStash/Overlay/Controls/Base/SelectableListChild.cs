using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Controls.Base
{
    internal class SelectableListChild<T> : ListBoxItemElement<T>
    {
        private readonly TextElement _textElement;
        private readonly ImageElement _radioButton;

        private readonly int _textMarginLeft = 30;

        private const float _alphaActive = 1f;
        private const float _alphaInactive = 0.66f;

        private bool _active = false;

        public SelectableListChild()
        {
            _textElement = new TextElement()
            {
                Align = StringAlignment.Near,
                AnchorPoint = Anchor.Left,
                X = _textMarginLeft,
                WidthToParent = true,
                Width = -_textMarginLeft,
                HeightToParent = true,
            };
            _radioButton = new ImageElement()
            {
                Resource = StaticResources.RadioButton0Resource,
                AnchorPoint = Anchor.Left,
                X = 10,
                Y = -2,
                Scale = 0.8f,
                AutoSize = true,
            };

            AddChild(_textElement);
            AddChild(_radioButton);

            Alpha = _alphaInactive;
            MouseEnter += delegate { Alpha = _alphaActive; };
            MouseLeave += delegate { if (!_active) Alpha = _alphaInactive; };
        }

        public float TextWidth
        {
            get => _textElement.Width;
            set => _textElement.Width = value;
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
            set
            {
                _active = value;
                _radioButton.Resource = value ? StaticResources.RadioButton1Resource : StaticResources.RadioButton0Resource;
                Alpha = value ? _alphaActive : _alphaInactive;
            }
        }

    }
}
