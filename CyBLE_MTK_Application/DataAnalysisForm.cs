using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{
    public partial class FPYForm : Form
    {
        double[] yValues = { CyBLE_MTK.COUNT_PASS, CyBLE_MTK.COUNT_FAIL};
        string[] xValues = { "PASS", "FAIL" };

        //double[] yValues = { 888, 20 };
        //string[] xValues = { "PASS", "FAIL" };


        public FPYForm()
        {
            InitializeComponent();

            this.CenterToScreen();

            chart_yield.Text = System.Net.Dns.GetHostName();

            chart_yield.Series["Series1"].Points.DataBindXY(xValues, yValues);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            chart_yield.Update();
            this.Close();

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CyBLE_MTK.COUNT_ALL = 0;
            CyBLE_MTK.COUNT_FAIL = 0;
            CyBLE_MTK.COUNT_PASS = 0;
        }
    }
}
