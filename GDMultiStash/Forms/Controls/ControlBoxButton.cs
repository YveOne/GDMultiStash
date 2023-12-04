using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;

namespace GDMultiStash.Forms.Controls
{
    [DesignerCategory("code")]
    internal class ControlBoxButton : Button
    {
        private bool _initialized = false;
        private bool _isMouseOver = false;
        private Color _backColor;
        private Color _backColorHover;
        private Color _backColorPressed;
        private Color _foreColor;
        private Color _foreColorHover;
        private Image _image;
        private Image _imageHover;

        public ControlBoxButton()
        {
            Size = new Size(40, 30);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Text = "";

            Margin = new Padding(2, 0, 0, 0);

            BackColor = C.ControlBoxButtonBackColor;
            BackColorHover = C.ControlBoxButtonBackColorHover;
            BackColorPressed = C.ControlBoxButtonBackColorPressed;
            ForeColor = C.InteractiveForeColor;
            ForeColorHover = C.InteractiveForeColorHighlight;

            MouseEnter += delegate
            {
                _isMouseOver = true;
                UpdateAppearance();
            };
            MouseLeave += delegate
            {
                _isMouseOver = false;
                UpdateAppearance();
            };

            _initialized = true;
            UpdateAppearance();
        }

        public new Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                base.BackColor = value;
            }
        }

        public Color BackColorHover
        {
            get => _backColorHover;
            set
            {
                _backColorHover = value;
                FlatAppearance.MouseOverBackColor = value;
            }
        }

        public Color BackColorPressed
        {
            get => _backColorPressed;
            set
            {
                _backColorPressed = value;
                FlatAppearance.MouseDownBackColor = value;
            }
        }

        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                UpdateAppearance();
            }
        }

        public Color ForeColorHover
        {
            get => _foreColorHover;
            set
            {
                _foreColorHover = value;
                UpdateAppearance();
            }
        }

        public new Image Image
        {
            get => _image;
            set
            {
                _image = value;
                if (_imageHover == null)
                    _imageHover = value;
                UpdateAppearance();
            }
        }

        public Image ImageHover
        {
            get => _imageHover;
            set
            {
                _imageHover = value;
                UpdateAppearance();
            }
        }

        private void UpdateAppearance()
        {
            if (!_initialized) return;
            if (_isMouseOver)
            {
                base.ForeColor = _foreColorHover;
                base.Image = _imageHover;
            }
            else
            {
                base.ForeColor = _foreColor;
                base.Image = _image;
            }
        }





    }

}
