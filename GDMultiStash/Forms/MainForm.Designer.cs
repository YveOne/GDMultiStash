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
            this.captionExportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionExportTransferFilesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionSettingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionHelpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionChangelogButton = new System.Windows.Forms.ToolStripMenuItem();
            this.captionAboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.stashes_menuStrip = new System.Windows.Forms.MenuStrip();
            this.stashes_createStashButton = new System.Windows.Forms.ToolStripMenuItem();
            this.pagesPaddingPanel = new System.Windows.Forms.Panel();
            this.groups_pagePanel = new System.Windows.Forms.Panel();
            this.groups_menuStrip = new System.Windows.Forms.MenuStrip();
            this.groups_createStashGroupButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groups_listViewBorderPanel = new System.Windows.Forms.Panel();
            this.groups_listView = new Controls.OLVCatchScrolling();
            this.stashes_pagePanel = new System.Windows.Forms.Panel();
            this.stashes_shownItemsCountLabel = new System.Windows.Forms.Label();
            this.stashes_menuFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.stashes_showExpansionComboBox = new GDMultiStash.Forms.Controls.FlatComboBox();
            this.stashes_showSoftCoreCheckbox = new GDMultiStash.Forms.Controls.MyCheckBox();
            this.stashes_showHardCoreCheckbox = new GDMultiStash.Forms.Controls.MyCheckBox();
            this.stashes_listViewBorderPanel = new System.Windows.Forms.Panel();
            this.stashes_listView = new GDMultiStash.Forms.Controls.OLVGroupFeatures();
            this.formPaddingPanel = new System.Windows.Forms.Panel();
            this.groupsPageButton = new System.Windows.Forms.Button();
            this.stashesPageButton = new System.Windows.Forms.Button();
            this.titlePanel = new GDMultiStash.Forms.Controls.TransparentPanel();
            this.formBackgroundPanel = new GDMultiStash.Forms.Controls.TransparentPanel();
            this.captionCloseButton = new System.Windows.Forms.Button();
            this.captionMinimizeButton = new System.Windows.Forms.Button();
            this.captionTrayButton = new System.Windows.Forms.Button();
            this.captionGameButton = new System.Windows.Forms.Button();
            this.captionMenuStrip.SuspendLayout();
            this.stashes_menuStrip.SuspendLayout();
            this.pagesPaddingPanel.SuspendLayout();
            this.groups_pagePanel.SuspendLayout();
            this.groups_menuStrip.SuspendLayout();
            this.groups_listViewBorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_listView)).BeginInit();
            this.stashes_pagePanel.SuspendLayout();
            this.stashes_menuFlowLayoutPanel.SuspendLayout();
            this.stashes_listViewBorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stashes_listView)).BeginInit();
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
            this.captionMenuStrip.Location = new System.Drawing.Point(130, 2);
            this.captionMenuStrip.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.captionMenuStrip.Name = "captionMenuStrip";
            this.captionMenuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.captionMenuStrip.Size = new System.Drawing.Size(119, 30);
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
            this.captionImportTransferFilesButton});
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
            // stashes_menuStrip
            // 
            this.stashes_menuStrip.BackColor = System.Drawing.Color.White;
            this.stashes_menuStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stashes_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stashes_createStashButton});
            this.stashes_menuStrip.Location = new System.Drawing.Point(0, 388);
            this.stashes_menuStrip.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.stashes_menuStrip.Name = "stashes_menuStrip";
            this.stashes_menuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.stashes_menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.stashes_menuStrip.Size = new System.Drawing.Size(431, 28);
            this.stashes_menuStrip.TabIndex = 1;
            this.stashes_menuStrip.Text = "bottomMenuStrip";
            // 
            // stashes_createStashButton
            // 
            this.stashes_createStashButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashes_createStashButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.stashes_createStashButton.Name = "stashes_createStashButton";
            this.stashes_createStashButton.Size = new System.Drawing.Size(103, 24);
            this.stashes_createStashButton.Text = "Create Stash";
            this.stashes_createStashButton.Click += new System.EventHandler(this.Stashes_CreateStashButton_Click);
            // 
            // pagesPaddingPanel
            // 
            this.pagesPaddingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pagesPaddingPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pagesPaddingPanel.Controls.Add(this.groups_pagePanel);
            this.pagesPaddingPanel.Controls.Add(this.stashes_pagePanel);
            this.pagesPaddingPanel.Location = new System.Drawing.Point(20, 60);
            this.pagesPaddingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.pagesPaddingPanel.Name = "pagesPaddingPanel";
            this.pagesPaddingPanel.Padding = new System.Windows.Forms.Padding(20);
            this.pagesPaddingPanel.Size = new System.Drawing.Size(782, 456);
            this.pagesPaddingPanel.TabIndex = 2;
            // 
            // groups_pagePanel
            // 
            this.groups_pagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groups_pagePanel.Controls.Add(this.groups_menuStrip);
            this.groups_pagePanel.Controls.Add(this.groups_listViewBorderPanel);
            this.groups_pagePanel.Location = new System.Drawing.Point(461, 20);
            this.groups_pagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.groups_pagePanel.Name = "groups_pagePanel";
            this.groups_pagePanel.Size = new System.Drawing.Size(301, 416);
            this.groups_pagePanel.TabIndex = 2;
            // 
            // groups_menuStrip
            // 
            this.groups_menuStrip.BackColor = System.Drawing.Color.White;
            this.groups_menuStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groups_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groups_createStashGroupButton});
            this.groups_menuStrip.Location = new System.Drawing.Point(0, 388);
            this.groups_menuStrip.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.groups_menuStrip.Name = "groups_menuStrip";
            this.groups_menuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.groups_menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groups_menuStrip.Size = new System.Drawing.Size(301, 28);
            this.groups_menuStrip.TabIndex = 2;
            this.groups_menuStrip.Text = "bottomMenuStrip";
            // 
            // groups_createStashGroupButton
            // 
            this.groups_createStashGroupButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groups_createStashGroupButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.groups_createStashGroupButton.Name = "groups_createStashGroupButton";
            this.groups_createStashGroupButton.Size = new System.Drawing.Size(109, 24);
            this.groups_createStashGroupButton.Text = "Create Group";
            this.groups_createStashGroupButton.Click += new System.EventHandler(this.Groups_CreateStashGroupButton_Click);
            // 
            // groups_listViewBorderPanel
            // 
            this.groups_listViewBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groups_listViewBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groups_listViewBorderPanel.Controls.Add(this.groups_listView);
            this.groups_listViewBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.groups_listViewBorderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.groups_listViewBorderPanel.Name = "groups_listViewBorderPanel";
            this.groups_listViewBorderPanel.Padding = new System.Windows.Forms.Padding(10);
            this.groups_listViewBorderPanel.Size = new System.Drawing.Size(301, 360);
            this.groups_listViewBorderPanel.TabIndex = 0;
            // 
            // groups_listView
            // 
            this.groups_listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.groups_listView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.groups_listView.CellEditUseWholeCell = false;
            this.groups_listView.Cursor = System.Windows.Forms.Cursors.Default;
            this.groups_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groups_listView.FullRowSelect = true;
            this.groups_listView.GridLines = true;
            this.groups_listView.HideSelection = false;
            this.groups_listView.Location = new System.Drawing.Point(10, 10);
            this.groups_listView.Margin = new System.Windows.Forms.Padding(20);
            this.groups_listView.Name = "groups_listView";
            this.groups_listView.SelectColumnsOnRightClick = false;
            this.groups_listView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.groups_listView.ShowGroups = false;
            this.groups_listView.ShowSortIndicators = false;
            this.groups_listView.Size = new System.Drawing.Size(281, 340);
            this.groups_listView.SortGroupItemsByPrimaryColumn = false;
            this.groups_listView.TabIndex = 1;
            this.groups_listView.UseCompatibleStateImageBehavior = false;
            this.groups_listView.View = System.Windows.Forms.View.Details;
            // 
            // stashes_pagePanel
            // 
            this.stashes_pagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.stashes_pagePanel.Controls.Add(this.stashes_shownItemsCountLabel);
            this.stashes_pagePanel.Controls.Add(this.stashes_menuFlowLayoutPanel);
            this.stashes_pagePanel.Controls.Add(this.stashes_listViewBorderPanel);
            this.stashes_pagePanel.Controls.Add(this.stashes_menuStrip);
            this.stashes_pagePanel.Location = new System.Drawing.Point(20, 20);
            this.stashes_pagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.stashes_pagePanel.Name = "stashes_pagePanel";
            this.stashes_pagePanel.Size = new System.Drawing.Size(431, 416);
            this.stashes_pagePanel.TabIndex = 1;
            // 
            // stashes_shownItemsCountLabel
            // 
            this.stashes_shownItemsCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stashes_shownItemsCountLabel.Location = new System.Drawing.Point(10, 360);
            this.stashes_shownItemsCountLabel.Name = "stashes_shownItemsCountLabel";
            this.stashes_shownItemsCountLabel.Size = new System.Drawing.Size(411, 27);
            this.stashes_shownItemsCountLabel.TabIndex = 26;
            this.stashes_shownItemsCountLabel.Text = "x/y stashes";
            this.stashes_shownItemsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stashes_menuFlowLayoutPanel
            // 
            this.stashes_menuFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stashes_menuFlowLayoutPanel.Controls.Add(this.stashes_showExpansionComboBox);
            this.stashes_menuFlowLayoutPanel.Controls.Add(this.stashes_showSoftCoreCheckbox);
            this.stashes_menuFlowLayoutPanel.Controls.Add(this.stashes_showHardCoreCheckbox);
            this.stashes_menuFlowLayoutPanel.Location = new System.Drawing.Point(1, 390);
            this.stashes_menuFlowLayoutPanel.Name = "stashes_menuFlowLayoutPanel";
            this.stashes_menuFlowLayoutPanel.Size = new System.Drawing.Size(364, 25);
            this.stashes_menuFlowLayoutPanel.TabIndex = 25;
            // 
            // stashes_showExpansionComboBox
            // 
            this.stashes_showExpansionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.stashes_showExpansionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stashes_showExpansionComboBox.DropDownWidth = 200;
            this.stashes_showExpansionComboBox.FormattingEnabled = true;
            this.stashes_showExpansionComboBox.Location = new System.Drawing.Point(0, 0);
            this.stashes_showExpansionComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.stashes_showExpansionComboBox.Name = "stashes_showExpansionComboBox";
            this.stashes_showExpansionComboBox.Size = new System.Drawing.Size(212, 23);
            this.stashes_showExpansionComboBox.TabIndex = 22;
            // 
            // stashes_showSoftCoreCheckbox
            // 
            this.stashes_showSoftCoreCheckbox.AutoSize = true;
            this.stashes_showSoftCoreCheckbox.Location = new System.Drawing.Point(228, 3);
            this.stashes_showSoftCoreCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.stashes_showSoftCoreCheckbox.Name = "stashes_showSoftCoreCheckbox";
            this.stashes_showSoftCoreCheckbox.Size = new System.Drawing.Size(15, 14);
            this.stashes_showSoftCoreCheckbox.TabIndex = 23;
            this.stashes_showSoftCoreCheckbox.ThreeState = true;
            this.stashes_showSoftCoreCheckbox.UseVisualStyleBackColor = true;
            // 
            // stashes_showHardCoreCheckbox
            // 
            this.stashes_showHardCoreCheckbox.AutoSize = true;
            this.stashes_showHardCoreCheckbox.Location = new System.Drawing.Point(251, 3);
            this.stashes_showHardCoreCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.stashes_showHardCoreCheckbox.Name = "stashes_showHardCoreCheckbox";
            this.stashes_showHardCoreCheckbox.Size = new System.Drawing.Size(15, 14);
            this.stashes_showHardCoreCheckbox.TabIndex = 24;
            this.stashes_showHardCoreCheckbox.ThreeState = true;
            this.stashes_showHardCoreCheckbox.UseVisualStyleBackColor = true;
            // 
            // stashes_listViewBorderPanel
            // 
            this.stashes_listViewBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stashes_listViewBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.stashes_listViewBorderPanel.Controls.Add(this.stashes_listView);
            this.stashes_listViewBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.stashes_listViewBorderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.stashes_listViewBorderPanel.Name = "stashes_listViewBorderPanel";
            this.stashes_listViewBorderPanel.Padding = new System.Windows.Forms.Padding(10);
            this.stashes_listViewBorderPanel.Size = new System.Drawing.Size(431, 360);
            this.stashes_listViewBorderPanel.TabIndex = 1;
            // 
            // stashes_listView
            // 
            this.stashes_listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stashes_listView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.stashes_listView.CellEditUseWholeCell = false;
            this.stashes_listView.Cursor = System.Windows.Forms.Cursors.Default;
            this.stashes_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stashes_listView.FullRowSelect = true;
            this.stashes_listView.GridLines = true;
            this.stashes_listView.GroupHeadingBackColor = System.Drawing.Color.Gray;
            this.stashes_listView.GroupHeadingCountFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashes_listView.GroupHeadingCountForeColor = System.Drawing.Color.Black;
            this.stashes_listView.GroupHeadingFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashes_listView.GroupHeadingForeColor = System.Drawing.Color.Black;
            this.stashes_listView.HideSelection = false;
            this.stashes_listView.Location = new System.Drawing.Point(10, 10);
            this.stashes_listView.Margin = new System.Windows.Forms.Padding(20);
            this.stashes_listView.Name = "stashes_listView";
            this.stashes_listView.SelectColumnsOnRightClick = false;
            this.stashes_listView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.stashes_listView.SeparatorColor = System.Drawing.Color.Empty;
            this.stashes_listView.ShowSortIndicators = false;
            this.stashes_listView.Size = new System.Drawing.Size(411, 340);
            this.stashes_listView.SortGroupItemsByPrimaryColumn = false;
            this.stashes_listView.TabIndex = 0;
            this.stashes_listView.UseCompatibleStateImageBehavior = false;
            this.stashes_listView.View = System.Windows.Forms.View.Details;
            // 
            // formPaddingPanel
            // 
            this.formPaddingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formPaddingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.formPaddingPanel.Controls.Add(this.groupsPageButton);
            this.formPaddingPanel.Controls.Add(this.stashesPageButton);
            this.formPaddingPanel.Controls.Add(this.pagesPaddingPanel);
            this.formPaddingPanel.Location = new System.Drawing.Point(8, 54);
            this.formPaddingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.formPaddingPanel.Name = "formPaddingPanel";
            this.formPaddingPanel.Padding = new System.Windows.Forms.Padding(20);
            this.formPaddingPanel.Size = new System.Drawing.Size(822, 536);
            this.formPaddingPanel.TabIndex = 28;
            // 
            // groupsPageButton
            // 
            this.groupsPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupsPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupsPageButton.Location = new System.Drawing.Point(165, 28);
            this.groupsPageButton.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.groupsPageButton.Name = "groupsPageButton";
            this.groupsPageButton.Size = new System.Drawing.Size(143, 32);
            this.groupsPageButton.TabIndex = 4;
            this.groupsPageButton.Text = "Groups";
            this.groupsPageButton.UseVisualStyleBackColor = true;
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
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.captionMenuStrip.ResumeLayout(false);
            this.captionMenuStrip.PerformLayout();
            this.stashes_menuStrip.ResumeLayout(false);
            this.stashes_menuStrip.PerformLayout();
            this.pagesPaddingPanel.ResumeLayout(false);
            this.groups_pagePanel.ResumeLayout(false);
            this.groups_pagePanel.PerformLayout();
            this.groups_menuStrip.ResumeLayout(false);
            this.groups_menuStrip.PerformLayout();
            this.groups_listViewBorderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groups_listView)).EndInit();
            this.stashes_pagePanel.ResumeLayout(false);
            this.stashes_pagePanel.PerformLayout();
            this.stashes_menuFlowLayoutPanel.ResumeLayout(false);
            this.stashes_menuFlowLayoutPanel.PerformLayout();
            this.stashes_listViewBorderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stashes_listView)).EndInit();
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
        private System.Windows.Forms.MenuStrip stashes_menuStrip;
        private System.Windows.Forms.Panel pagesPaddingPanel;
        private Controls.OLVGroupFeatures stashes_listView;
        private System.Windows.Forms.ToolStripMenuItem stashes_createStashButton;
        private Controls.FlatComboBox stashes_showExpansionComboBox;
        private Controls.MyCheckBox stashes_showSoftCoreCheckbox;
        private Controls.MyCheckBox stashes_showHardCoreCheckbox;
        private System.Windows.Forms.ToolStripMenuItem captionChangelogButton;
        private System.Windows.Forms.ToolStripMenuItem captionImportButton;
        private System.Windows.Forms.Panel formPaddingPanel;
        private GDMultiStash.Forms.Controls.TransparentPanel titlePanel;
        private GDMultiStash.Forms.Controls.TransparentPanel formBackgroundPanel;
        private System.Windows.Forms.ToolStripMenuItem captionImportTransferFilesButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportButton;
        private System.Windows.Forms.ToolStripMenuItem captionExportTransferFilesButton;
        private System.Windows.Forms.Panel stashes_listViewBorderPanel;
        private System.Windows.Forms.Panel stashes_pagePanel;
        private System.Windows.Forms.Button groupsPageButton;
        private System.Windows.Forms.Button stashesPageButton;
        private System.Windows.Forms.Panel groups_pagePanel;
        private System.Windows.Forms.Button captionGameButton;
        private System.Windows.Forms.Button captionTrayButton;
        private System.Windows.Forms.Button captionMinimizeButton;
        private System.Windows.Forms.Button captionCloseButton;
        private System.Windows.Forms.FlowLayoutPanel stashes_menuFlowLayoutPanel;
        private System.Windows.Forms.Label stashes_shownItemsCountLabel;
        private System.Windows.Forms.MenuStrip groups_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem groups_createStashGroupButton;
        private System.Windows.Forms.Panel groups_listViewBorderPanel;
        private Controls.OLVCatchScrolling groups_listView;
    }
}