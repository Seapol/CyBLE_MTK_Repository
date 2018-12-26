using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{
    public enum PortType { NoType, Host, DUT, Anritsu };
    public enum OpenPortStatus { NotOpen, Open, AlreadyInUse, NotCorrectType };
    public enum ClosePortStatus { NotClosed, Closed };
    public enum ExtPortType { NoType, Host, Dut, Host706 };

    public class CyBLEMTKSerialPort : SerialPort
    {


        public LogManager Log;
        public ExtPortType ExType = ExtPortType.NoType;
        byte[] BufferedBDAddress = null;
        public int PortNum = 0;
        static int NewCnt = 0;
        static int OpenCnt = 0;
        static int CloseCnt = 0;
        public string SN;



        //private List<COMPortInfo> _comPortInfoList;
        public List<COMPortInfo> ComPortList
        {
            get
            {
                //_comPortInfoList = COMPortInfo.GetCOMPortsInfo();
                //return _comPortInfoList;
                return COMPortInfo.GetCOMPortsInfo();
            }
        }

        private SerialPort _deviceSerialPort;
        public SerialPort DeviceSerialPort
        {
            get { return _deviceSerialPort; }
            set
            {
                _deviceSerialPort = value;
                foreach (COMPortInfo COMPort in ComPortList)
                {
                    if (COMPort.Name == _deviceSerialPort.PortName)
                    {
                        _portMSGName = COMPort.Name + " - " + COMPort.Description;
                        break;
                    }
                }
            }
        }

        private PortType _serialPortType;
        public PortType SerialPortType
        {
            get { return _serialPortType; }
            set { _serialPortType = value; }
        }

        private bool _checkDUTPresence;
        public bool CheckDUTPresence
        {
            get { return _checkDUTPresence; }
            set { _checkDUTPresence = value; }
        }

        private bool _portValidation;
        public bool ValidatePort
        {
            get { return _portValidation; }
            set { _portValidation = value; }
        }

        private bool _connectionTime;
        private bool _continuePoll;
        private Thread DevicePollThread;
        private string CommandResult, PrevDUTConnStatus, PrevHostConnStatus, CurHostConnStatus, CurDUTConnStatus;
        private int _pollInterval;
        private string _portMSGName;

        public event ConnectionEventHandler OnDUTConnectionStatusChange;
        public event ConnectionEventHandler OnHostConnectionStatusChange;

        public delegate void ConnectionEventHandler(string ConnectionStatus);
        
        public CyBLEMTKSerialPort()
        {
            Log = new LogManager();
            DeviceSerialPort = new SerialPort();
            DeviceSerialPort.BaudRate = 115200;
            DeviceSerialPort.DataBits = 8;
            DeviceSerialPort.Parity = Parity.None;
            DeviceSerialPort.StopBits = StopBits.One;
            DeviceSerialPort.Handshake = Handshake.None;
            SerialPortType = PortType.NoType;
            CheckDUTPresence = false;
            _connectionTime = false;
            _continuePoll = false;
            _pollInterval = 1000;
            _portMSGName = "";
            _portValidation = false;
        }

        public CyBLEMTKSerialPort(LogManager Logger) : this()
        {
            Log = Logger;
        }

        public void StopCheckingConnectionStatus()
        {
            _continuePoll = false;
            if (DevicePollThread != null)
            {
                DevicePollThread.Abort();
            }
        }

        public void StartCheckingConnectionStatus()
        {
            if (_deviceSerialPort.IsOpen == true)
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
                _continuePoll = true;
                DevicePollThread = new Thread(() => PollDevices());
                DevicePollThread.Start();
            }
        }

        private bool SendCommand(string Command, int WaitTimeMS)
        {
            char[] DelimiterChars = { '\n', '\r' };

            this.CommandResult = "";
            try
            {
                _deviceSerialPort.WriteLine(Command);
                Thread.Sleep(WaitTimeMS);
                string OuputACKNAC = _deviceSerialPort.ReadExisting();
                string[] Output = OuputACKNAC.Split(DelimiterChars);

                for (int i = 0; i < Output.Count(); i++)
                {
                    if ((Output[i] != "") && (Output[i] != "ACK") && (Output[i] != "NAC"))
                    {
                        this.CommandResult = Output[i];
                        break;
                    }
                }

                if (Output[0] == "ACK")
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private bool VerifyDeviceType()
        {
            if (SerialPortType == PortType.Host)
            {
                PrevHostConnStatus = CurHostConnStatus;
                SendCommand("WHO", 20);
                if (CommandResult != "HOST")
                {
                    CurHostConnStatus = "DISCONNECTED";
                    if (CurHostConnStatus != PrevHostConnStatus)
                    {
                        if (OnHostConnectionStatusChange != null)
                        {
                            OnHostConnectionStatusChange("DISCONNECTED");
                        }
                        if (CheckDUTPresence == true)
                        {
                            if (OnDUTConnectionStatusChange != null)
                            {
                                OnDUTConnectionStatusChange("DISCONNECTED");
                            }
                        }
                    }
                    return false;
                }
                CurHostConnStatus = "CONNECTED";
                if (CurHostConnStatus != PrevHostConnStatus)
                {
                    if (OnHostConnectionStatusChange != null)
                    {
                        OnHostConnectionStatusChange("CONNECTED");
                    }
                }
                if (CheckDUTPresence == true)
                {
                    PrevDUTConnStatus = CommandResult;
                    SendCommand("PCS", 200);
                    if (PrevDUTConnStatus != CommandResult)
                    {
                        if (OnDUTConnectionStatusChange != null)
                        {
                            OnDUTConnectionStatusChange(CommandResult);
                        }
                    }
                }
            }
            else if ((SerialPortType == PortType.DUT) && (CheckDUTPresence))
            {
                PrevDUTConnStatus = CurDUTConnStatus;

                try
                {
                    _deviceSerialPort.WriteLine("AB");
                    Thread.Sleep(100);
                    string OuputACKNAC = _deviceSerialPort.ReadExisting();
                    if (!OuputACKNAC.Contains("AB"))
                    {
                        //SendCommand("WHO", 20);
                        _deviceSerialPort.WriteLine("\n");
                    }
                }
                catch
                {
                }

                SendCommand("WHO", 20);
                if (CommandResult != "DUT")
                {
                    CurDUTConnStatus = "DISCONNECTED";
                    if (CurDUTConnStatus != PrevDUTConnStatus)
                    {
                        if (OnDUTConnectionStatusChange != null)
                        {
                            OnDUTConnectionStatusChange("DISCONNECTED");
                        }
                    }
                    return false;
                }
                CurDUTConnStatus = "CONNECTED";
                if (CurDUTConnStatus != PrevDUTConnStatus)
                {
                    if (OnDUTConnectionStatusChange != null)
                    {
                        OnDUTConnectionStatusChange("CONNECTED");
                    }
                }
            }
            else if (SerialPortType == PortType.Anritsu)
            {
                char[] DelimiterChars = { ',' };

                if (_connectionTime)
                {
                    _deviceSerialPort.WriteLine("*RST");
                    Thread.Sleep(3000);
                }

                _deviceSerialPort.WriteLine("*IDN?");
                Thread.Sleep(100);
                string OuputACKNAC = _deviceSerialPort.ReadExisting();
                string[] Output = OuputACKNAC.Split(DelimiterChars);

                if (Output.Count() >= 4)
                {
                    if ((Output[0] == "RANRITSU") && (Output[1] == "MT8852B"))
                    {
                        if (_connectionTime)
                        {
                            Log.PrintLog(this, "Device Found.", LogDetailLevel.LogRelevant);
                            Log.PrintLog(this, "Make: ANRITSU", LogDetailLevel.LogRelevant);
                            Log.PrintLog(this, "Model: " + Output[1], LogDetailLevel.LogRelevant);
                            Log.PrintLog(this, "Serial Number: " + Output[2], LogDetailLevel.LogRelevant);
                            Log.PrintLog(this, "Firmware Version: " + Output[3], LogDetailLevel.LogRelevant);
                        }
                    }
                    else
                    {
                        if (_connectionTime)
                        {
                            _deviceSerialPort.WriteLine("*RST");
                            Log.PrintLog(this, "Device unknown. Please reset the device and try again.", LogDetailLevel.LogRelevant);
                        }
                        return false;
                    }
                }
                else
                {
                    if (_connectionTime)
                    {
                        _deviceSerialPort.WriteLine("*RST");
                        Log.PrintLog(this, "Device misbehaviour. Please reset the device and try again.", LogDetailLevel.LogRelevant);
                    }
                    return false;
                }
            }

            return true;
        }

        private void PollDevices()
        {
            while (_continuePoll)
            {
                if (!VerifyDeviceType())
                {
                    _continuePoll = false;
                    return;
                }

                if ((SerialPortType == PortType.Host) && (CheckDUTPresence == true))
                {
                    PrevDUTConnStatus = CommandResult;
                    SendCommand("PCS", 200);
                    if (PrevDUTConnStatus != CommandResult)
                    {
                        if (OnDUTConnectionStatusChange != null)
                        {
                            OnDUTConnectionStatusChange(CommandResult);
                        }
                    }
                }

                Thread.Sleep(_pollInterval);
            }
        }

        public OpenPortStatus OpenPort(COMPortInfo portInfo)
        {
            OpenPortStatus returnValue = OpenPortStatus.NotOpen;

            _connectionTime = true;
            _portMSGName = portInfo.Name + " - " + portInfo.Description;
            _deviceSerialPort.PortName = portInfo.Name;
            try
            {
                _deviceSerialPort.Open();
            }
            catch
            {
                returnValue = OpenPortStatus.AlreadyInUse;
            }

            if (_deviceSerialPort.IsOpen == true)
            {
                returnValue = OpenPortStatus.Open;
                if (_portValidation)
                {
                    if (!VerifyDeviceType())
                    {
                        if (SerialPortType == PortType.Host)
                        {
                            Log.PrintLog(this, "Expecting a MTK Host device.", LogDetailLevel.LogRelevant);
                        }
                        else if (SerialPortType == PortType.DUT)
                        {
                            Log.PrintLog(this, "Expecting a DUT.", LogDetailLevel.LogRelevant);
                        }
                        else if (SerialPortType == PortType.Anritsu)
                        {
                            Log.PrintLog(this, "Expecting an Anritsu tester.", LogDetailLevel.LogRelevant);
                        }

                        _deviceSerialPort.Close();
                        returnValue = OpenPortStatus.NotCorrectType;
                    }
                    else
                    {
                        _continuePoll = true;
                        DevicePollThread = new Thread(() => PollDevices());
                        DevicePollThread.Start();
                        Log.PrintLog(this, "Connected to \"" + _portMSGName + "\".", LogDetailLevel.LogRelevant);
                    }
                }
                else
                {
                    Log.PrintLog(this, "Connected to \"" + _portMSGName + "\".", LogDetailLevel.LogRelevant);
                }
            }

            _connectionTime = false;
            return returnValue;
        }

        public bool OpenPortByName(string portName)
        {
            bool RetVal = false;

            if (portName == "")
            {
                return false;
            }

            foreach (COMPortInfo ComPort in ComPortList)
            {
                if (ComPort.Name == portName)
                {
                    if (!_deviceSerialPort.IsOpen)
                    {
                        OpenPort(ComPort);
                        RetVal = true;
                    }
                    break;
                }
            }

            return RetVal;
        }

        public ClosePortStatus ClosePort()
        {
            ClosePortStatus returnValue = ClosePortStatus.NotClosed;

            StopCheckingConnectionStatus();
            try
            {
                if (this._deviceSerialPort.IsOpen == true)
                {
                    this._deviceSerialPort.Close();

                    Log.PrintLog(this, "Disconnected from \"" + _portMSGName + "\".", LogDetailLevel.LogRelevant);
                    if (SerialPortType == PortType.Host)
                    {
                        CurHostConnStatus = "DISCONNECTED";
                        if (OnHostConnectionStatusChange != null)
                        {
                            OnHostConnectionStatusChange("DISCONNECTED");
                        }
                        if (CheckDUTPresence)
                        {
                            if (OnDUTConnectionStatusChange != null)
                            {
                                OnDUTConnectionStatusChange("DISCONNECTED");
                            }
                        }
                    }
                    else if (SerialPortType == PortType.DUT)
                    {
                        CurDUTConnStatus = "DISCONNECTED";
                        if (OnDUTConnectionStatusChange != null)
                        {
                            OnDUTConnectionStatusChange("DISCONNECTED");
                        }
                    }
                }
            }
            catch
            {
                Log.PrintLog(this, "Serial port already disconnected.", LogDetailLevel.LogRelevant);
                if (SerialPortType == PortType.Host)
                {
                    CurHostConnStatus = "DISCONNECTED";
                    if (OnHostConnectionStatusChange != null)
                    {
                        OnHostConnectionStatusChange("DISCONNECTED");
                    }
                    if (CheckDUTPresence)
                    {
                        if (OnDUTConnectionStatusChange != null)
                        {
                            OnDUTConnectionStatusChange("DISCONNECTED");
                        }
                    }
                }
                else if (SerialPortType == PortType.DUT)
                {
                    CurDUTConnStatus = "DISCONNECTED";
                    if (OnDUTConnectionStatusChange != null)
                    {
                        OnDUTConnectionStatusChange("DISCONNECTED");
                    }
                }
            }

            returnValue = ClosePortStatus.Closed;

            return returnValue;
        }


        public void OpenIt()
        {
            Log.PrintLog(this, "OpenSerialPort[" + OpenCnt.ToString() + "]", LogDetailLevel.LogRelevant);
            OpenCnt++;
            BufferedBDAddress = null;

            /*
             * Generate a number for temp BD address for TxRx test*
             */
            PortNum = 0;
            char[] Chars = PortName.ToArray();
            int FirstNumIndex = Chars.Length;
            for (int i = 0; i < Chars.Length; i++)
            {
                FirstNumIndex--;
                if (Chars[FirstNumIndex] < '0' || Chars[FirstNumIndex] > '9')
                {
                    FirstNumIndex++;
                    break;
                }
            }

            if (FirstNumIndex < Chars.Length)
            {
                try
                {
                    PortNum = Convert.ToInt32(PortName.Substring(FirstNumIndex));
                }
                catch
                {
                    PortNum = 0;
                }
            }

            if (PortNum == 0)
            {
                Log.PrintLog(this, "Failed to parse the PortNumber from PortName", LogDetailLevel.LogRelevant);
            }

            base.Open();
        }

        public void CloseIt()
        {
            Log.PrintLog(this, "CloseSerialPort[" + CloseCnt.ToString() + "]", LogDetailLevel.LogRelevant);
            CloseCnt++;
            BufferedBDAddress = null;
            base.Close();
        }

        public bool TryToOpenIt()
        {
            try
            {
                Log.PrintLog(this, "TryToOpen Cnt = " + OpenCnt.ToString() + "CloseCnt = " + CloseCnt.ToString(), LogDetailLevel.LogRelevant);
                OpenCnt = 0;
                CloseCnt = 0;
                OpenIt();
            }
            catch (Exception e)
            {
                Log.PrintLog(this, "TryToOpen Exception:" + e.ToString(), LogDetailLevel.LogRelevant);
            }

            return IsOpen;
        }

        public bool IsMTKHost()
        {
            //Don't buffer DUT BDAddress. Because software can't detect user's manul hardreset DUT.
            return (ExType == ExtPortType.Host || ExType == ExtPortType.Host706);
        }


        public override string ToString()
        {
            return "#MTK" + PortName;
        }
    }
}
