namespace CyBLE_MTK_Application
{
    partial class CyBLEMTKSettingsImpExp
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
            this.WhatToDoLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PropertiesDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // WhatToDoLabel
            // 
            this.WhatToDoLabel.AutoSize = true;
            this.WhatToDoLabel.Location = new System.Drawing.Point(12, 14);
            this.WhatToDoLabel.Name = "WhatToDoLabel";
            this.WhatToDoLabel.Size = new System.Drawing.Size(223, 13);
            this.WhatToDoLabel.TabIndex = 0;
            this.WhatToDoLabel.Text = "Select settings that you want to import/export.";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(454, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Default";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(373, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Select &None";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(292, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Select &All";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(454, 268);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 37;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(373, 268);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 36;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(11, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 2);
            this.label1.TabIndex = 38;
            // 
            // PropertiesDataGridView
            // 
            this.PropertiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PropertiesDataGridView.Location = new System.Drawing.Point(12, 38);
            this.PropertiesDataGridView.Name = "PropertiesDataGridView";
            this.PropertiesDataGridView.Size = new System.Drawing.Size(516, 220);
            this.PropertiesDataGridView.TabIndex = 39;
            // 
            // CyBLEMTKSettingsImpExp
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(541, 303);
            this.Controls.Add(this.PropertiesDataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.WhatToDoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CyBLEMTKSettingsImpExp";
            this.Text = "Settings Import/Export";
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WhatToDoLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView PropertiesDataGridView;
    }
}