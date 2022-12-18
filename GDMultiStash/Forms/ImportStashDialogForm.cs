using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms
{
    internal partial class ImportDialogForm : DialogForm
    {

        public ImportDialogForm() : base()
        {
            InitializeComponent();

        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = L.ImportButton();

            transferFileLabel.Text = L.TransferFileLabel();
            stashNameLabel.Text = L.StashNameLabel();
            expansionLabel.Text = L.ExpansionLabel();
            okButton.Text = L.ImportButton();
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
        }

        private void ImportForm_Shown(object sender, EventArgs e)
        {
            nameTextBox.Focus();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close(DialogResult.OK);
        }

        private void overwriteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            nameTextBox.Visible = true;
            okButton.Enabled = true;
            scCheckBox.Visible = true;
            hcCheckBox.Visible = true;
            stashNameLabel.Visible = true;
        }






















        #region Dialog

        private readonly List<StashObject> _importedStashes = new List<StashObject>();

        public DialogResult ShowDialog(IWin32Window owner, string srcFile)
        {
            if (!File.Exists(srcFile)) return DialogResult.None;

            GrimDawnGameExpansion exp = Common.TransferFile.GetExpansionByFile(srcFile);
            if (exp == GrimDawnGameExpansion.Unknown)
            {
                MessageBox.Show(Global.L.InvalidTransferFileMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            stashFileTextBox.Text = srcFile;
            nameTextBox.Text = Path.GetFileNameWithoutExtension(srcFile);
            expansionTextBox.Text = GrimDawn.GetExpansionName(exp);

            scCheckBox.Checked = Global.Configuration.Settings.ShowSoftcore;
            hcCheckBox.Checked = Global.Configuration.Settings.ShowHardcore;

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            GrimDawnGameMode mode = GrimDawnGameMode.None;
            if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
            if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
            StashObject stash = Global.Stashes.CreateImportStash(srcFile, nameTextBox.Text, exp, mode);

            if (stash != null)
            {
                Global.Runtime.ReloadOpenedStash(stash.ID);
                _importedStashes.Add(stash);
            }

            return result;
        }





        public DialogResult ShowDialog(IWin32Window owner, out StashObject[] importedStashes)
        {
            importedStashes = new StashObject[0];
            DialogResult result = GrimDawn.ShowSelectTransferFilesDialog(out string[] files, true);
            if (result == DialogResult.OK)
                return ShowDialog(owner, files, out importedStashes);
            return result;
        }

        public DialogResult ShowDialog(IWin32Window owner, IEnumerable<string> files, out StashObject[] importedStashes)
        {
            importedStashes = null;
            if (files.Count() != 0)
            {
                foreach (string srcFile in files)
                    ShowDialog(owner, srcFile);
                
                importedStashes = _importedStashes.ToArray();
                _importedStashes.Clear();

                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        #endregion

    }
}
