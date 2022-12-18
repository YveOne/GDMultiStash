namespace GDMultiStash.Forms
{
    partial class ChangelogDialogForm
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
            this.versionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.changelogTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // versionSelectComboBox
            // 
            this.versionSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionSelectComboBox.FormattingEnabled = true;
            this.versionSelectComboBox.Location = new System.Drawing.Point(12, 12);
            this.versionSelectComboBox.Name = "versionSelectComboBox";
            this.versionSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.versionSelectComboBox.TabIndex = 0;
            // 
            // changelogTextBox
            // 
            this.changelogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.changelogTextBox.Location = new System.Drawing.Point(12, 39);
            this.changelogTextBox.Multiline = true;
            this.changelogTextBox.Name = "changelogTextBox";
            this.changelogTextBox.ReadOnly = true;
            this.changelogTextBox.Size = new System.Drawing.Size(776, 399);
            this.changelogTextBox.TabIndex = 1;
            // 
            // ChangelogDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.changelogTextBox);
            this.Controls.Add(this.versionSelectComboBox);
            this.Name = "ChangelogDialogForm";
            this.Text = "Changelog";
            this.Load += new System.EventHandler(this.ChangelogDialogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox versionSelectComboBox;
        private System.Windows.Forms.RichTextBox changelogTextBox;
    }
}