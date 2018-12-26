using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CypressSemiconductor.ChinaManufacturingTest;

namespace CyBLE_MTK_Application
{
    public partial class Switch_Config_Dialog : Form
    {
        LogManager log = new LogManager();
        Agilent sw = new Agilent();

        public Switch_Config_Dialog()
        {
            InitializeComponent();

            SWAlias_textBox.Text = CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                sw.InitializeU2751A_WELLA(SWAlias_textBox.Text);
                sw.SetRelayWellA_ALLCLOSE();
                
                toolStripStatusLabel1.Text = "SW: " + SWAlias_textBox.Text + " is connected.";
                
                
                log.PrintLog(this, "U2751A is connected and all channels are closed.", LogDetailLevel.LogRelevant);
                button2.Focus();
            }
            catch
            {
                toolStripStatusLabel1.Text = "SW: " + SWAlias_textBox.Text + " is not connected.";
                button2.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias = SWAlias_textBox.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
