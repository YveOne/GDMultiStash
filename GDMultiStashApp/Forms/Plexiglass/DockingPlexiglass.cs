using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GDMultiStash.Forms.Plexiglass
{
    internal class DockingPlexiglass : Plexiglass
    {

        private Control _target;
        private Padding _margin = Padding.Empty;

        public DockingPlexiglass(Control target) : base()
        {
            this._target = target;

            base.ShowForm(target); // target.ParentForm
            target.Focus();


            target.ClientSizeChanged += Any_ClientSizeChanged;

            Control _parentForm = null;
            Control _parentControl = target;
            while (_parentControl != null)
            {
                _parentForm = _parentControl;
                _parentControl.LocationChanged += Any_LocationChanged;
                _parentControl = _parentControl.Parent;
            }
            if (_parentForm is Form _f)
            {
                _f.Deactivate += delegate { base.Show(); };
                _f.Activated += delegate { base.Hide(); };
            }


            this.UpdateLocation();
            this.UpdateSize();
        }

        private void UpdateLocation()
        {
            var p = _target.PointToScreen(Point.Empty);
            base.Location = new Point(
                p.X + _margin.Left,
                p.Y + _margin.Top
                );
        }

        private void UpdateSize()
        {
            var s = _target.ClientSize;
            base.Size = new Size(
                s.Width - _margin.Horizontal,
                s.Height - _margin.Vertical
                );
        }





        private void Any_LocationChanged(object sender, EventArgs e)
        {
            UpdateLocation();
        }

        private void Any_ClientSizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }




        public Padding Margin
        {
            get => _margin;
            set {
                _margin = value;
                UpdateLocation();
                UpdateSize();
            }
        }



        


    }
}
