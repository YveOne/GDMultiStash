using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BrightIdeasSoftware;

namespace GDMultiStash.Forms.MainWindow
{
    internal class DecorationCollection
    {

        private class MyCellBorderDecoration : CellBorderDecoration
        {
            private readonly int w;
            public MyCellBorderDecoration(int w, Color col)
            {
                this.w = w;
                BorderPen = new Pen(col);
                FillBrush = null;
                BoundsPadding = new Size(0, 0);
                CornerRounding = 0;
            }
            protected override Rectangle CalculateBounds()
            {
                Rectangle r = base.CellBounds;
                r.X -= 1;
                r.Y -= 1;
                r.Width += w;
                r.Height += 1;
                return r;
            }
        }

        private class MyRowBorderDecoration : RowBorderDecoration
        {
            private readonly int w;
            public MyRowBorderDecoration(int w, Color col)
            {
                this.w = w;
                BorderPen = new Pen(col);
                FillBrush = null;
                BoundsPadding = new Size(0, 0);
                CornerRounding = 0;
            }
            protected override Rectangle CalculateBounds()
            {
                Rectangle r = base.CellBounds;
                r.X -= 1;
                r.Y = r.Y - 2 + r.Height;
                r.Width += w;
                r.Height = 1;
                return r;
            }
        }

        public CellBorderDecoration CellBorderFirstDecoration { get; }
        public CellBorderDecoration CellBorderDecoration { get; }
        public RowBorderDecoration RowBorderFirstDecoration { get; }
        public RowBorderDecoration RowBorderDecoration { get; }

        public ImageDecoration LockIconDecoration { get; }
        public ImageDecoration HomeIconDecoration { get; }

        public ImageDecoration CheckBoxHideDecoration { get; }
        public ImageDecoration CheckBoxBackDecoration { get; }
        public ImageDecoration CheckBoxBackHoverDecoration { get; }
        public ImageDecoration CheckBoxTickDecoration { get; }
        public ImageDecoration CheckBoxTickDisabledDecoration { get; }
        public ImageDecoration CheckBoxCrossDecoration { get; }
        public ImageDecoration CheckBoxCrossDisabledDecoration { get; }

        public DecorationCollection()
        {

            CellBorderFirstDecoration = new MyCellBorderDecoration(-1, Constants.ListViewCellBorderColor);
            CellBorderDecoration = new MyCellBorderDecoration(0, Constants.ListViewCellBorderColor);
            RowBorderFirstDecoration = new MyRowBorderDecoration(-1, Constants.ListViewCellBorderColor);
            RowBorderDecoration = new MyRowBorderDecoration(0, Constants.ListViewCellBorderColor);

            using (Bitmap bmp = new Bitmap(15, 15, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (SolidBrush brush = new SolidBrush(Constants.ListViewBackColor))
            {
                gfx.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                CheckBoxHideDecoration = new ImageDecoration(new Bitmap(bmp), ContentAlignment.MiddleCenter)
                {
                    Transparency = 255,
                    Offset = new Size(-1, -1),
                    ShrinkToWidth = false,
                };
            }

            LockIconDecoration = new ImageDecoration(new Bitmap(Properties.Resources.LockWhiteIcon, new Size(15, 15)), ContentAlignment.MiddleRight)
            {
                Transparency = 180,
                Offset = new Size(-5, 0),
            };
            HomeIconDecoration = new ImageDecoration(new Bitmap(Properties.Resources.HomeIcon, new Size(15, 15)), ContentAlignment.MiddleRight)
            {
                Transparency = 180,
                Offset = new Size(-5, 0),
            };
            
            CheckBoxBackDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxBack, new Size(15, 15)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            CheckBoxBackHoverDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxBackHover, new Size(15, 15)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            CheckBoxTickDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxTick, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            CheckBoxTickDisabledDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxTickDisabled, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            CheckBoxCrossDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxCross, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            CheckBoxCrossDisabledDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxCrossDisabled, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };

        }

    }
}
