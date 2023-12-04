using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace GDMultiStash.Forms.Controls
{
    internal class BaseFormBorderPanel : TransparentPanel
    {
        private readonly Color c1 = Color.FromArgb(168, 32, 2); // TODO: make me a constant
        private readonly Color c2 = Color.FromArgb(22, 150, 100); // TODO: make me a constant

        public BaseFormBorderPanel()
        {
            DoubleBuffered = true;
            BackColor = C.FormBackColor;
            Margin = new Padding(0);
            Padding = new Padding(3);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       c1,
                                                                       c2,
                                                                       45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            using (SolidBrush bg = new SolidBrush(C.FormBackColor))
            {
                e.Graphics.FillRectangle(bg, 1, 1, Width-2, Height-2);
            }
        }

    }
}
