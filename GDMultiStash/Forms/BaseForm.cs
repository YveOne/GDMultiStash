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

        public BaseForm() : base()
        {
            Load += delegate {
                Utils.Funcs.RunThread(1, () => {
                    Utils.Funcs.Invoke(this, () => {
                        Localize(Global.L);
                    });
                });
            };
        }

        protected virtual void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
        }

        public void Localize()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Localize(); }));
                return;
            }
            Localize(Global.L);
        }

    }
}
