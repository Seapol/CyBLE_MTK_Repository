namespace CyBLE_MTK_Application
{
    partial class CyBLEMTKSplashScreen
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
            this.LoadProgressBar = new System.Windows.Forms.ProgressBar();
            this.LoadMsgLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadProgressBar
            // 
            this.LoadProgressBar.Location = new System.Drawing.Point(0, 200);
            this.LoadProgressBar.MarqueeAnimationSpeed = 10;
            this.LoadProgressBar.Name = "LoadProgressBar";
            this.LoadProgressBar.Size = new System.Drawing.Size(440, 13);
            this.LoadProgressBar.TabIndex = 0;
            // 
            // LoadMsgLabel
            // 
            this.LoadMsgLabel.Location = new System.Drawing.Point(0, 216);
            this.LoadMsgLabel.Name = "LoadMsgLabel";
            this.LoadMsgLabel.Size = new System.Drawing.Size(440, 23);
            this.LoadMsgLabel.TabIndex = 1;
            this.LoadMsgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.VersionLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.ForeColor = System.Drawing.Color.White;
            this.VersionLabel.Location = new System.Drawing.Point(274, 123);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 14);
            this.VersionLabel.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CyBLE_MTK_Application.Properties.Resources.SplashScreenBanner;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(440, 200);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // CyBLEMTKSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 240);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.LoadMsgLabel);
            this.Controls.Add(this.LoadProgressBar);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 240);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 240);
            this.Name = "CyBLEMTKSplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CyBLEMTKSplashScreen";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar LoadProgressBar;
        private System.Windows.Forms.Label LoadMsgLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label VersionLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}