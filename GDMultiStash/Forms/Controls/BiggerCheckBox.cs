using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace GDMultiStash.Forms.Controls
{
    [ToolboxItem(true)]
    public class BiggerCheckBox : CheckBox
    {
        public BiggerCheckBox()
        {
            TextAlign = ContentAlignment.MiddleRight;
        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h = ClientSize.Height - 4;
            var rc = new Rectangle(new Point(-1, Height / 2 - h / 2), new Size(h, h));
            if (FlatStyle == FlatStyle.Flat)
            {
                ControlPaint.DrawCheckBox(e.Graphics, rc, Checked ? ButtonState.Flat | ButtonState.Checked : ButtonState.Flat | ButtonState.Normal);
            }
            else
            {
                ControlPaint.DrawCheckBox(e.Graphics, rc, Checked ? ButtonState.Checked : ButtonState.Normal);
            }
        }


    }
}
