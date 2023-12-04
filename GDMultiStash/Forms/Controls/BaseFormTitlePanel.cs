using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GDMultiStash.Forms.Controls
{
    public class BaseFormTitlePanel : TransparentPanel
    {


        public BaseFormTitlePanel()
        {

            BackColor = C.FormTitleBackColor;
            Height = 30;
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            Icon = new TransparentPictureBox();
            Icon.Location = new Point(0, 0);
            Icon.Margin = new Padding(0);
            Icon.Name = "Icon";
            Icon.Size = new Size(30, 30);
            Icon.Image = Properties.Resources.icon32.ToBitmap();
            Icon.SizeMode = PictureBoxSizeMode.StretchImage;

            ControlBox = new TransparentFlowLayoutPanel();
            ControlBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ControlBox.AutoSize = true;
            ControlBox.BackColor = BackColor;
            ControlBox.FlowDirection = FlowDirection.RightToLeft;
            ControlBox.Size = new Size(150, 30);
            ControlBox.Location = new Point(Width - 150, 0);
            ControlBox.Margin = new Padding(0);
            ControlBox.Name = "ControlBox";
            ControlBox.TabIndex = 0;

            CloseButton = new ControlBoxButton();
            CloseButton.Name = "CloseButton";
            CloseButton.Image = Properties.Resources.buttonCloseGray;
            CloseButton.ImageHover = Properties.Resources.buttonCloseWhite;
            CloseButton.BackColorHover = Color.FromArgb(150, 32, 5); // TODO: put me inside Constants.cs
            CloseButton.BackColorPressed = Color.FromArgb(207, 49, 12); // TODO: put me inside Constants.cs

            MinimizeButton = new ControlBoxButton();
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Image = Properties.Resources.buttonMinimizeGray;
            MinimizeButton.ImageHover = Properties.Resources.buttonMinimizeWhite;

            Controls.Add(Icon);
            Controls.Add(ControlBox);
            ControlBox.Controls.Add(CloseButton);
            ControlBox.Controls.Add(MinimizeButton);
        }


        internal TransparentPictureBox Icon;
        internal TransparentFlowLayoutPanel ControlBox;
        internal ControlBoxButton CloseButton;
        internal ControlBoxButton MinimizeButton;

    }
}
