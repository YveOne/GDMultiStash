using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace GDMultiStash.Forms.ContextMenues
{
    [DesignerCategory("code")]
    internal abstract class BaseContextMenu : ContextMenuStrip
    {
        protected bool LastItemIsSeparator => Items.Count != 0 && Items[Items.Count - 1].GetType() == typeof(ToolStripSeparator);

        protected string X(string s) => s.Replace("&", "&&"); // used to escape & sign for item text

        public BaseContextMenu()
        {

        }

        public void AddComment(string text)
        {
            Items.Add(new ToolStripLabel(X(text))
            { ForeColor = Color.Gray });
        }

        public void AddSeparator()
        {
            if (LastItemIsSeparator) return;
            Items.Add(new ToolStripSeparator());
        }

    }
}
