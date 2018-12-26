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
    public partial class Instruments_Config_Dialog : Form
    {
        LogManager log = new LogManager();
        Agilent sw = new Agilent();


        public Instruments_Config_Dialog()
        {
            InitializeComponent();
            DmmAlias_textBox.Text = CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias;
            swAlias_textBox.Text = CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias;
            comboBoxCurrUnit.Text = CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureUnit;
            numericUpDownRelayDelay.Value = CyBLE_MTK_Application.Properties.Settings.Default.RelayDelayInMS;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MultiMeter dmm = new MultiMeter(DmmAlias_textBox.Text);
                sw.InitializeU2751A_WELLA(swAlias_textBox.Text);
                sw.SetRelayWellA_ALLCLOSE();

                toolStripStatusLabel1.Text = "DMM&SW: " + DmmAlias_textBox.Text + " && " +swAlias_textBox.Text + " are connected.";


                log.PrintLog(this, "U2751A is connected and all channels are closed.", LogDetailLevel.LogRelevant);
                button2.Focus();

            }
            catch
            {
                toolStripStatusLabel1.Text = "DMM&&SW: " + DmmAlias_textBox.Text + " OR " + swAlias_textBox.Text + " is not connected.";
                button2.Focus();

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias = DmmAlias_textBox.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias = swAlias_textBox.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureUnit = comboBoxCurrUnit.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.RelayDelayInMS = numericUpDownRelayDelay.Value;
            CyBLE_MTK_Application.Properties.Settings.Default.Save();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
