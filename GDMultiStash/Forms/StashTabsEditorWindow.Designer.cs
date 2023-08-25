namespace GDMultiStash.Forms
{
    partial class StashTabsEditorWindow
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
            this.tabsListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.backgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabsListPanel
            // 
            this.tabsListPanel.Location = new System.Drawing.Point(10, 10);
            this.tabsListPanel.Margin = new System.Windows.Forms.Padding(10);
            this.tabsListPanel.Name = "tabsListPanel";
            this.tabsListPanel.Size = new System.Drawing.Size(0, 0);
            this.tabsListPanel.TabIndex = 0;
            this.tabsListPanel.WrapContents = false;
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.Controls.Add(this.tabsListPanel);
            this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundPanel.Location = new System.Drawing.Point(1, 1);
            this.backgroundPanel.Margin = new System.Windows.Forms.Padding(1);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Padding = new System.Windows.Forms.Padding(10);
            this.backgroundPanel.Size = new System.Drawing.Size(382, 150);
            this.backgroundPanel.TabIndex = 1;
            // 
            // StashTabsEditorWindow
            // 
            this.ClientSize = new System.Drawing.Size(384, 152);
            this.Controls.Add(this.backgroundPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 100);
            this.Name = "StashTabsEditorWindow";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel tabsListPanel;
        private System.Windows.Forms.Panel backgroundPanel;
    }
}