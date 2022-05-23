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

        private Font _font;
        private string _text = "";
        private Color _color = Color.Black;
        private bool _needRecreate = true;
        private StringAlignment _alignment = StringAlignment.Near;

        private ImageElement _imageElement = null;
        private int _zIndex = 0;

        public TextElement()
        {
        }

        public override void Draw(float ms)
        {
            base.Draw(ms);
            if (_needRecreate)
            {

                if (TotalWidth <= 0 || TotalHeight <= 0) return;
                _needRecreate = false;

                int useWidth = (int)(TotalWidth / TotalScale);
                int useHeight = (int)(TotalHeight / TotalScale);

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
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                        g.DrawString(_text, _font, brush, new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
                    }

                    //bmp.Save(System.Windows.Forms.Application.StartupPath+"\\..\\"+ID+".png");

                    ClearChildren();
                    _imageElement = new ImageElement()
                    {
                        Resource = GetViewport().Resources.CreateImageResource(bmp),
                        ZIndex = _zIndex,
                    };
                    AddChild(_imageElement);
                }
            }
        }








        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                if (_imageElement != null)
                    _imageElement.ZIndex = _zIndex;
                Redraw();
            }
        }







        public override void End()
        {
            base.End();
            if (ResetHeight || ResetScale || ResetWidth) _needRecreate = true;
        }










        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                _needRecreate = true;
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _needRecreate = true;
            }
        }

        public StringAlignment Align
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                _needRecreate = true;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _needRecreate = true;
            }
        }

        
        
        







    }
}
