using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GDMultiStash.Forms.Plexiglass
{
    internal class ScreenPlexiglass : Plexiglass
    {
        public ScreenPlexiglass() : base()
        {

            base.ShowForm();
            Location = Point.Empty;
            Size = Screen.PrimaryScreen.WorkingArea.Size;




        }






    }
}
