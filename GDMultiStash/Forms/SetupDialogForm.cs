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
using System.Reflection;
using System.Text.RegularExpressions;

using GrimDawnLib;

namespace GDMultiStash.Forms
{
    internal partial class SetupDialogForm : DialogForm
    {

        private Common.Config.ConfigSettingList _settings = null;

        private readonly Dictionary<string, string> _autoStartCommandsList = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _autoStartArgumentsList = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _gameInstallPathsList = new Dictionary<string, string>();

        private const int OverlayWidthMin = 270;
        private const int OverlayWidthStep = 10;

        private const int OverlayScaleMin = 90;
        private const int OverlayScaleStep = 1;

        public SetupDialogForm() : base()
        {
            InitializeComponent();

            languageListView.ItemSelectionChanged += LanguageListView_ItemSelectionChanged;
            languageListView.ItemCheck += LanguageListView_ItemCheck;
            autoStartGDCommandComboBox.SelectionChangeCommitted += AutoStartGDCommandComboBox_SelectionChangeCommitted;
            gameInstallPathsComboBox.SelectionChangeCommitted += GameInstallPathsComboBox_SelectionChangeCommitted;
            defaultStashModeCheckBox.SelectionChangeCommitted += DefaultStashModeCheckBox_SelectionChangeCommitted;

            setupTabControl.GotFocus += delegate {
                setupTabControl.Enabled = false;
                setupTabControl.Enabled = true;
            };

            setupTabControl.SelectedIndexChanged += delegate {
                setupTabControl.Focus();
            };

        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            _settings = Core.Config.GetSettings();

            DataTable dt = new DataTable();
            dt.Columns.Add("lang", typeof(Core.Localization.Language));
            languageListView.Items.Clear();
            foreach (Core.Localization.Language lang in Core.Localization.GetLanguages())
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
            autoStartGDCheckBox.Checked = _settings.AutoStartGD;
            autoStartGDGroupBox.Enabled = _settings.AutoStartGD;
            autoBackToMainCheckBox.Checked = _settings.AutoBackToMain;
            checkVersionCheckBox.Checked = _settings.CheckForNewVersion;
            autoUpdateCheckBox.Checked = _settings.AutoUpdate;
            autoUpdateCheckBox.Enabled = checkVersionCheckBox.Checked;

            applyButton.Enabled = false;

            maxBackupsTrackBar.Value = Math.Max(
                maxBackupsTrackBar.Minimum,
                Math.Min(
                    maxBackupsTrackBar.Maximum,
                    _settings.MaxBackups
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

            UpdateGameInstallPathsList();
            UpdateAutoStartCommandList();
            UpdateDefaultStashModeList();

            UpdateMaxBackupsValueLabel();
            UpdateOverlayWidthValueLabel();
            UpdateOverlayScaleValueLabel();




        }

        private Dictionary<int, string> _defaultStashModeList = new Dictionary<int, string> {
            { 0, "None" },
            { 1, "SC" },
            { 2, "HC" },
            { 3, "Both" },
        };

        private void UpdateDefaultStashModeList()
        {
            if (_settings == null) return; // not loaded yet
            defaultStashModeCheckBox.DisplayMember = "Value";
            defaultStashModeCheckBox.ValueMember = "Key";
            defaultStashModeCheckBox.DataSource = new BindingSource(_defaultStashModeList, null);
            defaultStashModeCheckBox.SelectedValue = _settings.DefaultStashMode;
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
                case -1:
                    maxBackupsValueLabel.Text = _label_max_backups_unlimited;
                    break;
                case -0:
                    maxBackupsValueLabel.Text = _label_max_backups_off;
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

        private string _notice_shortcut_created;
        private string _err_gd_already_running;
        private string _confirm_save_changed;
        private string _label_max_backups_off;
        private string _label_max_backups_unlimited;

        protected override void Localize(Core.Localization.StringsProxy L)
        {
            Text = "GDMultiStash : " + L["window_setup"];
            commonTabPage.Text = L["setup_tab_common"];
            behaviourTabPage.Text = L["setup_tab_behaviour"];
            saveButton.Text = L["button_save"];
            applyButton.Text = L["button_apply"];
            languageLabel.Text = L["label_language"];
            gamePathLabel.Text = L["label_gamepath"];
            gamePathSearchButton.Text = L["button_search"];
            confirmClosingCheckBox.Text = L["label_confirm_closing"];
            closeWithGrimDawnCheckBox.Text = L["label_close_with_gd"];
            confirmStashDeleteCheckBox.Text = L["label_confirm_delete_stash"];
            createShortcutButton.Text = L["button_create_shortcut"];
            autoStartGDCheckBox.Text = L["label_autostart_gd"];
            autoStartGDCommandLabel.Text = L["label_autostart_gd_command"];
            autoStartGDArgumentsLabel.Text = L["label_autostart_gd_arguments"];
            autoStartStartNowButton.Text = L["button_start_now"];
            restartButton.Text = L["button_restart"];
            maxBackupsLabel.Text = L["label_max_backups"];
            overlayScaleLabel.Text = L["label_overlay_scale"];
            overlayWidthLabel.Text = L["label_overlay_width"];
            autoBackToMainCheckBox.Text = L["label_auto_back_to_main"];
            checkVersionCheckBox.Text = L["label_check_for_new_version"];
            autoUpdateCheckBox.Text = L["label_auto_update_version"];
            defaultStashModeLabel.Text = L["label_default_stash_mode"];

            _defaultStashModeList[0] = L["mode_none"];
            _defaultStashModeList[1] = L["mode_sc"];
            _defaultStashModeList[2] = L["mode_hc"];
            _defaultStashModeList[3] = L["mode_both"];
            UpdateDefaultStashModeList();

            _notice_shortcut_created = L["notice_shortcut_created"];
            _err_gd_already_running = L["err_gd_already_running"];
            _confirm_save_changed = L["confirm_save_changed"];
            _label_max_backups_off = L["label_max_backups_off"];
            _label_max_backups_unlimited = L["label_max_backups_unlimited"];
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
            autoUpdateCheckBox.Enabled = checkVersionCheckBox.Checked;
        }

        private void AutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoUpdate = autoUpdateCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void CreateShortcutButton_Click(object sender, EventArgs e)
        {

            Assembly asm = Assembly.GetExecutingAssembly();
            string description = ((AssemblyDescriptionAttribute)asm.GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;

            Native.Shortcut link = new Native.Shortcut();
            link.SetDescription(description);
            link.SetPath(asm.Location);
            link.SetIconLocation(Path.Combine(Core.Config.GamePath, "Grim Dawn.exe"), 0);
            link.SetWorkingDirectory(Application.StartupPath);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            link.SaveTo(Path.Combine(desktopPath, "GDMultiStash.lnk"));
            MessageBox.Show(_notice_shortcut_created);
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
            _settings.AutoStartGD = autoStartGDCheckBox.Checked;
            autoStartGDGroupBox.Enabled = autoStartGDCheckBox.Checked;
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

        private void DefaultStashModeCheckBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _settings.DefaultStashMode = defaultStashModeCheckBox.SelectedIndex;
            applyButton.Enabled = true;
        }


        private void AutoStartGDArgumentsTextBox_TextChanged(object sender, EventArgs e)
        {
            _settings.AutoStartGDArguments = autoStartGDArgumentsTextBox.Text;
            applyButton.Enabled = true;
        }

        private void AutoStartStartNowButton_Click(object sender, EventArgs e)
        {
            switch (Core.AutoStartGame(true))
            {
                case Core.AutoStartResult.AlreadyRunning:
                    MessageBox.Show(_err_gd_already_running, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case Core.AutoStartResult.Success:
                    break;
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            if (applyButton.Enabled)
            {
                if(MessageBox.Show(_confirm_save_changed, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Core.Config.SetSettings(_settings);
                    Core.Config.Save();
                }
            }
            Program.Restart();
        }

        private void MaxBackupsTrackBar_Scroll(object sender, EventArgs e)
        {
            _settings.MaxBackups = maxBackupsTrackBar.Value;
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
























        #region Dialog

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Core.Config.SetSettings(_settings);
            Core.Config.Save();
            Close(DialogResult.OK);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            languageLabel.Focus(); // just dont focus any other button or input
            applyButton.Enabled = false;
            Core.Config.SetSettings(_settings);
            Core.Config.Save();
        }

        public DialogResult ShowDialog(IWin32Window owner, bool isFirstSetup = false)
        {
            if (isFirstSetup)
            {
                setupTabControl.TabPages.Clear();
                setupTabControl.TabPages.AddRange(new TabPage[] {
                    commonTabPage,
                });
            }
            else
            {
                setupTabControl.TabPages.Clear();
                setupTabControl.TabPages.AddRange(new TabPage[] {
                    commonTabPage,
                    behaviourTabPage,
                });
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
