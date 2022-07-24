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

namespace GDMultiStash.Forms
{
    internal partial class ImportDialogForm : DialogForm
    {

        public ImportDialogForm() : base()
        {
            InitializeComponent();
            overwriteComboBox.SelectionChangeCommitted += overwriteComboBox_SelectionChangeCommitted;
        }

        private string _err_invalid_transfer_file;
        private string _err_no_stash_selected;

        protected override void Localize(Core.Localization.StringsProxy L)
        {
            Text = "GDMultiStash : " + L["window_import_stash"];

            _err_invalid_transfer_file = L["err_invalid_transfer_file"];
            _err_no_stash_selected = L["err_no_stash_selected"];

            label1.Text = L["label_transfer_file"];
            label3.Text = L["label_stash_name"];
            label2.Text = L["label_overwrite"];
            expansionLabel.Text = L["label_expansion"];
            okButton.Text = L["button_import"];
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
            if (overwriteCheckBox.Checked)
            {
                nameTextBox.Visible = false;
                overwriteComboBox.Enabled = true;
                okButton.Enabled = false;
                scCheckBox.Visible = false;
                hcCheckBox.Visible = false;
                label3.Visible = false;
            }
            else
            {
                nameTextBox.Visible = true;
                overwriteComboBox.SelectedIndex = -1;
                overwriteComboBox.Enabled = false;
                okButton.Enabled = true;
                scCheckBox.Visible = true;
                hcCheckBox.Visible = true;
                label3.Visible = true;
            }
        }

        private void overwriteComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (overwriteCheckBox.Checked && overwriteComboBox.SelectedIndex != -1)
            {
                okButton.Enabled = true;
            }
        }






















        #region Dialog

        private readonly List<Common.Stash> _importedStashes = new List<Common.Stash>();

        public DialogResult ShowDialog(IWin32Window owner, string srcFile)
        {
            if (!File.Exists(srcFile)) return DialogResult.None;

            GrimDawnGameExpansion exp = Common.TransferFile.GetExpansionByFile(srcFile);
            if (exp == GrimDawnGameExpansion.Unknown)
            {
                MessageBox.Show(_err_invalid_transfer_file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            overwriteCheckBox.Checked = false;
            stashFileTextBox.Text = srcFile;
            nameTextBox.Text = Path.GetFileNameWithoutExtension(srcFile);
            expansionTextBox.Text = GrimDawn.GetExpansionName(exp);

            overwriteComboBox.DisplayMember = "Name";
            overwriteComboBox.ValueMember = "ID";
            overwriteComboBox.DataSource = Core.Stashes.GetStashesForExpansion(exp);
            overwriteComboBox.SelectedIndex = -1;
            overwriteComboBox.Enabled = false;

            GrimDawnGameMode mode = (GrimDawnGameMode)Core.Config.DefaultStashMode;
            scCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.SC);
            hcCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.HC);

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            Common.Stash stash = null;
            if (overwriteCheckBox.Checked)
            {
                if (overwriteComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(_err_no_stash_selected, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DialogResult.None;
                }
                stash = Core.Stashes.ImportOverwriteStash(srcFile, (int)overwriteComboBox.SelectedValue);
            }
            else
            {
                mode = GrimDawnGameMode.None;
                if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
                if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
                stash = Core.Stashes.ImportStash(srcFile, nameTextBox.Text, exp, mode);
            }
            if (stash != null)
            {
                Core.Runtime.ReloadOpenedStash(stash.ID);
                _importedStashes.Add(stash);
            }

            return result;
        }





        public DialogResult ShowDialog(IWin32Window owner, out Common.Stash[] importedStashes)
        {
            importedStashes = null;
            string filter = string.Join(";", GrimDawn.GetAllTransferExtensions().Select(ext => "*" + ext));

            using (var dialog = new OpenFileDialog()
            {
                Filter = "Transfer File|" + filter,
                Multiselect = true,
            })
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return ShowDialog(owner, dialog.FileNames, out importedStashes);
                }
            }
            return DialogResult.Cancel;
        }

        public DialogResult ShowDialog(IWin32Window owner, IEnumerable<string> files, out Common.Stash[] importedStashes)
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
