using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.Controls
{
    internal class DefaultComboBox : Base.FlatComboBox
    {
        public DefaultComboBox() : base()
        {
            BorderColor = C.ComboBoxBorderColor;
            ButtonColor = C.ComboBoxButtonColor;
            ButtonColorHighlight = C.ComboBoxButtonColorHighlight;
            BackColor = C.ComboBoxBackColor;
            ForeColor = C.ComboBoxForeColor;
            BackColorHighlight = C.ComboBoxBackColorHighlight;
            ForeColorHighlight = C.ComboBoxForeColorHighlight;
            ListBorderColor = C.ComboBoxBorderListBorderColor;
        }
    }
}
