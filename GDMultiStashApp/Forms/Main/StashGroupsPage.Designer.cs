namespace GDMultiStash.Forms.Main
{
    partial class StashGroupsPage
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.createStashGroupButton = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewBorderPanel = new System.Windows.Forms.Panel();
            this.groupsListView = new GDMultiStash.Forms.Controls.OLVCatchScrolling();
            this.menuStrip.SuspendLayout();
            this.listViewBorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsListView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStashGroupButton});
            this.menuStrip.Location = new System.Drawing.Point(0, 450);
            this.menuStrip.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip.Size = new System.Drawing.Size(565, 28);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "bottomMenuStrip";
            // 
            // createStashGroupButton
            // 
            this.createStashGroupButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createStashGroupButton.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.createStashGroupButton.Name = "createStashGroupButton";
            this.createStashGroupButton.Size = new System.Drawing.Size(109, 24);
            this.createStashGroupButton.Text = "Create Group";
            this.createStashGroupButton.Click += new System.EventHandler(this.CreateStashGroupButton_Click);
            // 
            // listViewBorderPanel
            // 
            this.listViewBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBorderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.listViewBorderPanel.Controls.Add(this.groupsListView);
            this.listViewBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.listViewBorderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.listViewBorderPanel.Name = "listViewBorderPanel";
            this.listViewBorderPanel.Padding = new System.Windows.Forms.Padding(10);
            this.listViewBorderPanel.Size = new System.Drawing.Size(565, 418);
            this.listViewBorderPanel.TabIndex = 0;
            // 
            // groupsListView
            // 
            this.groupsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.groupsListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.groupsListView.CellEditUseWholeCell = false;
            this.groupsListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupsListView.FullRowSelect = true;
            this.groupsListView.GridLines = true;
            this.groupsListView.HideSelection = false;
            this.groupsListView.Location = new System.Drawing.Point(10, 10);
            this.groupsListView.Margin = new System.Windows.Forms.Padding(20);
            this.groupsListView.Name = "groupsListView";
            this.groupsListView.SelectColumnsOnRightClick = false;
            this.groupsListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.groupsListView.ShowGroups = false;
            this.groupsListView.ShowSortIndicators = false;
            this.groupsListView.Size = new System.Drawing.Size(545, 398);
            this.groupsListView.SortGroupItemsByPrimaryColumn = false;
            this.groupsListView.TabIndex = 1;
            this.groupsListView.UseCompatibleStateImageBehavior = false;
            this.groupsListView.View = System.Windows.Forms.View.Details;
            // 
            // StashGroupsPage
            // 
            this.ClientSize = new System.Drawing.Size(565, 478);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.listViewBorderPanel);
            this.Name = "StashGroupsPage";
            this.Text = "StashGroupsPage";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.listViewBorderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupsListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem createStashGroupButton;
        private System.Windows.Forms.Panel listViewBorderPanel;
        private Controls.OLVCatchScrolling groupsListView;
    }
}