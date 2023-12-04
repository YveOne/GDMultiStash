
using System.Drawing;

using D3DHook.Hook.Common;
using D3DHook.Overlay;
using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay.Controls.Base
{
    internal class RadioButton : CheckableElement
    {
        public override Color DebugColor => Color.FromArgb(255, 0, 0);

        protected override IImageResource UpResource => StaticResources.ButtonRoundUp;
        protected override IImageResource OverResource => StaticResources.ButtonRoundOver;
        protected override IImageResource CheckedUpResource => StaticResources.ButtonRoundDown;
        protected override IImageResource CheckedOverResource => StaticResources.ButtonRoundDownOver;
    }

    internal class SelectableListChild<T> : ListBoxItemElement<T>
    {
        private readonly TextElement _textElement;
        private readonly RadioButton _radioButton;

        private readonly int _textMarginLeft = 30;

        private const float _alphaActive = 1f;
        private const float _alphaInactive = 0.75f;

        private bool _active = false;

        public SelectableListChild(FontHandler fontHandler)
        {
            _textElement = new TextElement(fontHandler)
            {
                Align = StringAlignment.Near,
                AnchorPoint = Anchor.Left,
                X = _textMarginLeft,
                WidthToParent = true,
                Width = -_textMarginLeft,
                HeightToParent = true,
            };
            _radioButton = new RadioButton()
            {
                AnchorPoint = Anchor.Left,
                X = 10,
                Y = -1,
                Width = 20,
                Height = 16,
            };

            AddChild(_textElement);
            AddChild(_radioButton);

            MouseCheckChildren = false;
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
                _radioButton.Checked = value;
                Alpha = value ? _alphaActive : _alphaInactive;
            }
        }

    }
}
