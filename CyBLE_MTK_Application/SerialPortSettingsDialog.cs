using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;

namespace CyBLE_MTK_Application
{
    public partial class SerialPortSettingsDialog : Form
    {
        private const string _connectString = "Co&nnect";
        private const string _disconnectString = "Disco&nnect";
        //private CyBLEMTKSerialPort _mTKSerialPort;
        
        private List<COMPortInfo> _ComPortInfoList;

        private bool _closeOnConnect;
        private SerialPort _DeviceSerialPort;

        public bool CloseOnConnect
        {
            get { return _closeOnConnect; }
            set { _closeOnConnect = value; }
        }
        private LogManager Log;
        private string CommandResult, PrevDUTConnStatus, PrevHostConnStatus, CurHostConnStatus, CurDUTConnStatus;
        private bool ContinuePoll;
        private int PollInterval;
        private Thread DevicePollThread;
        object PollThreadLock = new object();
        public volatile int ThreadIsRunning;
        private bool ConnectionTime;

        public int ID;

        public event ConnectionEventHandler OnDUTConnectionStatusChange;
        public event ConnectionEventHandler OnHostConnectionStatusChange;

        public delegate void ConnectionEventHandler(string ConnectionStatus);

        public PortType SerialPortType;
        public bool CheckDUTPresence, AutoVerifyON;

        private bool PollingDevInThread
        {
            get { return false; }
        }

        public SerialPort DeviceSerialPort
        {
            get { return _DeviceSerialPort; }
            set
            {
                _DeviceSerialPort = value;
                SerialPortCombo.Enabled = true;
                RefreshButton.Enabled = true;
                ConnectButton.Text = "Co&nnect";
            }
        }

        public SerialPortSettingsDialog()
        {
            InitializeComponent();
            _DeviceSerialPort = new CyBLEMTKSerialPort();
            //_mTKSerialPort = new CyBLEMTKSerialPort();
            CloseOnConnect = false;
            Log = new LogManager();
            SerialPortType = PortType.NoType;
            CheckDUTPresence = false;
            AddPorts();
            CurHostConnStatus = "";
            CurDUTConnStatus = "";
            PrevHostConnStatus = "";
            PrevDUTConnStatus = "";
            ContinuePoll = false;
            PollInterval = 1000;
            AutoVerifyON = false;
            ConnectionTime = false;
        }

        public SerialPortSettingsDialog(LogManager Logger) : this()
        {
            Log = Logger;
        }

        public SerialPortSettingsDialog(CyBLEMTKSerialPort SerialPort)
            : this()
        {
           DeviceSerialPort  = SerialPort;
        }

