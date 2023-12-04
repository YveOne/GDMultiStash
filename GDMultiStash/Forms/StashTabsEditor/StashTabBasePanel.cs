using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using System.ComponentModel;

namespace GDMultiStash.Forms.StashTabsEditor
{

    [DesignerCategory("code")]
    internal abstract class StashTabBasePanel : Panel
    {
        public static int TabsMargin => 5;

        public Image HoverImage { get; set; }
        private int highlightCount = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Console.WriteLine(highlightCount);
            if (highlightCount > 0)
                e.Graphics.DrawImage(HoverImage, 0, 0, Width, Height);
        }

        public void ShowHighlight()
        {
            highlightCount += 1;
            Invalidate();
        }

        public void HideHighlight()
        {
            highlightCount -= 1;
            Invalidate();
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

            MouseEnter += delegate { ShowHighlight(); };
            MouseLeave += delegate { HideHighlight(); };

            Margin = new Padding(TabsMargin, TabsMargin, TabsMargin, TabsMargin);
            Cursor = Cursors.Hand;
        }
    }

}
