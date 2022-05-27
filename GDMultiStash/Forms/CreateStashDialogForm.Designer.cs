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
            this.label3 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.expansionComboBox = new System.Windows.Forms.ComboBox();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Name:";
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
            this.okButton.Size = new System.Drawing.Size(79, 61);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "Add";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // expansionComboBox
            // 
            this.expansionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expansionComboBox.FormattingEnabled = true;
            this.expansionComboBox.Location = new System.Drawing.Point(12, 64);
            this.expansionComboBox.Name = "expansionComboBox";
            this.expansionComboBox.Size = new System.Drawing.Size(224, 21);
            this.expansionComboBox.TabIndex = 21;
            // 
            // expansionLabel
            // 
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Location = new System.Drawing.Point(12, 48);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(59, 13);
            this.expansionLabel.TabIndex = 20;
            this.expansionLabel.Text = "Expansion:";
            // 
            // AddStashDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 98);
            this.Controls.Add(this.expansionComboBox);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddStashDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GDStashOverlay :: Create Stash";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddStashDialogForm_Load);
            this.Shown += new System.EventHandler(this.AddStashDialogForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox expansionComboBox;
        private System.Windows.Forms.Label expansionLabel;
    }
}