using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GDMultiStash.Global.Base
{
    internal abstract class Manager
    {
        private static Form _saveInvokeForm = null;

        public Manager()
        {
            if (_saveInvokeForm == null)
            {
                // its ugly ... but its working
                _saveInvokeForm = new Form()
                {
                    Visible = false,
                    Opacity = 0,
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    ShowInTaskbar = false,
                };
                _saveInvokeForm.Shown += delegate { _saveInvokeForm.Hide(); };
                _saveInvokeForm.Show();
            }
        }

        protected static void SaveInvoke(Action a)
        {
            _saveInvokeForm.Invoke(a);
        }

        internal virtual void Initialize()
        {

        }

    }
}
