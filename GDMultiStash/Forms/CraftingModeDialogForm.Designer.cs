namespace GDMultiStash.Forms
{
    partial class CraftingModeDialogForm
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
            this.startButton = new System.Windows.Forms.Button();
            this.transferFileComboBox = new System.Windows.Forms.ComboBox();
            this.autoFillCheckBox = new System.Windows.Forms.CheckBox();
            this.autoFillComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Location = new System.Drawing.Point(203, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 65);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // transferFileComboBox
            // 
            this.transferFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transferFileComboBox.FormattingEnabled = true;
            this.transferFileComboBox.Location = new System.Drawing.Point(12, 12);
            this.transferFileComboBox.Name = "transferFileComboBox";
            this.transferFileComboBox.Size = new System.Drawing.Size(185, 21);
            this.transferFileComboBox.TabIndex = 1;
            // 
            // autoFillCheckBox
            // 
            this.autoFillCheckBox.AutoSize = true;
            this.autoFillCheckBox.Location = new System.Drawing.Point(12, 39);
            this.autoFillCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.autoFillCheckBox.Name = "autoFillCheckBox";
            this.autoFillCheckBox.Size = new System.Drawing.Size(63, 17);
            this.autoFillCheckBox.TabIndex = 2;
            this.autoFillCheckBox.Text = "Auto Fill";
            this.autoFillCheckBox.UseVisualStyleBackColor = true;
            this.autoFillCheckBox.CheckedChanged += new System.EventHandler(this.AutoFillCheckBox_CheckedChanged);
            // 
            // autoFillComboBox
            // 
            this.autoFillComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoFillComboBox.FormattingEnabled = true;
            this.autoFillComboBox.Location = new System.Drawing.Point(12, 56);
            this.autoFillComboBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.autoFillComboBox.Name = "autoFillComboBox";
            this.autoFillComboBox.Size = new System.Drawing.Size(185, 21);
            this.autoFillComboBox.TabIndex = 3;
            // 
            // CraftingModeDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 89);
            this.Controls.Add(this.autoFillComboBox);
            this.Controls.Add(this.autoFillCheckBox);
            this.Controls.Add(this.transferFileComboBox);
            this.Controls.Add(this.startButton);
            this.Name = "CraftingModeDialogForm";
            this.Text = "CraftingModeDialogForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox transferFileComboBox;
        private System.Windows.Forms.CheckBox autoFillCheckBox;
        private System.Windows.Forms.ComboBox autoFillComboBox;
    }
}