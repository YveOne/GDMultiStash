namespace GDMultiStash.Forms
{
    partial class MainForm
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
            this.captionMenuStrip = new System.Windows.Forms.MenuStrip();
            this.captionFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionImportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionImportTransferFilesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionImportGDSCButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionImportCraftingModeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionExportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionExportTransferFilesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionSettingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionHelpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionChangelogButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionAboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.pagesPaddingPanel = new System.Windows.Forms.Panel();
            this.formPaddingPanel = new System.Windows.Forms.Panel();
            this.stashGroupsPageButton = new System.Windows.Forms.Button();
            this.stashesPageButton = new System.Windows.Forms.Button();
            this.titlePanel = new GDMultiStash.Forms.Controls.TransparentPanel();
            this.formBackgroundPanel = new GDMultiStash.Forms.Controls.TransparentPanel();
            this.captionCloseButton = new System.Windows.Forms.Button();
            this.captionMinimizeButton = new System.Windows.Forms.Button();
            this.captionTrayButton = new System.Windows.Forms.Button();
            this.captionGameButton = new System.Windows.Forms.Button();
            this.captionMenuStrip.SuspendLayout();
            this.formPaddingPanel.SuspendLayout();
            this.formBackgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // captionMenuStrip
            // 
            this.captionMenuStrip.BackColor = System.Drawing.Color.DimGray;
            this.captionMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.captionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captionFileButton,
            this.captionHelpButton});
            this.captionMenuStrip.Location = new System.Drawing.Point(141, 2);
            this.captionMenuStrip.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionMenuStrip.Name = "captionMenuStrip";
            this.captionMenuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.captionMenuStrip.Size = new System.Drawing.Size(239, 30);
            this.captionMenuStrip.TabIndex = 0;
            this.captionMenuStrip.Text = "topMenuStrip";
            // 
            // captionFileButton
            // 
            this.captionFileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captionImportButton,
            this.captionExportButton,
            this.captionSettingsButton});
            this.captionFileButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionFileButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.captionFileButton.Name = "captionFileButton";
            this.captionFileButton.Padding = new System.Windows.Forms.Padding(3);
            this.captionFileButton.Size = new System.Drawing.Size(42, 30);
            this.captionFileButton.Text = "File";
            // 
            // captionImportButton
            // 
            this.captionImportButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captionImportTransferFilesButton,
            this.captionImportGDSCButton,
            this.captionImportCraftingModeButton});
            this.captionImportButton.Name = "captionImportButton";
            this.captionImportButton.Size = new System.Drawing.Size(131, 24);
            this.captionImportButton.Text = "Import";
            // 
            // captionImportTransferFilesButton
            // 
            this.captionImportTransferFilesButton.Name = "captionImportTransferFilesButton";
            this.captionImportTransferFilesButton.Size = new System.Drawing.Size(212, 24);
            this.captionImportTransferFilesButton.Text = "Import Transfer Files";
            this.captionImportTransferFilesButton.Click += new System.EventHandler(this.ImportTransferFilesButton_Click);
            // 
            // captionImportGDSCButton
            // 
            this.captionImportGDSCButton.Name = "captionImportGDSCButton";
            this.captionImportGDSCButton.Size = new System.Drawing.Size(212, 24);
            this.captionImportGDSCButton.Text = "GD Stash Changer";
            this.captionImportGDSCButton.Click += new System.EventHandler(this.ImportGDSCButton_Click);
            // 
            // captionImportCraftingModeButton
            // 
            this.captionImportCraftingModeButton.Name = "captionImportCraftingModeButton";
            this.captionImportCraftingModeButton.Size = new System.Drawing.Size(212, 24);
            this.captionImportCraftingModeButton.Text = "Crafting Mode";
            this.captionImportCraftingModeButton.Click += new System.EventHandler(this.CaptionImportCraftingModeButton_Click);
            // 
            // captionExportButton
            // 
            this.captionExportButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captionExportTransferFilesButton});
            this.captionExportButton.Name = "captionExportButton";
            this.captionExportButton.Size = new System.Drawing.Size(131, 24);
            this.captionExportButton.Text = "Export";
            // 
            // captionExportTransferFilesButton
            // 
            this.captionExportTransferFilesButton.Name = "captionExportTransferFilesButton";
            this.captionExportTransferFilesButton.Size = new System.Drawing.Size(163, 24);
            this.captionExportTransferFilesButton.Text = "Transfer Files";
            this.captionExportTransferFilesButton.Click += new System.EventHandler(this.TopMenuExportTransferFilesButton_Click);
            // 
            // captionSettingsButton
            // 
            this.captionSettingsButton.Name = "captionSettingsButton";
            this.captionSettingsButton.Size = new System.Drawing.Size(131, 24);
            this.captionSettingsButton.Text = "Settings";
            this.captionSettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // captionHelpButton
            // 
            this.captionHelpButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captionChangelogButton,
            this.captionAboutButton});
            this.captionHelpButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionHelpButton.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.captionHelpButton.Name = "captionHelpButton";
            this.captionHelpButton.Padding = new System.Windows.Forms.Padding(3);
            this.captionHelpButton.Size = new System.Drawing.Size(51, 30);
            this.captionHelpButton.Text = "Help";
            // 
            // captionChangelogButton
            // 
            this.captionChangelogButton.Name = "captionChangelogButton";
            this.captionChangelogButton.Size = new System.Drawing.Size(150, 24);
            this.captionChangelogButton.Text = "Changelog";
            this.captionChangelogButton.Click += new System.EventHandler(this.ChangelogButton_Click);
            // 
            // captionAboutButton
            // 
            this.captionAboutButton.Name = "captionAboutButton";
            this.captionAboutButton.Size = new System.Drawing.Size(150, 24);
            this.captionAboutButton.Text = "About";
            this.captionAboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // pagesPaddingPanel
            // 
            this.pagesPaddingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pagesPaddingPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pagesPaddingPanel.Location = new System.Drawing.Point(20, 60);
            this.pagesPaddingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.pagesPaddingPanel.Name = "pagesPaddingPanel";
            this.pagesPaddingPanel.Padding = new System.Windows.Forms.Padding(20);
            this.pagesPaddingPanel.Size = new System.Drawing.Size(782, 456);
            this.pagesPaddingPanel.TabIndex = 2;
            // 
            // formPaddingPanel
            // 
            this.formPaddingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formPaddingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.formPaddingPanel.Controls.Add(this.stashGroupsPageButton);
            this.formPaddingPanel.Controls.Add(this.stashesPageButton);
            this.formPaddingPanel.Controls.Add(this.pagesPaddingPanel);
            this.formPaddingPanel.Location = new System.Drawing.Point(8, 54);
            this.formPaddingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.formPaddingPanel.Name = "formPaddingPanel";
            this.formPaddingPanel.Padding = new System.Windows.Forms.Padding(20);
            this.formPaddingPanel.Size = new System.Drawing.Size(822, 536);
            this.formPaddingPanel.TabIndex = 28;
            // 
            // stashGroupsPageButton
            // 
            this.stashGroupsPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stashGroupsPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashGroupsPageButton.Location = new System.Drawing.Point(165, 28);
            this.stashGroupsPageButton.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.stashGroupsPageButton.Name = "stashGroupsPageButton";
            this.stashGroupsPageButton.Size = new System.Drawing.Size(143, 32);
            this.stashGroupsPageButton.TabIndex = 4;
            this.stashGroupsPageButton.Text = "Groups";
            this.stashGroupsPageButton.UseVisualStyleBackColor = true;
            // 
            // stashesPageButton
            // 
            this.stashesPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stashesPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashesPageButton.Location = new System.Drawing.Point(20, 28);
            this.stashesPageButton.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.stashesPageButton.Name = "stashesPageButton";
            this.stashesPageButton.Size = new System.Drawing.Size(143, 32);
            this.stashesPageButton.TabIndex = 3;
            this.stashesPageButton.Text = "Stashes";
            this.stashesPageButton.UseVisualStyleBackColor = true;
            // 
            // titlePanel
            // 
            this.titlePanel.Location = new System.Drawing.Point(11, 11);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(115, 40);
            this.titlePanel.TabIndex = 29;
            // 
            // formBackgroundPanel
            // 
            this.formBackgroundPanel.Controls.Add(this.captionCloseButton);
            this.formBackgroundPanel.Controls.Add(this.captionMinimizeButton);
            this.formBackgroundPanel.Controls.Add(this.captionTrayButton);
            this.formBackgroundPanel.Controls.Add(this.captionGameButton);
            this.formBackgroundPanel.Controls.Add(this.captionMenuStrip);
            this.formBackgroundPanel.Controls.Add(this.titlePanel);
            this.formBackgroundPanel.Controls.Add(this.formPaddingPanel);
            this.formBackgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formBackgroundPanel.Location = new System.Drawing.Point(1, 1);
            this.formBackgroundPanel.Margin = new System.Windows.Forms.Padding(0);
            this.formBackgroundPanel.Name = "formBackgroundPanel";
            this.formBackgroundPanel.Padding = new System.Windows.Forms.Padding(8);
            this.formBackgroundPanel.Size = new System.Drawing.Size(838, 598);
            this.formBackgroundPanel.TabIndex = 30;
            // 
            // captionCloseButton
            // 
            this.captionCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.captionCloseButton.BackColor = System.Drawing.Color.DimGray;
            this.captionCloseButton.FlatAppearance.BorderSize = 0;
            this.captionCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.captionCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionCloseButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.captionCloseButton.Location = new System.Drawing.Point(796, 2);
            this.captionCloseButton.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionCloseButton.Name = "captionCloseButton";
            this.captionCloseButton.Size = new System.Drawing.Size(40, 30);
            this.captionCloseButton.TabIndex = 37;
            this.captionCloseButton.UseVisualStyleBackColor = false;
            // 
            // captionMinimizeButton
            // 
            this.captionMinimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.captionMinimizeButton.BackColor = System.Drawing.Color.DimGray;
            this.captionMinimizeButton.FlatAppearance.BorderSize = 0;
            this.captionMinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.captionMinimizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionMinimizeButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.captionMinimizeButton.Location = new System.Drawing.Point(754, 2);
            this.captionMinimizeButton.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionMinimizeButton.Name = "captionMinimizeButton";
            this.captionMinimizeButton.Size = new System.Drawing.Size(40, 30);
            this.captionMinimizeButton.TabIndex = 36;
            this.captionMinimizeButton.UseVisualStyleBackColor = false;
            // 
            // captionTrayButton
            // 
            this.captionTrayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.captionTrayButton.BackColor = System.Drawing.Color.DimGray;
            this.captionTrayButton.FlatAppearance.BorderSize = 0;
            this.captionTrayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.captionTrayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionTrayButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.captionTrayButton.Location = new System.Drawing.Point(712, 2);
            this.captionTrayButton.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionTrayButton.Name = "captionTrayButton";
            this.captionTrayButton.Size = new System.Drawing.Size(40, 30);
            this.captionTrayButton.TabIndex = 35;
            this.captionTrayButton.UseVisualStyleBackColor = false;
            // 
            // captionGameButton
            // 
            this.captionGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.captionGameButton.BackColor = System.Drawing.Color.DimGray;
            this.captionGameButton.FlatAppearance.BorderSize = 0;
            this.captionGameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.captionGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionGameButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.captionGameButton.Location = new System.Drawing.Point(567, 2);
            this.captionGameButton.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionGameButton.Name = "captionGameButton";
            this.captionGameButton.Size = new System.Drawing.Size(143, 30);
            this.captionGameButton.TabIndex = 5;
            this.captionGameButton.Text = "Groups";
            this.captionGameButton.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(840, 600);
            this.Controls.Add(this.formBackgroundPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.captionMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.captionMenuStrip.ResumeLayout(false);
            this.captionMenuStrip.PerformLayout();
            this.formPaddingPanel.ResumeLayout(false);
            this.formBackgroundPanel.ResumeLayout(false);
            this.formBackgroundPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip captionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem captionFileButton;
        private System.Windows.Forms.ToolStripMenuItem captionSettingsButton;
        private System.Windows.Forms.ToolStripMenuItem captionHelpButton;
        private System.Windows.Forms.ToolStripMenuItem captionAboutButton;
        private System.Windows.Forms.Panel pagesPaddingPanel;
        private System.Windows.Forms.ToolStripMenuItem captionChangelogButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportButton;
        private System.Windows.Forms.Panel formPaddingPanel;
        private GDMultiStash.Forms.Controls.TransparentPanel titlePanel;
        private GDMultiStash.Forms.Controls.TransparentPanel formBackgroundPanel;
        private System.Windows.Forms.ToolStripMenuItem captionImportTransferFilesButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportTransferFilesButton;
        private System.Windows.Forms.Button stashGroupsPageButton;
        private System.Windows.Forms.Button stashesPageButton;
        private System.Windows.Forms.Button captionGameButton;
        private System.Windows.Forms.Button captionTrayButton;
        private System.Windows.Forms.Button captionMinimizeButton;
        private System.Windows.Forms.Button captionCloseButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportGDSCButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportCraftingModeButton;
    }
}