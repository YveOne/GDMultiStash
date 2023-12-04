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
    internal class DefaultTitleMenuToolStripColorTable : CustomToolStripColorTable
    {
        public DefaultTitleMenuToolStripColorTable()
        {
            SetMenuBorder(C.ContextBorderColor);
        }
    }

    internal class DefaultTitleMenuStripRenderer : CustomToolStripRenderer
    {

        public DefaultTitleMenuStripRenderer()
            : base(new DefaultTitleMenuToolStripColorTable())
        {
            BackColor = C.ContextBackColor;
            BackColorSelected = C.ContextBackColorHover;
            ForeColor = C.ContextForeColor;
            ForeColorSelected = C.ContextForeColorHover;
            FirstBackColor = C.ContextFirstBackColor;
            FirstBackColorHover = C.ContextFirstBackColorHover;
            FirstBackColorSelected = C.ContextFirstBackColorSelected;
        }

    }
}
