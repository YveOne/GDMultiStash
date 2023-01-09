using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace GDMultiStash.Forms.Controls
{
    internal class MyCheckBox : CheckBox
    {
        private bool hover = false;
        public MyCheckBox()
        {
            MouseEnter += delegate { hover = true; };
            MouseLeave += delegate { hover = false; };
            ThreeState = true;
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
