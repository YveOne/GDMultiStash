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
            this.transferFileLabel = new System.Windows.Forms.Label();
            this.stashNameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.scCheckBox = new System.Windows.Forms.CheckBox();
            this.hcCheckBox = new System.Windows.Forms.CheckBox();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.expansionTextBox = new System.Windows.Forms.TextBox();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.groupLabel = new System.Windows.Forms.Label();
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
            // transferFileLabel
            // 
            this.transferFileLabel.AutoSize = true;
            this.transferFileLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.transferFileLabel.Location = new System.Drawing.Point(12, 9);
            this.transferFileLabel.Name = "transferFileLabel";
            this.transferFileLabel.Size = new System.Drawing.Size(65, 13);
            this.transferFileLabel.TabIndex = 3;
            this.transferFileLabel.Text = "Transfer file:";
            // 
            // stashNameLabel
            // 
            this.stashNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stashNameLabel.Location = new System.Drawing.Point(198, 55);
            this.stashNameLabel.Name = "stashNameLabel";
            this.stashNameLabel.Size = new System.Drawing.Size(66, 13);
            this.stashNameLabel.TabIndex = 10;
            this.stashNameLabel.Text = "Name:";
            this.stashNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(270, 52);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(224, 20);
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
            // groupComboBox
            // 
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.Location = new System.Drawing.Point(270, 79);
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(224, 21);
            this.groupComboBox.TabIndex = 27;
            // 
            // groupLabel
            // 
            this.groupLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabel.Location = new System.Drawing.Point(201, 82);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(63, 13);
            this.groupLabel.TabIndex = 26;
            this.groupLabel.Text = "Group:";
            this.groupLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ImportDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 149);
            this.Controls.Add(this.groupComboBox);
            this.Controls.Add(this.groupLabel);
            this.Controls.Add(this.expansionTextBox);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.hcCheckBox);
            this.Controls.Add(this.scCheckBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.stashNameLabel);
            this.Controls.Add(this.stashFileTextBox);
            this.Controls.Add(this.transferFileLabel);
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
        private System.Windows.Forms.Label transferFileLabel;
        private System.Windows.Forms.Label stashNameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox scCheckBox;
        private System.Windows.Forms.CheckBox hcCheckBox;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.TextBox expansionTextBox;
        private System.Windows.Forms.ComboBox groupComboBox;
        private System.Windows.Forms.Label groupLabel;
    }
}