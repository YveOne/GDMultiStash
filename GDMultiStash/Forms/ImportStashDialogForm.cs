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

        public DialogResult ShowDialog(IWin32Window owner, string srcFile)
        {
            if (!File.Exists(srcFile)) return DialogResult.None;

            GrimDawnGameEnvironment env = GrimDawn.GetEnvironmentByFilename(srcFile);
            if (env == null)
            {
                MessageBox.Show(_err_invalid_transfer_file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            if (!Common.TransferFile.ValidateFile(srcFile))
            {
                MessageBox.Show(_err_invalid_transfer_file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            overwriteCheckBox.Checked = false;
            stashFileTextBox.Text = srcFile;
            nameTextBox.Text = Path.GetFileNameWithoutExtension(srcFile);
            scCheckBox.Checked = env.Mode.HasFlag(GrimDawnGameMode.SC);
            hcCheckBox.Checked = env.Mode.HasFlag(GrimDawnGameMode.HC);
            expansionTextBox.Text = GrimDawn.GetExpansionName(env.Expansion);

            overwriteComboBox.DisplayMember = "Name";
            overwriteComboBox.ValueMember = "ID";
            overwriteComboBox.DataSource = Core.Stashes.GetStashesForExpansion(env.Expansion);
            overwriteComboBox.SelectedIndex = -1;
            overwriteComboBox.Enabled = false;

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            if (overwriteCheckBox.Checked)
            {
                if (overwriteComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(_err_no_stash_selected, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DialogResult.None;
                }
                Core.Stashes.ImportOverwriteStash(srcFile, (int)overwriteComboBox.SelectedValue);
            }
            else
            {
                GrimDawnGameMode mode = GrimDawnGameMode.None;
                if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
                if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
                Core.Stashes.ImportStash(srcFile, nameTextBox.Text, env.Expansion, mode);
            }

            return result;
        }

        public DialogResult ShowDialog(IWin32Window owner, IEnumerable<string> files)
        {
            if (files.Count() != 0)
            {
                string[] okExt = GrimDawn.GetAllTransferExtensions();
                foreach (string srcFile in files)
                {
                    if (!okExt.Contains(Path.GetExtension(srcFile).ToLower()))
                    {
                        //TODO: show warning?
                        continue;
                    }
                    ShowDialog(owner, srcFile);
                }
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        public override DialogResult ShowDialog(IWin32Window owner)
        {
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
                    return ShowDialog(owner, dialog.FileNames);
                }
            }
            return DialogResult.Cancel;
        }

        #endregion

    }
}
