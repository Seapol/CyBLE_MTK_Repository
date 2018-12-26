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
    public partial class DUTCurrentMeasureDialog : Form
    {

        private MTKTestDUTCurrentMeasure mTKTestDUTCurrentMeasure;

        public DUTCurrentMeasureDialog()
        {
            InitializeComponent();

            mTKTestDUTCurrentMeasure = new MTKTestDUTCurrentMeasure();

            //numericUpDownSampleInterval.Value = CyBLE_MTK_Application.Properties.Settings.Default.IntervalBetweenDUTCurrentMeasure;
            //comboBox_overall_condition.Text = CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureOverallPassCondition;
            //numericUpDownDelayBeforeTest.Value = CyBLE_MTK_Application.Properties.Settings.Default.DelayInMSBeforeDUTCurrentMeasure;
            //numericUpDownUpperlimit.Value = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentUpperLimitMilliAmp.ToString());
            //numericUpDownLowerlimit.Value = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentLowerLimitMilliAmp.ToString());
            //numericUpDownSampleCount.Value = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.SamplesCountForDUTCurrentMeasure.ToString());

        }

        public DUTCurrentMeasureDialog(MTKTestDUTCurrentMeasure NewmTKTestDUTCurrentMeasure) : this()
        {

            mTKTestDUTCurrentMeasure = NewmTKTestDUTCurrentMeasure;

        }

        private void OKbtn_DUTCurrentMeasureDialog_Click(object sender, EventArgs e)
        {
            mTKTestDUTCurrentMeasure.DUTCurrentUpperLimitMilliAmp = double.Parse(numericUpDownUpperlimit.Text);
            mTKTestDUTCurrentMeasure.DUTCurrentLowerLimitMilliAmp = double.Parse(numericUpDownLowerlimit.Text);
            mTKTestDUTCurrentMeasure.DelayBeforeTest = int.Parse(numericUpDownDelayBeforeTest.Text);
            mTKTestDUTCurrentMeasure.DelayAfterTest = int.Parse(numericUpDownDelayAfterTest.Text);
            mTKTestDUTCurrentMeasure.IntervalInMS = int.Parse(numericUpDownSampleInterval.Text);
            mTKTestDUTCurrentMeasure.SamplesCount = int.Parse(numericUpDownSampleCount.Text);
            mTKTestDUTCurrentMeasure.criterion_per_sample = comboBox_criteria_per_sample.Text;
            mTKTestDUTCurrentMeasure.overallpass_condition = comboBox_overall_condition.Text;
            

            //CyBLE_MTK_Application.Properties.Settings.Default.IntervalBetweenDUTCurrentMeasure = int.Parse(numericUpDownInterval.Text);
            //CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasureUnit = comboBox_unit.Text;
            //CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentMeasurePassCondition = comboBox_pass_condition.Text;
            //CyBLE_MTK_Application.Properties.Settings.Default.DelayInMSBeforeDUTCurrentMeasure = int.Parse(numericUpDownDelay.Text);
            //CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentUpperLimitMilliAmp = double.Parse(numericUpDownUpperlimit.Text);
            //CyBLE_MTK_Application.Properties.Settings.Default.DUTCurrentLowerLimitMilliAmp = double.Parse(numericUpDownLowerlimit.Text);
            //CyBLE_MTK_Application.Properties.Settings.Default.RepeatForDUTCurrentMeasure = int.Parse(numericUpDownRepeat.Text);

            CyBLE_MTK_Application.Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownRepeat_ValueChanged(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {

            numericUpDownUpperlimit.Value = (decimal)mTKTestDUTCurrentMeasure.DUTCurrentUpperLimitMilliAmp;
            numericUpDownLowerlimit.Value = (decimal)mTKTestDUTCurrentMeasure.DUTCurrentLowerLimitMilliAmp;
            numericUpDownDelayBeforeTest.Value = mTKTestDUTCurrentMeasure.DelayBeforeTest;
            numericUpDownDelayAfterTest.Value = mTKTestDUTCurrentMeasure.DelayAfterTest;
            numericUpDownSampleInterval.Value = mTKTestDUTCurrentMeasure.IntervalInMS;
            numericUpDownSampleCount.Value = mTKTestDUTCurrentMeasure.SamplesCount;
            comboBox_criteria_per_sample.Text = mTKTestDUTCurrentMeasure.criterion_per_sample;
            comboBox_overall_condition.Text = mTKTestDUTCurrentMeasure.overallpass_condition;

            base.OnLoad(e);
        }
    }
}
