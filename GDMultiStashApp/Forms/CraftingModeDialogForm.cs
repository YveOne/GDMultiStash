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
using GDMultiStash.GlobalHandlers;

namespace GDMultiStash.Forms
{
    internal partial class CraftingModeDialogForm : DialogForm
    {
        public CraftingModeDialogForm()
        {
            InitializeComponent();
        }

        protected override void Localize(LocalizationHandler.StringsHolder L)
        {
            Text = L.CraftingModeButton();

            autoFillCheckBox.Text = Global.L.AutoFillButton();
            autoFillComboBox.Items.Add(Global.L.AutoFillRandomSeedsButton());
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
                ? Global.L.StartButton()
                : Global.L.FinishButton();
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

            if (Global.Ingame.StashIsOpened) // todo: localize me
                Console.Warning("Close ingame stash before continue to prevent item loss!");

            _selectedEnvironment = (GrimDawnGameEnvironment)transferFileComboBox.SelectedValue;

            TransferFile.FromFile(_selectedEnvironment.TransferFilePath, out _originalTransferFile);
            _craftingTransferFile = TransferFile.CreateForExpansion(_selectedEnvironment.GameExpansion);
            _craftingTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);

            _craftingGroupObject = Global.Stashes.CreateStashGroup(Global.L.CraftingModeButton(), true);
            Global.Ingame.InvokeStashGroupsAdded(_craftingGroupObject);
            Global.Configuration.Save();

            _selectedAutoFillMode = autoFillCheckBox.Checked ? autoFillComboBox.SelectedIndex : -1;

            Global.FileSystem.TransferFileChanged += TransferFileChanged;
        }

        private void StopCrafting()
        {
            transferFileComboBox.Enabled = true;
            autoFillCheckBox.Enabled = true;
            autoFillComboBox.Enabled = true;
            ControlBox = true;

            Global.FileSystem.TransferFileChanged -= TransferFileChanged;

            _originalTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);
        }

        private void TransferFileChanged(object sender, FileSystemHandler.TransferFileChangedEventArgs e)
        {
            if (e.FilePath != _selectedEnvironment.TransferFilePath) return;
            if (_skipNextChange) {
                _skipNextChange = false;
                return;
            }

            var stash = Global.Stashes.CreateImportStash(_selectedEnvironment.TransferFilePath, "Crafted", _selectedEnvironment.GameExpansion, GrimDawnGameMode.Both);
            stash.GroupID = _craftingGroupObject.ID;
            switch (_selectedAutoFillMode)
            {
                case 0:
                    stash.AutoFill();
                    stash.SaveTransferFile();
                    stash.LoadTransferFile();
                    break;
            }
            Global.Ingame.InvokeStashesAdded(stash);
            Global.Configuration.Save();

            _skipNextChange = true;
            _craftingTransferFile.WriteToFile(_selectedEnvironment.TransferFilePath);
        }

        private void AutoFillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoFillComboBox.Enabled = autoFillCheckBox.Checked;
        }
    }
}
