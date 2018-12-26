namespace CyBLE_MTK_Application
{
    partial class DmmSwitchSettingsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_CurrUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_dmm_alias = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_RelayDelay = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_sw_alias = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RelayDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_CurrUnit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_dmm_alias);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DMM";
            // 
            // comboBox_CurrUnit
            // 
            this.comboBox_CurrUnit.FormattingEnabled = true;
            this.comboBox_CurrUnit.Items.AddRange(new object[] {
            "A",
            "mA",
            "uA",
            "nA"});
            this.comboBox_CurrUnit.Location = new System.Drawing.Point(109, 66);
            this.comboBox_CurrUnit.Name = "comboBox_CurrUnit";
            this.comboBox_CurrUnit.Size = new System.Drawing.Size(114, 24);
            this.comboBox_CurrUnit.TabIndex = 3;
            this.comboBox_CurrUnit.Text = "mA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current Unit: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Alias: ";
            // 
            // textBox_dmm_alias
            // 
            this.textBox_dmm_alias.Location = new System.Drawing.Point(109, 19);
            this.textBox_dmm_alias.Name = "textBox_dmm_alias";
            this.textBox_dmm_alias.Size = new System.Drawing.Size(114, 22);
            this.textBox_dmm_alias.TabIndex = 0;
            this.textBox_dmm_alias.Text = "dmm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown_RelayDelay);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_sw_alias);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 114);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Switch";
            // 
            // numericUpDown_RelayDelay
            // 
            this.numericUpDown_RelayDelay.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown_RelayDelay.Location = new System.Drawing.Point(109, 70);
            this.numericUpDown_RelayDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_RelayDelay.Name = "numericUpDown_RelayDelay";
            this.numericUpDown_RelayDelay.Size = new System.Drawing.Size(114, 22);
            this.numericUpDown_RelayDelay.TabIndex = 5;
            this.numericUpDown_RelayDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Relay Delay: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Alias: ";
            // 
            // textBox_sw_alias
            // 
            this.textBox_sw_alias.Location = new System.Drawing.Point(109, 33);
            this.textBox_sw_alias.Name = "textBox_sw_alias";
            this.textBox_sw_alias.Size = new System.Drawing.Size(114, 22);
            this.textBox_sw_alias.TabIndex = 2;
            this.textBox_sw_alias.Text = "sw";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(277, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "C&onnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(277, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(277, 197);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "&OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DmmSwitchSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 265);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "DmmSwitchSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DmmSwitchSettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RelayDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_CurrUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_dmm_alias;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_sw_alias;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_RelayDelay;
    }
}