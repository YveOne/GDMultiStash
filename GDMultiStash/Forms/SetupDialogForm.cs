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

using GrimDawnLib;

namespace GDMultiStash.Forms
{
    internal partial class SetupDialogForm : DialogForm
    {

        private Common.Config.ConfigSettingList _settings;

        private Dictionary<string, string> _autoStartCommandsList = new Dictionary<string, string>();

        public SetupDialogForm() : base()
        {
            InitializeComponent();

            languageListView.ItemSelectionChanged += LanguageListView_ItemSelectionChanged;
            languageListView.ItemCheck += LanguageListView_ItemCheck;
            autoStartGDCommandComboBox.SelectedIndexChanged += AutoStartGDCommandComboBox_SelectedIndexChanged;

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

            if (!GrimDawn.ValidGamePath(_settings.GamePath))
            {
                string gdPath = GrimDawn.Steam.FindGamePath();
                if (gdPath != null) _settings.GamePath = gdPath;
            }

            gamePathTextBox.Text = _settings.GamePath;
            confirmClosingCheckBox.Checked = _settings.ConfirmClosing;
            closeWithGrimDawnCheckBox.Checked = _settings.CloseWithGrimDawn;
            confirmStashDeleteCheckBox.Checked = _settings.ConfirmStashDelete;
            autoStartGDCheckBox.Checked = _settings.AutoStartGD;
            autoStartGDGroupBox.Enabled = _settings.AutoStartGD;
            autoBackToMainCheckBox.Checked = _settings.AutoBackToMain;
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

            UpdateAutoStartCommandList();
            UpdateMaxBackupsValueLabel();
            UpdateOverlayWidthValueLabel();
            UpdateOverlayScaleValueLabel();

            applyButton.Enabled = false;
        }

        private const int OverlayWidthMin = 270;
        private const int OverlayWidthStep = 10;

        private const int OverlayScaleMin = 90;
        private const int OverlayScaleStep = 1;

        private void UpdateAutoStartCommandList()
        {
            autoStartGDCommandComboBox.DataSource = null;
            _autoStartCommandsList.Clear();
            AddAutoStartCommand(_settings.AutoStartGDCommand);
            AddAutoStartCommand(GrimDawn.Steam.GameStartCommand);
            //AddAutoStartCommand(Path.Combine(_settings.GamePath, "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "x64", "Grim Dawn.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimInternals64.exe"));
            AddAutoStartCommand(Path.Combine(_settings.GamePath, "GrimCam.exe"));
            autoStartGDCommandComboBox.DataSource = new BindingSource(_autoStartCommandsList, null);
            autoStartGDCommandComboBox.SelectedIndex = 0; // we dont need to set selected index because current selected one will always be index 0
            autoStartGDCommandComboBox.DisplayMember = "Value";
            autoStartGDCommandComboBox.ValueMember = "Key";
        }

        private void AddAutoStartCommand(string cmd)
        {
            string command = cmd.Trim();
            if (command == "") return;
            if (Path.GetExtension(command).ToLower() == ".exe")
            {
                command = new FileInfo(command).FullName;
                if (!File.Exists(command)) return;
            }
            if (_autoStartCommandsList.ContainsKey(command)) return;
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
                _autoStartCommandsList.Add(command, "Grim Dawn.exe (64bit)");
                return;
            }
            //if (command.EndsWith("Grim Dawn.exe"))
            //{
            //    _autoStartCommandsList.Add(command, "Grim Dawn.exe (32bit)");
            //    return;
            //}
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

            _notice_shortcut_created = L["notice_shortcut_created"];
            _err_gd_already_running = L["err_gd_already_running"];
            _confirm_save_changed = L["confirm_save_changed"];
            _label_max_backups_off = L["label_max_backups_off"];
            _label_max_backups_unlimited = L["label_max_backups_unlimited"];
        }

















        private void GamePathSearchButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = _settings.GamePath,
                Filter = "Grim Dawn.exe|Grim Dawn.exe",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _settings.GamePath = Path.GetDirectoryName(openFileDialog1.FileName);
                gamePathTextBox.Text = _settings.GamePath;
                UpdateAutoStartCommandList();
                applyButton.Enabled = true;
            }
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

        private void autoBackToMainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoBackToMain = autoBackToMainCheckBox.Checked;
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

        private void AutoStartGDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.AutoStartGD = autoStartGDCheckBox.Checked;
            autoStartGDGroupBox.Enabled = autoStartGDCheckBox.Checked;
            applyButton.Enabled = true;
        }

        private void AutoStartGDCommandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!autoStartGDCommandComboBox.Focused) return;
            _settings.AutoStartGDCommand = (string)autoStartGDCommandComboBox.SelectedValue;
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

        public DialogResult ShowDialog(bool isFirstSetup = false)
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
            return base.ShowDialog();
        }

        public override DialogResult ShowDialog()
        {
            return ShowDialog(false);
        }

        #endregion

    }
}
