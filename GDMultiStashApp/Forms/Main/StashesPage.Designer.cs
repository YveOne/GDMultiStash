namespace GDMultiStash.Forms.Main
{
    partial class StashesPage
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
            this.shownItemsCountLabel = new System.Windows.Forms.Label();
            this.menuFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.showExpansionComboBox = new GDMultiStash.Forms.Controls.FlatComboBox();
            this.showSoftCoreCheckbox = new GDMultiStash.Forms.Controls.MyCheckBox();
            this.showHardCoreCheckbox = new GDMultiStash.Forms.Controls.MyCheckBox();
            this.listViewBorderPanel = new System.Windows.Forms.Panel();
            this.stashesListView = new GDMultiStash.Forms.Controls.OLVGroupFeatures();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.createStashButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFlowLayoutPanel.SuspendLayout();
            this.listViewBorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stashesListView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // shownItemsCountLabel
            // 
            this.shownItemsCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shownItemsCountLabel.Location = new System.Drawing.Point(10, 418);
            this.shownItemsCountLabel.Name = "shownItemsCountLabel";
            this.shownItemsCountLabel.Size = new System.Drawing.Size(545, 27);
            this.shownItemsCountLabel.TabIndex = 26;
            this.shownItemsCountLabel.Text = "x/y stashes";
            this.shownItemsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuFlowLayoutPanel
            // 
            this.menuFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.menuFlowLayoutPanel.Controls.Add(this.showExpansionComboBox);
            this.menuFlowLayoutPanel.Controls.Add(this.showSoftCoreCheckbox);
            this.menuFlowLayoutPanel.Controls.Add(this.showHardCoreCheckbox);
            this.menuFlowLayoutPanel.Location = new System.Drawing.Point(10, 450);
            this.menuFlowLayoutPanel.Name = "menuFlowLayoutPanel";
            this.menuFlowLayoutPanel.Size = new System.Drawing.Size(364, 25);
            this.menuFlowLayoutPanel.TabIndex = 25;
            // 
            // showExpansionComboBox
            // 
            this.showExpansionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.showExpansionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.showExpansionComboBox.DropDownWidth = 200;
            this.showExpansionComboBox.FormattingEnabled = true;
            this.showExpansionComboBox.Location = new System.Drawing.Point(0, 0);
            this.showExpansionComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.showExpansionComboBox.Name = "showExpansionComboBox";
            this.showExpansionComboBox.Size = new System.Drawing.Size(212, 21);
            this.showExpansionComboBox.TabIndex = 22;
            // 
            // showSoftCoreCheckbox
            // 
            this.showSoftCoreCheckbox.AutoSize = true;
            this.showSoftCoreCheckbox.Location = new System.Drawing.Point(228, 3);
            this.showSoftCoreCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.showSoftCoreCheckbox.Name = "showSoftCoreCheckbox";
            this.showSoftCoreCheckbox.Size = new System.Drawing.Size(15, 14);
            this.showSoftCoreCheckbox.TabIndex = 23;
            this.showSoftCoreCheckbox.ThreeState = true;
            this.showSoftCoreCheckbox.UseVisualStyleBackColor = true;
            // 
            // showHardCoreCheckbox
            // 
            this.showHardCoreCheckbox.AutoSize = true;
            this.showHardCoreCheckbox.Location = new System.Drawing.Point(251, 3);
            this.showHardCoreCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 4);
            this.showHardCoreCheckbox.Name = "showHardCoreCheckbox";
            this.showHardCoreCheckbox.Size = new System.Drawing.Size(15, 14);
            this.showHardCoreCheckbox.TabIndex = 24;
            this.showHardCoreCheckbox.ThreeState = true;
            this.showHardCoreCheckbox.UseVisualStyleBackColor = true;
            // 
            // listViewBorderPanel
            // 
            this.listViewBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.listViewBorderPanel.Controls.Add(this.stashesListView);
            this.listViewBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.listViewBorderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.listViewBorderPanel.Name = "listViewBorderPanel";
            this.listViewBorderPanel.Padding = new System.Windows.Forms.Padding(10);
            this.listViewBorderPanel.Size = new System.Drawing.Size(565, 418);
            this.listViewBorderPanel.TabIndex = 1;
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
            this.stashesListView.GroupHeadingBackColor = System.Drawing.Color.Gray;
            this.stashesListView.GroupHeadingCountFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashesListView.GroupHeadingCountForeColor = System.Drawing.Color.Black;
            this.stashesListView.GroupHeadingFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stashesListView.GroupHeadingForeColor = System.Drawing.Color.Black;
            this.stashesListView.HideSelection = false;
            this.stashesListView.Location = new System.Drawing.Point(10, 10);
            this.stashesListView.Margin = new System.Windows.Forms.Padding(20);
            this.stashesListView.Name = "stashesListView";
            this.stashesListView.SelectColumnsOnRightClick = false;
            this.stashesListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.stashesListView.SeparatorColor = System.Drawing.Color.Empty;
            this.stashesListView.ShowSortIndicators = false;
            this.stashesListView.Size = new System.Drawing.Size(545, 398);
            this.stashesListView.SortGroupItemsByPrimaryColumn = false;
            this.stashesListView.TabIndex = 0;
            this.stashesListView.UseCompatibleStateImageBehavior = false;
            this.stashesListView.View = System.Windows.Forms.View.Details;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStashButton});
            this.menuStrip.Location = new System.Drawing.Point(0, 450);
            this.menuStrip.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip.Size = new System.Drawing.Size(565, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "bottomMenuStrip";
            // 
            // createStashButton
            // 
            this.createStashButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createStashButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.createStashButton.Name = "createStashButton";
            this.createStashButton.Size = new System.Drawing.Size(103, 24);
            this.createStashButton.Text = "Create Stash";
            this.createStashButton.Click += new System.EventHandler(this.CreateStashButton_Click);
            // 
            // StashesPage
            // 
            this.ClientSize = new System.Drawing.Size(565, 478);
            this.Controls.Add(this.listViewBorderPanel);
            this.Controls.Add(this.shownItemsCountLabel);
            this.Controls.Add(this.menuFlowLayoutPanel);
            this.Controls.Add(this.menuStrip);
            this.Name = "StashesPage";
            this.Text = "StashesPage";
            this.menuFlowLayoutPanel.ResumeLayout(false);
            this.menuFlowLayoutPanel.PerformLayout();
            this.listViewBorderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stashesListView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label shownItemsCountLabel;
        private System.Windows.Forms.FlowLayoutPanel menuFlowLayoutPanel;
        private GDMultiStash.Forms.Controls.FlatComboBox showExpansionComboBox;
        private GDMultiStash.Forms.Controls.MyCheckBox showSoftCoreCheckbox;
        private GDMultiStash.Forms.Controls.MyCheckBox showHardCoreCheckbox;
        private System.Windows.Forms.Panel listViewBorderPanel;
        private GDMultiStash.Forms.Controls.OLVGroupFeatures stashesListView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem createStashButton;





    }
}