        private void AddPorts()
        {
            
            if (_ComPortInfoList == null || _ComPortInfoList.Count == 0)
                _ComPortInfoList = COMPortInfo.GetCOMPortsInfo();
            Graphics ComboGraphics = SerialPortCombo.CreateGraphics();
            Font ComboFont = SerialPortCombo.Font;
            int MaxWidth = 0;
            foreach (COMPortInfo ComPort in _ComPortInfoList)
            {
                string s = ComPort.Name + " - " + ComPort.Description;
                SerialPortCombo.Items.Add(s);
                int VertScrollBarWidth = (SerialPortCombo.Items.Count > SerialPortCombo.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;
                int DropDownWidth = (int)ComboGraphics.MeasureString(s, ComboFont).Width + VertScrollBarWidth;
                if (MaxWidth < DropDownWidth)
                {
                    SerialPortCombo.DropDownWidth = DropDownWidth;
                    MaxWidth = DropDownWidth;
                }
            }
            if (SerialPortCombo.Items.Count > 0)
            {
                SerialPortCombo.SelectedIndex = 0;
            }
            Log.PrintLog(this, _ComPortInfoList.Count.ToString() + " serial ports found.", LogDetailLevel.LogRelevant);
        }

        private void RefreshPortList()
        {
            SerialPortCombo.Items.Clear();
            AddPorts();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshPortList();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectButton.Enabled = false;
                if (ConnectButton.Text == _connectString)
                {
                    if (SerialPortCombo.SelectedIndex >= 0)
                    {
                        try
                        {
                            //_mTKSerialPort.DeviceSerialPort.DiscardOutBuffer();
                            //_mTKSerialPort.DeviceSerialPort.DiscardInBuffer();
                            if (_DeviceSerialPort.IsOpen)
                            {
                                _DeviceSerialPort.Close();
                            }

                            _DeviceSerialPort.PortName = _ComPortInfoList[SerialPortCombo.SelectedIndex].Name;
                            _DeviceSerialPort.Open();


                            //MessageBox.Show(status.ToString());

                            if (!_DeviceSerialPort.IsOpen)
                            {
                                MessageBox.Show("Unable to open " + _ComPortInfoList[SerialPortCombo.SelectedIndex].Name, "Error", MessageBoxButtons.OK); //cysp
                                                                                                                                                          //cysp: fix the problem -- whenever COM port missing, user has to disable and enable the COM in device manager
                            }
                            else
                            {
                                ConnectButton.Text = _disconnectString;
                                SerialPortCombo.Enabled = false;
                                if (_closeOnConnect)
                                {
                                    this.Close();
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            if (Log.LogDetails != LogDetailLevel.LogEverything)
                            {
                                MessageBox.Show("Unable to open " + _ComPortInfoList[SerialPortCombo.SelectedIndex].Name + "\nPlease check if it is in use.", "Error", MessageBoxButtons.OK); //cysp
                            }
                            else
                            {
                                MessageBox.Show("Unable to open " + _ComPortInfoList[SerialPortCombo.SelectedIndex].Name + "\nReasons:\n" + err.ToString(), "Error", MessageBoxButtons.OK); //cysp
                            }

                        }
                    }
                }
                else
                {
                    try
                    {
                        _DeviceSerialPort.Close();
                    }
                    catch
                    {
                    }
                    ConnectButton.Text = _connectString;
                    SerialPortCombo.Enabled = true;
                }
                ConnectButton.Enabled = true;
            }
            catch (Exception)
            {

            }



        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_DeviceSerialPort.IsOpen == true)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            AddPorts();

            if (_DeviceSerialPort.IsOpen)
            {
                bool isPresent = false;

                for (int i = 0; i < _ComPortInfoList.Count; i++)
                {
                    if (_ComPortInfoList[i].Name == _DeviceSerialPort.PortName)
                    {
                        SerialPortCombo.SelectedIndex = i;
                        isPresent = true;
                        break;
                    }
                }

                if (isPresent)
                {
                    ConnectButton.Text = _disconnectString;
                    SerialPortCombo.Enabled = false;
                }
                else
                {
                    try
                    {
                        _DeviceSerialPort.Close();
                    }
                    catch
                    {
                    }
                }
            }

            base.OnLoad(e);
        }

        public void StopCheckingConnectionStatus()
        {
            int i;

            lock (PollThreadLock)
            {
                if (DevicePollThread != null && ContinuePoll)
                {
                    Log.PrintLog(this, "StopCheckingConnectionStatus Thread " + Thread.CurrentThread.Name + "-" + Thread.CurrentThread.ManagedThreadId.ToString(), LogDetailLevel.LogEverything);

                    for (i = 0; ((i < 0) && ThreadIsRunning > 0); i++)
                    //for (i = 0; (ThreadIsRunning > 0); i++)
                    {
                        Log.PrintLog(this, "Stopping ThreadPollDevices retry " + i.ToString() + " ThreadIsRuning = " + ThreadIsRunning.ToString(), LogDetailLevel.LogEverything);
                        ContinuePoll = false;
                        Thread.Sleep(100);
                    }

                    if (i >= 20)
                    {
                        /* Sometimes DevicePollThread can't be scheduled (One case, it is blocked in SerialPort Write/Read) even wait about 3 seconds here */
                        Log.PrintLog(this, "Failed to Stop ThreadPollDevices " + " ThreadIsRuning: " + ThreadIsRunning.ToString(), LogDetailLevel.LogRelevant);
                    }

                    Log.PrintLog(this, "Abort ThreadPollDevices --> " + " ThreadIsRuning: " + ThreadIsRunning.ToString(), LogDetailLevel.LogEverything);

                    DevicePollThread.Abort();

                    DevicePollThread = null;
                }
            }
        }

        public void StartCheckingConnectionStatus()
        {
            lock (PollThreadLock)
            {
                if (_DeviceSerialPort.IsOpen == true && DevicePollThread == null && SerialPortType != PortType.NoType)
                {
                    if (SerialPortType == PortType.Host)
                    {
                        PrevHostConnStatus = "";
                        CurHostConnStatus = "";
                        if (CheckDUTPresence == true)
                        {
                            PrevDUTConnStatus = "";
                            CurDUTConnStatus = "";
                        }
                    }
                    if ((SerialPortType == PortType.DUT) && (CheckDUTPresence))
                    {
                        PrevDUTConnStatus = "";
                        CurDUTConnStatus = "";
                    }

                    if (PollingDevInThread)
                    {
                        Log.PrintLog(this, "StartCheckingConnectionStatus: Start Polling Thread", LogDetailLevel.LogEverything);
                        ContinuePoll = true;
                        ThreadIsRunning = 0;
                        DevicePollThread = new Thread(() => PollDevices());
                        DevicePollThread.Name = "DevicePollThread " + ToString();
                        DevicePollThread.Start();
                    }
                    else
                    {
                        Log.PrintLog(this, "StartCheckingConnectionStatus Poll Device Once", LogDetailLevel.LogEverything);
                        VerifyDeviceType();
                    }
                }
            }
        }

        private void PollDevices()
        {
            Log.PrintLog(this, "ThreadPollDevices Started. ContinuePoll = " + ContinuePoll.ToString(), LogDetailLevel.LogEverything);

            ThreadIsRunning = 1;

            while (ContinuePoll)
            {
                Log.PrintLog(this, "ThreadPollDevices Polling..", LogDetailLevel.LogEverything);

                ThreadIsRunning = 2;

                try
                {
                    Thread.Sleep(PollInterval);
                    if (!VerifyDeviceType())
                    {
                        ContinuePoll = false;
                        ThreadIsRunning = 3;
                        continue;
                    }
                    ThreadIsRunning = 4;
                }
                catch
                {
                    ContinuePoll = false;
                    ThreadIsRunning = 0;
                }




            }

            ThreadIsRunning = 0;
            Log.PrintLog(this, "ThreadPollDevices Exit", LogDetailLevel.LogEverything);
        }

        private bool VerifyDeviceType()
        {
            //cysp: so far nothing verified
            return true;
        }
    }
}
