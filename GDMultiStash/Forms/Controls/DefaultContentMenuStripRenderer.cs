using GDMultiStash.Forms.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms.Controls
{
    internal class DefaultContentMenuToolStripColorTable : CustomToolStripColorTable
    {
        public DefaultContentMenuToolStripColorTable()
        {
            SetMenuBorder(C.PageBackColor);
        }
    }

    internal class DefaultContentMenuStripRenderer : CustomToolStripRenderer
    {

        public DefaultContentMenuStripRenderer() : base(new DefaultContentMenuToolStripColorTable())
        {
            BackColor = C.ToolStripButtonBackColor;
            BackColorSelected = C.ToolStripButtonBackColorHover;
            ForeColor = C.InteractiveForeColor;
            ForeColorSelected = C.InteractiveForeColorHighlight;
            FirstBackColor = C.ToolStripButtonBackColor;
            FirstBackColorHover = C.ToolStripButtonBackColorHover;
            FirstBackColorSelected = C.ToolStripButtonBackColorHover;

        }

    }
}
