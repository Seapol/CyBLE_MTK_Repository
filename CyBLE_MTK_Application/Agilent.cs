#define debug
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Ivi.Visa.Interop;
using System.Windows.Forms;


namespace CypressSemiconductor.ChinaManufacturingTest
{
    public class AgilentMessageEventArgs : EventArgs
    {
        public AgilentMessageEventArgs(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
    
    
    public class Agilent
    {
        public static bool devReadyStatus = false;

        public static string IDN_SwitchA = "Missing SwitchA";


        private const string CURRENT_RANGE_CH1 = "R120mA";
        private const string CURRENT_RANGE_CH2 = "R120mA";
        private const string CURRENT_RANGE_CH3 = "R120mA";
        private const string CURRENT_LIMIT_CH1 = "0.12";
        private const string CURRENT_LIMIT_CH2 = "0.12";
        private const string CURRENT_LIMIT_CH3 = "0.12";
        private const string NPLC_DEFAULT = "30";           //VICD - NPLC defines the number of samples taken during a current measurement. NPLC 10 yeilds 215ms sample time at 50Hz

        private const string RELAY_CH1 = "101";             //VICD - These are the relay locations to close for each channel
        private const string RELAY_CH2 = "102";             //VICD - These assignments assume that SMU Ch 1 powers Ch 1 - 4, SMU Ch 2 powers Ch 5 - 8
        private const string RELAY_CH3 = "103";
        private const string RELAY_CH4 = "104";
        private const string RELAY_CH5 = "205";
        private const string RELAY_CH6 = "206";
        private const string RELAY_CH7 = "207";
        private const string RELAY_CH8 = "208";


        public static int RELAY_DELAY = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.RelayDelayInMS.ToString());        //200

        //private const int RELAY_DELAY = 200;        //50
        //private const int SMU_ON_DELAY = 200;
        //private const int SMU_OFF_DELAY = 200;
        //private const int SWITCH_DEALY = 200;

        private static FormattedIO488 ioVoltCntrl;
        private static FormattedIO488 ioSwitchCntrl;
        private static FormattedIO488 ioRelayWellACntrl;
        private static FormattedIO488 ioRelayWellBCntrl;

        ~Agilent()
        {
            ioVoltCntrl = null;
            ioSwitchCntrl = null;
            ioRelayWellACntrl = null;
            ioRelayWellBCntrl = null;
        }
        
        //****************************************//
        //    Define the Agilent Message Event    //
        //****************************************//
        public delegate void AgilentMessageEventHandler(object sender, AgilentMessageEventArgs ea);
        public event AgilentMessageEventHandler AgilentMessageEvent;
        protected virtual void OnAgilentMessage(AgilentMessageEventArgs ea)
        {
            AgilentMessageEventHandler handler = AgilentMessageEvent;
            if (handler != null)
            {
                handler(this, ea);
            }
        }
        
        #region U2722A Methods
        public void InitializeU2722A(string ConnectString)
        {
            bool devReady = false;
            string idnTemp;

            ioVoltCntrl = new FormattedIO488();   // Create the formatted I/O object

            devReady = U2722A_Connect(ConnectString);
            if (devReady == false)
            {
                throw new Exception("Connect to U2722A failed on " + ConnectString);
            }
            else
            {
                ioVoltCntrl.WriteString("*RST", true);              // Reset
                ioVoltCntrl.WriteString("*CLS", true);              // Clear
                ioVoltCntrl.WriteString("*IDN?", true);             // Read ID string
                idnTemp = ioVoltCntrl.ReadString();                 

                OnAgilentMessage(new AgilentMessageEventArgs("Connect to U2722A suceess! U2722A IDN: " + idnTemp.ToString()));

                //VICD - Set line frequency to 50Hz (This effects measurement accuracy)
                //VICD - Future Dev - Low - Check U2722 for errors at initialization
                //VICD - Future Dev - Med - Add VOLT:LIM command to set a maximum output voltage
                ioVoltCntrl.WriteString("SYST:LFREQ F50HZ", true);
                // Set the inital voltage range to 20V
                ioVoltCntrl.WriteString("VOLT:RANG R20V, (@1)", true);
                ioVoltCntrl.WriteString("VOLT:RANG R20V, (@2)", true);
                ioVoltCntrl.WriteString("VOLT:RANG R20V, (@3)", true);
                // Set the initial output voltage to 0.0V
                ioVoltCntrl.WriteString("VOLT 0, (@1)", true);
                ioVoltCntrl.WriteString("VOLT 0, (@2)", true);
                ioVoltCntrl.WriteString("VOLT 0, (@3)", true);
                // VICD - Set the current ranges
                ioVoltCntrl.WriteString("CURR:RANG " + CURRENT_RANGE_CH1 + " , (@1)", true);
                ioVoltCntrl.WriteString("CURR:RANG " + CURRENT_RANGE_CH2 + " , (@2)", true);
                ioVoltCntrl.WriteString("CURR:RANG " + CURRENT_RANGE_CH3 + " , (@3)", true);
                // VICD - Set the current limits
                ioVoltCntrl.WriteString("CURR:LIM " + CURRENT_LIMIT_CH1 + " , (@1)", true);
                ioVoltCntrl.WriteString("CURR:LIM " + CURRENT_LIMIT_CH2 + " , (@2)", true);
                ioVoltCntrl.WriteString("CURR:LIM " + CURRENT_LIMIT_CH3 + " , (@3)", true);
            }
        }

        public void SetChannelVoltage(double voltage, int ch)
        {
            lock (ioVoltCntrl)
            {
                string chVoltage = voltage.ToString();
                string chNum = ch.ToString();
                ioVoltCntrl.WriteString("VOLT " + chVoltage + ", (@" + chNum + ")", true);
            }
        }

        public void ActivateChannelOutput(int ch)
        {
            lock (ioVoltCntrl)
            {
                string chNum = ch.ToString();
                ioVoltCntrl.WriteString("OUTP 1, (@" + chNum + ")", true);
                //Thread.Sleep(SMU_ON_DELAY); // wait for voltage to settle
            }
        }

        public void De_ActivateChannelOutput(int ch)
        {
            lock (ioVoltCntrl)
            {
                string chNum = ch.ToString();
                ioVoltCntrl.WriteString("OUTP 0, (@" + chNum + ")", true);
                //Thread.Sleep(SMU_OFF_DELAY); // wait for voltage to settle
            }
        }

        public double MeasureChannelCurrent(int ch)
        {
            lock (ioVoltCntrl)
            {
                string chNum = ch.ToString();
                double ch_current;
                ioVoltCntrl.WriteString("SENS:CURR:NPLC " + NPLC_DEFAULT + " (@" + chNum + ")", true);  //VICD -  Set the current measurement aperture
                //VICD - Future Dev - Low - Add NPLC variable control to decrease test time
                ioVoltCntrl.WriteString("MEAS:CURR? (@" + chNum + ")", true);
                ch_current = (double)ioVoltCntrl.ReadNumber(IEEEASCIIType.ASCIIType_Any, true);
                return ch_current;
            }
        }
        #endregion

        #region U2751 Methods
        public bool InitializeU2751A_WELLA(string ConnectString)
        {
            bool devReady = false;
            string idnTemp;

            ioRelayWellACntrl = new FormattedIO488();     // Create the formatted I/O object
            devReady = U2751A_WellA_Connect(ConnectString);
            if (devReady == false)
            {
                devReadyStatus = false;
                //throw new Exception("Connect to U2751A_WELLA failed on " + ConnectString);
                //MessageBox.Show("Connect to U2751A_WELLA failed on " + ConnectString);
            }
            else
            {
                devReadyStatus = true;
                ioRelayWellACntrl.WriteString("*RST", true);        // Reset
                ioRelayWellACntrl.WriteString("*CLS", true);        // Clear
                ioRelayWellACntrl.WriteString("*IDN?", true);       // Read ID string
                idnTemp = ioRelayWellACntrl.ReadString();

                IDN_SwitchA = idnTemp;

                OnAgilentMessage(new AgilentMessageEventArgs("Connect to U2751A WellA suceess! U2751A WellA IDN: " + idnTemp.ToString()));
            }

            return devReadyStatus;
        }

        public void InitializeU2751A_WELLB(string ConnectString)
        {
            bool devReady = false;
            string idnTemp;

            ioRelayWellBCntrl = new FormattedIO488();      // Create the formatted I/O object
            devReady = U2751A_WellB_Connect(ConnectString);
            if (devReady == false)
            {
                devReadyStatus = false;
                //throw new Exception("Connect to U2751A_WELLB failed on " + ConnectString);
                //MessageBox.Show("Connect to U2751A_WELLB failed on " + ConnectString);

            }
            else
            {
                devReadyStatus = true;
                ioRelayWellBCntrl.WriteString("*RST", true);        // Reset
                ioRelayWellBCntrl.WriteString("*CLS", true);        // Clear
                ioRelayWellBCntrl.WriteString("*IDN?", true);       // Read ID string
                idnTemp = ioRelayWellBCntrl.ReadString();           

                OnAgilentMessage(new AgilentMessageEventArgs("Connect to U2751A WellB suceess! U2751A WellB IDN: " + idnTemp.ToString()));
            }
        }

        public void SetRelayWellA(byte RelaySetCh1, byte RelaySetCh2)
        {
            ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);

            if ((RelaySetCh1 & (byte)0x1) == (byte)0x1)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x2) == (byte)0x2)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH2 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x4) == (byte)0x4)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH3 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x8) == (byte)0x8)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH4 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x1) == (byte)0x1)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x2) == (byte)0x2)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH6 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x4) == (byte)0x4)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH7 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x8) == (byte)0x8)
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH8 + ")", true);
            }
            Thread.Sleep(RELAY_DELAY);      //Wait for relay to settle
        }

        public void SetRelayWellA_byCH(int CH_no, bool close)
        {
            if (close)
            {
                ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
                ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);

                switch (CH_no)
                {
                    case 1:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ")", true);
                        break;
                    case 2:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH2 + ")", true);
                        break;
                    case 3:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH3 + ")", true);
                        break;
                    case 4:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH4 + ")", true);
                        break;
                    case 5:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ")", true);
                        break;
                    case 6:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH6 + ")", true);
                        break;
                    case 7:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH7 + ")", true);
                        break;
                    case 8:
                        ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH8 + ")", true);
                        break;
                }
            }
            else
            {
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
                ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);

                switch (CH_no)
                {
                    case 1:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ")", true);
                        break;
                    case 2:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH2 + ")", true);
                        break;
                    case 3:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH3 + ")", true);
                        break;
                    case 4:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH4 + ")", true);
                        break;
                    case 5:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ")", true);
                        break;
                    case 6:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH6 + ")", true);
                        break;
                    case 7:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH7 + ")", true);
                        break;
                    case 8:
                        ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH8 + ")", true);
                        break;
                }
            }


            Thread.Sleep(RELAY_DELAY);      //Wait for relay to settle
        }


        public void SetRelayWellA_ALLCLOSE()
        {
            ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellACntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);
        }

        public void SetRelayWellA_ALLOPEN()
        {
            ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellACntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);
        }

        public void SetRelayWellB(byte RelaySetCh1, byte RelaySetCh2)
        {
            ioRelayWellBCntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellBCntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);

            if ((RelaySetCh1 & (byte)0x1) == (byte)0x1)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x2) == (byte)0x2)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH2 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x4) == (byte)0x4)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH3 + ")", true);
            }
            if ((RelaySetCh1 & (byte)0x8) == (byte)0x8)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH4 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x1) == (byte)0x1)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x2) == (byte)0x2)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH6 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x4) == (byte)0x4)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH7 + ")", true);
            }
            if ((RelaySetCh2 & (byte)0x8) == (byte)0x8)
            {
                ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH8 + ")", true);
            }
            Thread.Sleep(RELAY_DELAY);      //Wait for relay to settle
        }

        public void SetRelayWellB_ALLCLOSE()
        {
            ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellBCntrl.WriteString("ROUT:CLOS (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);
        }

        public void SetRelayWellB_ALLOPEN()
        {
            ioRelayWellBCntrl.WriteString("ROUT:OPEN (@" + RELAY_CH1 + ":" + RELAY_CH4 + ")", true);
            ioRelayWellBCntrl.WriteString("ROUT:OPEN (@" + RELAY_CH5 + ":" + RELAY_CH8 + ")", true);
        }

        #endregion

        #region U2651 Methods
        public void InitializeU2651A(string ConnectString)
        {
            bool devReady = false;
            string idnTemp;

            ioSwitchCntrl = new FormattedIO488();    // Create the formatted I/O object

            devReady = U2651A_Connect(ConnectString);
            if (devReady == false)
            {
                throw new Exception("Connect to U2651A failed on " + ConnectString);
            }
            else
            {
                ioSwitchCntrl.WriteString("*RST", true);            // Reset
                ioSwitchCntrl.WriteString("*CLS", true);            // Clear
                ioSwitchCntrl.WriteString("*IDN?", true);           // Read ID string
                idnTemp = ioSwitchCntrl.ReadString();               

                OnAgilentMessage(new AgilentMessageEventArgs("Connect to U2651A suceess! U2651A IDN: " + idnTemp.ToString()));
            }
        }

        public bool SetBit(string Channel, string BitIO, string BitStatus)
        {
            lock (ioSwitchCntrl)
            {
                //Set output
                ioSwitchCntrl.WriteString("SOUR:DIG:DATA:BIT " + BitStatus + "," + BitIO + ",(@" + Channel + ")", true);

                //Check output status
                string result;
                ioSwitchCntrl.WriteString("SOUR:DIG:DATA:BIT? " + BitIO + ",(@" + Channel + ")", true);

                result = ioSwitchCntrl.ReadString().Trim();
                if (result == BitStatus)
                {
                    //Thread.Sleep(SWITCH_DEALY);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string ReadBit(string Channel, string BitIO)
        {
            lock (ioSwitchCntrl)
            {
                string result;
                ioSwitchCntrl.WriteString("DIG:DATA:BIT? " + BitIO + ",(@" + Channel + ")", true);
                result = ioSwitchCntrl.ReadString().Trim();
                return result;   
            }
        }


        public bool SetChannel(string Channel, byte ChannelStatus)
        {
            lock (ioSwitchCntrl)
            {
                //Set output
                string sChannelStatus = Convert.ToString(ChannelStatus);
                ioSwitchCntrl.WriteString("SOUR:DIG:DATA:BYTE " + sChannelStatus + ",(@" + Channel + ")", true);

                //Check output status
                byte result;
                ioSwitchCntrl.WriteString("SOUR:DIG:DATA:BYTE? (@" + Channel + ")", true);

                result = Convert.ToByte(ioSwitchCntrl.ReadString().Trim());
                if (result == ChannelStatus)
                {
                    //Thread.Sleep(SWITCH_DEALY);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string ReadChannel(string Channel)
        {
            lock (ioSwitchCntrl)
            {
                string result;
                ioSwitchCntrl.WriteString("DIG:DATA:BYTE? (@" + Channel + ")", true);
                result = ioSwitchCntrl.ReadString().Trim();
                return result;
            }
        }
        #endregion

        #region device connect
        private bool U2722A_Connect(string connectString)
        {
            try
            {
                // connectString="USB0::2391::16664::MY48470014::0::INSTR";
                // Create the resource manager and open a session with the instrument specified on U2722AtxtAddr
                ResourceManager grm1 = new ResourceManager();
                ioVoltCntrl.IO = (IMessage)grm1.Open(connectString, AccessMode.NO_LOCK, 2000, "");
                ioVoltCntrl.IO.Timeout = 7000;
                // Only return true if previous calls have also returned true
                return true;
            }
            catch
            {
                ioVoltCntrl.IO = null;
                return false;
            }
        }

        private bool U2651A_Connect(string connectString)
        {
            try
            {
                // Create the resource manager and open a session with the instrument specified on txtAddr
                ResourceManager grm2 = new ResourceManager();
                ioSwitchCntrl.IO = (IMessage)grm2.Open(connectString, AccessMode.NO_LOCK, 2000, "");
                ioSwitchCntrl.IO.Timeout = 7000;
                // Only return true if previous calls have also returned true
                return true;
            }
            catch
            {
                ioSwitchCntrl.IO = null;
                return false;
            }
        }

        private bool U2751A_WellA_Connect(string connectString)
        {
            try
            {
                // Create the resource manager and open a session with the instrument specified on txtAddr
                ResourceManager grm3 = new ResourceManager();
                ioRelayWellACntrl.IO = (IMessage)grm3.Open(connectString, AccessMode.NO_LOCK, 2000, "");
                ioRelayWellACntrl.IO.Timeout = 7000;
                // Only return true if previous calls have also returned true
                return true;
            }
            catch
            {
                ioRelayWellACntrl.IO = null;
                return false;
            }
        }

        private bool U2751A_WellB_Connect(string connectString)
        {
            try
            {
                // Create the resource manager and open a session with the instrument specified on txtAddr
                ResourceManager grm4 = new ResourceManager();
                ioRelayWellBCntrl.IO = (IMessage)grm4.Open(connectString, AccessMode.NO_LOCK, 2000, "");
                ioRelayWellBCntrl.IO.Timeout = 7000;
                // Only return true if previous calls have also returned true
                return true;
            }
            catch
            {
                ioRelayWellBCntrl.IO = null;
                return false;
            }
        }
        #endregion

    }//end of agilent class

}//end of namespace
