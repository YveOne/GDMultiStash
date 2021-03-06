namespace GDMultiStash.Forms
{
    partial class ImportDialogForm
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
            this.stashFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.overwriteComboBox = new System.Windows.Forms.ComboBox();
            this.overwriteCheckBox = new System.Windows.Forms.CheckBox();
            this.scCheckBox = new System.Windows.Forms.CheckBox();
            this.hcCheckBox = new System.Windows.Forms.CheckBox();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.expansionTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // stashFileTextBox
            // 
            this.stashFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stashFileTextBox.Enabled = false;
            this.stashFileTextBox.Location = new System.Drawing.Point(12, 25);
            this.stashFileTextBox.Name = "stashFileTextBox";
            this.stashFileTextBox.Size = new System.Drawing.Size(560, 20);
            this.stashFileTextBox.TabIndex = 4;
            this.stashFileTextBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Transfer file:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(195, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(304, 52);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(190, 20);
            this.nameTextBox.TabIndex = 11;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(500, 51);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(72, 49);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "Import";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(198, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Overwrite:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // overwriteComboBox
            // 
            this.overwriteComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.overwriteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.overwriteComboBox.FormattingEnabled = true;
            this.overwriteComboBox.Location = new System.Drawing.Point(325, 78);
            this.overwriteComboBox.Name = "overwriteComboBox";
            this.overwriteComboBox.Size = new System.Drawing.Size(169, 21);
            this.overwriteComboBox.TabIndex = 14;
            // 
            // overwriteCheckBox
            // 
            this.overwriteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.overwriteCheckBox.AutoSize = true;
            this.overwriteCheckBox.Location = new System.Drawing.Point(304, 81);
            this.overwriteCheckBox.Name = "overwriteCheckBox";
            this.overwriteCheckBox.Size = new System.Drawing.Size(15, 14);
            this.overwriteCheckBox.TabIndex = 15;
            this.overwriteCheckBox.UseVisualStyleBackColor = true;
            this.overwriteCheckBox.CheckedChanged += new System.EventHandler(this.overwriteCheckBox_CheckedChanged);
            // 
            // scCheckBox
            // 
            this.scCheckBox.AutoSize = true;
            this.scCheckBox.Location = new System.Drawing.Point(12, 91);
            this.scCheckBox.Name = "scCheckBox";
            this.scCheckBox.Size = new System.Drawing.Size(40, 17);
            this.scCheckBox.TabIndex = 16;
            this.scCheckBox.Text = "SC";
            this.scCheckBox.UseVisualStyleBackColor = true;
            // 
            // hcCheckBox
            // 
            this.hcCheckBox.AutoSize = true;
            this.hcCheckBox.Location = new System.Drawing.Point(58, 91);
            this.hcCheckBox.Name = "hcCheckBox";
            this.hcCheckBox.Size = new System.Drawing.Size(41, 17);
            this.hcCheckBox.TabIndex = 17;
            this.hcCheckBox.Text = "HC";
            this.hcCheckBox.UseVisualStyleBackColor = true;
            // 
            // expansionLabel
            // 
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.expansionLabel.Location = new System.Drawing.Point(12, 48);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(59, 13);
            this.expansionLabel.TabIndex = 18;
            this.expansionLabel.Text = "Expansion:";
            // 
            // expansionTextBox
            // 
            this.expansionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.expansionTextBox.Enabled = false;
            this.expansionTextBox.Location = new System.Drawing.Point(12, 64);
            this.expansionTextBox.Name = "expansionTextBox";
            this.expansionTextBox.ReadOnly = true;
            this.expansionTextBox.Size = new System.Drawing.Size(180, 20);
            this.expansionTextBox.TabIndex = 19;
            this.expansionTextBox.TabStop = false;
            // 
            // ImportDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 117);
            this.Controls.Add(this.expansionTextBox);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.hcCheckBox);
            this.Controls.Add(this.scCheckBox);
            this.Controls.Add(this.overwriteCheckBox);
            this.Controls.Add(this.overwriteComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stashFileTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImportDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GDStashOverlay :: Import Stash";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ImportForm_Load);
            this.Shown += new System.EventHandler(this.ImportForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox stashFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox overwriteComboBox;
        private System.Windows.Forms.CheckBox overwriteCheckBox;
        private System.Windows.Forms.CheckBox scCheckBox;
        private System.Windows.Forms.CheckBox hcCheckBox;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.TextBox expansionTextBox;
    }
}