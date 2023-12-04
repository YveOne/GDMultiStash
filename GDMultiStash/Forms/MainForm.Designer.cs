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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.titleMenuStrip = new System.Windows.Forms.MenuStrip();
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
            this.titlePanelGameButton = new GDMultiStash.Forms.Controls.ControlBoxButton();
            this.titlePanelTrayButton = new GDMultiStash.Forms.Controls.ControlBoxButton();
            this.borderPanel = new GDMultiStash.Forms.Controls.BaseFormBorderPanel();
            this.titlePanel = new GDMultiStash.Forms.Controls.BaseFormTitlePanel();
            this.formPaddingPanel.SuspendLayout();
            this.borderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleMenuStrip
            // 
            this.titleMenuStrip.BackColor = System.Drawing.Color.DimGray;
            this.titleMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.titleMenuStrip.Location = new System.Drawing.Point(146, 0);
            this.titleMenuStrip.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.titleMenuStrip.Name = "titleMenuStrip";
            this.titleMenuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.titleMenuStrip.Size = new System.Drawing.Size(80, 30);
            this.titleMenuStrip.AutoSize = true;
            this.titleMenuStrip.TabIndex = 0;
            this.titleMenuStrip.Text = "topMenuStrip";
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
            this.pagesPaddingPanel.Size = new System.Drawing.Size(657, 464);
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
            this.formPaddingPanel.Location = new System.Drawing.Point(3, 53);
            this.formPaddingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.formPaddingPanel.Name = "formPaddingPanel";
            this.formPaddingPanel.Padding = new System.Windows.Forms.Padding(20);
            this.formPaddingPanel.Size = new System.Drawing.Size(697, 544);
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
            // titlePanelGameButton
            // 
            this.titlePanelGameButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.titlePanelGameButton.BackColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.titlePanelGameButton.BackColorPressed = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.titlePanelGameButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.titlePanelGameButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.titlePanelGameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titlePanelGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titlePanelGameButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.titlePanelGameButton.ForeColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.titlePanelGameButton.Image = null;
            this.titlePanelGameButton.ImageHover = null;
            this.titlePanelGameButton.Location = new System.Drawing.Point(2, 0);
            this.titlePanelGameButton.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.titlePanelGameButton.Name = "titlePanelGameButton";
            this.titlePanelGameButton.Size = new System.Drawing.Size(150, 30);
            this.titlePanelGameButton.TabIndex = 5;
            this.titlePanelGameButton.Text = "GAME";
            this.titlePanelGameButton.UseVisualStyleBackColor = false;
            // 
            // titlePanelTrayButton
            // 
            this.titlePanelTrayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.titlePanelTrayButton.BackColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.titlePanelTrayButton.BackColorPressed = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.titlePanelTrayButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.titlePanelTrayButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.titlePanelTrayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titlePanelTrayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.titlePanelTrayButton.ForeColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.titlePanelTrayButton.Image = null;
            this.titlePanelTrayButton.ImageHover = null;
            this.titlePanelTrayButton.Location = new System.Drawing.Point(154, 0);
            this.titlePanelTrayButton.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.titlePanelTrayButton.Name = "titlePanelTrayButton";
            this.titlePanelTrayButton.Size = new System.Drawing.Size(40, 30);
            this.titlePanelTrayButton.TabIndex = 35;
            this.titlePanelTrayButton.UseVisualStyleBackColor = false;
            // 
            // borderPanel
            // 
            this.borderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.borderPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("borderPanel.BackgroundImage")));
            this.borderPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.borderPanel.Controls.Add(this.formPaddingPanel);
            this.borderPanel.Controls.Add(this.titlePanel);
            this.borderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel.Location = new System.Drawing.Point(0, 0);
            this.borderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.borderPanel.Name = "borderPanel";
            this.borderPanel.Padding = new System.Windows.Forms.Padding(3);
            this.borderPanel.Size = new System.Drawing.Size(703, 600);
            this.borderPanel.TabIndex = 31;
            // 
            // titlePanel
            // 
            this.titlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.titlePanel.Location = new System.Drawing.Point(3, 3);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(697, 50);
            this.titlePanel.TabIndex = 31;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(703, 600);
            this.Controls.Add(this.borderPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.titleMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "MainForm";
            this.formPaddingPanel.ResumeLayout(false);
            this.borderPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BaseFormBorderPanel borderPanel;
        private Controls.BaseFormTitlePanel titlePanel;
        private GDMultiStash.Forms.Controls.ControlBoxButton titlePanelTrayButton;
        private GDMultiStash.Forms.Controls.ControlBoxButton titlePanelGameButton;




        private System.Windows.Forms.MenuStrip titleMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem captionFileButton;
        private System.Windows.Forms.ToolStripMenuItem captionSettingsButton;
        private System.Windows.Forms.ToolStripMenuItem captionHelpButton;
        private System.Windows.Forms.ToolStripMenuItem captionAboutButton;
        private System.Windows.Forms.Panel pagesPaddingPanel;
        private System.Windows.Forms.ToolStripMenuItem captionChangelogButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportButton;
        private System.Windows.Forms.Panel formPaddingPanel;
        private System.Windows.Forms.ToolStripMenuItem captionImportTransferFilesButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportTransferFilesButton;
        private System.Windows.Forms.Button stashGroupsPageButton;
        private System.Windows.Forms.Button stashesPageButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportGDSCButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportCraftingModeButton;
    }
}