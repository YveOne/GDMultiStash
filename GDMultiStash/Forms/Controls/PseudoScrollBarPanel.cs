using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.ComponentModel;

namespace GDMultiStash.Forms.Controls
{
    [DesignerCategory("code")]
    internal class PseudoScrollBarPanel : TransparentPanel, IDisposable
    {

        private int _barWidth = 10;
        public int BarWidth
        {
            get => _barWidth;
            set
            {
                if (value == _barWidth) return;
                _barWidth = value;
                UpdateBar();
            }
        }

        private Color _barColor = Color.Red;
        public Color BarColor
        {
            get => _barColor;
            set
            {
                if (value == _barColor) return;
                _barColor = value;
                UpdateBarColor();
            }
        }






        private readonly OLVCatchScrolling olv;
        private int scrollPos = 0;
        private readonly TransparentPanel _bar;




        public PseudoScrollBarPanel(OLVCatchScrolling olv)
        {
            this.olv = olv;
            Control parent = olv.Parent;
            Width = SystemInformation.VerticalScrollBarWidth;
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            Height = olv.Height;
            Location = new Point(parent.Width - Width - parent.Padding.Right, parent.Padding.Top);
            parent.Controls.Add(this);
            BringToFront();

            _bar = new TransparentPanel() {
                Width = 1,
                Height = 1,
            };
            Controls.Add(_bar);
            UpdateBar(true);
            UpdateBarColor();

            olv.NCCalcSize += delegate (object sender, OLVCatchScrolling.NCCalcSizeArgs eee) {
                Visible = eee.vScroll;
                UpdateBar(true);
            };
            olv.Scroll += delegate {
                UpdateBar(true);
            };
            olv.VerticalScrolling += delegate (object sender, OLVCatchScrolling.VerticalScrollingEventArgs eee) {
                scrollPos = eee.Y;
                UpdateBar(false);
            };
        }

        private void UpdateBarColor()
        {
            _bar.BackColor = _barColor;
        }

        private void UpdateBar(bool updatePos = false)
        {
            olv.GetScrollPosition(out int min, out int max, out int _pos, out int page);
            if (updatePos) scrollPos = _pos;

            int marginV = 0;
            int scrollAreaHeight = Height - marginV * 2;
            int scrollBarPosition = marginV + (int)(scrollAreaHeight * ((float)scrollPos / (float)max));
            int scrollBarHeight = (int)(scrollAreaHeight * ((float)page / (float)max));

            _bar.Width = _barWidth;
            _bar.Location = new Point(Width - _barWidth, scrollBarPosition);
            _bar.Width = _barWidth;
            _bar.Height = scrollBarHeight;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }


    }
}
