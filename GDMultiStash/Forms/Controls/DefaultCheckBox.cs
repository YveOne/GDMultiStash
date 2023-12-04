using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GDMultiStash.Forms.Controls
{
    [DesignerCategory("code")]
    internal class DefaultCheckBox : CheckBox
    {
        private bool hover = false;
        public DefaultCheckBox()
        {
            MouseEnter += delegate { hover = true; UpdateForeColor(); };
            MouseLeave += delegate { hover = false; UpdateForeColor(); };
            _foreColor = base.ForeColor;
            _foreColorHover = base.ForeColor;
        }

        private Color _foreColor;
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                UpdateForeColor();
            }
        }

        private Color _foreColorHover;
        public Color ForeColorHover
        {
            get => _foreColorHover;
            set
            {
                _foreColorHover = value;
                UpdateForeColor();
            }
        }

        private void UpdateForeColor()
        {
            base.ForeColor = hover ? _foreColorHover : _foreColor;
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            // just draw original and draw my own images above
            base.OnPaint(e);
            e.Graphics.DrawImage(hover
                ? Properties.Resources.CheckBoxBackHover
                : Properties.Resources.CheckBoxBack,
                -1, 1, 15, 15);
            switch (CheckState)
            {
                case CheckState.Checked:
                    e.Graphics.DrawImage(Properties.Resources.CheckBoxTick, -4, -2, 21, 21);
                    break;
                case CheckState.Unchecked:
                    e.Graphics.DrawImage(Properties.Resources.CheckBoxCross, -4, -2, 21, 21);
                    break;
            }
        }



    }
}
