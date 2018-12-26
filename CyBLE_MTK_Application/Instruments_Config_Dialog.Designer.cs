namespace CyBLE_MTK_Application
{
    partial class Instruments_Config_Dialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxCurrUnit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DmmAlias_textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownRelayDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.swAlias_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelayDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 43);
            this.button1.TabIndex = 15;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 265);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(483, 25);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(70, 20);
            this.toolStripStatusLabel1.Text = "Unknown";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 43);
            this.button2.TabIndex = 17;
            this.button2.Text = "Save&&Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxCurrUnit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DmmAlias_textBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 100);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DMM";
            // 
            // comboBoxCurrUnit
            // 
            this.comboBoxCurrUnit.FormattingEnabled = true;
            this.comboBoxCurrUnit.Items.AddRange(new object[] {
            "mA",
            "A"});
            this.comboBoxCurrUnit.Location = new System.Drawing.Point(160, 37);
            this.comboBoxCurrUnit.Name = "comboBoxCurrUnit";
            this.comboBoxCurrUnit.Size = new System.Drawing.Size(121, 24);
            this.comboBoxCurrUnit.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 17;
            this.label2.Text = "Current Unit";
            // 
            // DmmAlias_textBox
            // 
            this.DmmAlias_textBox.Location = new System.Drawing.Point(6, 37);
            this.DmmAlias_textBox.Name = "DmmAlias_textBox";
            this.DmmAlias_textBox.Size = new System.Drawing.Size(114, 22);
            this.DmmAlias_textBox.TabIndex = 16;
            this.DmmAlias_textBox.Text = "dmm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Alias";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownRelayDelay);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.swAlias_textBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 100);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SW";
            // 
            // numericUpDownRelayDelay
            // 
            this.numericUpDownRelayDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownRelayDelay.Location = new System.Drawing.Point(161, 39);
            this.numericUpDownRelayDelay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownRelayDelay.Name = "numericUpDownRelayDelay";
            this.numericUpDownRelayDelay.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownRelayDelay.TabIndex = 19;
            this.numericUpDownRelayDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Relay Delay (ms)";
            // 
            // swAlias_textBox
            // 
            this.swAlias_textBox.Location = new System.Drawing.Point(6, 39);
            this.swAlias_textBox.Name = "swAlias_textBox";
            this.swAlias_textBox.Size = new System.Drawing.Size(114, 22);
            this.swAlias_textBox.TabIndex = 16;
            this.swAlias_textBox.Text = "sw";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Alias";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(369, 122);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 35);
            this.button3.TabIndex = 20;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Instruments_Config_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 290);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Name = "Instruments_Config_Dialog";
            this.Text = "Instruments_Config_Dialog";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelayDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DmmAlias_textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox swAlias_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownRelayDelay;
        private System.Windows.Forms.ComboBox comboBoxCurrUnit;
        private System.Windows.Forms.Button button3;
    }
}