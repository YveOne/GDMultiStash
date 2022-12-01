using GrimDawnLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal partial class ConfigurationDialogForm : DialogForm
    {

        private Common.Config.ConfigSettingList _settings = null;

        private readonly Dictionary<string, string> _autoStartCommandsList = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _autoStartArgumentsList = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _gameInstallPathsList = new Dictionary<string, string>();

        private const int OverlayWidthMin = 300;
        private const int OverlayWidthStep = 10;

        private const int OverlayScaleMin = 90;
        private const int OverlayScaleStep = 1;

        private const int OverlayTransparencyMin = 0;
        private const int OverlayTransparencyStep = 10;

        private const int MaxBackupsMin = 0;
        private const int MaxBackupsStep = 5;

        private Dictionary<int, string> _defaultStashModeList = new Dictionary<int, string> {
            { 0, "None" },
            { 1, "SC" },
            { 2, "HC" },
            { 3, "Both" },
        };

        public ConfigurationDialogForm() : base()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            base.Initialize();

            languageListView.ItemSelectionChanged += LanguageListView_ItemSelectionChanged;
            languageListView.ItemCheck += LanguageListView_ItemCheck;
            autoStartGDCommandComboBox.SelectionChangeCommitted += AutoStartGDCommandComboBox_SelectionChangeCommitted;
            gameInstallPathsComboBox.SelectionChangeCommitted += GameInstallPathsComboBox_SelectionChangeCommitted;
            defaultStashModeComboBox.SelectionChangeCommitted += DefaultStashModeComboBox_SelectionChangeCommitted;
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            _settings = Global.Configuration.Settings.Copy();

            DataTable dt = new DataTable();
            dt.Columns.Add("lang", typeof(GlobalHandlers.LocalizationHandler.Language));
            languageListView.Items.Clear();
            foreach (GlobalHandlers.LocalizationHandler.Language lang in Global.Localization.Languages)
            {
                languageListView.Items.Add(new ListViewItem(lang.Name)
                {
                    Tag = lang.Code,
                    Checked = _settings.Language == lang.Code
                });
            }
            languageListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            if (!GrimDawn.ValidGamePath(_settings.GamePath))
                _settings.GamePath = GrimDawn.Steam.GamePath64 ?? "";
            if (!GrimDawn.ValidGamePath(_settings.GamePath))
                _settings.GamePath = GrimDawn.GOG.GamePath64 ?? "";

            confirmClosingCheckBox.Checked = _settings.ConfirmClosing;
            closeWithGrimDawnCheckBox.Checked = _settings.CloseWithGrimDawn;
            confirmStashDeleteCheckBox.Checked = _settings.ConfirmStashDelete;
            gameAutoStartCheckBox.Checked = _settings.AutoStartGD;
            autoBackToMainCheckBox.Checked = _settings.AutoBackToMain;
            checkVersionCheckBox.Checked = _settings.CheckForNewVersion;
            hideOnFormClosedCheckBox.Checked = _settings.HideOnFormClosed;
            saveOverLockedCheckBox.Checked = _settings.SaveOverwritesLocked;

            applyButton.Enabled = false;

            maxBackupsTrackBar.Value = Math.Max(
                maxBackupsTrackBar.Minimum,
                Math.Min(
                    maxBackupsTrackBar.Maximum,
                    (_settings.MaxBackups - MaxBackupsMin) / MaxBackupsStep
            ));
            overlayWidthTrackBar.Value = Math.Max(
                overlayWidthTrackBar.Minimum,
                Math.Min(
                    overlayWidthTrackBar.Maximum,
                    (_settings.OverlayWidth - OverlayWidthMin) / OverlayWidthStep
            ));
            overlayScaleTrackBar.Value = Math.Max(
                overlayScaleTrackBar.Minimum,
                Math.Min(
                    overlayScaleTrackBar.Maximum,
                    (_settings.OverlayScale - OverlayScaleMin) / OverlayScaleStep
            ));
            overlayTransparencyTrackBar.Value = Math.Max(
                overlayTransparencyTrackBar.Minimum,
                Math.Min(
                    overlayTransparencyTrackBar.Maximum,
                    (_settings.OverlayTransparency - OverlayTransparencyMin) / OverlayTransparencyStep
            ));

            UpdateGameInstallPathsList();
            UpdateAutoStartCommandList();
            UpdateDefaultStashModeList();

            UpdateMaxBackupsValueLabel();
            UpdateOverlayWidthValueLabel();
            UpdateOverlayScaleValueLabel();
            UpdateOverlayTransparencyValueLabel();
        }

        private void UpdateDefaultStashModeList()
        {
            if (_settings == null) return; // not loaded yet
            defaultStashModeComboBox.DisplayMember = "Value";
            defaultStashModeComboBox.ValueMember = "Key";
            defaultStashModeComboBox.DataSource = new BindingSource(_defaultStashModeList, null);
            defaultStashModeComboBox.SelectedValue = _settings.DefaultStashMode;
        }

        private void UpdateGameInstallPathsList()
        {
            gameInstallPathsComboBox.DataSource = null;
            _gameInstallPathsList.Clear();
            AddGameInstallPath(_settings.GamePath);
            AddGameInstallPath(GrimDawn.Steam.GamePath64);
            AddGameInstallPath(GrimDawn.GOG.GamePath64);
            gameInstallPathsComboBox.DisplayMember = "Value";
            gameInstallPathsComboBox.ValueMember = "Key";
            gameInstallPathsComboBox.DataSource = new BindingSource(_gameInstallPathsList, null);
            gameInstallPathsComboBox.SelectedIndex = 0; // we dont need to set selected index because current selected one will always be index 0
        }

        private void AddGameInstallPath(string path)
        {
            if (path == null) return;
            path = path.Trim();
            if (!GrimDawn.ValidGamePath(path)) return;
            _gameInstallPathsList[path] = path;
        }












        private void UpdateAutoStartCommandList()
        {
            autoStartGDCommandComboBox.DataSource = null;
            _autoStartCommandsList.Clear();
            _autoStartArgumentsList.Clear();
            AddAutoStartCommand(_settings.AutoStartGDCommand);
            AddAutoStartCommand(GrimDawn.Steam.GameStartCommand);
            AddAutoStartCommand(GrimDawn.GOG.GameStartCommand64);
            //AddAutoStartCommand(Path.Combine(_settings.GamePath, "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "x64", "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimInternals64.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimCam.exe"));
            autoStartGDCommandComboBox.DisplayMember = "Value";
            autoStartGDCommandComboBox.ValueMember = "Key";
            autoStartGDCommandComboBox.DataSource = new BindingSource(_autoStartCommandsList, null);
            autoStartGDCommandComboBox.SelectedIndex = _autoStartCommandsList.Count >= 1 && _autoStartCommandsList.Keys.ElementAt(0) == _settings.AutoStartGDCommand ? 0 :-1;
            autoStartGDArgumentsTextBox.Text = _settings.AutoStartGDArguments;
        }

        private void AddAutoStartCommand(string command)
        {
            if (command == null) return;
            command = command.Trim();
            if (command == "") return;

            // command is combination of command+arguments
            string args = "";
            if (command.StartsWith(@""""))
            {
                Match match = Regex.Match(command, @"^""([^""]+)""\s*(.*?)$");
                if (!match.Success) return; // something wrong here
                command = match.Groups[1].Value;
                args = match.Groups[2].Value;
            }

            // check if file realy exists
            if (Path.GetExtension(command).ToLower() == ".exe")
            {
                command = new FileInfo(command).FullName;
                if (!File.Exists(command)) return;
            }

            // already in list
            if (_autoStartCommandsList.ContainsKey(command)) return;

            // add args for command
            _autoStartArgumentsList.Add(command, args);

            if (command.StartsWith("steam://"))
            {
                if (GrimDawn.Steam.SteamClientPath64 == null) return; // not installed
                if (_settings.GamePath != GrimDawn.Steam.GamePath64) return;
                _autoStartCommandsList.Add(command, "Steam Client");
                return;
            }

            if (command.EndsWith("GalaxyClient.exe"))
            {
                if (GrimDawn.GOG.GalaxyClientPath == null) return; // not installed
                if (_settings.GamePath != GrimDawn.GOG.GamePath64) return;
                _autoStartCommandsList.Add(command, "GOG Galaxy Client");
                return;
            }

            if (command.EndsWith("GrimInternals64.exe"))
            {
                _autoStartCommandsList.Add(command, "Grim Internals");
                return;
            }

            if (command.EndsWith("GrimCam.exe"))
            {
                _autoStartCommandsList.Add(command, "GrimCam");
                return;
            }

            if (command.EndsWith(Path.Combine("x64", "Grim Dawn.exe")))
            {
                _autoStartCommandsList.Add(command, "Grim Dawn.exe");
                return;
            }

            // disabled because 32bit no more supported
            //if (command.EndsWith("Grim Dawn.exe"))
            //{
            //    _autoStartCommandsList.Add(command, "Grim Dawn.exe (32bit)");
            //    return;
            //}

            // fallback ... dont care, just add
            _autoStartCommandsList.Add(command, command);
        }















        private void UpdateMaxBackupsValueLabel()
        {
            switch (_settings.MaxBackups)
            {
                case 0:
                    maxBackupsValueLabel.Text = _label_maxBackups_off;
                    break;
                default:
                    maxBackupsValueLabel.Text = _settings.MaxBackups.ToString();
                    break;
            }
        }

        private void UpdateOverlayWidthValueLabel()
        {
            overlayWidthValueLabel.Text = _settings.OverlayWidth.ToString();
        }

        private void UpdateOverlayScaleValueLabel()
        {
            overlayScaleValueLabel.Text = _settings.OverlayScale.ToString() + "%";
        }

        private void UpdateOverlayTransparencyValueLabel()
        {
            overlayTransparencyValueLabel.Text = _settings.OverlayTransparency.ToString() + "%";
        }

        private string _msg_shortcutCreated;
        private string _label_maxBackups_off;

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsProxy L)
        {
            Text = L["configWindow"];

            saveButton.Text = L["configWindow_saveButton"];
            applyButton.Text = L["configWindow_applyButton"];
            gamePathSearchButton.Text = L["configWindow_gamePathSearchButton"];
            createShortcutButton.Text = L["configWindow_createShortcutButton"];
            cleanupBackupsButton.Text = L["configWindow_cleanupBackupsButton"];
            languageLabel.Text = L["configWindow_languageLabel"];
            gamePathLabel.Text = L["configWindow_gamePathLabel"];
            confirmClosingCheckBox.Text = L["configWindow_confirmClosingCheckBox"];
            closeWithGrimDawnCheckBox.Text = L["configWindow_closeWithGrimDawnCheckBox"];
            confirmStashDeleteCheckBox.Text = L["configWindow_confirmStashDeleteCheckBox"];
            gameStartGroupBox.Text = L["configWindow_gameStartGroupBox"];
            gameStartCommandLabel.Text = L["configWindow_gameStartCommandLabel"];
            gameStartArgumentsLabel.Text = L["configWindow_gameStartArgumentsLabel"];
            gameAutoStartCheckBox.Text = L["configWindow_gameAutoStartCheckBox"];
            maxBackupsLabel.Text = L["configWindow_maxBackupsLabel"];
            overlayScaleLabel.Text = L["configWindow_overlayScaleLabel"];
            overlayWidthLabel.Text = L["configWindow_overlayWidthLabel"];
            overlayTransparencyLabel.Text = L["configWindow_overlayTransparencyLabel"];
            autoBackToMainCheckBox.Text = L["configWindow_autoBackToMainCheckBox"];
            checkVersionCheckBox.Text = L["configWindow_checkVersionCheckBox"];
            hideOnFormClosedCheckBox.Text = L["configWindow_hideOnFormClosedCheckBox"];
            saveOverLockedCheckBox.Text = L["configWindow_saveOverLockedCheckBox"];
            defaultStashModeLabel.Text = L["configWindow_defaultStashModeLabel"];
            _defaultStashModeList[0] = L["configWindow_defaultStashMode_none"];
            _defaultStashModeList[1] = L["configWindow_defaultStashMode_sc"];
            _defaultStashModeList[2] = L["configWindow_defaultStashMode_hc"];
            _defaultStashModeList[3] = L["configWindow_defaultStashMode_both"];
            UpdateDefaultStashModeList();

            _msg_shortcutCreated = L["configWindow_shortcutCreatedMessage"];
            _label_maxBackups_off = L["configWindow_maxBackups_noBackups"];

        }

















        private void LanguageListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = false;
            e.Item.Focused = false;
        }

        private void LanguageListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (sender is ListView listView)
            {
                int checkedCount = listView.CheckedItems.Count;
                listView.ItemCheck -= LanguageListView_ItemCheck;
                for (var i = 0; i < checkedCount; i++)
                {
                    listView.CheckedItems[i].Checked = false;
                }
                _settings.Language = (string)listView.Items[e.Index].Tag;
                listView.ItemCheck += LanguageListView_ItemCheck;
            }
            applyButton.Enabled = true;
        }

        private void ConfirmClosingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.ConfirmClosing = confirmClosingCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CloseWithGrimDawnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.CloseWithGrimDawn = closeWithGrimDawnCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void ConfirmStashDeleteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.ConfirmStashDelete = confirmStashDeleteCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void AutoBackToMainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoBackToMain = autoBackToMainCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CheckVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.CheckForNewVersion = checkVersionCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CreateShortcutButton_Click(object sender, EventArgs e)
        {

            Assembly asm = Assembly.GetExecutingAssembly();
            string description = ((AssemblyDescriptionAttribute)asm.GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;

            Native.Shortcut link = new Native.Shortcut();
            link.SetDescription(description);
            link.SetPath(asm.Location);
            link.SetIconLocation(Path.Combine(Global.Configuration.Settings.GamePath, "Grim Dawn.exe"), 0);
            link.SetWorkingDirectory(Application.StartupPath);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            link.SaveTo(Path.Combine(desktopPath, "GDMultiStash.lnk"));
            MessageBox.Show(_msg_shortcutCreated);
        }

        private void CleanupBackupsButton_Click(object sender, EventArgs e)
        {
            foreach(var stash in Global.Stashes.GetAllStashes())
            {
                Global.FileSystem.BackupCleanupStashTransferFile(stash.ID);
            }
            MessageBox.Show("OK");
        }

        private void GamePathSearchButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog()
            {
            };

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string p = folderBrowserDialog1.SelectedPath;
                if (GrimDawn.ValidGamePath(p))
                {
                    _settings.GamePath = p;
                    _settings.AutoStartGDCommand = "";
                    UpdateGameInstallPathsList();
                    UpdateAutoStartCommandList();
                    applyButton.Enabled = true;
                }
                else
                {
                    // TODO: show warning?
                }
            }

        }

        private void GameInstallPathsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string p = gameInstallPathsComboBox.SelectedValue.ToString();
            _settings.GamePath = p;
            _settings.AutoStartGDCommand = "";
            _settings.AutoStartGDArguments = "";
            UpdateAutoStartCommandList();
            applyButton.Enabled = true;
        }

        private void AutoStartGDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoStartGD = gameAutoStartCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void HideOnFormClosedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.HideOnFormClosed = hideOnFormClosedCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void SaveOverLockedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.SaveOverwritesLocked = saveOverLockedCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void AutoStartGDCommandComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!autoStartGDCommandComboBox.Focused) return;
            string command = (string)autoStartGDCommandComboBox.SelectedValue;
            _settings.AutoStartGDCommand = command;
            autoStartGDArgumentsTextBox.Text = _autoStartArgumentsList[command];
            applyButton.Enabled = true;
        }

        private void DefaultStashModeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _settings.DefaultStashMode = defaultStashModeComboBox.SelectedIndex;
            applyButton.Enabled = true;
        }

        private void AutoStartGDArgumentsTextBox_TextChanged(object sender, EventArgs e)
        {
            _settings.AutoStartGDArguments = autoStartGDArgumentsTextBox.Text;
            applyButton.Enabled = true;
        }

        private void MaxBackupsTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.MaxBackups = maxBackupsTrackBar.Value * MaxBackupsStep + MaxBackupsMin;
            applyButton.Enabled = true;
            UpdateMaxBackupsValueLabel();
        }

        private void OverlayWidthTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.OverlayWidth = overlayWidthTrackBar.Value * OverlayWidthStep + OverlayWidthMin;
            applyButton.Enabled = true;
            UpdateOverlayWidthValueLabel();
        }

        private void OverlayScaleTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.OverlayScale = overlayScaleTrackBar.Value * OverlayScaleStep + OverlayScaleMin;
            applyButton.Enabled = true;
            UpdateOverlayScaleValueLabel();
        }

        private void overlayTransparencyTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.OverlayTransparency = overlayTransparencyTrackBar.Value * OverlayTransparencyStep + OverlayTransparencyMin;
            applyButton.Enabled = true;
            UpdateOverlayTransparencyValueLabel();
        }























        #region Dialog

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Global.Configuration.SetSettings(_settings);
            Global.Configuration.Save();
            Close(DialogResult.OK);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            languageLabel.Focus(); // just dont focus any other button or input
            applyButton.Enabled = false;
            Global.Configuration.SetSettings(_settings);
            Global.Configuration.Save();
        }

        public DialogResult ShowDialog(IWin32Window owner, bool isFirstSetup = false)
        {
            if (isFirstSetup)
            {
                //panel5.Enabled = false;
            }
            return base.ShowDialog(owner);
        }

        public override DialogResult ShowDialog(IWin32Window owner)
        {
            return ShowDialog(owner, false);
        }


        #endregion

    }
}
