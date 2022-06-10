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

        private Font _font = null;
        private string _text = "";
        private Color _color = Color.Black;
        private bool _needRecreate = true;
        private StringAlignment _alignment = StringAlignment.Near;

        private ImageElement _imageElement = null;
        private int _zIndex = 0;

        public TextElement()
        {
            _imageElement = new ImageElement();
            AddChild(_imageElement);
        }

        private static readonly int recreateMaxPerFrame = 50; 
        private static int _recreatedThisFrame = 0;
        private static readonly Dictionary<string, D3DHook.Hook.Common.IImageResource> _resourceCache = new Dictionary<string, D3DHook.Hook.Common.IImageResource>();

        public override void Draw(float ms)
        {
            base.Draw(ms);
            if (_needRecreate)
            {
                if (_font == null) return;
                if (_recreatedThisFrame >= recreateMaxPerFrame) return;
                if (TotalWidth <= 0 || TotalHeight <= 0) return;
                _needRecreate = false;
                _recreatedThisFrame += 1;

                int useWidth = (int)(TotalWidth / TotalScale);
                int useHeight = (int)(TotalHeight / TotalScale);

                string resKey = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                    _font.Name,
                    _font.Bold ? 1 : 0,
                    _font.Italic ? 1 : 0,
                    _text,
                    useWidth,
                    useHeight,
                    _color.ToString(),
                    _alignment.ToString()
                    );

                if (_resourceCache.ContainsKey(resKey))
                {
                    _imageElement.Resource = _resourceCache[resKey];
                }
                else
                {
                    using (Bitmap bmp = new Bitmap(useWidth, useHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        Graphics g = Graphics.FromImage(bmp);
                        var brush = new SolidBrush(_color);
                        using (StringFormat sf = new StringFormat()
                        {
                            Alignment = _alignment,
                            LineAlignment = StringAlignment.Center,
                            Trimming = StringTrimming.None,
                            FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                        })
                        {
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                            g.DrawString(_text, _font, brush, new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
                        }

                        //bmp.Save(System.Windows.Forms.Application.StartupPath+"\\..\\"+ID+".png");

                        _imageElement.Resource = GetViewport().Resources.CreateImageResource(bmp);
                        _resourceCache.Add(resKey, _imageElement.Resource);
                    }
                }
            }
        }

        public override void End()
        {
            base.End();
            if (ResetHeight || ResetScale || ResetWidth) _needRecreate = true;
            _recreatedThisFrame = 0;
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                if (_font == value) return;
                _font = value;
                _needRecreate = true;
            }
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
