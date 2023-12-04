using System.Windows.Forms;
using System.Drawing;

namespace GDMultiStash.Forms.Controls.Base
{

    internal class CustomToolStripColorTable : ProfessionalColorTable
    {
        private Color _MenuItemBorder = Color.Empty;
        public override Color MenuItemBorder => _MenuItemBorder;
        public void SetMenuItemBorder(Color c)
        {
            _MenuItemBorder = c;
        }

        private Color _MenuBorder = Color.Empty;
        public override Color MenuBorder => _MenuBorder;

        public void SetMenuBorder(Color c)
        {
            _MenuBorder = c;
        }

    }
    
    internal class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {

        public Color FirstBackColor = Color.White;
        public Color FirstBackColorHover = Color.White;
        public Color FirstBackColorSelected = Color.White;

        public Color BackColor = Color.White;
        public Color BackColorSelected = Color.White;
        public Color ForeColor = Color.Black;
        public Color ForeColorSelected = Color.Black;
        public Color BorderColor = Color.White;

        public CustomToolStripRenderer() : base()
        {
        }

        public CustomToolStripRenderer(ProfessionalColorTable colorTable) : base(colorTable)
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);

            if (e.Item.OwnerItem == null)
            {
                // first level buttons
                e.Item.Overflow = ToolStripItemOverflow.Never;

                Color c1 = e.Item.Pressed ? FirstBackColorSelected : (
                    e.Item.Selected ? FirstBackColorHover : FirstBackColor
                    );
                e.Graphics.FillRectangle(new SolidBrush(c1), rc);
                return;
            }

            Color c = e.Item.Selected ? BackColorSelected : BackColor;
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);

        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Color c = e.Item.Selected ? ForeColorSelected : ForeColor;
            if (e.Item.OwnerItem == null)
            {
                //base.OnRenderItemText(e);
                //return;
                if (e.Item.Pressed)
                    c = ForeColorSelected; ;
            }
            
            e.TextColor = c;
            base.OnRenderItemText(e);
        }


        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem)
            {
                Color c = e.Item.Selected ? ForeColorSelected : ForeColor;
                e.ArrowColor = c;
            }
            base.OnRenderArrow(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
            e.Graphics.FillRectangle(new SolidBrush(ColorTable.MenuBorder), e.ConnectedArea);
            e.Graphics.DrawRectangle(new Pen(ColorTable.MenuBorder), new Rectangle(0, 1, e.AffectedBounds.Width - 2, e.AffectedBounds.Height - 3));
        }









    }
}
