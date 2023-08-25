﻿namespace GDMultiStash.Forms
{
    partial class ConfigurationDialogForm
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
            this.gameAutoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.gameStartGroupBox = new System.Windows.Forms.GroupBox();
            this.gameStartCommandLabel = new System.Windows.Forms.Label();
            this.autoStartGDArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.autoStartGDCommandComboBox = new System.Windows.Forms.ComboBox();
            this.gameStartArgumentsLabel = new System.Windows.Forms.Label();
            this.gameInstallPathsComboBox = new System.Windows.Forms.ComboBox();
            this.gamePathSearchButton = new System.Windows.Forms.Button();
            this.languageLabel = new System.Windows.Forms.Label();
            this.gamePathLabel = new System.Windows.Forms.Label();
            this.checkVersionCheckBox = new System.Windows.Forms.CheckBox();
            this.autoBackToMainCheckBox = new System.Windows.Forms.CheckBox();
            this.confirmStashDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.closeWithGrimDawnCheckBox = new System.Windows.Forms.CheckBox();
            this.confirmClosingCheckBox = new System.Windows.Forms.CheckBox();
            this.maxBackupsValueLabel = new System.Windows.Forms.Label();
            this.maxBackupsLabel = new System.Windows.Forms.Label();
            this.maxBackupsTrackBar = new System.Windows.Forms.TrackBar();
            this.overlayWidthLabel = new System.Windows.Forms.Label();
            this.overlayTransparencyValueLabel = new System.Windows.Forms.Label();
            this.overlayWidthTrackBar = new System.Windows.Forms.TrackBar();
            this.overlayScaleValueLabel = new System.Windows.Forms.Label();
            this.overlayTransparencyLabel = new System.Windows.Forms.Label();
            this.overlayScaleLabel = new System.Windows.Forms.Label();
            this.overlayWidthValueLabel = new System.Windows.Forms.Label();
            this.overlayScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.overlayTransparencyTrackBar = new System.Windows.Forms.TrackBar();
            this.createShortcutButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.extractTranslationFilesButton = new System.Windows.Forms.Button();
            this.cleanupBackupsButton = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.behaviorGroupBox = new System.Windows.Forms.GroupBox();
            this.saveExternalChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.saveOverLockedCheckBox = new System.Windows.Forms.CheckBox();
            this.selectFirstStashInGroupCheckBox = new System.Windows.Forms.CheckBox();
            this.overlayWindowGroupBox = new System.Windows.Forms.GroupBox();
            this.overlayShowWorkloadCheckBox = new System.Windows.Forms.CheckBox();
            this.overlayStashesCountLabel = new System.Windows.Forms.Label();
            this.overlayStashesCountTrackBar = new System.Windows.Forms.TrackBar();
            this.overlayStashesCountValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.gameStartGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxBackupsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayWidthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayScaleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayTransparencyTrackBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.behaviorGroupBox.SuspendLayout();
            this.overlayWindowGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayStashesCountTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // gameAutoStartCheckBox
            // 
            this.gameAutoStartCheckBox.AutoSize = true;
            this.gameAutoStartCheckBox.Location = new System.Drawing.Point(6, 104);
            this.gameAutoStartCheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.gameAutoStartCheckBox.Name = "gameAutoStartCheckBox";
            this.gameAutoStartCheckBox.Size = new System.Drawing.Size(126, 17);
            this.gameAutoStartCheckBox.TabIndex = 4;
            this.gameAutoStartCheckBox.Text = "Auto start Grim Dawn";
            this.gameAutoStartCheckBox.UseVisualStyleBackColor = true;
            this.gameAutoStartCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartGDCheckBox_CheckedChanged);
            // 
            // gameStartGroupBox
            // 
            this.gameStartGroupBox.Controls.Add(this.gameStartCommandLabel);
            this.gameStartGroupBox.Controls.Add(this.autoStartGDArgumentsTextBox);
            this.gameStartGroupBox.Controls.Add(this.autoStartGDCommandComboBox);
            this.gameStartGroupBox.Controls.Add(this.gameStartArgumentsLabel);
            this.gameStartGroupBox.Controls.Add(this.gameAutoStartCheckBox);
            this.gameStartGroupBox.Location = new System.Drawing.Point(20, 13);
            this.gameStartGroupBox.Name = "gameStartGroupBox";
            this.gameStartGroupBox.Size = new System.Drawing.Size(364, 127);
            this.gameStartGroupBox.TabIndex = 9;
            this.gameStartGroupBox.TabStop = false;
            // 
            // gameStartCommandLabel
            // 
            this.gameStartCommandLabel.AutoSize = true;
            this.gameStartCommandLabel.Location = new System.Drawing.Point(6, 22);
            this.gameStartCommandLabel.Name = "gameStartCommandLabel";
            this.gameStartCommandLabel.Size = new System.Drawing.Size(57, 13);
            this.gameStartCommandLabel.TabIndex = 6;
            this.gameStartCommandLabel.Text = "Command:";
            // 
            // autoStartGDArgumentsTextBox
            // 
            this.autoStartGDArgumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoStartGDArgumentsTextBox.Location = new System.Drawing.Point(6, 78);
            this.autoStartGDArgumentsTextBox.Name = "autoStartGDArgumentsTextBox";
            this.autoStartGDArgumentsTextBox.Size = new System.Drawing.Size(352, 20);
            this.autoStartGDArgumentsTextBox.TabIndex = 8;
            this.autoStartGDArgumentsTextBox.TextChanged += new System.EventHandler(this.AutoStartGDArgumentsTextBox_TextChanged);
            // 
            // autoStartGDCommandComboBox
            // 
            this.autoStartGDCommandComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoStartGDCommandComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoStartGDCommandComboBox.FormattingEnabled = true;
            this.autoStartGDCommandComboBox.Location = new System.Drawing.Point(6, 38);
            this.autoStartGDCommandComboBox.Name = "autoStartGDCommandComboBox";
            this.autoStartGDCommandComboBox.Size = new System.Drawing.Size(352, 21);
            this.autoStartGDCommandComboBox.TabIndex = 5;
            // 
            // gameStartArgumentsLabel
            // 
            this.gameStartArgumentsLabel.AutoSize = true;
            this.gameStartArgumentsLabel.Location = new System.Drawing.Point(6, 62);
            this.gameStartArgumentsLabel.Name = "gameStartArgumentsLabel";
            this.gameStartArgumentsLabel.Size = new System.Drawing.Size(60, 13);
            this.gameStartArgumentsLabel.TabIndex = 7;
            this.gameStartArgumentsLabel.Text = "Arguments:";
            // 
            // gameInstallPathsComboBox
            // 
            this.gameInstallPathsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameInstallPathsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameInstallPathsComboBox.FormattingEnabled = true;
            this.gameInstallPathsComboBox.Location = new System.Drawing.Point(258, 28);
            this.gameInstallPathsComboBox.Name = "gameInstallPathsComboBox";
            this.gameInstallPathsComboBox.Size = new System.Drawing.Size(765, 21);
            this.gameInstallPathsComboBox.TabIndex = 10;
            // 
            // gamePathSearchButton
            // 
            this.gamePathSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gamePathSearchButton.Location = new System.Drawing.Point(1029, 26);
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
            this.gamePathLabel.Location = new System.Drawing.Point(260, 13);
            this.gamePathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.gamePathLabel.Name = "gamePathLabel";
            this.gamePathLabel.Size = new System.Drawing.Size(63, 13);
            this.gamePathLabel.TabIndex = 1;
            this.gamePathLabel.Text = "Game Path:";
            // 
            // checkVersionCheckBox
            // 
            this.checkVersionCheckBox.AutoSize = true;
            this.checkVersionCheckBox.Location = new System.Drawing.Point(26, 205);
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
            this.autoBackToMainCheckBox.Location = new System.Drawing.Point(9, 61);
            this.autoBackToMainCheckBox.Name = "autoBackToMainCheckBox";
            this.autoBackToMainCheckBox.Size = new System.Drawing.Size(218, 17);
            this.autoBackToMainCheckBox.TabIndex = 12;
            this.autoBackToMainCheckBox.Text = "Switch to main Stash when Stash closed";
            this.autoBackToMainCheckBox.UseVisualStyleBackColor = true;
            this.autoBackToMainCheckBox.CheckedChanged += new System.EventHandler(this.AutoBackToMainCheckBox_CheckedChanged);
            // 
            // confirmStashDeleteCheckBox
            // 
            this.confirmStashDeleteCheckBox.AutoSize = true;
            this.confirmStashDeleteCheckBox.Location = new System.Drawing.Point(26, 228);
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
            this.closeWithGrimDawnCheckBox.Location = new System.Drawing.Point(26, 182);
            this.closeWithGrimDawnCheckBox.Name = "closeWithGrimDawnCheckBox";
            this.closeWithGrimDawnCheckBox.Size = new System.Drawing.Size(238, 17);
            this.closeWithGrimDawnCheckBox.TabIndex = 1;
            this.closeWithGrimDawnCheckBox.Text = "Close GDMultiStash when Grim Dawn closed";
            this.closeWithGrimDawnCheckBox.UseVisualStyleBackColor = true;
            this.closeWithGrimDawnCheckBox.CheckedChanged += new System.EventHandler(this.CloseWithGrimDawnCheckBox_CheckedChanged);
            // 
            // confirmClosingCheckBox
            // 
            this.confirmClosingCheckBox.AutoSize = true;
            this.confirmClosingCheckBox.Location = new System.Drawing.Point(26, 159);
            this.confirmClosingCheckBox.Name = "confirmClosingCheckBox";
            this.confirmClosingCheckBox.Size = new System.Drawing.Size(246, 17);
            this.confirmClosingCheckBox.TabIndex = 0;
            this.confirmClosingCheckBox.Text = "Confirm closing when Grim Dawn is still running";
            this.confirmClosingCheckBox.UseVisualStyleBackColor = true;
            this.confirmClosingCheckBox.CheckedChanged += new System.EventHandler(this.ConfirmClosingCheckBox_CheckedChanged);
            // 
            // maxBackupsValueLabel
            // 
            this.maxBackupsValueLabel.AutoSize = true;
            this.maxBackupsValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxBackupsValueLabel.Location = new System.Drawing.Point(398, 29);
            this.maxBackupsValueLabel.Name = "maxBackupsValueLabel";
            this.maxBackupsValueLabel.Size = new System.Drawing.Size(46, 18);
            this.maxBackupsValueLabel.TabIndex = 5;
            this.maxBackupsValueLabel.Text = "label1";
            // 
            // maxBackupsLabel
            // 
            this.maxBackupsLabel.AutoSize = true;
            this.maxBackupsLabel.Location = new System.Drawing.Point(6, 29);
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
            this.maxBackupsTrackBar.LargeChange = 1;
            this.maxBackupsTrackBar.Location = new System.Drawing.Point(201, 29);
            this.maxBackupsTrackBar.Maximum = 6;
            this.maxBackupsTrackBar.Name = "maxBackupsTrackBar";
            this.maxBackupsTrackBar.Size = new System.Drawing.Size(191, 20);
            this.maxBackupsTrackBar.TabIndex = 3;
            this.maxBackupsTrackBar.Scroll += new System.EventHandler(this.MaxBackupsTrackBar_Scroll);
            // 
            // overlayWidthLabel
            // 
            this.overlayWidthLabel.AutoSize = true;
            this.overlayWidthLabel.Location = new System.Drawing.Point(6, 26);
            this.overlayWidthLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayWidthLabel.Name = "overlayWidthLabel";
            this.overlayWidthLabel.Size = new System.Drawing.Size(77, 13);
            this.overlayWidthLabel.TabIndex = 7;
            this.overlayWidthLabel.Text = "Overlay Width:";
            // 
            // overlayTransparencyValueLabel
            // 
            this.overlayTransparencyValueLabel.AutoSize = true;
            this.overlayTransparencyValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayTransparencyValueLabel.Location = new System.Drawing.Point(398, 78);
            this.overlayTransparencyValueLabel.Name = "overlayTransparencyValueLabel";
            this.overlayTransparencyValueLabel.Size = new System.Drawing.Size(46, 18);
            this.overlayTransparencyValueLabel.TabIndex = 19;
            this.overlayTransparencyValueLabel.Text = "label1";
            // 
            // overlayWidthTrackBar
            // 
            this.overlayWidthTrackBar.AutoSize = false;
            this.overlayWidthTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayWidthTrackBar.LargeChange = 1;
            this.overlayWidthTrackBar.Location = new System.Drawing.Point(201, 26);
            this.overlayWidthTrackBar.Maximum = 20;
            this.overlayWidthTrackBar.Name = "overlayWidthTrackBar";
            this.overlayWidthTrackBar.Size = new System.Drawing.Size(191, 20);
            this.overlayWidthTrackBar.TabIndex = 6;
            this.overlayWidthTrackBar.Scroll += new System.EventHandler(this.OverlayWidthTrackBar_Scroll);
            // 
            // overlayScaleValueLabel
            // 
            this.overlayScaleValueLabel.AutoSize = true;
            this.overlayScaleValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayScaleValueLabel.Location = new System.Drawing.Point(398, 52);
            this.overlayScaleValueLabel.Name = "overlayScaleValueLabel";
            this.overlayScaleValueLabel.Size = new System.Drawing.Size(46, 18);
            this.overlayScaleValueLabel.TabIndex = 11;
            this.overlayScaleValueLabel.Text = "label1";
            // 
            // overlayTransparencyLabel
            // 
            this.overlayTransparencyLabel.AutoSize = true;
            this.overlayTransparencyLabel.Location = new System.Drawing.Point(6, 78);
            this.overlayTransparencyLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayTransparencyLabel.Name = "overlayTransparencyLabel";
            this.overlayTransparencyLabel.Size = new System.Drawing.Size(114, 13);
            this.overlayTransparencyLabel.TabIndex = 18;
            this.overlayTransparencyLabel.Text = "Overlay Transparency:";
            // 
            // overlayScaleLabel
            // 
            this.overlayScaleLabel.AutoSize = true;
            this.overlayScaleLabel.Location = new System.Drawing.Point(6, 52);
            this.overlayScaleLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayScaleLabel.Name = "overlayScaleLabel";
            this.overlayScaleLabel.Size = new System.Drawing.Size(76, 13);
            this.overlayScaleLabel.TabIndex = 10;
            this.overlayScaleLabel.Text = "Overlay Scale:";
            // 
            // overlayWidthValueLabel
            // 
            this.overlayWidthValueLabel.AutoSize = true;
            this.overlayWidthValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayWidthValueLabel.Location = new System.Drawing.Point(398, 26);
            this.overlayWidthValueLabel.Name = "overlayWidthValueLabel";
            this.overlayWidthValueLabel.Size = new System.Drawing.Size(46, 18);
            this.overlayWidthValueLabel.TabIndex = 8;
            this.overlayWidthValueLabel.Text = "label1";
            // 
            // overlayScaleTrackBar
            // 
            this.overlayScaleTrackBar.AutoSize = false;
            this.overlayScaleTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayScaleTrackBar.LargeChange = 1;
            this.overlayScaleTrackBar.Location = new System.Drawing.Point(201, 52);
            this.overlayScaleTrackBar.Maximum = 20;
            this.overlayScaleTrackBar.Name = "overlayScaleTrackBar";
            this.overlayScaleTrackBar.Size = new System.Drawing.Size(191, 20);
            this.overlayScaleTrackBar.TabIndex = 9;
            this.overlayScaleTrackBar.Scroll += new System.EventHandler(this.OverlayScaleTrackBar_Scroll);
            // 
            // overlayTransparencyTrackBar
            // 
            this.overlayTransparencyTrackBar.AutoSize = false;
            this.overlayTransparencyTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayTransparencyTrackBar.LargeChange = 1;
            this.overlayTransparencyTrackBar.Location = new System.Drawing.Point(201, 78);
            this.overlayTransparencyTrackBar.Name = "overlayTransparencyTrackBar";
            this.overlayTransparencyTrackBar.Size = new System.Drawing.Size(191, 20);
            this.overlayTransparencyTrackBar.TabIndex = 17;
            this.overlayTransparencyTrackBar.Scroll += new System.EventHandler(this.OverlayTransparencyTrackBar_Scroll);
            // 
            // createShortcutButton
            // 
            this.createShortcutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createShortcutButton.AutoSize = true;
            this.createShortcutButton.Location = new System.Drawing.Point(6, 465);
            this.createShortcutButton.Name = "createShortcutButton";
            this.createShortcutButton.Size = new System.Drawing.Size(226, 23);
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
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1110, 566);
            this.panel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.languageComboBox);
            this.panel4.Controls.Add(this.extractTranslationFilesButton);
            this.panel4.Controls.Add(this.cleanupBackupsButton);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.createShortcutButton);
            this.panel4.Controls.Add(this.languageLabel);
            this.panel4.Controls.Add(this.gamePathLabel);
            this.panel4.Controls.Add(this.gamePathSearchButton);
            this.panel4.Controls.Add(this.gameInstallPathsComboBox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(1110, 566);
            this.panel4.TabIndex = 1;
            // 
            // languageComboBox
            // 
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Location = new System.Drawing.Point(6, 28);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(226, 21);
            this.languageComboBox.TabIndex = 9;
            // 
            // extractTranslationFilesButton
            // 
            this.extractTranslationFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.extractTranslationFilesButton.AutoSize = true;
            this.extractTranslationFilesButton.Location = new System.Drawing.Point(6, 523);
            this.extractTranslationFilesButton.Name = "extractTranslationFilesButton";
            this.extractTranslationFilesButton.Size = new System.Drawing.Size(226, 23);
            this.extractTranslationFilesButton.TabIndex = 21;
            this.extractTranslationFilesButton.Text = "Extract translation files";
            this.extractTranslationFilesButton.UseVisualStyleBackColor = true;
            this.extractTranslationFilesButton.Click += new System.EventHandler(this.ExtractTranslationFilesButton_Click);
            // 
            // cleanupBackupsButton
            // 
            this.cleanupBackupsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cleanupBackupsButton.AutoSize = true;
            this.cleanupBackupsButton.Location = new System.Drawing.Point(6, 494);
            this.cleanupBackupsButton.Name = "cleanupBackupsButton";
            this.cleanupBackupsButton.Size = new System.Drawing.Size(226, 23);
            this.cleanupBackupsButton.TabIndex = 4;
            this.cleanupBackupsButton.Text = "Cleanup Backups";
            this.cleanupBackupsButton.UseVisualStyleBackColor = true;
            this.cleanupBackupsButton.Click += new System.EventHandler(this.CleanupBackupsButton_Click);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.behaviorGroupBox);
            this.panel5.Controls.Add(this.overlayWindowGroupBox);
            this.panel5.Controls.Add(this.confirmClosingCheckBox);
            this.panel5.Controls.Add(this.confirmStashDeleteCheckBox);
            this.panel5.Controls.Add(this.closeWithGrimDawnCheckBox);
            this.panel5.Controls.Add(this.checkVersionCheckBox);
            this.panel5.Controls.Add(this.gameStartGroupBox);
            this.panel5.Location = new System.Drawing.Point(238, 55);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(866, 491);
            this.panel5.TabIndex = 20;
            // 
            // behaviorGroupBox
            // 
            this.behaviorGroupBox.Controls.Add(this.saveExternalChangesCheckBox);
            this.behaviorGroupBox.Controls.Add(this.autoBackToMainCheckBox);
            this.behaviorGroupBox.Controls.Add(this.saveOverLockedCheckBox);
            this.behaviorGroupBox.Controls.Add(this.selectFirstStashInGroupCheckBox);
            this.behaviorGroupBox.Controls.Add(this.maxBackupsValueLabel);
            this.behaviorGroupBox.Controls.Add(this.maxBackupsTrackBar);
            this.behaviorGroupBox.Controls.Add(this.maxBackupsLabel);
            this.behaviorGroupBox.Location = new System.Drawing.Point(394, 192);
            this.behaviorGroupBox.Name = "behaviorGroupBox";
            this.behaviorGroupBox.Size = new System.Drawing.Size(459, 172);
            this.behaviorGroupBox.TabIndex = 28;
            this.behaviorGroupBox.TabStop = false;
            this.behaviorGroupBox.Text = "Behavior";
            // 
            // saveExternalChangesCheckBox
            // 
            this.saveExternalChangesCheckBox.AutoSize = true;
            this.saveExternalChangesCheckBox.Location = new System.Drawing.Point(9, 130);
            this.saveExternalChangesCheckBox.Name = "saveExternalChangesCheckBox";
            this.saveExternalChangesCheckBox.Size = new System.Drawing.Size(203, 17);
            this.saveExternalChangesCheckBox.TabIndex = 27;
            this.saveExternalChangesCheckBox.Text = "Save changes made by external tools";
            this.saveExternalChangesCheckBox.UseVisualStyleBackColor = true;
            this.saveExternalChangesCheckBox.CheckedChanged += new System.EventHandler(this.SaveExternalChangesCheckBox_CheckedChanged);
            // 
            // saveOverLockedCheckBox
            // 
            this.saveOverLockedCheckBox.AutoSize = true;
            this.saveOverLockedCheckBox.Location = new System.Drawing.Point(9, 107);
            this.saveOverLockedCheckBox.Name = "saveOverLockedCheckBox";
            this.saveOverLockedCheckBox.Size = new System.Drawing.Size(261, 17);
            this.saveOverLockedCheckBox.TabIndex = 21;
            this.saveOverLockedCheckBox.Text = "Overwrite locked stashes when using save button";
            this.saveOverLockedCheckBox.UseVisualStyleBackColor = true;
            this.saveOverLockedCheckBox.CheckedChanged += new System.EventHandler(this.SaveOverLockedCheckBox_CheckedChanged);
            // 
            // selectFirstStashInGroupCheckBox
            // 
            this.selectFirstStashInGroupCheckBox.AutoSize = true;
            this.selectFirstStashInGroupCheckBox.Location = new System.Drawing.Point(9, 84);
            this.selectFirstStashInGroupCheckBox.Name = "selectFirstStashInGroupCheckBox";
            this.selectFirstStashInGroupCheckBox.Size = new System.Drawing.Size(223, 17);
            this.selectFirstStashInGroupCheckBox.TabIndex = 26;
            this.selectFirstStashInGroupCheckBox.Text = "Automatically switch to first Stash in group";
            this.selectFirstStashInGroupCheckBox.UseVisualStyleBackColor = true;
            this.selectFirstStashInGroupCheckBox.CheckedChanged += new System.EventHandler(this.SelectFirstStashInGroupCheckBox_CheckedChanged);
            // 
            // overlayWindowGroupBox
            // 
            this.overlayWindowGroupBox.Controls.Add(this.overlayWidthLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayWidthTrackBar);
            this.overlayWindowGroupBox.Controls.Add(this.overlayShowWorkloadCheckBox);
            this.overlayWindowGroupBox.Controls.Add(this.overlayWidthValueLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayStashesCountLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayScaleTrackBar);
            this.overlayWindowGroupBox.Controls.Add(this.overlayStashesCountTrackBar);
            this.overlayWindowGroupBox.Controls.Add(this.overlayScaleValueLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayStashesCountValueLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayTransparencyValueLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayScaleLabel);
            this.overlayWindowGroupBox.Controls.Add(this.overlayTransparencyTrackBar);
            this.overlayWindowGroupBox.Controls.Add(this.overlayTransparencyLabel);
            this.overlayWindowGroupBox.Location = new System.Drawing.Point(394, 13);
            this.overlayWindowGroupBox.Name = "overlayWindowGroupBox";
            this.overlayWindowGroupBox.Size = new System.Drawing.Size(459, 173);
            this.overlayWindowGroupBox.TabIndex = 27;
            this.overlayWindowGroupBox.TabStop = false;
            this.overlayWindowGroupBox.Text = "Overlay Window";
            // 
            // overlayShowWorkloadCheckBox
            // 
            this.overlayShowWorkloadCheckBox.AutoSize = true;
            this.overlayShowWorkloadCheckBox.Location = new System.Drawing.Point(9, 134);
            this.overlayShowWorkloadCheckBox.Name = "overlayShowWorkloadCheckBox";
            this.overlayShowWorkloadCheckBox.Size = new System.Drawing.Size(147, 17);
            this.overlayShowWorkloadCheckBox.TabIndex = 25;
            this.overlayShowWorkloadCheckBox.Text = "Show workload in overlay";
            this.overlayShowWorkloadCheckBox.UseVisualStyleBackColor = true;
            this.overlayShowWorkloadCheckBox.CheckedChanged += new System.EventHandler(this.OverlayShowWorkloadCheckBox_CheckedChanged);
            // 
            // overlayStashesCountLabel
            // 
            this.overlayStashesCountLabel.AutoSize = true;
            this.overlayStashesCountLabel.Location = new System.Drawing.Point(6, 104);
            this.overlayStashesCountLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.overlayStashesCountLabel.Name = "overlayStashesCountLabel";
            this.overlayStashesCountLabel.Size = new System.Drawing.Size(125, 13);
            this.overlayStashesCountLabel.TabIndex = 23;
            this.overlayStashesCountLabel.Text = "Displayed stashes count:";
            // 
            // overlayStashesCountTrackBar
            // 
            this.overlayStashesCountTrackBar.AutoSize = false;
            this.overlayStashesCountTrackBar.BackColor = System.Drawing.Color.White;
            this.overlayStashesCountTrackBar.LargeChange = 1;
            this.overlayStashesCountTrackBar.Location = new System.Drawing.Point(201, 104);
            this.overlayStashesCountTrackBar.Maximum = 15;
            this.overlayStashesCountTrackBar.Name = "overlayStashesCountTrackBar";
            this.overlayStashesCountTrackBar.Size = new System.Drawing.Size(191, 20);
            this.overlayStashesCountTrackBar.TabIndex = 22;
            this.overlayStashesCountTrackBar.Scroll += new System.EventHandler(this.OverlayShownStashesTrackBar_Scroll);
            // 
            // overlayStashesCountValueLabel
            // 
            this.overlayStashesCountValueLabel.AutoSize = true;
            this.overlayStashesCountValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayStashesCountValueLabel.Location = new System.Drawing.Point(399, 104);
            this.overlayStashesCountValueLabel.Name = "overlayStashesCountValueLabel";
            this.overlayStashesCountValueLabel.Size = new System.Drawing.Size(46, 18);
            this.overlayStashesCountValueLabel.TabIndex = 24;
            this.overlayStashesCountValueLabel.Text = "label1";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(1043, 578);
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
            this.applyButton.Location = new System.Drawing.Point(962, 578);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ConfigurationDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 608);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.panel1);
            this.Name = "ConfigurationDialogForm";
            this.Text = "GDMultiStash : Setup";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.gameStartGroupBox.ResumeLayout(false);
            this.gameStartGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxBackupsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayWidthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayScaleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayTransparencyTrackBar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.behaviorGroupBox.ResumeLayout(false);
            this.behaviorGroupBox.PerformLayout();
            this.overlayWindowGroupBox.ResumeLayout(false);
            this.overlayWindowGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayStashesCountTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label gamePathLabel;
        private System.Windows.Forms.Button gamePathSearchButton;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.CheckBox confirmClosingCheckBox;
        private System.Windows.Forms.CheckBox closeWithGrimDawnCheckBox;
        private System.Windows.Forms.CheckBox confirmStashDeleteCheckBox;
        private System.Windows.Forms.Button createShortcutButton;
        private System.Windows.Forms.CheckBox gameAutoStartCheckBox;
        private System.Windows.Forms.GroupBox gameStartGroupBox;
        private System.Windows.Forms.TextBox autoStartGDArgumentsTextBox;
        private System.Windows.Forms.Label gameStartArgumentsLabel;
        private System.Windows.Forms.Label gameStartCommandLabel;
        private System.Windows.Forms.ComboBox autoStartGDCommandComboBox;
        private System.Windows.Forms.TrackBar maxBackupsTrackBar;
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
        private System.Windows.Forms.CheckBox checkVersionCheckBox;
        private System.Windows.Forms.Label overlayTransparencyValueLabel;
        private System.Windows.Forms.Label overlayTransparencyLabel;
        private System.Windows.Forms.TrackBar overlayTransparencyTrackBar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button cleanupBackupsButton;
        private System.Windows.Forms.CheckBox saveOverLockedCheckBox;
        private System.Windows.Forms.Button extractTranslationFilesButton;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label overlayStashesCountLabel;
        private System.Windows.Forms.TrackBar overlayStashesCountTrackBar;
        private System.Windows.Forms.Label overlayStashesCountValueLabel;
        private System.Windows.Forms.CheckBox overlayShowWorkloadCheckBox;
        private System.Windows.Forms.CheckBox selectFirstStashInGroupCheckBox;
        private System.Windows.Forms.GroupBox overlayWindowGroupBox;
        private System.Windows.Forms.GroupBox behaviorGroupBox;
        private System.Windows.Forms.CheckBox saveExternalChangesCheckBox;
    }
}