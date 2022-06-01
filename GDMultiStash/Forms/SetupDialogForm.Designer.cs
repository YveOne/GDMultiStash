namespace GDMultiStash.Forms
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.setupTabControl = new System.Windows.Forms.TabControl();
            this.commonTabPage = new System.Windows.Forms.TabPage();
            this.gameInstallPathsComboBox = new System.Windows.Forms.ComboBox();
            this.autoStartGDCheckBox = new System.Windows.Forms.CheckBox();
            this.autoStartGDGroupBox = new System.Windows.Forms.GroupBox();
            this.autoStartStartNowButton = new System.Windows.Forms.Button();
            this.autoStartGDCommandLabel = new System.Windows.Forms.Label();
            this.autoStartGDArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.autoStartGDCommandComboBox = new System.Windows.Forms.ComboBox();
            this.autoStartGDArgumentsLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.languageListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gamePathSearchButton = new System.Windows.Forms.Button();
            this.languageLabel = new System.Windows.Forms.Label();
            this.gamePathLabel = new System.Windows.Forms.Label();
            this.behaviourTabPage = new System.Windows.Forms.TabPage();
            this.defaultStashModeLabel = new System.Windows.Forms.Label();
            this.defaultStashModeCheckBox = new System.Windows.Forms.ComboBox();
            this.autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.checkVersionCheckBox = new System.Windows.Forms.CheckBox();
            this.autoBackToMainCheckBox = new System.Windows.Forms.CheckBox();
            this.overlayScaleValueLabel = new System.Windows.Forms.Label();
            this.overlayScaleLabel = new System.Windows.Forms.Label();
            this.confirmStashDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.closeWithGrimDawnCheckBox = new System.Windows.Forms.CheckBox();
            this.overlayScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.overlayWidthValueLabel = new System.Windows.Forms.Label();
            this.confirmClosingCheckBox = new System.Windows.Forms.CheckBox();
            this.overlayWidthLabel = new System.Windows.Forms.Label();
            this.overlayWidthTrackBar = new System.Windows.Forms.TrackBar();
            this.maxBackupsValueLabel = new System.Windows.Forms.Label();
            this.maxBackupsLabel = new System.Windows.Forms.Label();
            this.maxBackupsTrackBar = new System.Windows.Forms.TrackBar();
            this.createShortcutButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.setupTabControl.SuspendLayout();
            this.commonTabPage.SuspendLayout();
            this.autoStartGDGroupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.behaviourTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayScaleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayWidthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxBackupsTrackBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // setupTabControl
            // 
            this.setupTabControl.Controls.Add(this.commonTabPage);
            this.setupTabControl.Controls.Add(this.behaviourTabPage);
            this.setupTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupTabControl.Location = new System.Drawing.Point(0, 0);
            this.setupTabControl.Name = "setupTabControl";
            this.setupTabControl.SelectedIndex = 0;
            this.setupTabControl.Size = new System.Drawing.Size(649, 343);
            this.setupTabControl.TabIndex = 0;
            // 
            // commonTabPage
            // 
            this.commonTabPage.Controls.Add(this.gameInstallPathsComboBox);
            this.commonTabPage.Controls.Add(this.autoStartGDCheckBox);
            this.commonTabPage.Controls.Add(this.autoStartGDGroupBox);
            this.commonTabPage.Controls.Add(this.panel2);
            this.commonTabPage.Controls.Add(this.gamePathSearchButton);
            this.commonTabPage.Controls.Add(this.languageLabel);
            this.commonTabPage.Controls.Add(this.gamePathLabel);
            this.commonTabPage.Location = new System.Drawing.Point(4, 22);
            this.commonTabPage.Name = "commonTabPage";
            this.commonTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commonTabPage.Size = new System.Drawing.Size(641, 317);
            this.commonTabPage.TabIndex = 0;
            this.commonTabPage.Text = "Common";
            this.commonTabPage.UseVisualStyleBackColor = true;
            // 
            // gameInstallPathsComboBox
            // 
            this.gameInstallPathsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameInstallPathsComboBox.FormattingEnabled = true;
            this.gameInstallPathsComboBox.Location = new System.Drawing.Point(6, 134);
            this.gameInstallPathsComboBox.Name = "gameInstallPathsComboBox";
            this.gameInstallPathsComboBox.Size = new System.Drawing.Size(398, 21);
            this.gameInstallPathsComboBox.TabIndex = 10;
            // 
            // autoStartGDCheckBox
            // 
            this.autoStartGDCheckBox.AutoSize = true;
            this.autoStartGDCheckBox.Location = new System.Drawing.Point(13, 174);
            this.autoStartGDCheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.autoStartGDCheckBox.Name = "autoStartGDCheckBox";
            this.autoStartGDCheckBox.Size = new System.Drawing.Size(126, 17);
            this.autoStartGDCheckBox.TabIndex = 4;
            this.autoStartGDCheckBox.Text = "Auto start Grim Dawn";
            this.autoStartGDCheckBox.UseVisualStyleBackColor = true;
            this.autoStartGDCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartGDCheckBox_CheckedChanged);
            // 
            // autoStartGDGroupBox
            // 
            this.autoStartGDGroupBox.Controls.Add(this.autoStartStartNowButton);
            this.autoStartGDGroupBox.Controls.Add(this.autoStartGDCommandLabel);
            this.autoStartGDGroupBox.Controls.Add(this.autoStartGDArgumentsTextBox);
            this.autoStartGDGroupBox.Controls.Add(this.autoStartGDCommandComboBox);
            this.autoStartGDGroupBox.Controls.Add(this.autoStartGDArgumentsLabel);
            this.autoStartGDGroupBox.Location = new System.Drawing.Point(6, 176);
            this.autoStartGDGroupBox.Name = "autoStartGDGroupBox";
            this.autoStartGDGroupBox.Size = new System.Drawing.Size(398, 112);
            this.autoStartGDGroupBox.TabIndex = 9;
            this.autoStartGDGroupBox.TabStop = false;
            // 
            // autoStartStartNowButton
            // 
            this.autoStartStartNowButton.Location = new System.Drawing.Point(317, 32);
            this.autoStartStartNowButton.Name = "autoStartStartNowButton";
            this.autoStartStartNowButton.Size = new System.Drawing.Size(75, 60);
            this.autoStartStartNowButton.TabIndex = 9;
            this.autoStartStartNowButton.Text = "Start\r\nNow";
            this.autoStartStartNowButton.UseVisualStyleBackColor = true;
            this.autoStartStartNowButton.Click += new System.EventHandler(this.AutoStartStartNowButton_Click);
            // 
            // autoStartGDCommandLabel
            // 
            this.autoStartGDCommandLabel.AutoSize = true;
            this.autoStartGDCommandLabel.Location = new System.Drawing.Point(6, 16);
            this.autoStartGDCommandLabel.Name = "autoStartGDCommandLabel";
            this.autoStartGDCommandLabel.Size = new System.Drawing.Size(57, 13);
            this.autoStartGDCommandLabel.TabIndex = 6;
            this.autoStartGDCommandLabel.Text = "Command:";
            // 
            // autoStartGDArgumentsTextBox
            // 
            this.autoStartGDArgumentsTextBox.Location = new System.Drawing.Point(7, 72);
            this.autoStartGDArgumentsTextBox.Name = "autoStartGDArgumentsTextBox";
            this.autoStartGDArgumentsTextBox.Size = new System.Drawing.Size(304, 20);
            this.autoStartGDArgumentsTextBox.TabIndex = 8;
            this.autoStartGDArgumentsTextBox.TextChanged += new System.EventHandler(this.AutoStartGDArgumentsTextBox_TextChanged);
            // 
            // autoStartGDCommandComboBox
            // 
            this.autoStartGDCommandComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoStartGDCommandComboBox.FormattingEnabled = true;
            this.autoStartGDCommandComboBox.Location = new System.Drawing.Point(6, 32);
            this.autoStartGDCommandComboBox.Name = "autoStartGDCommandComboBox";
            this.autoStartGDCommandComboBox.Size = new System.Drawing.Size(305, 21);
            this.autoStartGDCommandComboBox.TabIndex = 5;
            // 
            // autoStartGDArgumentsLabel
            // 
            this.autoStartGDArgumentsLabel.AutoSize = true;
            this.autoStartGDArgumentsLabel.Location = new System.Drawing.Point(6, 56);
            this.autoStartGDArgumentsLabel.Name = "autoStartGDArgumentsLabel";
            this.autoStartGDArgumentsLabel.Size = new System.Drawing.Size(60, 13);
            this.autoStartGDArgumentsLabel.TabIndex = 7;
            this.autoStartGDArgumentsLabel.Text = "Arguments:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(7, 30);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(397, 76);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.languageListView);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(395, 74);
            this.panel3.TabIndex = 10;
            // 
            // languageListView
            // 
            this.languageListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.languageListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.languageListView.CheckBoxes = true;
            this.languageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.languageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.languageListView.HideSelection = false;
            this.languageListView.LabelWrap = false;
            this.languageListView.Location = new System.Drawing.Point(5, 5);
            this.languageListView.MultiSelect = false;
            this.languageListView.Name = "languageListView";
            this.languageListView.Scrollable = false;
            this.languageListView.ShowGroups = false;
            this.languageListView.Size = new System.Drawing.Size(385, 64);
            this.languageListView.TabIndex = 0;
            this.languageListView.UseCompatibleStateImageBehavior = false;
            this.languageListView.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 100;
            // 
            // gamePathSearchButton
            // 
            this.gamePathSearchButton.Location = new System.Drawing.Point(410, 132);
            this.gamePathSearchButton.Name = "gamePathSearchButton";
            this.gamePathSearchButton.Size = new System.Drawing.Size(62, 23);
            this.gamePathSearchButton.TabIndex = 4;
            this.gamePathSearchButton.Text = "Search";
            this.gamePathSearchButton.UseVisualStyleBackColor = true;
            this.gamePathSearchButton.Click += new System.EventHandler(this.GamePathSearchButton_Click);
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.Location = new System.Drawing.Point(6, 13);
            this.languageLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(58, 13);
            this.languageLabel.TabIndex = 2;
            this.languageLabel.Text = "Language:";
            // 
            // gamePathLabel
            // 
            this.gamePathLabel.AutoSize = true;
            this.gamePathLabel.Location = new System.Drawing.Point(6, 119);
            this.gamePathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.gamePathLabel.Name = "gamePathLabel";
            this.gamePathLabel.Size = new System.Drawing.Size(63, 13);
            this.gamePathLabel.TabIndex = 1;
            this.gamePathLabel.Text = "Game Path:";
            // 
            // behaviourTabPage
            // 
            this.behaviourTabPage.Controls.Add(this.defaultStashModeLabel);
            this.behaviourTabPage.Controls.Add(this.defaultStashModeCheckBox);
            this.behaviourTabPage.Controls.Add(this.autoUpdateCheckBox);
            this.behaviourTabPage.Controls.Add(this.checkVersionCheckBox);
            this.behaviourTabPage.Controls.Add(this.autoBackToMainCheckBox);
            this.behaviourTabPage.Controls.Add(this.overlayScaleValueLabel);
            this.behaviourTabPage.Controls.Add(this.overlayScaleLabel);
            this.behaviourTabPage.Controls.Add(this.confirmStashDeleteCheckBox);
            this.behaviourTabPage.Controls.Add(this.closeWithGrimDawnCheckBox);
            this.behaviourTabPage.Controls.Add(this.overlayScaleTrackBar);
            this.behaviourTabPage.Controls.Add(this.overlayWidthValueLabel);
            this.behaviourTabPage.Controls.Add(this.confirmClosingCheckBox);
            this.behaviourTabPage.Controls.Add(this.overlayWidthLabel);
            this.behaviourTabPage.Controls.Add(this.overlayWidthTrackBar);
            this.behaviourTabPage.Controls.Add(this.maxBackupsValueLabel);
            this.behaviourTabPage.Controls.Add(this.maxBackupsLabel);
            this.behaviourTabPage.Controls.Add(this.maxBackupsTrackBar);
            this.behaviourTabPage.Location = new System.Drawing.Point(4, 22);
            this.behaviourTabPage.Name = "behaviourTabPage";
            this.behaviourTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.behaviourTabPage.Size = new System.Drawing.Size(641, 317);
            this.behaviourTabPage.TabIndex = 1;
            this.behaviourTabPage.Text = "Behaviour";
            this.behaviourTabPage.UseVisualStyleBackColor = true;
            // 
            // defaultStashModeLabel
            // 
            this.defaultStashModeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultStashModeLabel.Location = new System.Drawing.Point(325, 134);
            this.defaultStashModeLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.defaultStashModeLabel.Name = "defaultStashModeLabel";
            this.defaultStashModeLabel.Size = new System.Drawing.Size(183, 13);
            this.defaultStashModeLabel.TabIndex = 16;
            this.defaultStashModeLabel.Text = "Default mode for new stashes:";
            this.defaultStashModeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // defaultStashModeCheckBox
            // 
            this.defaultStashModeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultStashModeCheckBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultStashModeCheckBox.FormattingEnabled = true;
            this.defaultStashModeCheckBox.Location = new System.Drawing.Point(514, 131);
            this.defaultStashModeCheckBox.Name = "defaultStashModeCheckBox";
            this.defaultStashModeCheckBox.Size = new System.Drawing.Size(121, 21);
            this.defaultStashModeCheckBox.TabIndex = 15;
            // 
            // autoUpdateCheckBox
            // 
            this.autoUpdateCheckBox.AutoSize = true;
            this.autoUpdateCheckBox.Location = new System.Drawing.Point(23, 156);
            this.autoUpdateCheckBox.Name = "autoUpdateCheckBox";
            this.autoUpdateCheckBox.Size = new System.Drawing.Size(159, 17);
            this.autoUpdateCheckBox.TabIndex = 14;
            this.autoUpdateCheckBox.Text = "Auto update on new version";
            this.autoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.autoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.AutoUpdateCheckBox_CheckedChanged);
            // 
            // checkVersionCheckBox
            // 
            this.checkVersionCheckBox.AutoSize = true;
            this.checkVersionCheckBox.Location = new System.Drawing.Point(6, 133);
            this.checkVersionCheckBox.Name = "checkVersionCheckBox";
            this.checkVersionCheckBox.Size = new System.Drawing.Size(132, 17);
            this.checkVersionCheckBox.TabIndex = 13;
            this.checkVersionCheckBox.Text = "Check for new version";
            this.checkVersionCheckBox.UseVisualStyleBackColor = true;
            this.checkVersionCheckBox.CheckedChanged += new System.EventHandler(this.CheckVersionCheckBox_CheckedChanged);
            // 
            // autoBackToMainCheckBox
            // 
            this.autoBackToMainCheckBox.AutoSize = true;
            this.autoBackToMainCheckBox.Location = new System.Drawing.Point(6, 248);
            this.autoBackToMainCheckBox.Name = "autoBackToMainCheckBox";
            this.autoBackToMainCheckBox.Size = new System.Drawing.Size(218, 17);
            this.autoBackToMainCheckBox.TabIndex = 12;
            this.autoBackToMainCheckBox.Text = "Switch to main Stash when Stash closed";
            this.autoBackToMainCheckBox.UseVisualStyleBackColor = true;
            this.autoBackToMainCheckBox.CheckedChanged += new System.EventHandler(this.AutoBackToMainCheckBox_CheckedChanged);
            // 
            // overlayScaleValueLabel
            // 
            this.overlayScaleValueLabel.AutoSize = true;
            this.overlayScaleValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayScaleValueLabel.Location = new System.Drawing.Point(321, 86);
            this.overlayScaleValueLabel.Name = "overlayScaleValueLabel";
            this.overlayScaleValueLabel.Size = new System.Drawing.Size(53, 20);
            this.overlayScaleValueLabel.TabIndex = 11;
            this.overlayScaleValueLabel.Text = "label1";
            // 
            // overlayScaleLabel
            // 
            this.overlayScaleLabel.AutoSize = true;
            this.overlayScaleLabel.Location = new System.Drawing.Point(6, 89);
            this.overlayScaleLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayScaleLabel.Name = "overlayScaleLabel";
            this.overlayScaleLabel.Size = new System.Drawing.Size(76, 13);
            this.overlayScaleLabel.TabIndex = 10;
            this.overlayScaleLabel.Text = "Overlay Scale:";
            // 
            // confirmStashDeleteCheckBox
            // 
            this.confirmStashDeleteCheckBox.AutoSize = true;
            this.confirmStashDeleteCheckBox.Location = new System.Drawing.Point(6, 225);
            this.confirmStashDeleteCheckBox.Name = "confirmStashDeleteCheckBox";
            this.confirmStashDeleteCheckBox.Size = new System.Drawing.Size(140, 17);
            this.confirmStashDeleteCheckBox.TabIndex = 2;
            this.confirmStashDeleteCheckBox.Text = "Confirm deleting stashes";
            this.confirmStashDeleteCheckBox.UseVisualStyleBackColor = true;
            this.confirmStashDeleteCheckBox.CheckedChanged += new System.EventHandler(this.ConfirmStashDeleteCheckBox_CheckedChanged);
            // 
            // closeWithGrimDawnCheckBox
            // 
            this.closeWithGrimDawnCheckBox.AutoSize = true;
            this.closeWithGrimDawnCheckBox.Location = new System.Drawing.Point(6, 179);
            this.closeWithGrimDawnCheckBox.Name = "closeWithGrimDawnCheckBox";
            this.closeWithGrimDawnCheckBox.Size = new System.Drawing.Size(238, 17);
            this.closeWithGrimDawnCheckBox.TabIndex = 1;
            this.closeWithGrimDawnCheckBox.Text = "Close GDMultiStash when Grim Dawn closed";
            this.closeWithGrimDawnCheckBox.UseVisualStyleBackColor = true;
            this.closeWithGrimDawnCheckBox.CheckedChanged += new System.EventHandler(this.CloseWithGrimDawnCheckBox_CheckedChanged);
            // 
            // overlayScaleTrackBar
            // 
            this.overlayScaleTrackBar.AutoSize = false;
            this.overlayScaleTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayScaleTrackBar.Location = new System.Drawing.Point(86, 86);
            this.overlayScaleTrackBar.Maximum = 20;
            this.overlayScaleTrackBar.Name = "overlayScaleTrackBar";
            this.overlayScaleTrackBar.Size = new System.Drawing.Size(229, 32);
            this.overlayScaleTrackBar.TabIndex = 9;
            this.overlayScaleTrackBar.Scroll += new System.EventHandler(this.OverlayScaleTrackBar_Scroll);
            // 
            // overlayWidthValueLabel
            // 
            this.overlayWidthValueLabel.AutoSize = true;
            this.overlayWidthValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayWidthValueLabel.Location = new System.Drawing.Point(321, 48);
            this.overlayWidthValueLabel.Name = "overlayWidthValueLabel";
            this.overlayWidthValueLabel.Size = new System.Drawing.Size(53, 20);
            this.overlayWidthValueLabel.TabIndex = 8;
            this.overlayWidthValueLabel.Text = "label1";
            // 
            // confirmClosingCheckBox
            // 
            this.confirmClosingCheckBox.AutoSize = true;
            this.confirmClosingCheckBox.Location = new System.Drawing.Point(6, 202);
            this.confirmClosingCheckBox.Name = "confirmClosingCheckBox";
            this.confirmClosingCheckBox.Size = new System.Drawing.Size(246, 17);
            this.confirmClosingCheckBox.TabIndex = 0;
            this.confirmClosingCheckBox.Text = "Confirm closing when Grim Dawn is still running";
            this.confirmClosingCheckBox.UseVisualStyleBackColor = true;
            this.confirmClosingCheckBox.CheckedChanged += new System.EventHandler(this.ConfirmClosingCheckBox_CheckedChanged);
            // 
            // overlayWidthLabel
            // 
            this.overlayWidthLabel.AutoSize = true;
            this.overlayWidthLabel.Location = new System.Drawing.Point(6, 51);
            this.overlayWidthLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayWidthLabel.Name = "overlayWidthLabel";
            this.overlayWidthLabel.Size = new System.Drawing.Size(77, 13);
            this.overlayWidthLabel.TabIndex = 7;
            this.overlayWidthLabel.Text = "Overlay Width:";
            // 
            // overlayWidthTrackBar
            // 
            this.overlayWidthTrackBar.AutoSize = false;
            this.overlayWidthTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayWidthTrackBar.LargeChange = 10;
            this.overlayWidthTrackBar.Location = new System.Drawing.Point(86, 48);
            this.overlayWidthTrackBar.Maximum = 18;
            this.overlayWidthTrackBar.Name = "overlayWidthTrackBar";
            this.overlayWidthTrackBar.Size = new System.Drawing.Size(229, 32);
            this.overlayWidthTrackBar.TabIndex = 6;
            this.overlayWidthTrackBar.Value = 10;
            this.overlayWidthTrackBar.Scroll += new System.EventHandler(this.OverlayWidthTrackBar_Scroll);
            // 
            // maxBackupsValueLabel
            // 
            this.maxBackupsValueLabel.AutoSize = true;
            this.maxBackupsValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxBackupsValueLabel.Location = new System.Drawing.Point(321, 10);
            this.maxBackupsValueLabel.Name = "maxBackupsValueLabel";
            this.maxBackupsValueLabel.Size = new System.Drawing.Size(53, 20);
            this.maxBackupsValueLabel.TabIndex = 5;
            this.maxBackupsValueLabel.Text = "label1";
            // 
            // maxBackupsLabel
            // 
            this.maxBackupsLabel.AutoSize = true;
            this.maxBackupsLabel.Location = new System.Drawing.Point(6, 13);
            this.maxBackupsLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.maxBackupsLabel.Name = "maxBackupsLabel";
            this.maxBackupsLabel.Size = new System.Drawing.Size(74, 13);
            this.maxBackupsLabel.TabIndex = 4;
            this.maxBackupsLabel.Text = "Max backups:";
            // 
            // maxBackupsTrackBar
            // 
            this.maxBackupsTrackBar.AutoSize = false;
            this.maxBackupsTrackBar.BackColor = System.Drawing.Color.White;
            this.maxBackupsTrackBar.Location = new System.Drawing.Point(86, 10);
            this.maxBackupsTrackBar.Minimum = -1;
            this.maxBackupsTrackBar.Name = "maxBackupsTrackBar";
            this.maxBackupsTrackBar.Size = new System.Drawing.Size(229, 32);
            this.maxBackupsTrackBar.TabIndex = 3;
            this.maxBackupsTrackBar.Scroll += new System.EventHandler(this.MaxBackupsTrackBar_Scroll);
            // 
            // createShortcutButton
            // 
            this.createShortcutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createShortcutButton.AutoSize = true;
            this.createShortcutButton.Location = new System.Drawing.Point(130, 355);
            this.createShortcutButton.Name = "createShortcutButton";
            this.createShortcutButton.Size = new System.Drawing.Size(256, 23);
            this.createShortcutButton.TabIndex = 3;
            this.createShortcutButton.Text = "Create Shortcut";
            this.createShortcutButton.UseVisualStyleBackColor = true;
            this.createShortcutButton.Click += new System.EventHandler(this.CreateShortcutButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.setupTabControl);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 343);
            this.panel1.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(582, 355);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(501, 355);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.restartButton.AutoSize = true;
            this.restartButton.Location = new System.Drawing.Point(9, 355);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(115, 23);
            this.restartButton.TabIndex = 10;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 385);
            this.Controls.Add(this.createShortcutButton);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.panel1);
            this.Name = "SetupDialogForm";
            this.Text = "GDMultiStash : Setup";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.setupTabControl.ResumeLayout(false);
            this.commonTabPage.ResumeLayout(false);
            this.commonTabPage.PerformLayout();
            this.autoStartGDGroupBox.ResumeLayout(false);
            this.autoStartGDGroupBox.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.behaviourTabPage.ResumeLayout(false);
            this.behaviourTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayScaleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayWidthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxBackupsTrackBar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl setupTabControl;
        private System.Windows.Forms.TabPage commonTabPage;
        private System.Windows.Forms.TabPage behaviourTabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView languageListView;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label gamePathLabel;
        private System.Windows.Forms.Button gamePathSearchButton;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.CheckBox confirmClosingCheckBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox closeWithGrimDawnCheckBox;
        private System.Windows.Forms.CheckBox confirmStashDeleteCheckBox;
        private System.Windows.Forms.Button createShortcutButton;
        private System.Windows.Forms.CheckBox autoStartGDCheckBox;
        private System.Windows.Forms.GroupBox autoStartGDGroupBox;
        private System.Windows.Forms.TextBox autoStartGDArgumentsTextBox;
        private System.Windows.Forms.Label autoStartGDArgumentsLabel;
        private System.Windows.Forms.Label autoStartGDCommandLabel;
        private System.Windows.Forms.ComboBox autoStartGDCommandComboBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TrackBar maxBackupsTrackBar;
        private System.Windows.Forms.Button autoStartStartNowButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Label maxBackupsLabel;
        private System.Windows.Forms.Label maxBackupsValueLabel;
        private System.Windows.Forms.Label overlayWidthValueLabel;
        private System.Windows.Forms.Label overlayWidthLabel;
        private System.Windows.Forms.TrackBar overlayWidthTrackBar;
        private System.Windows.Forms.Label overlayScaleValueLabel;
        private System.Windows.Forms.Label overlayScaleLabel;
        private System.Windows.Forms.TrackBar overlayScaleTrackBar;
        private System.Windows.Forms.CheckBox autoBackToMainCheckBox;
        private System.Windows.Forms.ComboBox gameInstallPathsComboBox;
        private System.Windows.Forms.CheckBox autoUpdateCheckBox;
        private System.Windows.Forms.CheckBox checkVersionCheckBox;
        private System.Windows.Forms.Label defaultStashModeLabel;
        private System.Windows.Forms.ComboBox defaultStashModeCheckBox;
    }
}