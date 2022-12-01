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
        }

        public override void Initialize()
        {
            base.Initialize();
            overwriteComboBox.SelectionChangeCommitted += overwriteComboBox_SelectionChangeCommitted;
        }

        private string _msg_invalid_transfer_file;
        private string _msg_no_stash_selected;

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsProxy L)
        {
            Text = L["importWindow"];

            _msg_invalid_transfer_file = L["msg_invalid_transfer_file"];
            _msg_no_stash_selected = L["msg_no_stash_selected"];

            transferFileLabel.Text = L["importWindow_transferFileLabel"];
            stashNameLabel.Text = L["importWindow_stashNameLabel"];
            overwriteLabel.Text = L["importWindow_overwriteLabel"];
            expansionLabel.Text = L["importWindow_expansionLabel"];
            okButton.Text = L["importWindow_okButton"];
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
                stashNameLabel.Visible = false;
            }
            else
            {
                nameTextBox.Visible = true;
                overwriteComboBox.SelectedIndex = -1;
                overwriteComboBox.Enabled = false;
                okButton.Enabled = true;
                scCheckBox.Visible = true;
                hcCheckBox.Visible = true;
                stashNameLabel.Visible = true;
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

        private readonly List<Common.StashObject> _importedStashes = new List<Common.StashObject>();

        public DialogResult ShowDialog(IWin32Window owner, string srcFile)
        {
            if (!File.Exists(srcFile)) return DialogResult.None;

            GrimDawnGameExpansion exp = Common.TransferFile.GetExpansionByFile(srcFile);
            if (exp == GrimDawnGameExpansion.Unknown)
            {
                MessageBox.Show(_msg_invalid_transfer_file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.None;
            }

            overwriteCheckBox.Checked = false;
            stashFileTextBox.Text = srcFile;
            nameTextBox.Text = Path.GetFileNameWithoutExtension(srcFile);
            expansionTextBox.Text = GrimDawn.GetExpansionName(exp);

            overwriteComboBox.DisplayMember = "Name";
            overwriteComboBox.ValueMember = "ID";
            overwriteComboBox.DataSource = Global.Stashes.GetStashesForExpansion(exp);
            overwriteComboBox.SelectedIndex = -1;
            overwriteComboBox.Enabled = false;

            GrimDawnGameMode mode = (GrimDawnGameMode)Global.Configuration.Settings.DefaultStashMode;
            scCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.SC);
            hcCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.HC);

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            Common.StashObject stash = null;
            if (overwriteCheckBox.Checked)
            {
                if (overwriteComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(_msg_no_stash_selected, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DialogResult.None;
                }
                stash = Global.Stashes.ImportOverwriteStash(srcFile, (int)overwriteComboBox.SelectedValue);
            }
            else
            {
                mode = GrimDawnGameMode.None;
                if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
                if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
                stash = Global.Stashes.ImportStash(srcFile, nameTextBox.Text, exp, mode);
            }
            if (stash != null)
            {
                Global.Runtime.ReloadOpenedStash(stash.ID);
                _importedStashes.Add(stash);
            }

            return result;
        }





        public DialogResult ShowDialog(IWin32Window owner, out Common.StashObject[] importedStashes)
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

        public DialogResult ShowDialog(IWin32Window owner, IEnumerable<string> files, out Common.StashObject[] importedStashes)
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
