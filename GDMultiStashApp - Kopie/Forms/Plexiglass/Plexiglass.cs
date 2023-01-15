using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace GDMultiStash.Forms.Plexiglass
{
    internal class Plexiglass
    {
        internal class PlexiForm : Form
        {
            public PlexiForm()
            {
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                this.ControlBox = false;
                this.ShowInTaskbar = false;
                this.ShowIcon = false;
                this.StartPosition = FormStartPosition.Manual;
                this.AutoScaleMode = AutoScaleMode.None;
                this.Padding = Padding.Empty;
            }
            
            protected override CreateParams CreateParams
            {
                // i am using this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // to hide this form from alt+tab
                // and the following is to remove border+titlebar 
                get
                {
                    int WS_CAPTION = 0x00C00000;
                    CreateParams result = base.CreateParams;
                    result.Style &= ~WS_CAPTION;
                    return result;
                }
            }
            
            protected override void OnLoad(EventArgs e)
            {
                if (this.Owner != null && Environment.OSVersion.Version.Major >= 6)
                {
                    int value = 1;
                    DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
                }
                base.OnLoad(e);
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                if (this.Owner != null && !this.Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
                {
                    int value = 1;
                    DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
                }
                base.OnFormClosing(e);
            }

            protected override void OnActivated(EventArgs e)
            {
                if (this.Owner != null)
                    this.BeginInvoke(new Action(() => this.Owner.Activate()));
            }

            private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
            [DllImport("dwmapi.dll")]
            private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        }

        private readonly PlexiForm _form;
        private bool _shown = false;
        private double _opacity = 0.5;
        private Point _location = Point.Empty;
        private Size _size = Size.Empty;
        private Color _color = Color.Black;

        public Plexiglass()
        {
            _form = new PlexiForm();
            _form.Opacity = 0;
            _form.BackColor = _color;
        }

        public virtual void Show(bool bringToFront = false)
        {
            if (_shown) return;
            _shown = true;
            if (!_form.IsDisposed) _form.Opacity = _opacity;
            if (bringToFront) _form.BringToFront();
        }

        public virtual void Hide()
        {
            if (!_shown) return;
            _shown = false;
            if (!_form.IsDisposed) _form.Opacity = 0;
        }

        protected void ShowForm(Control parent)
        {
            _form.Show(parent);
        }

        protected void ShowForm()
        {
            _form.Show();
        }







        public bool Shown => _shown;

        public double Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                if (_shown) _form.Opacity = value;
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _form.BackColor = value;
            }
        }

        public Point Location
        {
            get => _location;
            protected set
            {
                _location = value;
                _form.Location = value;
            }
        }

        public Size Size
        {
            get => _size;
            protected set
            {
                _size = value;
                _form.ClientSize = value;
            }
        }






    }
}
