namespace CyBLE_MTK_Application
{
    partial class MTKTestProgramAllDialog
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EndRadioButton = new System.Windows.Forms.RadioButton();
            this.BegningRadioButton = new System.Windows.Forms.RadioButton();
            this.HexFilePathTextBox = new System.Windows.Forms.TextBox();
            this.HexGroupBox = new System.Windows.Forms.GroupBox();
            this.OpenHEXFileButton = new System.Windows.Forms.Button();
            this.ModuleCheckGroupBox = new System.Windows.Forms.GroupBox();
            this.BCCheckBox = new System.Windows.Forms.CheckBox();
            this.BCTextBox = new System.Windows.Forms.TextBox();
            this.DelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EventTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MACACheckBox = new System.Windows.Forms.CheckBox();
            this.PVCheckBox = new System.Windows.Forms.CheckBox();
            this.HWIDCheckBox = new System.Windows.Forms.CheckBox();
            this.BSVCheckBox = new System.Windows.Forms.CheckBox();
            this.AVCheckBox = new System.Windows.Forms.CheckBox();
            this.MACATtextBox = new System.Windows.Forms.TextBox();
            this.HWIDTextBox = new System.Windows.Forms.TextBox();
            this.PVTextBox = new System.Windows.Forms.TextBox();
            this.BLESVTextBox = new System.Windows.Forms.TextBox();
            this.AVTextBox = new System.Windows.Forms.TextBox();
            this.ModuleFWValidCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.HexGroupBox.SuspendLayout();
            this.ModuleCheckGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(172, 383);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 18;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(91, 383);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 17;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EndRadioButton);
            this.groupBox1.Controls.Add(this.BegningRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 45);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Program all devices at the";
            // 
            // EndRadioButton
            // 
            this.EndRadioButton.AutoSize = true;
            this.EndRadioButton.Location = new System.Drawing.Point(195, 19);
            this.EndRadioButton.Name = "EndRadioButton";
            this.EndRadioButton.Size = new System.Drawing.Size(112, 17);
            this.EndRadioButton.TabIndex = 1;
            this.EndRadioButton.TabStop = true;
            this.EndRadioButton.Text = "end of each batch";
            this.EndRadioButton.UseVisualStyleBackColor = true;
            this.EndRadioButton.CheckedChanged += new System.EventHandler(this.EndRadioButton_CheckedChanged);
            // 
            // BegningRadioButton
            // 
            this.BegningRadioButton.AutoSize = true;
            this.BegningRadioButton.Checked = true;
            this.BegningRadioButton.Location = new System.Drawing.Point(6, 19);
            this.BegningRadioButton.Name = "BegningRadioButton";
            this.BegningRadioButton.Size = new System.Drawing.Size(132, 17);
            this.BegningRadioButton.TabIndex = 0;
            this.BegningRadioButton.TabStop = true;
            this.BegningRadioButton.Text = "begning of each batch";
            this.BegningRadioButton.UseVisualStyleBackColor = true;
            this.BegningRadioButton.CheckedChanged += new System.EventHandler(this.BegningRadioButton_CheckedChanged);
            // 
            // HexFilePathTextBox
            // 
            this.HexFilePathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HexFilePathTextBox.Location = new System.Drawing.Point(6, 16);
            this.HexFilePathTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.HexFilePathTextBox.Name = "HexFilePathTextBox";
            this.HexFilePathTextBox.ReadOnly = true;
            this.HexFilePathTextBox.Size = new System.Drawing.Size(276, 13);
            this.HexFilePathTextBox.TabIndex = 0;
            this.HexFilePathTextBox.TabStop = false;
            // 
            // HexGroupBox
            // 
            this.HexGroupBox.Controls.Add(this.OpenHEXFileButton);
            this.HexGroupBox.Controls.Add(this.HexFilePathTextBox);
            this.HexGroupBox.Location = new System.Drawing.Point(12, 64);
            this.HexGroupBox.Name = "HexGroupBox";
            this.HexGroupBox.Size = new System.Drawing.Size(314, 40);
            this.HexGroupBox.TabIndex = 37;
            this.HexGroupBox.TabStop = false;
            this.HexGroupBox.Text = "HEX File Path";
            // 
            // OpenHEXFileButton
            // 
            this.OpenHEXFileButton.Location = new System.Drawing.Point(285, 11);
            this.OpenHEXFileButton.Margin = new System.Windows.Forms.Padding(0);
            this.OpenHEXFileButton.Name = "OpenHEXFileButton";
            this.OpenHEXFileButton.Size = new System.Drawing.Size(26, 23);
            this.OpenHEXFileButton.TabIndex = 2;
            this.OpenHEXFileButton.Text = "...";
            this.OpenHEXFileButton.UseVisualStyleBackColor = true;
            this.OpenHEXFileButton.Click += new System.EventHandler(this.OpenHEXFileButton_Click);
            // 
            // ModuleCheckGroupBox
            // 
            this.ModuleCheckGroupBox.Controls.Add(this.BCCheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.BCTextBox);
            this.ModuleCheckGroupBox.Controls.Add(this.DelayNumericUpDown);
            this.ModuleCheckGroupBox.Controls.Add(this.label3);
            this.ModuleCheckGroupBox.Controls.Add(this.label2);
            this.ModuleCheckGroupBox.Controls.Add(this.EventTypeComboBox);
            this.ModuleCheckGroupBox.Controls.Add(this.label1);
            this.ModuleCheckGroupBox.Controls.Add(this.MACACheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.PVCheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.HWIDCheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.BSVCheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.AVCheckBox);
            this.ModuleCheckGroupBox.Controls.Add(this.MACATtextBox);
            this.ModuleCheckGroupBox.Controls.Add(this.HWIDTextBox);
            this.ModuleCheckGroupBox.Controls.Add(this.PVTextBox);
            this.ModuleCheckGroupBox.Controls.Add(this.BLESVTextBox);
            this.ModuleCheckGroupBox.Controls.Add(this.AVTextBox);
            this.ModuleCheckGroupBox.Enabled = false;
            this.ModuleCheckGroupBox.Location = new System.Drawing.Point(12, 134);
            this.ModuleCheckGroupBox.Name = "ModuleCheckGroupBox";
            this.ModuleCheckGroupBox.Size = new System.Drawing.Size(314, 243);
            this.ModuleCheckGroupBox.TabIndex = 38;
            this.ModuleCheckGroupBox.TabStop = false;
            this.ModuleCheckGroupBox.Text = "Module Firmware Check Settings";
            // 
            // BCCheckBox
            // 
            this.BCCheckBox.AutoSize = true;
            this.BCCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BCCheckBox.Location = new System.Drawing.Point(57, 182);
            this.BCCheckBox.Name = "BCCheckBox";
            this.BCCheckBox.Size = new System.Drawing.Size(100, 17);
            this.BCCheckBox.TabIndex = 13;
            this.BCCheckBox.Text = "Boot Cause (C):";
            this.BCCheckBox.UseVisualStyleBackColor = true;
            this.BCCheckBox.CheckedChanged += new System.EventHandler(this.BCCheckBox_CheckedChanged);
            // 
            // BCTextBox
            // 
            this.BCTextBox.Enabled = false;
            this.BCTextBox.Location = new System.Drawing.Point(163, 180);
            this.BCTextBox.Name = "BCTextBox";
            this.BCTextBox.Size = new System.Drawing.Size(130, 20);
            this.BCTextBox.TabIndex = 14;
            // 
            // DelayNumericUpDown
            // 
            this.DelayNumericUpDown.Location = new System.Drawing.Point(132, 23);
            this.DelayNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DelayNumericUpDown.Name = "DelayNumericUpDown";
            this.DelayNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.DelayNumericUpDown.TabIndex = 3;
            this.DelayNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DelayNumericUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "ms before checks.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Capture UART lines for";
            // 
            // EventTypeComboBox
            // 
            this.EventTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EventTypeComboBox.FormattingEnabled = true;
            this.EventTypeComboBox.Items.AddRange(new object[] {
            "BOOT",
            "TFAC"});
            this.EventTypeComboBox.Location = new System.Drawing.Point(163, 49);
            this.EventTypeComboBox.Name = "EventTypeComboBox";
            this.EventTypeComboBox.Size = new System.Drawing.Size(130, 21);
            this.EventTypeComboBox.TabIndex = 4;
            this.EventTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.EventTypeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Event Type:";
            // 
            // MACACheckBox
            // 
            this.MACACheckBox.AutoSize = true;
            this.MACACheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MACACheckBox.Location = new System.Drawing.Point(48, 208);
            this.MACACheckBox.Name = "MACACheckBox";
            this.MACACheckBox.Size = new System.Drawing.Size(109, 17);
            this.MACACheckBox.TabIndex = 15;
            this.MACACheckBox.Text = "MAC Address (A):";
            this.MACACheckBox.UseVisualStyleBackColor = true;
            this.MACACheckBox.CheckedChanged += new System.EventHandler(this.MACACheckBox_CheckedChanged);
            // 
            // PVCheckBox
            // 
            this.PVCheckBox.AutoSize = true;
            this.PVCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PVCheckBox.Location = new System.Drawing.Point(35, 130);
            this.PVCheckBox.Name = "PVCheckBox";
            this.PVCheckBox.Size = new System.Drawing.Size(122, 17);
            this.PVCheckBox.TabIndex = 9;
            this.PVCheckBox.Text = "Protocol Version (P):";
            this.PVCheckBox.UseVisualStyleBackColor = true;
            this.PVCheckBox.CheckedChanged += new System.EventHandler(this.PVCheckBox_CheckedChanged);
            // 
            // HWIDCheckBox
            // 
            this.HWIDCheckBox.AutoSize = true;
            this.HWIDCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HWIDCheckBox.Location = new System.Drawing.Point(51, 156);
            this.HWIDCheckBox.Name = "HWIDCheckBox";
            this.HWIDCheckBox.Size = new System.Drawing.Size(106, 17);
            this.HWIDCheckBox.TabIndex = 11;
            this.HWIDCheckBox.Text = "Hardware ID (H):";
            this.HWIDCheckBox.UseVisualStyleBackColor = true;
            this.HWIDCheckBox.CheckedChanged += new System.EventHandler(this.HWIDCheckBox_CheckedChanged);
            // 
            // BSVCheckBox
            // 
            this.BSVCheckBox.AutoSize = true;
            this.BSVCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BSVCheckBox.Location = new System.Drawing.Point(23, 104);
            this.BSVCheckBox.Name = "BSVCheckBox";
            this.BSVCheckBox.Size = new System.Drawing.Size(134, 17);
            this.BSVCheckBox.TabIndex = 7;
            this.BSVCheckBox.Text = "BLE Stack Version (S):";
            this.BSVCheckBox.UseVisualStyleBackColor = true;
            this.BSVCheckBox.CheckedChanged += new System.EventHandler(this.BSVCheckBox_CheckedChanged);
            // 
            // AVCheckBox
            // 
            this.AVCheckBox.AutoSize = true;
            this.AVCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AVCheckBox.Location = new System.Drawing.Point(22, 78);
            this.AVCheckBox.Name = "AVCheckBox";
            this.AVCheckBox.Size = new System.Drawing.Size(135, 17);
            this.AVCheckBox.TabIndex = 5;
            this.AVCheckBox.Text = "Application Version (E):";
            this.AVCheckBox.UseVisualStyleBackColor = true;
            this.AVCheckBox.CheckedChanged += new System.EventHandler(this.AVCheckBox_CheckedChanged);
            // 
            // MACATtextBox
            // 
            this.MACATtextBox.Enabled = false;
            this.MACATtextBox.Location = new System.Drawing.Point(163, 206);
            this.MACATtextBox.Name = "MACATtextBox";
            this.MACATtextBox.Size = new System.Drawing.Size(130, 20);
            this.MACATtextBox.TabIndex = 16;
            // 
            // HWIDTextBox
            // 
            this.HWIDTextBox.Enabled = false;
            this.HWIDTextBox.Location = new System.Drawing.Point(163, 154);
            this.HWIDTextBox.Name = "HWIDTextBox";
            this.HWIDTextBox.Size = new System.Drawing.Size(130, 20);
            this.HWIDTextBox.TabIndex = 12;
            // 
            // PVTextBox
            // 
            this.PVTextBox.Enabled = false;
            this.PVTextBox.Location = new System.Drawing.Point(163, 128);
            this.PVTextBox.Name = "PVTextBox";
            this.PVTextBox.Size = new System.Drawing.Size(130, 20);
            this.PVTextBox.TabIndex = 10;
            // 
            // BLESVTextBox
            // 
            this.BLESVTextBox.Enabled = false;
            this.BLESVTextBox.Location = new System.Drawing.Point(163, 102);
            this.BLESVTextBox.Name = "BLESVTextBox";
            this.BLESVTextBox.Size = new System.Drawing.Size(130, 20);
            this.BLESVTextBox.TabIndex = 8;
            // 
            // AVTextBox
            // 
            this.AVTextBox.Enabled = false;
            this.AVTextBox.Location = new System.Drawing.Point(163, 76);
            this.AVTextBox.Name = "AVTextBox";
            this.AVTextBox.Size = new System.Drawing.Size(130, 20);
            this.AVTextBox.TabIndex = 6;
            // 
            // ModuleFWValidCheckBox
            // 
            this.ModuleFWValidCheckBox.AutoSize = true;
            this.ModuleFWValidCheckBox.Location = new System.Drawing.Point(13, 111);
            this.ModuleFWValidCheckBox.Name = "ModuleFWValidCheckBox";
            this.ModuleFWValidCheckBox.Size = new System.Drawing.Size(179, 17);
            this.ModuleFWValidCheckBox.TabIndex = 3;
            this.ModuleFWValidCheckBox.Text = "Enable module firmware checks.";
            this.ModuleFWValidCheckBox.UseVisualStyleBackColor = true;
            this.ModuleFWValidCheckBox.CheckedChanged += new System.EventHandler(this.ModuleCheckCheckBox_CheckedChanged);
            // 
            // MTKTestProgramAllDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 418);
            this.Controls.Add(this.ModuleFWValidCheckBox);
            this.Controls.Add(this.ModuleCheckGroupBox);
            this.Controls.Add(this.HexGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MTKTestProgramAllDialog";
            this.Text = "Program All Devices";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.HexGroupBox.ResumeLayout(false);
            this.HexGroupBox.PerformLayout();
            this.ModuleCheckGroupBox.ResumeLayout(false);
            this.ModuleCheckGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton BegningRadioButton;
        private System.Windows.Forms.RadioButton EndRadioButton;
        private System.Windows.Forms.TextBox HexFilePathTextBox;
        private System.Windows.Forms.GroupBox HexGroupBox;
        private System.Windows.Forms.Button OpenHEXFileButton;
        private System.Windows.Forms.GroupBox ModuleCheckGroupBox;
        private System.Windows.Forms.CheckBox ModuleFWValidCheckBox;
        private System.Windows.Forms.TextBox HWIDTextBox;
        private System.Windows.Forms.TextBox PVTextBox;
        private System.Windows.Forms.TextBox BLESVTextBox;
        private System.Windows.Forms.TextBox AVTextBox;
        private System.Windows.Forms.TextBox MACATtextBox;
        private System.Windows.Forms.CheckBox MACACheckBox;
        private System.Windows.Forms.CheckBox PVCheckBox;
        private System.Windows.Forms.CheckBox HWIDCheckBox;
        private System.Windows.Forms.CheckBox BSVCheckBox;
        private System.Windows.Forms.CheckBox AVCheckBox;
        private System.Windows.Forms.ComboBox EventTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown DelayNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox BCCheckBox;
        private System.Windows.Forms.TextBox BCTextBox;
    }
}