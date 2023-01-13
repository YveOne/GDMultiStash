using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal partial class ProgressDialogForm : DialogForm
    {

        public delegate void ActionDelegate(ProgressHandler methods);

        public class ProgressHandler
        {
            private ProgressDialogForm Form;
            public FormClosingEventHandler FormClosing;
            public FormClosedEventHandler FormClosed;
            public ProgressHandler(ProgressDialogForm Form)
            {
                this.Form = Form;
            }
            public void SetProgress()
            {
                Form.Invoke(new Action(() => { 
                    Form.progressBar1.Minimum = 0;
                    Form.progressBar1.Maximum = 100;
                    Form.progressBar1.Value = 50;
                }));
            }
            public void Close(bool force = false)
            {
                Form.Invoke(new Action(() => {
                    if (force) Form.ForceClose();
                    else Form.Close();
                }));
            }
        }

        private ProgressHandler invoker;
        private Thread runningThread = null;
        private bool forcedClose = false;

        public ProgressDialogForm() : base()
        {
            InitializeComponent();
            FormClosing += delegate (object sender, FormClosingEventArgs e) {
                if (!forcedClose) invoker?.FormClosing?.Invoke(sender, e);
            };
            FormClosed += delegate (object sender, FormClosedEventArgs e) {
                if (!forcedClose) invoker?.FormClosed?.Invoke(sender, e);
            };
        }

        public void ForceClose()
        {
            forcedClose = true;
            Close();
        }

        public DialogResult ShowDialog(IWin32Window owner, ActionDelegate action)
        {
            forcedClose = false;
            invoker = new ProgressHandler(this);
            runningThread =  new Thread(() => {
                Thread.Sleep(100);
                action(invoker);
            });
            runningThread.Start();
            base.ShowDialog(owner);
            return DialogResult.OK;
        }


    }
}
