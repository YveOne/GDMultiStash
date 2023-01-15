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
        private readonly Dictionary<string, string> _languageDataSet = new Dictionary<string, string>();

        private const int OverlayWidthMin = 300;
        private const int OverlayWidthStep = 10;

        private const int OverlayScaleMin = 50;
        private const int OverlayScaleStep = 5;

        private const int OverlayTransparencyMin = 0;
        private const int OverlayTransparencyStep = 10;

        private const int OverlayStashesCountMin = 15;
        private const int OverlayStashesCountStep = 1;

        private const int MaxBackupsMin = 0;
        private const int MaxBackupsStep = 5;

        public ConfigurationDialogForm() : base()
        {
            InitializeComponent();

            languageComboBox.SelectionChangeCommitted += LanguageComboBox_SelectionChangeCommitted;
            autoStartGDCommandComboBox.SelectionChangeCommitted += AutoStartGDCommandComboBox_SelectionChangeCommitted;
            gameInstallPathsComboBox.SelectionChangeCommitted += GameInstallPathsComboBox_SelectionChangeCommitted;

            Shown += delegate { gamePathLabel.Focus(); };

        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            _settings = Global.Configuration.GetSettings();

            _languageDataSet.Clear();
            foreach (GlobalHandlers.LocalizationHandler.Language lang in Global.Localization.Languages)
                _languageDataSet.Add(lang.Code, lang.Name);
            languageComboBox.ValueMember = "Key";
            languageComboBox.DisplayMember = "Value";
            languageComboBox.DataSource = new BindingSource(_languageDataSet, null);
            languageComboBox.SelectedValue = _settings.Language;

            confirmClosingCheckBox.Checked = _settings.ConfirmClosing;
            closeWithGrimDawnCheckBox.Checked = _settings.CloseWithGrimDawn;
            confirmStashDeleteCheckBox.Checked = _settings.ConfirmStashDelete;
            gameAutoStartCheckBox.Checked = _settings.AutoStartGame;
            autoBackToMainCheckBox.Checked = _settings.AutoBackToMain;
            selectFirstStashInGroupCheckBox.Checked = _settings.AutoSelectFirstStashInGroup;
            checkVersionCheckBox.Checked = _settings.CheckForNewVersion;
            saveOverLockedCheckBox.Checked = _settings.SaveOverwritesLocked;
            overlayShowWorkloadCheckBox.Checked = _settings.OverlayShowWorkload;

            applyButton.Enabled = false;

            maxBackupsTrackBar.Value = (_settings.MaxBackups - MaxBackupsMin) / MaxBackupsStep;
            overlayWidthTrackBar.Value = (_settings.OverlayWidth - OverlayWidthMin) / OverlayWidthStep;
            overlayScaleTrackBar.Value = (_settings.OverlayScale - OverlayScaleMin) / OverlayScaleStep;
            overlayTransparencyTrackBar.Value = (_settings.OverlayTransparency - OverlayTransparencyMin) / OverlayTransparencyStep;
            overlayStashesCountTrackBar.Value = (_settings.OverlayStashesCount - OverlayStashesCountMin) / OverlayStashesCountStep;

            UpdateGameInstallPathsList();
            UpdateAutoStartCommandList();

            UpdateMaxBackupsValueLabel();
            UpdateOverlayWidthValueLabel();
            UpdateOverlayScaleValueLabel();
            UpdateOverlayTransparencyValueLabel();
            UpdateOverlayStashesCountValueLabel();



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
            AddAutoStartCommand(_settings.StartGameCommand);
            AddAutoStartCommand(GrimDawn.Steam.GameStartCommand);
            AddAutoStartCommand(GrimDawn.GOG.GameStartCommand64);
            //AddAutoStartCommand(Path.Combine(_settings.GamePath, "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "x64", "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimInternals64.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimCam.exe"));
            autoStartGDCommandComboBox.DisplayMember = "Value";
            autoStartGDCommandComboBox.ValueMember = "Key";
            autoStartGDCommandComboBox.DataSource = new BindingSource(_autoStartCommandsList, null);
            autoStartGDCommandComboBox.SelectedIndex = _autoStartCommandsList.Count >= 1 && _autoStartCommandsList.Keys.ElementAt(0) == _settings.StartGameCommand ? 0 :-1;
            autoStartGDArgumentsTextBox.Text = _settings.StartGameArguments;
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
                    maxBackupsValueLabel.Text = Global.L.BackupsOffLabel();
                    break;
                default:
                    maxBackupsValueLabel.Text = _settings.MaxBackups.ToString();
                    break;
            }
        }

        private void UpdateOverlayWidthValueLabel()
        {
            overlayWidthValueLabel.Text = $"{_settings.OverlayWidth}";
        }

        private void UpdateOverlayScaleValueLabel()
        {
            overlayScaleValueLabel.Text = $"{_settings.OverlayScale}%";
        }

        private void UpdateOverlayTransparencyValueLabel()
        {
            overlayTransparencyValueLabel.Text = $"{_settings.OverlayTransparency}%";
        }

        private void UpdateOverlayStashesCountValueLabel()
        {
            overlayStashesCountValueLabel.Text = $"{_settings.OverlayStashesCount}";
        }
        
        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = L.SettingsButton();

            saveButton.Text = L.SaveButton();
            applyButton.Text = L.ApplyButton();
            gamePathSearchButton.Text = L.SearchButton();
            createShortcutButton.Text = L.CreateShortcutButton();
            cleanupBackupsButton.Text = L.CleanupBackupsButton();
            languageLabel.Text = L.LanguageLabel();
            gamePathLabel.Text = L.GamePathLabel();
            confirmClosingCheckBox.Text = L.ConfirmClosingLabel();
            closeWithGrimDawnCheckBox.Text = L.CloseWithGameLabel();
            confirmStashDeleteCheckBox.Text = L.ConfirmDeletingLabel();
            gameStartGroupBox.Text = L.GameStartOptionsLabel();
            gameStartCommandLabel.Text = L.GameStartCommandLabel();
            gameStartArgumentsLabel.Text = L.GameStartArgumentsLabel();
            gameAutoStartCheckBox.Text = L.AutoStartGameLabel();
            maxBackupsLabel.Text = L.MaxBackupsLabel();
            overlayScaleLabel.Text = L.OverlayScaleLabel();
            overlayWidthLabel.Text = L.OverlayWidthLabel();
            overlayTransparencyLabel.Text = L.OverlayTransparencyLabel();
            overlayStashesCountLabel.Text = L.OverlayStashesCountLabel();
            overlayShowWorkloadCheckBox.Text = L.OverlayShowWorkloadLabel();
            autoBackToMainCheckBox.Text = L.AutoBackToMainLabel();
            checkVersionCheckBox.Text = L.CheckVersionLabel();
            saveOverLockedCheckBox.Text = L.SaveLockedStashesLabel();
            extractTranslationFilesButton.Text = L.ExtractTranslationsButton();
        }
















        



        private void LanguageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _settings.Language = languageComboBox.SelectedValue.ToString();
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

        private void SelectFirstStashInGroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoSelectFirstStashInGroup = selectFirstStashInGroupCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CheckVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.CheckForNewVersion = checkVersionCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CreateShortcutButton_Click(object sender, EventArgs e)
        {
            Utils.AssemblyInfo ass = new Utils.AssemblyInfo();
            Native.Shortcut link = new Native.Shortcut();
            link.SetDescription(ass.Description);
            link.SetPath(ass.Location);
            link.SetIconLocation(Path.Combine(Global.Configuration.Settings.GamePath, "Grim Dawn.exe"), 0);
            link.SetWorkingDirectory(Application.StartupPath);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            link.SaveTo(Path.Combine(desktopPath, "GDMultiStash.lnk"));
            Console.Alert(Global.L.ShortcutCreatedMessage());
        }

        private void CleanupBackupsButton_Click(object sender, EventArgs e)
        {
            Global.Stashes.CleanupBackups();
        }

        private void ExtractTranslationFilesButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog()
            {
                Filter = $"{Global.L.ZipArchive()}|*.zip",
                FileName = "GDMS Localizations.zip",
            })
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    Utils.ZipFileWriter zip = new Utils.ZipFileWriter();
                    foreach(GlobalHandlers.LocalizationHandler.Language lang in Global.Localization.Languages)
                    {
                        zip.AddString($"{lang.Code}.txt", lang.Text);
                    }
                    zip.SaveTo(dialog.FileName);
                }
            }
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
                    _settings.StartGameCommand = "";
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
            _settings.StartGameCommand = "";
            _settings.StartGameArguments = "";
            UpdateAutoStartCommandList();
            applyButton.Enabled = true;
        }

        private void AutoStartGDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoStartGame = gameAutoStartCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void SaveOverLockedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.SaveOverwritesLocked = saveOverLockedCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void OverlayShowWorkloadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.OverlayShowWorkload = overlayShowWorkloadCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void AutoStartGDCommandComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!autoStartGDCommandComboBox.Focused) return;
            string command = (string)autoStartGDCommandComboBox.SelectedValue;
            _settings.StartGameCommand = command;
            autoStartGDArgumentsTextBox.Text = _autoStartArgumentsList[command];
            applyButton.Enabled = true;
        }

        private void AutoStartGDArgumentsTextBox_TextChanged(object sender, EventArgs e)
        {
            _settings.StartGameArguments = autoStartGDArgumentsTextBox.Text;
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

        private void OverlayTransparencyTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.OverlayTransparency = overlayTransparencyTrackBar.Value * OverlayTransparencyStep + OverlayTransparencyMin;
            applyButton.Enabled = true;
            UpdateOverlayTransparencyValueLabel();
        }

        private void OverlayShownStashesTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.OverlayStashesCount = overlayStashesCountTrackBar.Value * OverlayStashesCountStep + OverlayStashesCountMin;
            applyButton.Enabled = true;
            UpdateOverlayStashesCountValueLabel();
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

        public override DialogResult ShowDialog(IWin32Window owner)
        {
            return base.ShowDialog(owner);
        }

        #endregion

    }
}
