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

        public CreateStashDialogForm()
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


        
        private readonly List<Common.Stash> _createdStashes = new List<Common.Stash>();


        public DialogResult ShowDialog(IWin32Window owner, out Common.Stash[] createdStashes)
        {
            createdStashes = null;
            bool loop = true;
            while (loop)
                if (base.ShowDialog(owner) != DialogResult.OK)
                    loop = false;
            if (_createdStashes.Count != 0)
            {
                createdStashes = _createdStashes.ToArray();
                _createdStashes.Clear();
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            GrimDawnGameExpansion exp = (GrimDawnGameExpansion)_lastSelectedExpansionIndex;
            Common.Stash stash = Core.Stashes.CreateStash(nameTextBox.Text, exp, GrimDawnGameMode.None);
            _createdStashes.Add(stash);
            Close(DialogResult.OK);
        }

    }
}
