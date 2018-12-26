namespace CyBLE_MTK_Application
{
    partial class Switch_Config_Dialog
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SWAlias_textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 120);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(302, 25);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(70, 20);
            this.toolStripStatusLabel1.Text = "Unknown";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 43);
            this.button1.TabIndex = 19;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SWAlias_textBox
            // 
            this.SWAlias_textBox.Location = new System.Drawing.Point(27, 36);
            this.SWAlias_textBox.Name = "SWAlias_textBox";
            this.SWAlias_textBox.Size = new System.Drawing.Size(114, 22);
            this.SWAlias_textBox.TabIndex = 18;
            this.SWAlias_textBox.Text = "sw";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "SW Alias";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 43);
            this.button2.TabIndex = 21;
            this.button2.Text = "Save&&Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Switch_Config_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 145);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SWAlias_textBox);
            this.Controls.Add(this.label6);
            this.Name = "Switch_Config_Dialog";
            this.Text = "Switch_Config_Dialog";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SWAlias_textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
    }
}