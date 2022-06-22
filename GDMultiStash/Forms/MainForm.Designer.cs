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
            this.topMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomMenuStrip = new System.Windows.Forms.MenuStrip();
            this.importStashesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.createStashButton = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.stashesListView = new BrightIdeasSoftware.ObjectListView();
            this.showExpansionComboBox = new System.Windows.Forms.ComboBox();
            this.showSoftCoreComboBox = new System.Windows.Forms.CheckBox();
            this.showHardCoreComboBox = new System.Windows.Forms.CheckBox();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.unselectAllButton = new System.Windows.Forms.Button();
            this.displayMenuPanel = new System.Windows.Forms.Panel();
            this.topMenuStrip.SuspendLayout();
            this.bottomMenuStrip.SuspendLayout();
            this.backgroundPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stashesListView)).BeginInit();
            this.displayMenuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMenuStrip
            // 
            this.topMenuStrip.BackColor = System.Drawing.Color.White;
            this.topMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileButton,
            this.helpButton});
            this.topMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.topMenuStrip.Name = "topMenuStrip";
            this.topMenuStrip.Size = new System.Drawing.Size(634, 24);
            this.topMenuStrip.TabIndex = 0;
            this.topMenuStrip.Text = "topMenuStrip";
            // 
            // fileButton
            // 
            this.fileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsButton});
            this.fileButton.Name = "fileButton";
            this.fileButton.Size = new System.Drawing.Size(37, 20);
            this.fileButton.Text = "File";
            // 
            // settingsButton
            // 
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(116, 22);
            this.settingsButton.Text = "Settings";
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutButton});
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(44, 20);
            this.helpButton.Text = "Help";
            // 
            // aboutButton
            // 
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(107, 22);
            this.aboutButton.Text = "About";
            this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // bottomMenuStrip
            // 
            this.bottomMenuStrip.BackColor = System.Drawing.Color.White;
            this.bottomMenuStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importStashesButton,
            this.createStashButton});
            this.bottomMenuStrip.Location = new System.Drawing.Point(0, 437);
            this.bottomMenuStrip.Name = "bottomMenuStrip";
            this.bottomMenuStrip.Padding = new System.Windows.Forms.Padding(6, 0, 0, 2);
            this.bottomMenuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bottomMenuStrip.Size = new System.Drawing.Size(634, 24);
            this.bottomMenuStrip.TabIndex = 1;
            this.bottomMenuStrip.Text = "bottomMenuStrip";
            // 
            // importStashesButton
            // 
            this.importStashesButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.importStashesButton.Name = "importStashesButton";
            this.importStashesButton.Size = new System.Drawing.Size(97, 20);
            this.importStashesButton.Text = "Import Stashes";
            this.importStashesButton.Click += new System.EventHandler(this.ImportStashesButton_Click);
            // 
            // createStashButton
            // 
            this.createStashButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.createStashButton.Name = "createStashButton";
            this.createStashButton.Size = new System.Drawing.Size(84, 20);
            this.createStashButton.Text = "Create Stash";
            this.createStashButton.Click += new System.EventHandler(this.CreateStashButton_Click);
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.backgroundPanel.Controls.Add(this.stashesListView);
            this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundPanel.Location = new System.Drawing.Point(0, 24);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Padding = new System.Windows.Forms.Padding(5);
            this.backgroundPanel.Size = new System.Drawing.Size(634, 413);
            this.backgroundPanel.TabIndex = 2;
            // 
            // stashesListView
            // 
            this.stashesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stashesListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.stashesListView.CellEditUseWholeCell = false;
            this.stashesListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.stashesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stashesListView.FullRowSelect = true;
            this.stashesListView.GridLines = true;
            this.stashesListView.HeaderUsesThemes = true;
            this.stashesListView.HideSelection = false;
            this.stashesListView.Location = new System.Drawing.Point(5, 5);
            this.stashesListView.Name = "stashesListView";
            this.stashesListView.RowHeight = 20;
            this.stashesListView.SelectColumnsOnRightClick = false;
            this.stashesListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.stashesListView.ShowSortIndicators = false;
            this.stashesListView.Size = new System.Drawing.Size(624, 403);
            this.stashesListView.SortGroupItemsByPrimaryColumn = false;
            this.stashesListView.TabIndex = 0;
            this.stashesListView.UseCompatibleStateImageBehavior = false;
            this.stashesListView.View = System.Windows.Forms.View.Details;
            // 
            // showExpansionComboBox
            // 
            this.showExpansionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showExpansionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.showExpansionComboBox.DropDownWidth = 200;
            this.showExpansionComboBox.FormattingEnabled = true;
            this.showExpansionComboBox.Location = new System.Drawing.Point(42, 3);
            this.showExpansionComboBox.Name = "showExpansionComboBox";
            this.showExpansionComboBox.Size = new System.Drawing.Size(173, 21);
            this.showExpansionComboBox.TabIndex = 22;
            // 
            // showSoftCoreComboBox
            // 
            this.showSoftCoreComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showSoftCoreComboBox.AutoSize = true;
            this.showSoftCoreComboBox.Location = new System.Drawing.Point(223, 6);
            this.showSoftCoreComboBox.Name = "showSoftCoreComboBox";
            this.showSoftCoreComboBox.Size = new System.Drawing.Size(15, 14);
            this.showSoftCoreComboBox.TabIndex = 23;
            this.showSoftCoreComboBox.UseVisualStyleBackColor = true;
            // 
            // showHardCoreComboBox
            // 
            this.showHardCoreComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showHardCoreComboBox.AutoSize = true;
            this.showHardCoreComboBox.Location = new System.Drawing.Point(253, 6);
            this.showHardCoreComboBox.Name = "showHardCoreComboBox";
            this.showHardCoreComboBox.Size = new System.Drawing.Size(15, 14);
            this.showHardCoreComboBox.TabIndex = 24;
            this.showHardCoreComboBox.UseVisualStyleBackColor = true;
            // 
            // selectAllButton
            // 
            this.selectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectAllButton.Location = new System.Drawing.Point(1, 437);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(125, 23);
            this.selectAllButton.TabIndex = 25;
            this.selectAllButton.Text = "Select all";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // unselectAllButton
            // 
            this.unselectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unselectAllButton.Location = new System.Drawing.Point(125, 437);
            this.unselectAllButton.Name = "unselectAllButton";
            this.unselectAllButton.Size = new System.Drawing.Size(125, 23);
            this.unselectAllButton.TabIndex = 26;
            this.unselectAllButton.Text = "Unselect all";
            this.unselectAllButton.UseVisualStyleBackColor = true;
            this.unselectAllButton.Click += new System.EventHandler(this.UnselectAllButton_Click);
            // 
            // displayMenuPanel
            // 
            this.displayMenuPanel.Controls.Add(this.showExpansionComboBox);
            this.displayMenuPanel.Controls.Add(this.showSoftCoreComboBox);
            this.displayMenuPanel.Controls.Add(this.showHardCoreComboBox);
            this.displayMenuPanel.Location = new System.Drawing.Point(354, -1);
            this.displayMenuPanel.Name = "displayMenuPanel";
            this.displayMenuPanel.Size = new System.Drawing.Size(275, 25);
            this.displayMenuPanel.TabIndex = 27;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Controls.Add(this.displayMenuPanel);
            this.Controls.Add(this.unselectAllButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.backgroundPanel);
            this.Controls.Add(this.topMenuStrip);
            this.Controls.Add(this.bottomMenuStrip);
            this.MainMenuStrip = this.topMenuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topMenuStrip.ResumeLayout(false);
            this.topMenuStrip.PerformLayout();
            this.bottomMenuStrip.ResumeLayout(false);
            this.bottomMenuStrip.PerformLayout();
            this.backgroundPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stashesListView)).EndInit();
            this.displayMenuPanel.ResumeLayout(false);
            this.displayMenuPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileButton;
        private System.Windows.Forms.ToolStripMenuItem settingsButton;
        private System.Windows.Forms.ToolStripMenuItem helpButton;
        private System.Windows.Forms.ToolStripMenuItem aboutButton;
        private System.Windows.Forms.MenuStrip bottomMenuStrip;
        private System.Windows.Forms.Panel backgroundPanel;
        private BrightIdeasSoftware.ObjectListView stashesListView;
        private System.Windows.Forms.ToolStripMenuItem importStashesButton;
        private System.Windows.Forms.ToolStripMenuItem createStashButton;
        private System.Windows.Forms.ComboBox showExpansionComboBox;
        private System.Windows.Forms.CheckBox showSoftCoreComboBox;
        private System.Windows.Forms.CheckBox showHardCoreComboBox;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button unselectAllButton;
        private System.Windows.Forms.Panel displayMenuPanel;
    }
}