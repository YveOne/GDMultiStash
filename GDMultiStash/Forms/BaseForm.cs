using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace GDMultiStash.Forms
{
    internal class BaseForm : Form
    {

        private static readonly Core.Localization.StringsProxy L = new Core.Localization.StringsProxy();

        public BaseForm() : base()
        {
            Load += delegate {
                Localize(L);
            };
        }

        protected virtual void Localize(Core.Localization.StringsProxy L)
        { 
        }

        public void Localize()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Localize(); });
                return;
            }
            Localize(L);
        }

    }
}
