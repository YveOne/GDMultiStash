namespace GDMultiStash.Forms
{
    partial class CreateStashDialogForm
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.expansionComboBox = new System.Windows.Forms.ComboBox();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.hcCheckBox = new System.Windows.Forms.CheckBox();
            this.scCheckBox = new System.Windows.Forms.CheckBox();
            this.groupLabel = new System.Windows.Forms.Label();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(12, 25);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(224, 20);
            this.nameTextBox.TabIndex = 11;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(242, 24);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(79, 21);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // expansionComboBox
            // 
            this.expansionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expansionComboBox.FormattingEnabled = true;
            this.expansionComboBox.Location = new System.Drawing.Point(12, 104);
            this.expansionComboBox.Name = "expansionComboBox";
            this.expansionComboBox.Size = new System.Drawing.Size(224, 21);
            this.expansionComboBox.TabIndex = 21;
            // 
            // expansionLabel
            // 
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Location = new System.Drawing.Point(12, 88);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(59, 13);
            this.expansionLabel.TabIndex = 20;
            this.expansionLabel.Text = "Expansion:";
            // 
            // hcCheckBox
            // 
            this.hcCheckBox.AutoSize = true;
            this.hcCheckBox.Location = new System.Drawing.Point(61, 131);
            this.hcCheckBox.Name = "hcCheckBox";
            this.hcCheckBox.Size = new System.Drawing.Size(41, 17);
            this.hcCheckBox.TabIndex = 23;
            this.hcCheckBox.Text = "HC";
            this.hcCheckBox.UseVisualStyleBackColor = true;
            // 
            // scCheckBox
            // 
            this.scCheckBox.AutoSize = true;
            this.scCheckBox.Location = new System.Drawing.Point(15, 131);
            this.scCheckBox.Name = "scCheckBox";
            this.scCheckBox.Size = new System.Drawing.Size(40, 17);
            this.scCheckBox.TabIndex = 22;
            this.scCheckBox.Text = "SC";
            this.scCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(12, 48);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(39, 13);
            this.groupLabel.TabIndex = 24;
            this.groupLabel.Text = "Group:";
            // 
            // groupComboBox
            // 
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.Location = new System.Drawing.Point(12, 64);
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(224, 21);
            this.groupComboBox.TabIndex = 25;
            // 
            // CreateStashDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 157);
            this.Controls.Add(this.groupComboBox);
            this.Controls.Add(this.groupLabel);
            this.Controls.Add(this.hcCheckBox);
            this.Controls.Add(this.scCheckBox);
            this.Controls.Add(this.expansionComboBox);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CreateStashDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TITLE";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.AddStashDialogForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox expansionComboBox;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.CheckBox hcCheckBox;
        private System.Windows.Forms.CheckBox scCheckBox;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.ComboBox groupComboBox;
    }
}