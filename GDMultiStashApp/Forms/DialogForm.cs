using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal class DialogForm : BaseForm
    {

        private DialogResult _result;

        public DialogForm() : base()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void DialogForm_Load(object sender, EventArgs e)
        {
            TopMost = false;
        }

        public void Close(DialogResult result)
        {
            _result = result;
            base.Close();
        }

        public new void Close()
        {
            _result = DialogResult.None;
            base.Close();
        }

        public virtual new DialogResult ShowDialog(IWin32Window owner)
        {
            _result = DialogResult.None;
            base.ShowDialog(owner);
            return _result;
        }

    }

}
