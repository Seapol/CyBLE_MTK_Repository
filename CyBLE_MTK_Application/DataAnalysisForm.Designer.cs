namespace CyBLE_MTK_Application
{
    partial class FPYForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart_yield = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart_yield)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_yield
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_yield.ChartAreas.Add(chartArea1);
            this.chart_yield.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_yield.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart_yield.Legends.Add(legend1);
            this.chart_yield.Location = new System.Drawing.Point(0, 0);
            this.chart_yield.Name = "chart_yield";
            this.chart_yield.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart_yield.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0))))),
        System.Drawing.Color.Red};
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Label = "#VALX  #PERCENT";
            series1.Legend = "Legend1";
            series1.LegendText = "#VALX    #VAL";
            series1.Name = "Series1";
            this.chart_yield.Series.Add(series1);
            this.chart_yield.Size = new System.Drawing.Size(582, 555);
            this.chart_yield.TabIndex = 0;
            this.chart_yield.Text = "chart1";
            title1.Name = "First Pass Yield";
            this.chart_yield.Titles.Add(title1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(457, 484);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(457, 433);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 45);
            this.button2.TabIndex = 2;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FPYForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 555);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart_yield);
            this.Name = "FPYForm";
            this.Text = "First Pass Yield";
            ((System.ComponentModel.ISupportInitialize)(this.chart_yield)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_yield;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}