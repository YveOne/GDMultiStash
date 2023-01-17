using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GDMultiStash.GlobalHandlers.Base
{
    internal abstract class BaseHandler
    {
        private static Form _saveInvokeForm = null;

        public BaseHandler()
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

        protected void SaveInvoke(Action a)
        {
            _saveInvokeForm.Invoke(a);
        }

    }
}
