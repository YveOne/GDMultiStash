using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal partial class BaseForm : Form
    {

        public BaseForm() : base()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            Load += delegate {
                Utils.Funcs.RunThread(1, () => {
                    Initialize();
                    Localize();
                });
            };
            Deactivate += delegate
            {
                // when form lost focus
                // clear any focused control
                Focus();
            };
        }

        #region Properties

        public bool Resizable { get; set; } = true;
        public int CaptionHeight { get; set; } = 0;

        #endregion

        #region Initialize & Localize

        protected virtual void Initialize()
        {
        }

        protected virtual void Localize(Global.LocalizationManager.StringsHolder L)
        {
        }

        public void Localize()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Localize(); }));
                return;
            }
            Localize(G.L);
        }

        #endregion

        #region WndProc 

        Rectangle BorderTop { get { return new Rectangle(0, 0, ClientSize.Width, C.WindowResizeBorderSize); } }
        Rectangle BorderLeft { get { return new Rectangle(0, 0, C.WindowResizeBorderSize, ClientSize.Height); } }
        Rectangle BorderBottom { get { return new Rectangle(0, ClientSize.Height - C.WindowResizeBorderSize, ClientSize.Width, C.WindowResizeBorderSize); } }
        Rectangle BorderRight { get { return new Rectangle(ClientSize.Width - C.WindowResizeBorderSize, 0, C.WindowResizeBorderSize, ClientSize.Height); } }

        protected override CreateParams CreateParams
        {
            get
            {
                // this is required to be able
                // to minimize/restore the window by click
                // on taskbar item
                CreateParams cp = base.CreateParams;
                cp.Style |= Native.WS.MINIMIZEBOX;
                cp.ClassStyle |= Native.CS_DBLCLKS;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_NCLBUTTONDBLCLK)
                return; // disable doubleclick on titlebar

            base.WndProc(ref m);

            if (m.Msg == 0x84)
            {
                var cursor = PointToClient(Cursor.Position);

                bool tHit = BorderTop.Contains(cursor);
                bool lHit = BorderLeft.Contains(cursor);
                bool rHit = BorderRight.Contains(cursor);
                bool bHit = BorderBottom.Contains(cursor);
                if (cursor.Y < CaptionHeight)
                {
                    m.Result = (IntPtr)Native.HT.CAPTION;
                }
                else if (Resizable)
                {
                    if (bHit && rHit) m.Result = (IntPtr)Native.HT.BOTTOMRIGHT;
                    else if (bHit && lHit) m.Result = (IntPtr)Native.HT.BOTTOMLEFT;
                    else if (tHit && rHit) m.Result = (IntPtr)Native.HT.TOPRIGHT;
                    else if (tHit && lHit) m.Result = (IntPtr)Native.HT.TOPLEFT;
                    else if (bHit) m.Result = (IntPtr)Native.HT.BOTTOM;
                    else if (rHit) m.Result = (IntPtr)Native.HT.RIGHT;
                    else if (lHit) m.Result = (IntPtr)Native.HT.LEFT;
                    else if (tHit) m.Result = (IntPtr)Native.HT.TOP;
                }


            }

        }

        #endregion

    }
}
