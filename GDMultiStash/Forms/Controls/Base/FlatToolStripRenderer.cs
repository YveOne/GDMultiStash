using System.Windows.Forms;
using System.Drawing;
namespace GDMultiStash.Forms.Controls
{
    internal class FlatToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.OwnerItem == null)
            {
                // first level buttons
                Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rc);
                return;
            }
            //if (!e.Item.Selected) base.OnRenderMenuItemBackground(e);
            base.OnRenderMenuItemBackground(e);
        }
    }
}