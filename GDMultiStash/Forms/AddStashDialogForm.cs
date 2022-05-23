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
    internal partial class AddStashDialogForm : DialogForm
    {

        private string _newStashName;
        private int _lastSelectedExpansionIndex = -1;

        public AddStashDialogForm()
        {
            InitializeComponent();
            expansionComboBox.SelectionChangeCommitted += expansionComboBox_SelectionChangeCommitted;
        }

        protected override void Localize(Core.Localization.StringsProxy L)
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
                _lastSelectedExpansionIndex = (int)Core.GD.InstalledGameExpansion;
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


        



        public override DialogResult ShowDialog()
        {
            return base.ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            GrimDawnGameExpansion exp = (GrimDawnGameExpansion)_lastSelectedExpansionIndex;
            Core.Stashes.CreateStash(nameTextBox.Text, exp, GrimDawnGameMode.None);
            Core.Config.Save();
            Close(DialogResult.OK);
        }

    }
}
