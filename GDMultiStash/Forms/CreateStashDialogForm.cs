using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GrimDawnLib;

namespace GDMultiStash.Forms
{
    internal partial class CreateStashDialogForm : DialogForm
    {

        private string _newStashName;
        private int _lastSelectedExpansionIndex = -1;

        public CreateStashDialogForm() : base()
        {
            InitializeComponent();
        }

        public override void InitWindow()
        {
            base.InitWindow();
            expansionComboBox.SelectionChangeCommitted += expansionComboBox_SelectionChangeCommitted;
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsProxy L)
        {
            Text = "GDMultiStash : " + L["window_create_stash"];
            expansionLabel.Text = L["label_expansion"];

            _newStashName = L["new_stash"];
        }

        private void expansionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _lastSelectedExpansionIndex = expansionComboBox.SelectedIndex;
        }

        private void AddStashDialogForm_Load(object sender, EventArgs e)
        {
            if (_lastSelectedExpansionIndex == -1)
            {
                _lastSelectedExpansionIndex = (int)GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath);
            }

            expansionComboBox.DisplayMember = "Value";
            expansionComboBox.ValueMember = "Key";
            expansionComboBox.DataSource = new BindingSource(GrimDawn.GetExpansionNames(), null);
            expansionComboBox.SelectedIndex = _lastSelectedExpansionIndex;
        }

        private void AddStashDialogForm_Shown(object sender, EventArgs e)
        {
            nameTextBox.Text = _newStashName;
            nameTextBox.Focus();
        }

        private GlobalHandlers.StashObject _createdStash = null;

        public DialogResult ShowDialog(IWin32Window owner, out GlobalHandlers.StashObject createdStash)
        {
            createdStash = null;
            _createdStash = null;

            GrimDawnGameMode mode = (GrimDawnGameMode)Global.Configuration.Settings.DefaultStashMode;
            scCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.SC);
            hcCheckBox.Checked = mode.HasFlag(GrimDawnGameMode.HC);

            DialogResult result = base.ShowDialog(owner);
            if (result != DialogResult.OK) return result;

            if (_createdStash != null)
            {
                createdStash = _createdStash;
                _createdStash = null;
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            GrimDawnGameExpansion exp = (GrimDawnGameExpansion)_lastSelectedExpansionIndex;
            GrimDawnGameMode mode = GrimDawnGameMode.None;
            if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
            if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;

            _createdStash = Global.Stashes.CreateStash(nameTextBox.Text, exp, mode);
            Close(DialogResult.OK);
        }

    }
}
