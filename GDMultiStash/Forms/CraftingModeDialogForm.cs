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

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Global;

namespace GDMultiStash.Forms
{
    internal partial class CraftingModeDialogForm : DialogForm
    {
        public CraftingModeDialogForm()
        {
            InitializeComponent();
        }

        protected override void Localize(LocalizationManager.StringsHolder L)
        {
            Text = L.CraftingModeButton();

            autoFillCheckBox.Text = G.L.AutoFillButton();
            autoFillComboBox.Items.Add(G.L.AutoFillRandomSeedsButton());
            autoFillComboBox.SelectedIndex = 0;

            transferFileComboBox.ValueMember = "Key";
            transferFileComboBox.DataSource = GrimDawn.GameEnvironmentList;
            transferFileComboBox.SelectedIndex = 0;

            UpdateStartButtonText();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            autoFillCheckBox.Checked = false;
            autoFillComboBox.Enabled = false;
        }

        private void UpdateStartButtonText()
        {
            startButton.Text = !_isCrafting
                ? G.L.StartButton()
                : G.L.FinishButton();
            startButton.ForeColor = !_isCrafting
                ? Color.Black
                : Color.White;
            startButton.BackColor = !_isCrafting
                ? SystemColors.Control
                : Color.Red;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _isCrafting = !_isCrafting;
            UpdateStartButtonText();
            if (_isCrafting) StartCrafting();
            else StopCrafting();
        }

        private bool _isCrafting = false;
        private GrimDawnGameEnvironment _selectedEnvironment;
        private StashGroupObject _craftingGroupObject;
        private TransferFile _originalTransferFile;
        private TransferFile _craftingTransferFile;
        private bool _skipNextChange = false;
        private int _selectedAutoFillMode = -1;

        private void StartCrafting()
        {
            transferFileComboBox.Enabled = false;
            autoFillCheckBox.Enabled = false;
            autoFillComboBox.Enabled = false;
            ControlBox = false;

            if (G.Runtime.StashIsOpened) // todo: localize me
                Console.AlertWarning("Close ingame stash before continue to prevent item loss!");

            _selectedEnvironment = (GrimDawnGameEnvironment)transferFileComboBox.SelectedValue;

            TransferFile.FromFile(_selectedEnvironment.TransferFilePath, out _originalTransferFile);
            _craftingTransferFile = TransferFile.CreateForExpansion(_selectedEnvironment.GameExpansion);
            _craftingTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);

            _craftingGroupObject = G.StashGroups.CreateGroup(G.L.CraftingModeButton(), true);
            G.StashGroups.InvokeStashGroupsAdded(_craftingGroupObject);
            G.Configuration.Save();

            _selectedAutoFillMode = autoFillCheckBox.Checked ? autoFillComboBox.SelectedIndex : -1;

            G.FileSystem.TransferFileChanged += TransferFileChanged;
        }

        private void StopCrafting()
        {
            transferFileComboBox.Enabled = true;
            autoFillCheckBox.Enabled = true;
            autoFillComboBox.Enabled = true;
            ControlBox = true;

            G.FileSystem.TransferFileChanged -= TransferFileChanged;

            _originalTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);
        }

        private void TransferFileChanged(object sender, FileSystemManager.TransferFileChangedEventArgs e)
        {
            if (e.FilePath != _selectedEnvironment.TransferFilePath) return;
            if (_skipNextChange) {
                _skipNextChange = false;
                return;
            }

            var stash = G.Stashes.CreateAndImportStash(_selectedEnvironment.TransferFilePath, "Crafted", _selectedEnvironment.GameExpansion, GrimDawnGameMode.Both);
            stash.GroupID = _craftingGroupObject.ID;
            switch (_selectedAutoFillMode)
            {
                case 0:
                    stash.AutoFill();
                    stash.SaveTransferFile();
                    stash.LoadTransferFile();
                    break;
            }
            G.Stashes.InvokeStashesAdded(stash);
            G.Configuration.Save();

            _skipNextChange = true;
            _craftingTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);
        }

        private void AutoFillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoFillComboBox.Enabled = autoFillCheckBox.Checked;
        }
    }
}
