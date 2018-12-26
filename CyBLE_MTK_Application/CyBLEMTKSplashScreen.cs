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
    public partial class CyBLEMTKSplashScreen : Form
    {
        public int LoadStatus
        {
            get
            {
                return this.LoadProgressBar.Value;
            }
            set
            {
                if ((value <= 100) && (value >= 0))
                {
                    this.LoadProgressBar.Value = value;
                }
                else if (value > 100)
                {
                    this.LoadProgressBar.Value = 100;
                }
                else if (value < 0)
                {
                    this.LoadProgressBar.Value = 0;
                }
            }
        }

        public string LoadMessage
        {
            get
            {
                return this.LoadMsgLabel.Text;
            }
            set
            {
                this.LoadMsgLabel.Text = value;
            }
        }

        public string AppVersion
        {
            set
            {
                this.VersionLabel.Text = value;
            }
        }

        public CyBLEMTKSplashScreen()
        {
            InitializeComponent();
            this.VersionLabel.Parent = this.pictureBox1;
        }
    }
}
