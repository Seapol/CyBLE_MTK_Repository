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
    public partial class MTKTestProgramAllDialog : Form
    {
        private MTKTestProgramAll ProgAll;

        public MTKTestProgramAllDialog()
        {
            InitializeComponent();
            ProgAll = new MTKTestProgramAll();
        }

        public MTKTestProgramAllDialog(MTKTestProgramAll ProgramAllTest) : this()
        {
            ProgAll = ProgramAllTest;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ProgAll.ProgramAllAtEnd)
            {
                EndRadioButton.Checked = true;
            }
            else
            {
                BegningRadioButton.Checked = true;
            }
            HexFilePathTextBox.Text = ProgAll.SelectedHEXFilePath;

            ModuleFWValidCheckBox.Checked = ProgAll.EnableModuleVerification;
            AVCheckBox.Checked = ProgAll.ApplicationVersionEnable;
            BSVCheckBox.Checked = ProgAll.BLEStackVersionEnable;
            PVCheckBox.Checked = ProgAll.ProtocolVersionEnable;
            BCCheckBox.Checked = ProgAll.BootCauseEnable;
            MACACheckBox.Checked = ProgAll.MACAddressEnable;
            HWIDCheckBox.Checked = ProgAll.HWIDEnable;

            EventTypeComboBox.Text = ProgAll.EventType;
            AVTextBox.Text = ProgAll.ApplicationVersion;
            BLESVTextBox.Text = ProgAll.BLEStackVersion;
            PVTextBox.Text = ProgAll.ProtocolVersion;
            BCTextBox.Text = ProgAll.BootCause;
            MACATtextBox.Text = ProgAll.MACAddress;
            HWIDTextBox.Text = ProgAll.HWIDValue;

            DelayNumericUpDown.Value = (decimal)(ProgAll.UARTCaptureDelay);

            base.OnLoad(e);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (HexFilePathTextBox.Text == "")
            {
                MessageBox.Show("Hex file path cannot be left blank.", "Information", MessageBoxButtons.OK);
                return;
            }
            if ((AVCheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(AVTextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                AVTextBox.Focus();
                AVTextBox.SelectAll();
                return;
            }
            if ((BSVCheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(BLESVTextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                BLESVTextBox.Focus();
                BLESVTextBox.SelectAll();
                return;
            }
            if ((PVCheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(PVTextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                PVTextBox.Focus();
                PVTextBox.SelectAll();
                return;
            }
            if ((HWIDCheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(HWIDTextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                HWIDTextBox.Focus();
                HWIDTextBox.SelectAll();
                return;
            }
            if ((BCCheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(BCTextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                BCTextBox.Focus();
                BCTextBox.SelectAll();
                return;
            }
            if ((MACACheckBox.Checked) &&
                (System.Text.RegularExpressions.Regex.IsMatch(MACATtextBox.Text, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z") == false))
            {
                MessageBox.Show("Invalid value found. Enter hex numbers only.", "Information", MessageBoxButtons.OK);
                MACATtextBox.Focus();
                MACATtextBox.SelectAll();
                return;
            }

            ProgAll.SelectedHEXFilePath = HexFilePathTextBox.Text;

            ProgAll.EnableModuleVerification = ModuleFWValidCheckBox.Checked;
            ProgAll.ApplicationVersionEnable = AVCheckBox.Checked;
            ProgAll.BLEStackVersionEnable = BSVCheckBox.Checked;
            ProgAll.ProtocolVersionEnable = PVCheckBox.Checked;
            ProgAll.BootCauseEnable = BCCheckBox.Checked;
            ProgAll.MACAddressEnable = MACACheckBox.Checked;
            ProgAll.HWIDEnable = HWIDCheckBox.Checked;

            ProgAll.EventType = EventTypeComboBox.Text;
            ProgAll.ApplicationVersion = AVTextBox.Text;
            ProgAll.BLEStackVersion = BLESVTextBox.Text;
            ProgAll.ProtocolVersion = PVTextBox.Text;
            ProgAll.BootCause = BCTextBox.Text;
            ProgAll.MACAddress = MACATtextBox.Text;
            ProgAll.HWIDValue = HWIDTextBox.Text;

            ProgAll.UARTCaptureDelay = (int)(DelayNumericUpDown.Value);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BegningRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BegningRadioButton.Checked == true)
            {
                ProgAll.ProgramAllAtEnd = false;
                MTKTestProgramAll.MTKTestProgramAllAtEnd = false;
            }
        }

        private void EndRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (EndRadioButton.Checked == true)
            {
                ProgAll.ProgramAllAtEnd = true;
                MTKTestProgramAll.MTKTestProgramAllAtEnd = true;
            }
        }

        private void OpenHEXFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog TestProgOpenFileDialog = new OpenFileDialog();
            TestProgOpenFileDialog.Filter = "HEX Files (*.hex)|*.hex|All Files (*.*)|*.*";
            TestProgOpenFileDialog.FilterIndex = 1;

            if (TestProgOpenFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            HexFilePathTextBox.Text = TestProgOpenFileDialog.FileName;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }

        private void ModuleCheckCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModuleCheckGroupBox.Enabled = ModuleFWValidCheckBox.Checked;
            AVCheckBox.Checked = ModuleFWValidCheckBox.Checked;
            BSVCheckBox.Checked = ModuleFWValidCheckBox.Checked;
            PVCheckBox.Checked = ModuleFWValidCheckBox.Checked;
            HWIDCheckBox.Checked = ModuleFWValidCheckBox.Checked;
            BCCheckBox.Checked = ModuleFWValidCheckBox.Checked;
            MACACheckBox.Checked = ModuleFWValidCheckBox.Checked;
            EventTypeComboBox.SelectedIndex = 0;
        }

        private void AVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AVTextBox.Enabled = AVCheckBox.Checked;
            if (AVTextBox.Enabled == false)
            {
                AVTextBox.Text = "";
            }
        }

        private void BSVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            BLESVTextBox.Enabled = BSVCheckBox.Checked;
            if (BLESVTextBox.Enabled == false)
            {
                BLESVTextBox.Text = "";
            }
        }

        private void PVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PVTextBox.Enabled = PVCheckBox.Checked;
            if (PVTextBox.Enabled == false)
            {
                PVTextBox.Text = "";
            }
        }

        private void HWIDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HWIDTextBox.Enabled = HWIDCheckBox.Checked;
            if (HWIDTextBox.Enabled == false)
            {
                HWIDTextBox.Text = "";
            }
        }

        private void MACACheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MACATtextBox.Enabled = MACACheckBox.Checked;
            if (MACATtextBox.Enabled == false)
            {
                MACATtextBox.Text = "";
            }
        }

        private void EventTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventTypeComboBox.Text == "BOOT")
            {
                MACACheckBox.Enabled = true;
            }
            else if (EventTypeComboBox.Text == "TFAC")
            {
                MACACheckBox.Checked = false;
                MACACheckBox.Enabled = false;
            }
        }

        private void BCCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            BCTextBox.Enabled = BCCheckBox.Checked;
            if (BCTextBox.Enabled == false)
            {
                BCTextBox.Text = "";
            }
        }
    }
}
