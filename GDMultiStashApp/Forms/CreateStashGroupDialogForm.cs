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
    internal partial class CreateStashGroupDialogForm : DialogForm
    {

        public CreateStashGroupDialogForm() : base()
        {
            InitializeComponent();

            nameTextBox.KeyDown += delegate (object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                    okButton.PerformClick();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = L.CreateGroupButton();
            nameLabel.Text = L.NameLabel();
            okButton.Text = L.CreateButton();
        }

        private void AddStashDialogForm_Shown(object sender, EventArgs e)
        {
            nameTextBox.Text = Global.L.DefaultStashGroupName();
            nameTextBox.SelectAll();
            nameTextBox.Focus();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
            return DialogResult.OK;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            StashGroupObject group = Global.Stashes.CreateStashGroup(nameTextBox.Text);
            Global.Configuration.Save();
            Global.Runtime.NotifyStashGroupsAdded(group);
            nameTextBox.SelectAll();
            nameTextBox.Focus();
        }

    }
}
