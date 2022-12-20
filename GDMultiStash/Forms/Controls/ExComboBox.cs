using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GDMultiStash.Forms.Controls
{
    internal class ExComboBox : ComboBox
    {
        public ExComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            ExComboBoxItem item = (ExComboBoxItem)this.Items[e.Index];
            using (Brush ForeBrush = new SolidBrush(item.Forecolor))
            {
                using (Brush BackBrush = new SolidBrush(e.State == DrawItemState.Selected ? Color.FromArgb(66,66,66) :  item.Backcolor))
                {
                    e.Graphics.FillRectangle(BackBrush, e.Bounds);
                    e.Graphics.TranslateTransform(0, e.Bounds.Y);
                    e.Graphics.DrawString(item.Text, this.Font, ForeBrush, 2, 2);
                }
            }
        }
    }
}
