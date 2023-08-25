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

        private int lastSelectedGroupIndex = 0;

        public ImportDialogForm() : base()
        {
            InitializeComponent();

            Load += delegate {
                groupComboBox.DisplayMember = "Name";
                groupComboBox.ValueMember = "Key";
                groupComboBox.DataSource = Global.Groups.GetSortedGroups();
                groupComboBox.SelectedIndex = lastSelectedGroupIndex;
            };

            groupComboBox.SelectionChangeCommitted += delegate {
                lastSelectedGroupIndex = groupComboBox.SelectedIndex;
            };

            nameTextBox.KeyDown += delegate (object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                    okButton.PerformClick();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = L.ImportButton();

            transferFileLabel.Text = L.TransferFileLabel();
            stashNameLabel.Text = L.StashNameLabel();
            expansionLabel.Text = L.ExpansionLabel();
            groupLabel.Text = L.GroupLabel();
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
























        #region Dialog

        private readonly List<StashObject> _importedStashes = new List<StashObject>();

        public DialogResult ShowDialog(IWin32Window owner, string srcFile)
        {
            if (!File.Exists(srcFile)) return DialogResult.None;





            GrimDawnGameExpansion exp = Common.TransferFile.GetExpansionByFile(srcFile);
            if (exp == GrimDawnGameExpansion.Unknown)
            {
                Console.Error(Global.L.InvalidTransferFileMessage());
                return DialogResult.None;
            }

            stashFileTextBox.Text = srcFile;
            nameTextBox.Text = Path.GetFileNameWithoutExtension(srcFile);
            expansionTextBox.Text = GrimDawn.ExpansionNames[exp];

            scCheckBox.Checked = Global.Configuration.Settings.ShowSoftcoreState != 0;
            hcCheckBox.Checked = Global.Configuration.Settings.ShowHardcoreState != 0;

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            GrimDawnGameMode mode = GrimDawnGameMode.None;
            if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
            if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
            StashObject stash = Global.Stashes.ImportCreateStash(srcFile, nameTextBox.Text, exp, mode);

            if (stash != null)
            {
                stash.GroupID = ((StashGroupObject)groupComboBox.SelectedItem).ID;
                _importedStashes.Add(stash);
            }

            return result;
        }





        public DialogResult ShowDialog(IWin32Window owner, out StashObject[] importedStashes)
        {
            importedStashes = new StashObject[0];
            DialogResult result = GrimDawn.ShowSelectTransferFilesDialog(out string[] files, true, true);
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
