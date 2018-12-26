using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{
    public partial class ManufacturingModeSettings : Form
    {
        public ManufacturingModeSettings()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {

            CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialNumberDuplicationCheck = DUTSerialNumberDuplicationCheck.Checked;


            CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialNumberDuplicationCheck = EnableComPortsPredefineCheck.Checked;


            CyBLE_MTK_Application.Properties.Settings.Default.Save();

            CyBLE_MTK_Application.Properties.Settings.Default.Upgrade();

            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            //CyBLE_MTK_Application.Properties.Settings.Default.Reset();
            this.Close();
        }

        private void ManufacturingModeSettings_Load(object sender, EventArgs e)
        {
            DUTSerialNumberDuplicationCheck.Checked = CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialNumberDuplicationCheck;
            EnableComPortsPredefineCheck.Checked = CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialNumberDuplicationCheck;
        }
    }
}
