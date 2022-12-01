using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GDMultiStash.Controls
{
    internal class ExComboBoxItem
    {
        public ExComboBoxItem(object Value, string Text)
        {
            this.Value = Value;
            this.Text = Text;
        }

        public string Text { get; set; } = null;
        public object Value { get; set; } = null;
        public Color Forecolor { get; set; } = Color.Black;
        public Color Backcolor { get; set; } = Color.Black;

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
