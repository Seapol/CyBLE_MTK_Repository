using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CypressSemiconductor.ChinaManufacturingTest;

namespace CyBLE_MTK_Application
{
    public partial class DmmSwitchSettingsDialog : Form
    {
        protected LogManager logger;

        public DmmSwitchSettingsDialog(LogManager Logger)
        {
            InitializeComponent();

            if (Logger == null)
            {
                Logger = new LogManager();
            }
            logger = Logger;
        }



        public void SaveParameters()
        {
            bool changed = false;

            if (textBox_dmm_alias.Text.ToLower()!= CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias.ToLower())
            {
                CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias = textBox_dmm_alias.Text;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
                changed = true;
            }

            if (textBox_sw_alias.Text.ToLower() != CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias.ToLower())
            {
                CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias = textBox_sw_alias.Text;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
                changed = true;
            }

            if (comboBox_CurrUnit.Text.ToLower() != CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureUnit.ToLower())
            {
                CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureUnit = comboBox_CurrUnit.Text;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
                changed = true;
            }

            if (numericUpDown_RelayDelay.Value != CyBLE_MTK_Application.Properties.Settings.Default.RelayDelayInMS)
            {
                CyBLE_MTK_Application.Properties.Settings.Default.RelayDelayInMS = numericUpDown_RelayDelay.Value;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
                changed = true;
            }


            if (changed)
            {
                MTKInstruments.SyncParams();
                logger.PrintLog(this, "Parameters Sync done.", LogDetailLevel.LogRelevant);
            }
            else
            {
                //logger.PrintLog(this, "Parameters hasn't been changed.", LogDetailLevel.LogRelevant);
            }

        }


        //OK Btn
        private void button3_Click(object sender, EventArgs e)
        {
            SaveParameters();
            this.Close();
        }

        //Cancel Btn
        private void button2_Click(object sender, EventArgs e)
        {
            CyBLE_MTK_Application.Properties.Settings.Default.Reset();
            this.Close();

        }

        //Connect Btn
        private void button1_Click(object sender, EventArgs e)
        {
            SaveParameters();

            try
            {
                MTKInstruments.ConnectDMM();
                if (MTKInstruments.DmmConnected)
                {
                    logger.PrintLog(this, "DMM " + MultiMeter.IDN_MultiMeter + " has been connected successfully.", LogDetailLevel.LogRelevant);
                }
                else
                {
                    logger.PrintLog(this, "DMM Not Connected: " + MultiMeter.IDN_MultiMeter, LogDetailLevel.LogRelevant);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Fail to connect DMM ... \n\n" + ex.ToString() , "MTK Instrument Error");
                return;
            }

            try
            {
                MTKInstruments.ConnectSwitch();
                if (MTKInstruments.SwConnected)
                {
                    logger.PrintLog(this, "Switch " + Agilent.IDN_SwitchA + " has been connected successfully.", LogDetailLevel.LogRelevant);
                }
                else
                {
                    logger.PrintLog(this, "Switch Not Connected: " + Agilent.IDN_SwitchA, LogDetailLevel.LogRelevant);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Fail to connect Swtich ... \n\n" + ex.ToString(), "MTK Instrument Error");
                return;
            }

            if (MTKInstruments.AlldevReady)
            {
                logger.PrintLog(this, "DMM and Switch have been connected successfully.", LogDetailLevel.LogRelevant);
            }

        }
    }
}
