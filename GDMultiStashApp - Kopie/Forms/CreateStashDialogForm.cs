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
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms
{
    internal partial class CreateStashDialogForm : DialogForm
    {

        public CreateStashDialogForm() : base()
        {
            InitializeComponent();

            expansionComboBox.DisplayMember = "Value";
            expansionComboBox.ValueMember = "Key";
            expansionComboBox.DataSource = new BindingSource(GrimDawn.ExpansionList
                .Where(exp => exp != GrimDawnGameExpansion.Unknown)
                .Select(exp => GrimDawn.ExpansionNames[exp]), null);

            Load += delegate {
                groupComboBox.DisplayMember = "Name";
                groupComboBox.ValueMember = "Key";
                groupComboBox.DataSource = Global.Stashes.GetSortedStashGroups();
            };

            nameTextBox.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                    okButton.PerformClick();
            };

            expansionComboBox.SelectionChangeCommitted += delegate {
                UpdateTabsComboBox();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = L.CreateStashButton();
            nameLabel.Text = L.NameLabel();
            expansionLabel.Text = L.ExpansionLabel();
            groupLabel.Text = L.GroupLabel();
            tabsLabel.Text = L.TabsLabel();
            okButton.Text = L.CreateButton();
        }

        private void UpdateTabsComboBox()
        {
            tabsComboBox.Items.Clear();
            GrimDawnGameExpansion exp = (GrimDawnGameExpansion)expansionComboBox.SelectedIndex;
            int maxTabs = (int)GrimDawn.Stashes.GetStashInfoForExpansion(exp).MaxTabs;
            for (var i=1; i<= maxTabs; i+=1)
            {
                tabsComboBox.Items.Add(i);
            }
            tabsComboBox.SelectedIndex = tabsComboBox.Items.Count - 1;
        }

        private void AddStashDialogForm_Shown(object sender, EventArgs e)
        {
            nameTextBox.Text = Global.L.DefaultStashName();
            nameTextBox.SelectAll();
            nameTextBox.Focus();
        }

        public DialogResult ShowDialog(IWin32Window owner, GrimDawnGameExpansion exp)
        {
            scCheckBox.Checked = Global.Configuration.Settings.ShowSoftcoreState != 0;
            hcCheckBox.Checked = Global.Configuration.Settings.ShowHardcoreState != 0;
            expansionComboBox.SelectedIndex = (int)exp;
            UpdateTabsComboBox();
            base.ShowDialog(owner);
            return DialogResult.OK;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            GrimDawnGameExpansion exp = (GrimDawnGameExpansion)expansionComboBox.SelectedIndex;
            GrimDawnGameMode mode = GrimDawnGameMode.None;
            if (scCheckBox.Checked) mode |= GrimDawnGameMode.SC;
            if (hcCheckBox.Checked) mode |= GrimDawnGameMode.HC;
            int tabsCount = tabsComboBox.SelectedIndex + 1;
            StashObject stash = Global.Stashes.CreateStash(nameTextBox.Text, exp, mode, tabsCount);
            stash.GroupID = ((StashGroupObject)groupComboBox.SelectedItem).ID;
            Global.Configuration.Save();
            Global.Ingame.InvokeStashesAdded(stash);
            nameTextBox.SelectAll();
            nameTextBox.Focus();
        }

    }
}
