namespace CyBLE_MTK_Application
{
    partial class ManufacturingModeSettings
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DUTSerialNumberDuplicationCheck = new System.Windows.Forms.CheckBox();
            this.EnableComPortsPredefineCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(309, 225);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(390, 225);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EnableComPortsPredefineCheck);
            this.groupBox1.Controls.Add(this.DUTSerialNumberDuplicationCheck);
            this.groupBox1.Controls.Add(this.button_ok);
            this.groupBox1.Controls.Add(this.button_cancel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 260);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // DUTSerialNumberDuplicationCheck
            // 
            this.DUTSerialNumberDuplicationCheck.AutoSize = true;
            this.DUTSerialNumberDuplicationCheck.Enabled = false;
            this.DUTSerialNumberDuplicationCheck.Location = new System.Drawing.Point(12, 26);
            this.DUTSerialNumberDuplicationCheck.Name = "DUTSerialNumberDuplicationCheck";
            this.DUTSerialNumberDuplicationCheck.Size = new System.Drawing.Size(196, 17);
            this.DUTSerialNumberDuplicationCheck.TabIndex = 4;
            this.DUTSerialNumberDuplicationCheck.Text = "DUTSerialNumberDuplicationCheck";
            this.DUTSerialNumberDuplicationCheck.UseVisualStyleBackColor = true;
            // 
            // EnableComPortsPredefineCheck
            // 
            this.EnableComPortsPredefineCheck.AutoSize = true;
            this.EnableComPortsPredefineCheck.Enabled = false;
            this.EnableComPortsPredefineCheck.Location = new System.Drawing.Point(12, 59);
            this.EnableComPortsPredefineCheck.Name = "EnableComPortsPredefineCheck";
            this.EnableComPortsPredefineCheck.Size = new System.Drawing.Size(180, 17);
            this.EnableComPortsPredefineCheck.TabIndex = 5;
            this.EnableComPortsPredefineCheck.Text = "EnableComPortsPredefineCheck";
            this.EnableComPortsPredefineCheck.UseVisualStyleBackColor = true;
            // 
            // ManufacturingModeSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 260);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "ManufacturingModeSettings";
            this.Text = "ManufacturingModeSettings";
            this.Load += new System.EventHandler(this.ManufacturingModeSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox EnableComPortsPredefineCheck;
        private System.Windows.Forms.CheckBox DUTSerialNumberDuplicationCheck;
    }
}