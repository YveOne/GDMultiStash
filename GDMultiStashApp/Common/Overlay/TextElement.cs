using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public class TextElement : Element
    {

        private readonly FontHandler _fontHandler;
        private string _text = "";
        private Color _color = Color.Black;
        private bool _needRecreate = true;
        private StringAlignment _alignment = StringAlignment.Near;
        private ImageElement _imageElement = null;
        private int _zIndex = 0;

        public TextElement(FontHandler fontHandler)
        {
            _fontHandler = fontHandler;
            _imageElement = new ImageElement()
            {
                WidthToParent = true,
                HeightToParent = true,
            };
            AddChild(_imageElement);
        }

        private void FontHandler_FontChanged(object sender, EventArgs e)
        {
            _needRecreate = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_needRecreate && _fontHandler != null)
            {
                if (TotalWidth <= 0 || TotalHeight <= 0) return;
                var _font = _fontHandler.GetFont(TotalScale);
                _needRecreate = false;

                int useWidth = (int)(TotalWidth);
                int useHeight = (int)(TotalHeight);

                ParentViewport.OverlayResources.AsyncCreateTextImageResource(_text, _font, useWidth, useHeight, _color, _alignment)
                    .ResourceCreated += delegate (object sender, ResourceHandler.ResourceCreatedEventArgs args) {
                        ParentViewport.OverlayResources.DeleteResource(_imageElement.Resource);
                        _imageElement.Resource = args.Resource;
                    };
            }
        }




        protected override void OnDrawEnd()
        {
            base.OnDrawEnd();
            if (ResetHeight || ResetScale || ResetWidth) _needRecreate = true;
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value) return;
                _color = value;
                _needRecreate = true;
            }
        }

        public StringAlignment Align
        {
            get { return _alignment; }
            set
            {
                if (_alignment == value) return;
                _alignment = value;
                _needRecreate = true;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                _needRecreate = true;
            }
        }

        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                if (_zIndex == value) return;
                _zIndex = value;
                if (_imageElement != null)
                    _imageElement.ZIndex = _zIndex;
                Redraw();
            }
        }

    }
}
