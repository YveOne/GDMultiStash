using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.StashTabsEditor
{

    internal abstract class StashTabBasePanel : Panel
    {
        public static int TabsMargin => 5;

        public Image HoverImage { get; set; }
        private bool hover = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (hover)
                e.Graphics.DrawImage(HoverImage, 0, 0, Width, Height);
        }

        public StashTabBasePanel(StashObject stashObject)
        {
            var stashInfo = TransferFile.GetStashInfoForExpansion(stashObject.Expansion);
            switch (stashInfo.Width)
            {
                case 8:
                    HoverImage = Properties.Resources.caravanWindow8x16hover;
                    break;
                default:
                    HoverImage = Properties.Resources.caravanWindow10x18hover;
                    break;
            }
            Width = HoverImage.Width;
            Height = HoverImage.Height;
            BackgroundImageLayout = ImageLayout.Stretch;

            MouseEnter += delegate { hover = true; Invalidate(); };
            MouseLeave += delegate { hover = false; Invalidate(); };

            Margin = new Padding(TabsMargin, TabsMargin, TabsMargin, TabsMargin);
            Cursor = Cursors.Hand;
        }
    }

}
