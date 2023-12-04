using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace GDMultiStash.Forms.Controls
{

    [DesignerCategory("code")]
    public class TransparentPictureBox : PictureBox
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }

    [DesignerCategory("code")]
    public class TransparentUserControl : UserControl
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }



    [DesignerCategory("code")]
    public class TransparentSplitContainer : SplitContainer
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }





    [DesignerCategory("code")]
    public class TransparentPanel : Panel
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }

    [DesignerCategory("code")]
    public class TransparentFlowLayoutPanel : FlowLayoutPanel
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }

    [DesignerCategory("code")]
    public class TransparentLabel : Label
    {
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }
}
