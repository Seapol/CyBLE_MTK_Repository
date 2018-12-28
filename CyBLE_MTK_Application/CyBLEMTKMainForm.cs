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
using System.Xml;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Reflection;
using System.Diagnostics;
using CypressSemiconductor.ChinaManufacturingTest;
using System.Net;
using System.Net.Sockets;
//using MTKRobot;


namespace CyBLE_MTK_Application
{
    public partial class CyBLE_MTK : Form
    {

        private LogManager Logger;
        private SerialPortSettingsDialog MTKSerialPortDialog, DUTSerialPortDialog;
        private PreferencesDialog MTKPreferences;
        private bool UpdateTestInfo;
        private string BackupWindowText;
        private ToolStripMenuItem RecentFile1;
        private ToolStripMenuItem RecentFile2;
        private ToolStripMenuItem RecentFile3;
        private ToolStripMenuItem RecentFile4;
        private ToolStripMenuItem RecentFile5;
        private int DUTInfoDataGridColumn1Width;
        private int TestProgramGridViewColumn1Width;
        private int NumDUTsBackup;// CurrentlyRunningTest; CurrentlyTestedDUT;
        private string AppStatBackup;
        private Color DefaultApplicationModeColor;
        //private MTKPSoCProgrammer MTKProgrammer;
        private MTKTestBDA MTKBDAProgrammer;
        private Thread TestThread;
        private Thread BDAProgrammingThread;
        //private Thread ProgramAllDUTsThread;
        private TestProgramManager MTKTestProgram;
        private List<MTKTestResult> TestResults;
        private List<string> DUTTestResults = new List<string>();    //cysp: string
        private int RunCount;
        //private string ProgrammerMSGBackup;
        private List<MTKPSoCProgrammer> DUTProgrammers;
        private List<SerialPort> DUTSerialPorts;
        private List<string> DUTSerialNumbers;
        private List<string> DUTSerialPortsName;
        private CyBLEMTKSerialPort DUTSelectSerialPort, AnritsuSerialPort;
        private List<MTKTestStatus>[] DUT_TestStatus;
        private CyBLEMTKSplashScreen SplashScreen;
        private System.Threading.Timer TestResultLogTimer;
        private static bool _runLock;
        private SFCS m_SFCS; //// shopfloor by pzho
        private bool[] shopfloor_permission;
        private ECCS errcode_cal = new ECCS();   //// errorcode by cysp
        private List<UInt16> errcodes = new List<UInt16>();

        private string TesterID = System.Net.Dns.GetHostName();

        public static bool[] DUTsTestFlag = {true,true,true,true, true, true, true, true };

        public static string LogWriteLine = "None";


        /// <summary>
        /// END
        /// </summary>

        private UInt16 errorcode;   //// returned errorcode by cysp
        private bool _isU2751A_allchannelclosed = false;
        
        /// </summary>
        //private string BDACurrentDUT;

        delegate void SetVoidCallback();
        delegate DialogResult SetDialogCallback();

        // To support flashing.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;


        //To Support FPY Data Statistics
        public static int COUNT_PASS;
        public static int COUNT_FAIL;
        public static int COUNT_ALL;


        //Find out the 1st row index of DUT
        public static int IndexConfiguredSerialPortfor1stRow = -1;

        //Find out the final row index of DUT
        public static int IndexConfiguredSerialPortforfinalRow = -1;



        private List<MTKTestError> ProgAllErr, ProgramStatus;

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        /// <summary>
        /// SWJShopfloorDB
        /// </summary>
        private SFCS_DB_Helper SFCS_DB;


        public CyBLE_MTK()
        {



            SplashScreen = new CyBLEMTKSplashScreen();
            SplashScreen.AppVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SplashScreen.LoadStatus = 0;
            SplashScreen.Show();
            SplashScreen.LoadMessage = "Initializing...";
            InitializeComponent();
            RunCount = 0;

            ////RESET FPY BY CYSP
            //COUNT_PASS = 0;
            //COUNT_FAIL = 0;
            //COUNT_ALL = 0;

            double[] yValues = { COUNT_PASS, COUNT_FAIL };
            string[] xValues = { "PASS", "FAIL" };




            this.ApplicationStatus.Text = "Initializing...";
            SplashScreen.LoadMessage = "Setting up logger...";
            LogDetailLevel LogLVL = PreferencesDialog.ConvertLogLevel(CyBLE_MTK_Application.Properties.Settings.Default.LogLevel);
            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoSaveAppLogs == true)
            {
                string AppLogPath = CyBLE_MTK_Application.Properties.Settings.Default.ApplicationLogPath;
                Logger = new LogManager(LogLVL, this.LogTextBox, AppLogPath);
            }
            else
            {
                Logger = new LogManager(LogLVL, this.LogTextBox);
            }
            Logger.TestResultsLogPath = CyBLE_MTK_Application.Properties.Settings.Default.TestLogPath;
            Logger.EnableTestResultLogging = CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTests;
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up MTK serial port dialog...";
            MTKSerialPortDialog = new SerialPortSettingsDialog(Logger);
            MTKSerialPortDialog.ID = 0x00;
            MTKSerialPortDialog.Text = "MTK Host Serial Port Setting";
            MTKSerialPortDialog.SerialPortType = PortType.Host;
            MTKSerialPortDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            MTKSerialPortDialog.AutoVerifyON = true;
            HostStatus.BackColor = Color.Red;
            MTKSerialPortDialog.OnDUTConnectionStatusChange += new SerialPortSettingsDialog.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
            MTKSerialPortDialog.OnHostConnectionStatusChange += new SerialPortSettingsDialog.ConnectionEventHandler(MTKSerialPortDialog_OnHostConnectionStatusChange);
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up DUT serial port dialog...";
            DUTSerialPortDialog = new SerialPortSettingsDialog(Logger);
            DUTSerialPortDialog.ID = 0x10;
            DUTSerialPortDialog.Text = "DUT Serial Port Setting";
            DUTSerialPortDialog.SerialPortType = PortType.DUT;
            DUTSerialPortDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            DUTSerialPortDialog.AutoVerifyON = true;
            DataBaseStatus.BackColor = Color.Red;
            DUTSerialPortDialog.OnDUTConnectionStatusChange += new SerialPortSettingsDialog.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
            SplashScreen.LoadStatus += 8;

            //SplashScreen.LoadMessage = "Setting up Anritsu serial port dialog...";
            //AnritsuSerialPort = new CyBLEMTKSerialPort(Logger);
            ////AnritsuSerialPortDialog.Text = "Anrtisu Serial Port Setting";
            ////AnritsuSerialPortDialog.CloseOnConnect = false;
            //AnritsuSerialPort.SerialPortType = PortType.Anritsu;
            //AnritsuSerialPort.ValidatePort = true;
            //AnritsuSerialPort.DeviceSerialPort.BaudRate = 57600;
            //AnritsuSerialPort.DeviceSerialPort.Handshake = Handshake.RequestToSend;
            //if (AnritsuSerialPort.OpenPortByName(CyBLE_MTK_Application.Properties.Settings.Default.AnritsuSerialPort))
            //{
            //    SplashScreen.LoadMessage = "Applying Anritsu settings...";
            //    SetupAnritsu();
            //}
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up DUT Mux serial port...";
            DUTSelectSerialPort = new CyBLEMTKSerialPort(Logger);
            //DUTSelectSerialPortDialog.Text = "DUT Multiplexer Serial Port Setting";
            //DUTSelectSerialPortDialog.CloseOnConnect = false;
            DUTSelectSerialPort.SerialPortType = PortType.NoType;
            DUTSelectSerialPort.ValidatePort = false;
            DUTSelectSerialPort.DeviceSerialPort.ReadTimeout = 500;
            DUTSelectSerialPort.OpenPortByName(CyBLE_MTK_Application.Properties.Settings.Default.DUTMultiplexerSerialPort);
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up MTK connection...";
            if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType != "UART")
            {
                MTKSerialPortDialog.CheckDUTPresence = true;
                DUTToolStripMenuItem.Enabled = false;
                DUTSerialPortDialog.CheckDUTPresence = false;
            }
            else if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
            {
                DUTToolStripMenuItem.Enabled = true;
                DUTSerialPortDialog.CheckDUTPresence = true;
            }
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Checking for PSoC Programmer...";
            MTKPSoCProgrammer MTKProgrammer = new MTKPSoCProgrammer(Logger);
            if (MTKProgrammer.PSoCProgrammerInstalled == false)
            {
                MessageBox.Show("PSoC Programmer not installed. All programming functionalities will be " +
                "disabled. Please install PSoC Programmer 3.25 and relaunch CYBLE MTK Application.",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.PrintLog(this, "PSoC Programmer not installed. All programming functionalities will be " +
                "disabled. Please install PSoC Programmer 3.25 and relaunch CYBLE MTK Application."
                , LogDetailLevel.LogRelevant);
                //ProgrammingInterfaceGroupBox.Enabled = false;
                BDAWriteButton.Enabled = false;
            }

            if ((MTKProgrammer.PSoCProgrammerInstalled == true) && (MTKProgrammer.IsCorrectVersion() == false))
            {
                MessageBox.Show("Incorrect PSoC Programmer version detected. All programming functionalities" +
                " will be disabled. Please install PSoC Programmer 3.25 and relaunch CYBLE " +
                        "MTK Application.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.PrintLog(this, "Incorrect PSoC Programmer version detected. All programming functionalities" +
                " will be disabled. Please install PSoC Programmer 3.25 and relaunch CYBLE " +
                        "MTK Application.", LogDetailLevel.LogRelevant);
                //ProgrammingInterfaceGroupBox.Enabled = false;
            }
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up DUT programmers and serial ports...";
            DUTProgrammers = new List<MTKPSoCProgrammer>();
            DUTSerialPorts = new List<SerialPort>();
            for (int i = 0; i < 16; i++)
            {
                DUTProgrammers.Add(new MTKPSoCProgrammer(Logger));
                if (CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerName[i] != "Configure...")
                {
                    DUTProgrammers[i].SelectedProgrammer = CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerName[i];
                    DUTProgrammers[i].SelectedVoltageSetting = CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVoltage[i];
                    DUTProgrammers[i].SelectedAquireMode = CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPM[i];
                    DUTProgrammers[i].SelectedConnectorType = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerConn[i]);
                    DUTProgrammers[i].SelectedClock = CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerClock[i];
                    DUTProgrammers[i].StringToPA(CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPA[i]);
                    DUTProgrammers[i].ValidateAfterProgramming = bool.Parse(CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVerify[i]);
                    DUTProgrammers[i].SelectedHEXFilePath = CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerHexPath[i];
                }
                DUTSerialPorts.Add(new CyBLEMTKSerialPort(Logger));
                DUTSerialPorts[i].BaudRate = 115200;
                SplashScreen.LoadMessage = "Setting up DUT programmers and serial ports (" + (i + 1).ToString() + "/16)";
                if ((i % 2) == 0)
                {
                    SplashScreen.LoadStatus += 5;
                }
            }

            SplashScreen.LoadMessage = "Setting up BD Address prgorammer...";
            MTKBDAProgrammer = new MTKTestBDA(Logger);
            MTKBDAProgrammer.AutoIncrementBDA = CyBLE_MTK_Application.Properties.Settings.Default.BDAIncrement;
            MTKBDAProgrammer.UseProgrammer = CyBLE_MTK_Application.Properties.Settings.Default.BDAUseProgrammer;
            BDATextBox.Text = CyBLE_MTK_Application.Properties.Settings.Default.BDA;
            MTKBDAProgrammer.BDAddress = BDATextBox.ToByteArray();
            MTKBDAProgrammer.OnBDAChange += new MTKTestBDA.BDAChangeEventHandler(MTKBDAProgrammer_OnBDAChange);

            MTKBDAProgrammer.BDAProgrammer.GlobalProgrammerSelected = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerGlobal;
            if ((CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerGlobal == false) &&
                (CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerName != "Configure..."))
            {
                MTKBDAProgrammer.BDAProgrammer.SelectedProgrammer = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerName;
                MTKBDAProgrammer.BDAProgrammer.SelectedVoltageSetting = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerVoltage;
                MTKBDAProgrammer.BDAProgrammer.SelectedAquireMode = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerPM;
                MTKBDAProgrammer.BDAProgrammer.SelectedConnectorType = int.Parse(CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerConn);
                MTKBDAProgrammer.BDAProgrammer.SelectedClock = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerClock;
                MTKBDAProgrammer.BDAProgrammer.StringToPA(CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerPA);
                MTKBDAProgrammer.BDAProgrammer.ValidateAfterProgramming = bool.Parse(CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerVerify);
                MTKBDAProgrammer.BDAProgrammer.SelectedHEXFilePath = CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerHexPath;
            }
            BDAWriteButton.Enabled = false;
            BDAConfigLabel.Text = MTKBDAProgrammer.GetDisplayText();
            SplashScreen.LoadStatus += 8;

            SplashScreen.LoadMessage = "Setting up preferences dialog...";
            MTKPreferences = new PreferencesDialog(this, Logger);
            SplashScreen.LoadStatus += 4;

            //cysp: IsSetupMTKInstrumentsInTheMTKBegin
            if (CyBLE_MTK_Application.Properties.Settings.Default.IsSetupMTKInstrumentsInTheMTKBegin == true)
            {

                SplashScreen.LoadMessage = "Setting up DMM and Switch ...";
                SplashScreen.LoadStatus += 4;

                try
                {
                    //Agilent sw = new Agilent();
                    //sw.InitializeU2751A_WELLA(CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias);
                    //sw.InitializeU2751A_WELLA(CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias);
                    //sw.SetRelayWellA_ALLCLOSE();

                    MTKInstruments.ConnectInstruments();

                    if (MTKInstruments.AlldevReady)
                    {
                        _isU2751A_allchannelclosed = true;
                        Logger.PrintLog(this, "Setting up DMM and Switch Successfully ...", LogDetailLevel.LogRelevant);
                    }
                    else
                    {
                        SplashScreen.LoadMessage = "Failure in Setting up DMM and Switch ...";
                        Logger.PrintLog(this, "Failure in Setting up DMM and Switch ...", LogDetailLevel.LogRelevant);
                    }


                }
                catch
                {
                    SplashScreen.LoadMessage = "Failure in Setting up DMM and Switch ...";
                    Logger.PrintLog(this, "Failure in Setting up DMM and Switch ...", LogDetailLevel.LogRelevant);
                }

            }
            SplashScreen.LoadStatus += 4;

            SplashScreen.LoadMessage = "Setting up test manager...";
            MTKTestProgram = new TestProgramManager(Logger, MTKSerialPortDialog.DeviceSerialPort, DUTSerialPortDialog.DeviceSerialPort);
            MTKTestProgram.SupervisorMode = this.SupervisorModeMenuItem.Checked;
            MTKTestProgram.IgnoreDUT = CyBLE_MTK_Application.Properties.Settings.Default.IgnoreDUTs;
            MTKTestProgram.OnTestProgramRunError += new TestProgramManager.TestProgramRunErrorEventHandler(MTKTestProgram_OnTestProgramRunError); 
            MTKTestProgram.OnNextIteration += new TestProgramManager.TestProgramNextIterationEventHandler(MTKTestProgram_OnNextIteration);
            MTKTestProgram.OnCurrentIterationComplete += new TestProgramManager.TestProgramCurrentIterationEventHandler(MTKTestProgram_OnCurrentIterationComplete);
            MTKTestProgram.OnNextTest += new TestProgramManager.TestProgramNextTestEventHandler(MTKTestProgram_OnNextTest);
            MTKTestProgram.OnTestComplete += new TestProgramManager.TestCompleteEventHandler(MTKTestProgram_OnTestComplete);
            MTKTestProgram.OnTestError += new TestProgramManager.TestRunErrorEventHandler(MTKTestProgram_OnTestError);
            MTKTestProgram.OnOverallFail += new TestProgramManager.TestRunFailEventHandler(MTKTestProgram_OnOverallFail);
            MTKTestProgram.OnTestPaused += new TestProgramManager.TestProgramPausedEventHandler(MTKTestProgram_OnTestPaused);       //cysp: manual
            MTKTestProgram.OnOverallPass += new TestProgramManager.TestRunPassEventHandler(MTKTestProgram_OnOverallPass);
            MTKTestProgram.OnTestStopped += new TestProgramManager.TestProgramStoppedEventHandler(MTKTestProgram_OnTestStopped);
            MTKTestProgram.DUTConnectionType = CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType;
            MTKTestProgram.OnDUTPortOpen += new TestProgramManager.SerialPortEventHandler(MTKTestProgram_OnDUTPortOpen);
            MTKTestProgram.OnMTKPortOpen += new TestProgramManager.SerialPortEventHandler(MTKTestProgram_OnMTKPortOpen);
            MTKTestProgram.OnAnritsuPortOpen += new TestProgramManager.SerialPortEventHandler(MTKTestProgram_OnAnritsuPortOpen);
            MTKTestProgram.OnIgnoreDUT += new TestProgramManager.IgnoreDUTEventHandler(MTKTestProgram_OnIgnoreDUT);
            
            SplashScreen.LoadStatus += 8;

            //SplashScreen.LoadMessage = "Initializing...";
            PopulateTestInfo(MTKTestProgram.TestProgram);

            TestProgramGridView.DragDrop += new DragEventHandler(TestProgramGridView_DragDrop);
            TestProgramGridView.DragEnter += new DragEventHandler(TestProgramGridView_DragEnter);

            UpdateTestInfo = false;

            DefaultApplicationModeColor = ApplicationMode.BackColor;
            BackupWindowText = this.Text;
            this.Text = BackupWindowText;

            InitializeRecentlyOpened();

            TestRunStopButton.TextChanged += new EventHandler(TestRunStopButton_TextChanged);
            TestProgramGridView.MouseDoubleClick += new MouseEventHandler(TestProgramGridView_MouseDoubleClick);
            TestResults = new List<MTKTestResult>();

            DUTSerialPortDialog.DeviceSerialPort = DUTSerialPorts[0];

            this.ApplicationStatus.Text = "Idle";
            AppStatBackup = this.ApplicationStatus.Text;
            //MainSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(splitContainer1_SplitterMoved);

            DUT_TestStatus = new List<MTKTestStatus>[16];

            TestResultLogTimer = new System.Threading.Timer(SaveTestResults);
            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting.ToLower() == "day")
            {
                LogResultsTimerSetup();
            }

            SplashScreen.LoadStatus = 100;

            //cysp display SFCSInterface onto CyBLEMTKMainForm

            if (CyBLE_MTK_Application.Properties.Settings.Default.GeneralLinkSqlLocalDebug)
            {
                this.label_appmode.Text = CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface.ToUpperInvariant() + "_Dbg";
                this.label_appmode.BackColor = Color.Red;
            }
            else
            {
                this.label_appmode.Text = CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface.ToUpperInvariant();
            }
            

            //Init MTKRobotServer parameters
            CyBLEMTKRobotServer.gNumDuts = (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs;
            if (CyBLE_MTK_Application.Properties.Settings.Default.RobotServerPort > 255)
                CyBLEMTKRobotServer.gServerPort = CyBLE_MTK_Application.Properties.Settings.Default.RobotServerPort;
            if (CyBLE_MTK_Application.Properties.Settings.Default.RobotServerDbg)
                CyBLEMTKRobotServer.gDebugLevel = LogDetailLevel.LogRelevant;
            if (CyBLE_MTK_Application.Properties.Settings.Default.RobotAckMsgTimeoutSeconds > 0)
                CyBLEMTKRobotServer.gAckMsgTimeoutMsec = CyBLE_MTK_Application.Properties.Settings.Default.RobotAckMsgTimeoutSeconds * 1000;




            if (_isU2751A_allchannelclosed)
            {
                Logger.PrintLog(this, "All channels of U2751A are closed", LogDetailLevel.LogRelevant);
            }

            Logger.PrintLog(this, "Initialization complete", LogDetailLevel.LogRelevant);
            SplashScreen.LoadMessage = "Initialization Complete";


            ///Shopfloor Access Database
            ///

            if (CyBLE_MTK_Application.Properties.Settings.Default.ShopfloorDataBaseEnable)
            {

                DataBaseStatus.ForeColor = Color.Black;
                DataBaseStatus.BackColor = Color.Green;
            }


        }

        /// <summary>
        /// cysp: To find
        /// </summary>
        /// <param name="CurrentDUT"></param>
        /// <returns></returns>
        public void RedirectTheFirstFinalRowIndexfromDUTInfoDataGridView()
        {
            IndexConfiguredSerialPortfor1stRow = -1;
            IndexConfiguredSerialPortforfinalRow = -1;

            for (int i = 0; i < CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
            {
                if (DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString() != "Configure...")
                {
                    IndexConfiguredSerialPortfor1stRow = i;
                    break;
                }
            }

            for (int i = ((int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs - 1); i >= 0; i--)
            {
                if (DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString() != "Configure...")
                {
                    IndexConfiguredSerialPortforfinalRow = i;
                    break;
                }
            }

        }



        private void LogResultsTimerSetup()
        {
            // Figure how much time until 4:00
            DateTime now = DateTime.Now;
            DateTime LoggingTime = CyBLE_MTK_Application.Properties.Settings.Default.NewLogTime;

            // If it's already past 4:00, wait until 4:00 tomorrow    
            if (now > LoggingTime)
            {
                LoggingTime = LoggingTime.AddDays((now - LoggingTime).Days + 1);
            }

            TimeSpan FirstLogTime = LoggingTime - now;
            TimeSpan LogIntervalThereafter = new TimeSpan(24, 0, 0);
            TestResultLogTimer.Change(FirstLogTime, LogIntervalThereafter);

            RunCount = 0;

            string ResultLogFileName = Logger.GenerateTestResultsFileName("");
            Logger.OpenTestLogFile(ResultLogFileName);
        }

        private void SaveTestResults(object obj)
        {
            //if (TestResults.Count > 0)
            //{
            //    if (Logger.IsTestLogFileOpen == true)
            //    {
            //        Logger.WriteTestLog(TestResults);
            //        Logger.CloseTestLogFile();
            //    }
            //    else
            //    {
            //        string ResultLogFileName = Logger.GenerateTestResultsFileName("");
            //        Logger.LogTestResults(ResultLogFileName, TestResults);
            //    }
            //    TestResults.Clear();
            //}
            while (_runLock == true)
            {
                Thread.Sleep(100);
            }
            _runLock = true;

            string LogFileName = Logger.GenerateTestResultsFileName("");
            Logger.OpenTestLogFile(LogFileName);

            RunCount = 0;
            _runLock = false;
        }

        //private void MTKProgrammer_OnTestStatusUpdate(MTKTestMessageType Type, string Message)
        //{
        //    char[] DelimiterChars = { '/' };

        //    if ((Message == "Programming...") || (Message == "Verifying..."))
        //    {
        //        ProgrammerMSGBackup = Message;
        //    }

        //    string[] Output = Message.Split(DelimiterChars);

        //    if (Output.Count() == 2)
        //    {
        //        if ((Int32.Parse(Output[0]) <= 1024) && (Int32.Parse(Output[1]) == 1024))
        //        {
        //            this.Invoke(new MethodInvoker(() => ApplicationStatus.Text = ProgrammerMSGBackup + Message));
        //        }
        //        else
        //        {
        //            //ProgrammerMSGBackup = "";
        //            this.Invoke(new MethodInvoker(() => ApplicationStatus.Text = Message));
        //        }
        //    }
        //    else
        //    {
        //        //ProgrammerMSGBackup = "";
        //        this.Invoke(new MethodInvoker(() => ApplicationStatus.Text = Message));
        //    }
        //}

        /* Do the flashing - this does not involve a raincoat.*/
        public static bool FlashWindowEx(Form form)
        {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }

        private void TestProgramGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (SupervisorModeMenuItem.Checked == true)
            {
                if (MTKTestProgram.TestProgramStatus == TestProgramState.Stopped)
                {
                    for (int i = 0; i < TestProgramGridView.Rows.Count; i++)
                    {
                        if (TestProgramGridView.Rows[i].Cells[1].Selected == true)
                        {
                            EditTestProgram(i);
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Stop test program before editing it.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("You have to be in supervisor mode to edit a test program.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeRecentlyOpened()
        {
            RecentFile1 = new ToolStripMenuItem("Empty");
            RecentFile1.Click += new EventHandler(AnyRecentFileMenuItem_Click);
            RecentFile1.Enabled = false;
            RecentFile2 = new ToolStripMenuItem("Empty");
            RecentFile2.Click += new EventHandler(AnyRecentFileMenuItem_Click);
            RecentFile2.Enabled = false;
            RecentFile3 = new ToolStripMenuItem("Empty");
            RecentFile3.Click += new EventHandler(AnyRecentFileMenuItem_Click);
            RecentFile3.Enabled = false;
            RecentFile4 = new ToolStripMenuItem("Empty");
            RecentFile4.Click += new EventHandler(AnyRecentFileMenuItem_Click);
            RecentFile4.Enabled = false;
            RecentFile5 = new ToolStripMenuItem("Empty");
            RecentFile5.Click += new EventHandler(AnyRecentFileMenuItem_Click);
            RecentFile5.Enabled = false;

            if (CyBLE_MTK_Application.Properties.Settings.Default.RecentPath1 == "Empty")
            {
                RecentMenuItem.DropDownItems.Add(RecentFile1);
                return;
            }

            RecentFile1.Text = CyBLE_MTK_Application.Properties.Settings.Default.RecentPath1;
            RecentFile1.ToolTipText = Path.GetFileName(RecentFile1.Text);
            RecentFile1.Enabled = true;
            RecentMenuItem.DropDownItems.Add(RecentFile1);
            if (CyBLE_MTK_Application.Properties.Settings.Default.RecentPath2 == "Empty")
            {
                return;
            }
            RecentFile2.Text = CyBLE_MTK_Application.Properties.Settings.Default.RecentPath2;
            RecentFile2.ToolTipText = Path.GetFileName(RecentFile2.Text);
            RecentFile2.Enabled = true;
            RecentMenuItem.DropDownItems.Add(RecentFile2);
            if (CyBLE_MTK_Application.Properties.Settings.Default.RecentPath3 == "Empty")
            {
                return;
            }
            RecentFile3.Text = CyBLE_MTK_Application.Properties.Settings.Default.RecentPath3;
            RecentFile3.ToolTipText = Path.GetFileName(RecentFile3.Text);
            RecentFile3.Enabled = true;
            RecentMenuItem.DropDownItems.Add(RecentFile3);
            if (CyBLE_MTK_Application.Properties.Settings.Default.RecentPath4 == "Empty")
            {
                return;
            }
            RecentFile4.Text = CyBLE_MTK_Application.Properties.Settings.Default.RecentPath4;
            RecentFile4.ToolTipText = Path.GetFileName(RecentFile4.Text);
            RecentFile4.Enabled = true;
            RecentMenuItem.DropDownItems.Add(RecentFile4);
            if (CyBLE_MTK_Application.Properties.Settings.Default.RecentPath5 == "Empty")
            {
                return;
            }
            RecentFile5.Text = CyBLE_MTK_Application.Properties.Settings.Default.RecentPath5;
            RecentFile5.ToolTipText = Path.GetFileName(RecentFile5.Text);
            RecentFile5.Enabled = true;
            RecentMenuItem.DropDownItems.Add(RecentFile5);
        }

        private void UpdateRecentlyOpened()
        {
            RecentMenuItem.DropDownItems.Clear();
            if (RecentFile1.Text == "Empty")
            {
                RecentFile1.Enabled = false;
                RecentFile1.ToolTipText = "";
                RecentMenuItem.DropDownItems.Add(RecentFile1);
                return;
            }
            else
            {
                RecentFile1.Enabled = true;
                RecentFile1.ToolTipText = Path.GetFileName(RecentFile1.Text);
                RecentMenuItem.DropDownItems.Add(RecentFile1);
            }

            if (RecentFile2.Text != "Empty")
            {
                RecentFile2.Enabled = true;
                RecentFile2.ToolTipText = Path.GetFileName(RecentFile2.Text);
                RecentMenuItem.DropDownItems.Add(RecentFile2);
            }

            if (RecentFile3.Text != "Empty")
            {
                RecentFile3.Enabled = true;
                RecentFile3.ToolTipText = Path.GetFileName(RecentFile3.Text);
                RecentMenuItem.DropDownItems.Add(RecentFile3);
            }

            if (RecentFile4.Text != "Empty")
            {
                RecentFile4.Enabled = true;
                RecentFile4.ToolTipText = Path.GetFileName(RecentFile4.Text);
                RecentMenuItem.DropDownItems.Add(RecentFile4);
            }

            if (RecentFile5.Text != "Empty")
            {
                RecentFile5.Enabled = true;
                RecentFile5.ToolTipText = Path.GetFileName(RecentFile5.Text);
                RecentMenuItem.DropDownItems.Add(RecentFile5);
            }
        }

        private void PushRecentlyOpened()
        {
            if (MTKTestProgram.FullFileName == RecentFile1.Text)
            {
                return;
            }
            else if (MTKTestProgram.FullFileName == RecentFile2.Text)
            {
                string temp = RecentFile2.Text;
                RecentFile2.Text = RecentFile1.Text;
                RecentFile1.Text = temp;
            }
            else if (MTKTestProgram.FullFileName == RecentFile3.Text)
            {
                string temp = RecentFile3.Text;
                RecentFile3.Text = RecentFile2.Text;
                RecentFile2.Text = RecentFile1.Text;
                RecentFile1.Text = temp;
            }
            else if (MTKTestProgram.FullFileName == RecentFile4.Text)
            {
                string temp = RecentFile4.Text;
                RecentFile4.Text = RecentFile3.Text;
                RecentFile3.Text = RecentFile2.Text;
                RecentFile2.Text = RecentFile1.Text;
                RecentFile1.Text = temp;
            }
            else if (MTKTestProgram.FullFileName == RecentFile5.Text)
            {
                string temp = RecentFile5.Text;
                RecentFile5.Text = RecentFile4.Text;
                RecentFile4.Text = RecentFile3.Text;
                RecentFile3.Text = RecentFile2.Text;
                RecentFile2.Text = RecentFile1.Text;
                RecentFile1.Text = temp;
            }
            else
            {
                RecentFile5.Text = RecentFile4.Text;
                RecentFile4.Text = RecentFile3.Text;
                RecentFile3.Text = RecentFile2.Text;
                RecentFile2.Text = RecentFile1.Text;
                RecentFile1.Text = MTKTestProgram.FullFileName;
            }
            UpdateRecentlyOpened();
        }

        private void InitializeDUTInfo()
        {
            DataTable myTable;
            DataColumn colItem1, colItem2, colItem5;
            DataGridViewDisableButtonColumn colItem3, colItem4;
            DataRow NewRow;
            DataView myView;

            // DataTable to hold data that is displayed in DataGrid
            myTable = new DataTable("myTable");

            // the three columns in the table
            colItem1 = new DataColumn("DUT #", Type.GetType("System.Int32"));
            colItem2 = new DataColumn("Unique ID", Type.GetType("System.String"));
            colItem5 = new DataColumn("Status", Type.GetType("System.String"));

            colItem3 = new DataGridViewDisableButtonColumn();
            //colItem3.UseColumnTextForButtonValue = true;
            //colItem3.Text = "Configure...";
            colItem3.Name = "Serial Port";
            colItem4 = new DataGridViewDisableButtonColumn();
            //colItem4.UseColumnTextForButtonValue = true;
            //colItem4.Text = "Configure...";
            colItem4.Name = "DUT Programmer";

            // add the columns to the table
            myTable.Columns.Add(colItem1);
            myTable.Columns.Add(colItem2);
            myTable.Columns.Add(colItem5);

            // Fill in some data
            for (int i = 1; i <= (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
            {
                NewRow = myTable.NewRow();
                NewRow[0] = i;
                NewRow[1] = "";
                NewRow[2] = "Queued";
                myTable.Rows.Add(NewRow);
            }

            // DataView for the DataGridView
            myView = new DataView(myTable);
            myView.AllowDelete = false;
            myView.AllowEdit = true;
            myView.AllowNew = false;

            // Assign DataView to DataGrid
            DUTInfoDataGridView.DataSource = myView;
            DataGridViewColumn column = DUTInfoDataGridView.Columns[0];
            //column.Resizable = DataGridViewTriState.False;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.ReadOnly = true;
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            DUTInfoDataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DUTInfoDataGridView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DUTInfoDataGridView.Columns[2].ReadOnly = true;
            DUTInfoDataGridView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            DUTInfoDataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DUTInfoDataGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DUTInfoDataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (DUTInfoDataGridView.Columns.Count > 3)
            {
                DUTInfoDataGridView.Columns.Remove("Serial Port");
                DUTInfoDataGridView.Columns.Remove("DUT Programmer");
            }

            //if ((int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs > 1)
            {
                DUTInfoDataGridView.Columns.Add(colItem4);
                DUTInfoDataGridView.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DUTInfoDataGridView.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

                DUTInfoDataGridView.Columns.Add(colItem3);
                DUTInfoDataGridView.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DUTInfoDataGridView.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

                DUTInfoDataGridView.CellClick -= new DataGridViewCellEventHandler(DUTInfoDataGridView_CellClick);
                DUTInfoDataGridView.CellClick += new DataGridViewCellEventHandler(DUTInfoDataGridView_CellClick);

                for (int i = DUTToolStripMenuItem.DropDownItems.Count; i > 0; i--)
                {
                    DUTToolStripMenuItem.DropDownItems.RemoveAt(i - 1);
                }

                CyBLEMTKSerialPort TempSP = new CyBLEMTKSerialPort(Logger);
                if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
                {
                    TempSP.CheckDUTPresence = true;
                }
                //TempDialog.Text = "DUT Serial Port Setting";
                //TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                TempSP.SerialPortType = PortType.DUT;
                TempSP.ValidatePort = false;
                TempSP.OnDUTConnectionStatusChange += new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
                for (int i = 0; i < (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
                {
                    DUTInfoDataGridView["DUT Programmer", i].Value = (DUTProgrammers[i].SelectedProgrammer != "")?DUTProgrammers[i].SelectedProgrammer:"Configure...";
                    TempSP.DeviceSerialPort = DUTSerialPorts[i];
                    for (int j = 0; j < 3; j++)
                    {
                        if (TempSP.OpenPortByName(GetDUTSerialPortSettings(i)))
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    DUTInfoDataGridView["Serial Port", i].Value = (DUTSerialPorts[i].IsOpen)?DUTSerialPorts[i].PortName:"Configure...";
                    ToolStripMenuItem DUTMenuItem = new ToolStripMenuItem();
                    DUTMenuItem.Text = (i + 1).ToString() + ": " + ((DUTSerialPorts[i].IsOpen) ? DUTSerialPorts[i].PortName : "Configure...");
                    DUTMenuItem.Name = "DUT" + i.ToString() + "MenuItem";
                    DUTMenuItem.Click += new EventHandler(DUTMenuItem_Click);
                    DUTToolStripMenuItem.DropDownItems.Add(DUTMenuItem);
                }
                TempSP.StopCheckingConnectionStatus();
                TempSP.OnDUTConnectionStatusChange -= new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
                for (int i = (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i < 16; i++)
                {
                    try
                    {
                        if (DUTSerialPorts[i].IsOpen)
                        {
                            DUTSerialPorts[i].Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }

            DUTInfoDataGridColumn1Width = DUTInfoDataGridView.Columns[1].Width;
            DUTInfoDataGridView.RowHeadersDefaultCellStyle.Padding = new Padding(3);
            NumDUTsBackup = (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs;
            DUTInfoDataGridView.RowHeadersVisible = false;

            //DUTInfoDataGridView.Columns[0].Frozen = false;
            //DUTInfoDataGridView.Columns[1].Frozen = false;
            //DUTInfoDataGridView.Columns[2].Frozen = false;
            //DUTInfoDataGridView.Columns[3].Frozen = false;
            //DUTInfoDataGridView.Columns[4].Frozen = false;

            //DUTInfoDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //DUTInfoDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //DUTInfoDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //DUTInfoDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //DUTInfoDataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DUTInfoDataGridView.Refresh();
        }

        private void DUTMenuItem_Click(object sender, EventArgs e)
        {
            string temp = sender.ToString();
            char[] DelimiterChars = { ':'};
            string[] Output = temp.Split(DelimiterChars);

            int DUTIndex = Int32.Parse(Output[0]) - 1;

            CyBLEMTKSerialPort TempSP = new CyBLEMTKSerialPort(Logger);
            if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
            {
                TempSP.CheckDUTPresence = true;
            }
            TempSP.SerialPortType = PortType.DUT;
            TempSP.ValidatePort = true;
            TempSP.OnDUTConnectionStatusChange += new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
            TempSP.DeviceSerialPort = DUTSerialPorts[DUTIndex];
            SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(TempSP);
            TempDialog.Text = "DUT Serial Port Setting";
            TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            if (TempDialog.ShowDialog() == DialogResult.OK)
            {
                DUTSerialPorts[DUTIndex] = TempDialog.DeviceSerialPort;
                DUTInfoDataGridView[4, DUTIndex].Value = DUTSerialPorts[DUTIndex].PortName;
                DUTToolStripMenuItem.DropDownItems[DUTIndex].Text = (DUTIndex + 1).ToString() + ": " + DUTSerialPorts[DUTIndex].PortName;
                SaveDUTSerialPortSettings(DUTIndex, DUTSerialPorts[DUTIndex].PortName);
            }
            else
            {
                DUTInfoDataGridView[4, DUTIndex].Value = "Configure...";
                DUTToolStripMenuItem.DropDownItems[DUTIndex].Text = (DUTIndex + 1).ToString() + ": " + "Configure...";
                SaveDUTSerialPortSettings(DUTIndex, "");
            }
            TempSP.StopCheckingConnectionStatus();
            TempSP.OnDUTConnectionStatusChange -= new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
        }

        private string GetDUTSerialPortSettings(int DUTNumber)
        {
            switch (DUTNumber)
            {
                case 0:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort0;
                case 1:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort1;
                case 2:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort2;
                case 3:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort3;
                case 4:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort4;
                case 5:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort5;
                case 6:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort6;
                case 7:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort7;
                case 8:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort8;
                case 9:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort9;
                case 10:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort10;
                case 11:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort11;
                case 12:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort12;
                case 13:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort13;
                case 14:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort14;
                case 15:
                    return CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort15;
            }
            return "";
        }
        
        private void SetupDUTRelated()
        {
            InitializeDUTInfo();
        }

        private void PopulateTestInfo(List<MTKTest> TestList)
        {
            DataTable myTable;
            DataColumn colItem1, colItem2, colItem3;
            DataRow NewRow;
            DataView myView;

            // DataTable to hold data that is displayed in DataGrid
            myTable = new DataTable("myTable");
            
            // the three columns in the table
            colItem1 = new DataColumn("#", Type.GetType("System.Int32"));
            colItem2 = new DataColumn("Test", Type.GetType("System.String"));
            colItem3 = new DataColumn("Status", Type.GetType("System.String"));

            // add the columns to the table
            myTable.Columns.Add(colItem1);
            myTable.Columns.Add(colItem2);
            myTable.Columns.Add(colItem3);

            // Fill in some data
            for (int i = 0; i < TestList.Count; i++)
            {
                NewRow = myTable.NewRow();
                NewRow[0] = i + 1;
                NewRow[1] = TestList[i].GetDisplayText();
                NewRow[2] = "Queued";
                myTable.Rows.Add(NewRow);
            }

            // DataView for the DataGridView
            myView = new DataView(myTable);
            myView.AllowDelete = false;
            myView.AllowEdit = true;
            myView.AllowNew = false;

            // Assign DataView to DataGrid
            TestProgramGridView.DataSource = myView;
            TestProgramGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TestProgramGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TestProgramGridView.Columns[0].ReadOnly = true;
            TestProgramGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            TestProgramGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TestProgramGridView.Columns[1].ReadOnly = true;
            TestProgramGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            TestProgramGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TestProgramGridView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TestProgramGridView.Columns[0].Resizable = DataGridViewTriState.False;
            TestProgramGridView.Columns[2].ReadOnly = true;
            TestProgramGridView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            TestProgramGridView.RowHeadersDefaultCellStyle.Padding = new Padding(3);
            TestProgramGridView.RowHeadersVisible = false;
            TestProgramGridView.Columns[0].Frozen = false;
            TestProgramGridView.Columns[1].Frozen = false;
            TestProgramGridView.Columns[2].Frozen = false;
            TestProgramGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            TestProgramGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TestProgramGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            TestProgramGridViewColumn1Width = TestProgramGridView.Columns[1].Width;
        }

        private void DUTInfoDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if ((e.ColumnIndex == DUTInfoDataGridView.Columns["DUT Programmer"].Index) &&
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["DUT Programmer", e.RowIndex]).Enabled)
                {
                    if (!DUTProgrammers[e.RowIndex].PSoCProgrammerInstalled || !DUTProgrammers[e.RowIndex].IsCorrectVersion())
                    {
                        return;
                    }

                    MTKPSoCProgrammerDialog TempDialog = new MTKPSoCProgrammerDialog(DUTProgrammers[e.RowIndex]);
                    TempDialog.SetupForMainForm();
                    if (TempDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (DUTProgrammers[e.RowIndex].SelectedProgrammer != "")
                        {
                            DUTInfoDataGridView[e.ColumnIndex, e.RowIndex].Value = DUTProgrammers[e.RowIndex].SelectedProgrammer;
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerName[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedProgrammer;
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVoltage[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedVoltageSetting;
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPM[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedAquireMode;
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerConn[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedConnectorType.ToString();
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerClock[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedClock;
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPA[e.RowIndex] = DUTProgrammers[e.RowIndex].PAToString();
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVerify[e.RowIndex] = DUTProgrammers[e.RowIndex].ValidateAfterProgramming.ToString();
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerHexPath[e.RowIndex] = DUTProgrammers[e.RowIndex].SelectedHEXFilePath;
                        }
                        else
                        {
                            DUTInfoDataGridView[e.ColumnIndex, e.RowIndex].Value = "Configure...";
                            CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerName[e.RowIndex] = "Configure...";
                        }
                        CyBLE_MTK_Application.Properties.Settings.Default.Save();
                    }
                }
                else if ((e.ColumnIndex == DUTInfoDataGridView.Columns["Serial Port"].Index) &&
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["Serial Port", e.RowIndex]).Enabled)
                {
                    CyBLEMTKSerialPort TempSP = new CyBLEMTKSerialPort(Logger);
                    if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
                    {
                        TempSP.CheckDUTPresence = true;
                    }
                    TempSP.SerialPortType = PortType.DUT;
                    //TempDialog.AutoVerifyON = true;
                    TempSP.OnDUTConnectionStatusChange += new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
                    TempSP.DeviceSerialPort = DUTSerialPorts[e.RowIndex];

                    SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(TempSP);
                    TempDialog.Text = "DUT Serial Port Setting";
                    TempDialog.DeviceSerialPort = TempSP.DeviceSerialPort;
                    TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                    if (TempDialog.ShowDialog() == DialogResult.OK)
                    {
                        DUTSerialPorts[e.RowIndex] = TempDialog.DeviceSerialPort;
                        DUTInfoDataGridView[e.ColumnIndex, e.RowIndex].Value = DUTSerialPorts[e.RowIndex].PortName;
                        DUTToolStripMenuItem.DropDownItems[e.RowIndex].Text = (e.RowIndex + 1).ToString() + ": " + DUTSerialPorts[e.RowIndex].PortName;
                        SaveDUTSerialPortSettings(e.RowIndex, DUTSerialPorts[e.RowIndex].PortName);
                    }
                    else
                    {
                        DUTInfoDataGridView[e.ColumnIndex, e.RowIndex].Value = "Configure...";
                        DUTToolStripMenuItem.DropDownItems[e.RowIndex].Text = (e.RowIndex + 1).ToString() + ": " + "Configure...";
                        SaveDUTSerialPortSettings(e.RowIndex, "");
                    }
                    TempSP.StopCheckingConnectionStatus();
                    TempSP.OnDUTConnectionStatusChange -= new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
                }
                else
                {
                    if ((MTKTestProgram.TestProgramStatus == TestProgramState.Paused) || (MTKTestProgram.TestProgramStatus == TestProgramState.Stopped))
                    {
                        //if ((e.ColumnIndex == DUTInfoDataGridView.Columns["DUT #"].Index))
                        //{
                        //    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
                        //    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[e.RowIndex].Selected = true));
                        //}
                        UpdateTestInfoForDUT(e.RowIndex, DUT_TestStatus[e.RowIndex]);
                    }
                }
            }
        }

        //Update TestInfoForDUT in the TestProgramDataGrid
        private void UpdateTestInfoForDUT(int DUTno, List<MTKTestStatus> TestStatus)
        {
            if (TestStatus != null)
            {
                for (int j = 0; j < MTKTestProgram.TestCaseCount; j++)
                {
                    int Testno = j;

                    if (MTKTestProgram.DUTOverallSFCSErrCode[DUTno] == ECCS.ERRORCODE_DUT_NOT_TEST)
                    {
                        TestStatusUpdate(Testno, MTKTestMessageType.Information, "Ignored");
                        
                    }
                    else if (MTKTestProgram.DUTTmplSFCSErrCode[DUTno, Testno] == ECCS.ERRORCODE_ALL_PASS)
                    {
                        TestStatusUpdate(Testno, MTKTestMessageType.Success, "Pass");

                    }
                    else
                    {
                        if (MTKTestProgram.DUTTmplSFCSErrCode[DUTno, Testno] != ECCS.ERRORCODE_ALL_PASS)
                        {
                            if (MTKTestProgram.DUTTmplSFCSErrCode[DUTno, Testno] == ECCS.ERRORCODE_DUT_NOT_TEST)
                            {
                                TestStatusUpdate(Testno, MTKTestMessageType.Information, "Ignored");
                            }
                            else if (MTKTestProgram.DUTTmplSFCSErrCode[DUTno, Testno] == ECCS.ERRORCODE_ALL_PASS)
                            {
                                TestStatusUpdate(Testno, MTKTestMessageType.Success, "Pass");
                            }
                            else
                            {
                                TestStatusUpdate(Testno, MTKTestMessageType.Failure, "Fail");
                            }
                            
                        }
                    }


                }
            }
        }

        private void SaveDUTSerialPortSettings(int DUTNumber, string PortName)
        {
            switch (DUTNumber)
            {
                case 0:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort0 = PortName;
                    break;
                case 1:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort1 = PortName;
                    break;
                case 2:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort2 = PortName;
                    break;
                case 3:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort3 = PortName;
                    break;
                case 4:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort4 = PortName;
                    break;
                case 5:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort5 = PortName;
                    break;
                case 6:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort6 = PortName;
                    break;
                case 7:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort7 = PortName;
                    break;
                case 8:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort8 = PortName;
                    break;
                case 9:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort9 = PortName;
                    break;
                case 10:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort10 = PortName;
                    break;
                case 11:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort11 = PortName;
                    break;
                case 12:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort12 = PortName;
                    break;
                case 13:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort13 = PortName;
                    break;
                case 14:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort14 = PortName;
                    break;
                case 15:
                    CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialPort15 = PortName;
                    break;
            }
            CyBLE_MTK_Application.Properties.Settings.Default.Save();
        }

        private void MTKHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MTKSerialPortDialog.Text = "MTK Serial Port Setting";
            MTKSerialPortDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            MTKSerialPortDialog.ShowDialog();
            if (MTKSerialPortDialog.DeviceSerialPort.IsOpen)
            {
                CyBLE_MTK_Application.Properties.Settings.Default.MTKHostSerialPort = MTKSerialPortDialog.DeviceSerialPort.PortName;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
            }
        }

        private void SupervisoryMode(bool SupervisoryModeEnabled)
        {
            bool ReturnValue = true;

            MTKTestProgram.SupervisorMode = SupervisoryModeEnabled;
            SupervisorModeMenuItem.Checked = SupervisoryModeEnabled;
            PreferencesMenuItem.Enabled = SupervisoryModeEnabled;
            NewTestProgramMenuItem.Enabled = SupervisoryModeEnabled;
            importApplicationSettingsToolStripMenuItem.Enabled = SupervisoryModeEnabled;
            exportApplicationSettingsToolStripMenuItem.Enabled = SupervisoryModeEnabled;

            if (MTKTestProgram.IsFileLoaded)
            {
                TestProgramMenuItem.Enabled = SupervisoryModeEnabled;
                SaveTestProgramAsMenuItem.Enabled = SupervisoryModeEnabled;
                SaveTestMenuItem.Enabled = SupervisoryModeEnabled;
            }

            if (SupervisoryModeEnabled == true)
            {
                if ((MTKTestProgram.TestProgramStatus != TestProgramState.Paused) && (MTKTestProgram.TestProgramStatus != TestProgramState.Stopped))
                {
                    PreferencesMenuItem.Enabled = false;
                    TestProgramMenuItem.Enabled = false;
                    SaveTestMenuItem.Enabled = false;
                    SaveTestProgramAsMenuItem.Enabled = false;
                    NewTestProgramMenuItem.Enabled = false;
                }
                else if (MTKTestProgram.TestProgramStatus == TestProgramState.Paused)
                {
                    TestProgramMenuItem.Enabled = false;
                    NewTestProgramMenuItem.Enabled = false; 
                }
                ApplicationMode.Text = "Supervisor";
                ApplicationMode.BackColor = Color.Red;
            }
            else
            {
                if ((MTKTestProgram.IsFileSaved == false) && (MTKTestProgram.IsFileLoaded))
                {
                    if (MessageBox.Show(MTKTestProgram.TestFileName + " - not saved. Do you want to save this file before" +
                        " exiting Supervisor mode?", "Information", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        ReturnValue = SaveTest(false);
                    }
                    else
                    {
                        DialogResult retValue;
                        CloseTestProgram(out retValue);
                    }
                }
                ApplicationMode.Text = "Tester";
                ApplicationMode.BackColor = DefaultApplicationModeColor;
            }

            if (ReturnValue == false)
            {
                SupervisoryMode(true);
            }
        }

        private void BackupAndApplyAppStatus(string AppStatus)
        {
            this.Invoke(new MethodInvoker(() => AppStatBackup = this.ApplicationStatus.Text));
            this.Invoke(new MethodInvoker(() => this.ApplicationStatus.Text = AppStatus));
        }

        private void RestoreAppStatus()
        {
            this.Invoke(new MethodInvoker(() => this.ApplicationStatus.Text = AppStatBackup));
        }

        private void SupervisorModeMenuItem_Click(object sender, EventArgs e)
        {

            if (SupervisorModeMenuItem.Checked == false)
            {
                Logger.PrintLog(this, "Authenticating...", LogDetailLevel.LogRelevant);
                BackupAndApplyAppStatus("Authenticating...");

                AuthDialog PassowordDialog = new AuthDialog(Logger);

                PassowordDialog.ShowDialog();
                if (PassowordDialog.Authorized == true)
                {
                    SupervisoryMode(true);
                    Logger.PrintLog(this, "Supervisory mode entered.", LogDetailLevel.LogRelevant);
                }
                RestoreAppStatus();
            }
            else
            {
                SupervisoryMode(false);
                Logger.PrintLog(this, "Supervisory mode exited.", LogDetailLevel.LogRelevant);
            }

        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
            System.Environment.Exit(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            SplashScreen.LoadMessage = "Updating controls...";
            SetupDUTRelated();
            SplashScreen.LoadStatus = 100;
            //this.ConfigureProgrammerButton.Select();

            SplashScreen.Close();
            SplashScreen = null;
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            Logger.PrintLog(this, "Cleaning Up...", LogDetailLevel.LogRelevant);

            if (CyBLEMTKRobotServer.gServer != null)
                CyBLEMTKRobotServer.gServer.Stop();

            DialogResult retValue;
            CloseTestProgram(out retValue);

            if (retValue == DialogResult.Cancel)
            {
                e.Cancel = true;
                base.OnFormClosing(e);
                return;
            }

            if ((CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "session") && (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTests == true))
            {
                string ResultLogFileName = Logger.GenerateTestResultsFileName("Test_Results");
                Logger.LogTestResults(ResultLogFileName, TestResults);
                TestResults.Clear();
            }

            if (TestResults.Count > 0)
            {
                DialogResult result = MessageBox.Show("There are unsaved test results, do you wish to save them?", "Test Results", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    string ResultLogFileName = Logger.GenerateTestResultsFileName("Test_Results");
                    Logger.LogTestResults(ResultLogFileName, TestResults);
                }
                else
                {
                    TestResults.Clear();
                }

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    base.OnFormClosing(e);
                    return;
                }
            }

            Logger.PrintLog(this, "Cleaning Up...", LogDetailLevel.LogRelevant);
            if (TestThread != null)
            {
                if (MTKTestProgram.TestProgramStatus != TestProgramState.Stopped)
                {
                    StopTests();
                }
                TestThread.Abort();
            }

            try
            {
                MTKSerialPortDialog.DeviceSerialPort.Close();
                DUTSerialPortDialog.DeviceSerialPort.Close();
                AnritsuSerialPort.ClosePort();
                DUTSelectSerialPort.ClosePort();
            }
            catch
            {
            }

            for (int i = 0; i < 16; i++)
            {
                try
                {
                    if (DUTSerialPorts[i].IsOpen)
                    {
                        DUTSerialPorts[i].Close();
                    }
                }
                catch
                {
                }
            }

            //if (ConnectProgrammerButton.Text == "&Disconnect")
            //{
            //    DisconnectProg();
            //}


            CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerGlobal = MTKBDAProgrammer.BDAProgrammer.GlobalProgrammerSelected;
            if ((MTKBDAProgrammer.BDAProgrammer.GlobalProgrammerSelected == false) && (MTKBDAProgrammer.BDAProgrammer.SelectedProgrammer != ""))
            {
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerName = MTKBDAProgrammer.BDAProgrammer.SelectedProgrammer;
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerVoltage = MTKBDAProgrammer.BDAProgrammer.SelectedVoltageSetting;
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerPM = MTKBDAProgrammer.BDAProgrammer.SelectedAquireMode;
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerConn = MTKBDAProgrammer.BDAProgrammer.SelectedConnectorType.ToString();
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerClock = MTKBDAProgrammer.BDAProgrammer.SelectedClock;
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerPA = MTKBDAProgrammer.BDAProgrammer.PAToString();
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerVerify = MTKBDAProgrammer.BDAProgrammer.ValidateAfterProgramming.ToString();
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerHexPath = MTKBDAProgrammer.BDAProgrammer.SelectedHEXFilePath;
            }
            else
            {
                CyBLE_MTK_Application.Properties.Settings.Default.BDAProgrammerName = "Configure...";
            }


            CyBLE_MTK_Application.Properties.Settings.Default.RecentPath1 = RecentFile1.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.RecentPath2 = RecentFile2.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.RecentPath3 = RecentFile3.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.RecentPath4 = RecentFile4.Text;
            CyBLE_MTK_Application.Properties.Settings.Default.RecentPath5 = RecentFile5.Text;

            CyBLE_MTK_Application.Properties.Settings.Default.Save();
            Logger.StopLogging();
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (DUTInfoDataGridView.ColumnCount > 0)
            {
                int SumRowWidth = 3;// DUTInfoDataGridView.RowHeadersWidth + 2;

                for (int i = 0; i < DUTInfoDataGridView.Columns.Count; i++)
                {
                    if (DUTInfoDataGridView.Columns[i].Name != "Unique ID")
                    {
                        SumRowWidth += DUTInfoDataGridView.Columns[i].Width;
                    }
                }
                int temp = DUTInfoDataGridView.Width - SumRowWidth;

                if ((DUTInfoDataGridColumn1Width >= temp))
                {
                    DUTInfoDataGridView.Columns["Unique ID"].Width = DUTInfoDataGridColumn1Width;
                }
                else 
                {
                    DUTInfoDataGridView.Columns["Unique ID"].Width = temp;
                }
            }
            //if (TestProgramGridView.ColumnCount > 0)
            //{
            //    int temp = TestProgramGridView.Width - (TestProgramGridView.RowHeadersWidth +
            //            TestProgramGridView.Columns[0].Width + TestProgramGridView.Columns[2].Width + 2);
            //    if (TestProgramGridViewColumn1Width >= temp)
            //    {
            //        TestProgramGridView.Columns[1].Width = TestProgramGridViewColumn1Width;
            //    }
            //    else
            //    {
            //        TestProgramGridView.Columns[1].Width = temp;
            //    }
            //}
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutMTKDialog AboutMTK = new AboutMTKDialog();
            AboutMTK.ShowDialog();
        }

        public bool EditTestProgramDialog(int PreSelectIndex)
        {
            Logger.PrintLog(this, "Editing test program.", LogDetailLevel.LogRelevant);

            TestProgramDialog EditTests = new TestProgramDialog(Logger, MTKSerialPortDialog.DeviceSerialPort, DUTSerialPortDialog.DeviceSerialPort);
            EditTests.EnableMessages = CyBLE_MTK_Application.Properties.Settings.Default.EnableTestProgDialogMsg;
            EditTests.TestProgramList = MTKTestProgram.TestProgram;//EditTests.CopyTestList(MTKTestProgram.TestProgram);
            EditTests.TestProgramOpenEditIndex = PreSelectIndex;
            EditTests.BDAProgrammer = MTKBDAProgrammer;
            if (EditTests.ShowDialog() == DialogResult.OK)
            {
                //MTKTestProgram.TestProgram = EditTests.CopyTestList(EditTests.TestProgramList);
                MTKTestProgram.TestProgramEdited();
                Logger.PrintLog(this, "Test program edits complete.", LogDetailLevel.LogRelevant);
                //UpdateBDA();
                return true;
            }


            Logger.PrintLog(this, "Test program edits cancelled.", LogDetailLevel.LogRelevant);
            return false;
        }

        private void EditTestProgram(int PreSelectIndex)
        {
            BackupAndApplyAppStatus("Editing tests...");

            if (EditTestProgramDialog(PreSelectIndex) == true)
            {
                SaveTestMenuItem.Enabled = true;
                SaveTestProgramAsMenuItem.Enabled = true;
                PopulateTestInfo(MTKTestProgram.TestProgram);
                this.Text = MTKTestProgram.TestFileName + "* - " + BackupWindowText;
            }

            RestoreAppStatus();
        }

        private void TestProgramMenuItem_Click(object sender, EventArgs e)
        {
            EditTestProgram(-1);
        }

        private void PreferencesMenuItem_Click(object sender, EventArgs e)
        {
            Logger.PrintLog(this, "Editing preferences.", LogDetailLevel.LogRelevant);
            BackupAndApplyAppStatus("Editing preferences...");

            string tempLogSetting = CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting;
            //int Temp1 = CyBLE_MTK_Application.Properties.Settings.Default.AnritsuScriptID;
            if (MTKPreferences.ShowDialog() == DialogResult.OK)
            {
                Logger.EnableTestResultLogging = CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTests;

                if (MTKPreferences.RestartRequired == true)
                {
                    MessageBox.Show("Application has to be restarted for some settings to take effect!", "Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MTKSerialPortDialog.StopCheckingConnectionStatus();
                DUTSerialPortDialog.StopCheckingConnectionStatus();
                if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
                {
                    DUTToolStripMenuItem.Enabled = true;
                    MTKSerialPortDialog.CheckDUTPresence = false;
                    DUTSerialPortDialog.CheckDUTPresence = true;
                    DataBaseStatus.BackColor = Color.Red;
                    MTKSerialPortDialog.StartCheckingConnectionStatus();
                    DUTSerialPortDialog.StartCheckingConnectionStatus();
                }
                else
                {
                    DUTToolStripMenuItem.Enabled = false;
                    MTKSerialPortDialog.CheckDUTPresence = true;
                    DUTSerialPortDialog.CheckDUTPresence = false;
                    DataBaseStatus.BackColor = Color.Red;
                    MTKSerialPortDialog.StartCheckingConnectionStatus();
                }

                //MTKSerialPort.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                //DUTSerialPort.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;

                if (NumDUTsBackup != CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs)
                {
                    if ((MTKTestProgram.TestProgramStatus == TestProgramState.Paused) ||
                        (MTKTestProgram.TestProgramStatus == TestProgramState.Pausing) ||
                        (MTKTestProgram.TestProgramStatus == TestProgramState.Running))
                    {
                        UpdateTestInfo = false;
                        if (MessageBox.Show("You are trying to update the DUT information table when a test program is running." +
                            " Click \"OK\" to update now.", "Information", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            StopTests();
                            while (MTKTestProgram.TestProgramStatus != TestProgramState.Stopped) ;
                            SetupDUTRelated();
                        }
                        else
                        {
                            MessageBox.Show("The DUTs information table will be updated after the tests are stopped.",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UpdateTestInfo = true;
                        }
                    }
                    else
                    {
                        SetupDUTRelated();
                    }
                }

                MTKTestProgram.IgnoreDUT = CyBLE_MTK_Application.Properties.Settings.Default.IgnoreDUTs;

                if (tempLogSetting != CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting)
                {
                    if (tempLogSetting == "day")
                    {
                        TestResultLogTimer.Change(-1, -1);
                    }
                    if (TestResults.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("There are unsaved test results, do you wish to save them?", "Test Results", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            string ResultLogFileName = Logger.GenerateTestResultsFileName("Test_Results");
                            Logger.LogTestResults(ResultLogFileName, TestResults);
                        }
                        else
                        {
                            TestResults.Clear();
                        }
                    }
                    if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "day")
                    {
                        LogResultsTimerSetup();
                    }
                }
                else
                {
                    if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "day")
                    {
                        LogResultsTimerSetup();
                    }
                }
            }
            RestoreAppStatus();
        }

        public void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            Logger.PrintLog(this, "Changing password.", LogDetailLevel.LogRelevant);
            BackupAndApplyAppStatus("Changing password...");
            
            ChangePasswordDialog ChangePassword = new ChangePasswordDialog(Logger);
            ChangePassword.ShowDialog();
            if (ChangePassword.PasswordChanged == true)
            {
                if (SupervisorModeMenuItem.Checked == true)
                {
                    SupervisorModeMenuItem.PerformClick();
                }
            }

            RestoreAppStatus();
        }

        private void HandleStop()
        {


            MTKTestProgram.StopTestProgram();
            TestRunStopButton.Text = "&Start";
            TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Go;
            ResetTestButton.Enabled = false;
            StopButton.Enabled = false;
            SerialPortMenuItem.Enabled = true;


            LoadTestMenuItem.Enabled = true;
            CloseTestProgramMenuItem.Enabled = true;
            RecentMenuItem.Enabled = true;
            if (SupervisorModeMenuItem.Checked == true)
            {
                NewTestProgramMenuItem.Enabled = true;
                SaveTestMenuItem.Enabled = true;
                SaveTestProgramAsMenuItem.Enabled = true;

                TestProgramMenuItem.Enabled = true;
                PreferencesMenuItem.Enabled = true;

                importApplicationSettingsToolStripMenuItem.Enabled = true;
                exportApplicationSettingsToolStripMenuItem.Enabled = true;
            }

            setHexFileForAllDUTProgrammersToolStripMenuItem.Enabled = true;

            BDATextBox.Enabled = true;
            ConfigBDAButton.Enabled = true;
            if (MTKBDAProgrammer.BDAProgrammer.SelectedProgrammer != "")
            {
                BDAWriteButton.Enabled = true;
            }

            for (int i = 0; i < (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
            {
                ((DataGridViewDisableButtonCell)DUTInfoDataGridView["DUT Programmer", i]).Enabled = true;
                ((DataGridViewDisableButtonCell)DUTInfoDataGridView["Serial Port", i]).Enabled = true;
                DUTInfoDataGridView["Unique ID", i].ReadOnly = false;
            }

            DUTInfoDataGridView.Refresh();
            TestProgramGridView.ClearSelection();
            DUTInfoDataGridView.ClearSelection();


            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "'Run' cycle")
            {
                string ResultLogFileName = Logger.GenerateTestResultsFileName("Run#" + RunCount.ToString() + "_Results");
                Logger.LogTestResults(ResultLogFileName, TestResults);
                TestResults.Clear();
            }

            MTKSerialPortDialog.StartCheckingConnectionStatus();
            //DUTSerialPortDialog.StartCheckingConnectionStatus();

            Logger.PrintLog(this, "Test program stopped.", LogDetailLevel.LogRelevant);
            this.ApplicationStatus.Text = "Idle";
            AppStatBackup = "Idle";

            //this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
            //this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Selected = true));
            DUTInfoDataGridView.ClearSelection();
            DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Selected = true;
            UpdateTestInfoForDUT(MTKTestProgram.CurrentDUT, DUT_TestStatus[MTKTestProgram.CurrentDUT]);

            _runLock = false;
        }

        private void StopTests()
        {
            if (this.InvokeRequired)
            {
                SetVoidCallback d = new SetVoidCallback(this.HandleStop);
                this.Invoke(d, null);
            }
            else
            {
                HandleStop();
            }
        }



        private void MTKTestProgram_OnTestStopped()
        {

            StopTests();


            this.Invoke(new MethodInvoker(() => UploadResult()));
        }



        private void ScrollIfRequired(DataGridView DGVToScroll, int CurrentRow)
        {
            int RowPadding = DGVToScroll.DisplayedRowCount(false) / 2;
            if (CurrentRow > RowPadding)
            {
                this.Invoke(new MethodInvoker(() => DGVToScroll.FirstDisplayedScrollingRowIndex = CurrentRow - RowPadding));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => DGVToScroll.FirstDisplayedScrollingRowIndex = 0));
            }
        }

        private void MTKTestProgram_OnTestProgramRunError(TestManagerError Error, string Message)
        {
            if (Error == TestManagerError.TestProgramEmpty)
            {
                MessageBox.Show("Test program empty. Plese add tests to the current test program (Edit > Test Program).",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MTKTestProgram_OnNextIteration(int CurrentDUT)
        {
            ScrollIfRequired(DUTInfoDataGridView, CurrentDUT);

            this.Invoke(new MethodInvoker(() => MTKTestProgram.DUTSerialPort = DUTSerialPorts[CurrentDUT]));
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[CurrentDUT].Selected = true));
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[CurrentDUT].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black }));
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[CurrentDUT].Cells["Status"].Value = "Testing..."));

            this.Invoke(new MethodInvoker(() => TestProgramGridView.ClearSelection()));


            if (MTKTestProgram.CurrentMTKTestType == MTKTestType.MTKTestProgramAll && (!MTKTestProgramAll.MTKTestProgramAllAtEnd) && MTKTestProgram.CurrentDUT == 0)
            {
                for (int i = 0; i < MTKTestProgram.TestProgram.Count; i++)
                {

                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White }));
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Value = "Queued"));
                }
            }
            else
            {
                for (int i = 1; i < MTKTestProgram.TestProgram.Count; i++)
                {

                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White }));
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Value = "Queued"));
                }
            }



            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "DUT")
            {
                this.Invoke(new MethodInvoker(() => TestResults.Clear()));
            }

            bool IsMuxPresent = false;
            if ((CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs > 1) && (MTKTestProgram.IsAnritsuTestPresent()))
            {
                if (!DUTSelectSerialPort.DeviceSerialPort.IsOpen)
                {
                    Logger.PrintLog(this, "DUT Multiplexer not connected.", LogDetailLevel.LogRelevant);
                    DialogResult Temp = DialogResult.None;
                    SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(DUTSelectSerialPort);
                    this.Invoke(new MethodInvoker(() => TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog));
                    this.Invoke(new MethodInvoker(() => TempDialog.Text = "DUT Multiplexer Serial Port Setting"));
                    this.Invoke(new MethodInvoker(() => Temp = TempDialog.ShowDialog()));
                    if (Temp == DialogResult.Cancel)
                    {
                        StopTests();
                        IsMuxPresent = false;
                    }
                    else
                    {
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTMultiplexerSerialPort = DUTSelectSerialPort.DeviceSerialPort.PortName;
                        CyBLE_MTK_Application.Properties.Settings.Default.Save();
                        IsMuxPresent = true;
                    }
                }
            }
            else
            {
                IsMuxPresent = false;
            }

            if (IsMuxPresent)
            {
                int output = 0;
                //string temp = CurrentDUT.ToString("X1");
                this.Invoke(new MethodInvoker(() => DUTSelectSerialPort.DeviceSerialPort.Write("G")));
                try
                {
                    Thread.Sleep(10);
                    this.Invoke(new MethodInvoker(() => output = DUTSelectSerialPort.DeviceSerialPort.ReadChar()));
                }
                catch
                {
                }
                if ((char)output != 'g')
                {
                    MTKTestProgram_OnOverallFail();
                    StopTests();
                }
            }
        }

        private void MTKTestProgram_OnCurrentIterationComplete(int CurrentDUT, bool Ignore)
        {

            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "DUT" && !Ignore)
            {
                string ResultLogFileName = "DUT#" + (CurrentDUT + 1).ToString() + "_" +
                    DUTInfoDataGridView.Rows[CurrentDUT].Cells[1].Value + "_Results";

                if (DUTInfoDataGridView.Rows[CurrentDUT].Cells[2].Value.ToString().ToUpper() == "PASS")
                {
                    ResultLogFileName = "PASS_" + ResultLogFileName;
                }
                else
                {
                    ResultLogFileName = "FAIL_" + ResultLogFileName;
                }

                ResultLogFileName = Logger.GenerateTestResultsFileName(ResultLogFileName);
                Logger.LogTestResults(ResultLogFileName, TestResults);
                TestResults.Clear();
            }


        }

        //private void MTKTestProgram_OnCurrentIterationComplete(int CurrentDUT, bool Ignore)
        //{
        //    if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "DUT" &&
        //        MTKTestProgram.DUTOverallTestErrors[CurrentDUT] != MTKTestError.IgnoringDUT)
        //    {
        //        string ResultLogFileName = "DUT#" + (CurrentDUT + 1).ToString() + "_" +
        //            DUTInfoDataGridView.Rows[CurrentDUT].Cells[1].Value + "_Results";
        //        if (MTKTestProgram.DUTOverallTestErrors[CurrentDUT] == MTKTestError.NoError)
        //        {
        //            ResultLogFileName = "PASS_" + ResultLogFileName;
        //        }
        //        else
        //        {
        //            ResultLogFileName = "FAIL_" + ResultLogFileName;
        //        }
        //        ResultLogFileName = Logger.GenerateTestResultsFileName(ResultLogFileName);
        //        Logger.LogTestResults(ResultLogFileName, MTKTestProgram.CollectTestResults(CurrentDUT));
        //    }
        //}

        private void SetupSelection(int CurrentTest)
        {
            this.Invoke(new MethodInvoker(() => TestProgramGridView.ClearSelection()));
            this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[CurrentTest].Selected = true));
            this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[CurrentTest].Cells["Status"].Value = "Running..."));
        }

        private void SetDevErrStat(MTKTestError err, int DevNum)
        {
            if (err == MTKTestError.TestFailed && DUTsTestFlag[DevNum])
            {
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Value = "FAIL"));
            }
            else if (err == MTKTestError.NoError && DUTsTestFlag[DevNum])
            {
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.LightGreen }));
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Value = "PASS"));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Gray, BackColor = Color.LightGray }));
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[DevNum].Cells["Status"].Value = "IGNORE"));
            }
        }

        private void SetProgAllErrCtrl(List<MTKTestError> err, int start)
        {
            for (int i = start; i < err.Count(); i++)
            {
                SetDevErrStat(err[i], i);
            }
        }

        private void SetProgAllErr(List<MTKTestError> err)
        {
            SetProgAllErrCtrl(err, 0);
        }

        private void CyBLE_MTK_OnProgComplete(List<MTKTestError> err)
        {
            ProgAllErr = err;
            SetProgAllErr(ProgAllErr);
        }

        private void MTKTestProgram_OnNextTest(int CurrentTest)
        {
            BackupAndApplyAppStatus("Running test " + (CurrentTest + 1).ToString() + "/" + MTKTestProgram.TestProgram.Count.ToString());

            ScrollIfRequired(TestProgramGridView, CurrentTest);

            if ((!MTKTestProgramAll.MTKTestProgramAllAtEnd) && (MTKTestProgram.CurrentMTKTestType == MTKTestType.MTKTestProgramAll) && (MTKTestProgram.CurrentDUT != 0))
            {
                
            }
            else
            {
                SetupSelection(CurrentTest);
            }
            

            if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestBDAProgrammer")
            {
                ((MTKTestBDAProgrammer)MTKTestProgram.TestProgram[CurrentTest]).OnBDAProgram -= new MTKTestBDAProgrammer.BDAProgrammerEventHandler(CyBLE_MTK_OnBDAProgram);
                ((MTKTestBDAProgrammer)MTKTestProgram.TestProgram[CurrentTest]).OnBDAProgram += new MTKTestBDAProgrammer.BDAProgrammerEventHandler(CyBLE_MTK_OnBDAProgram);
                MTKBDAProgrammer.OnTestStatusUpdate -= new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKBDAProgrammer.OnTestStatusUpdate += new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKBDAProgrammer.OnTestResult -= new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);
                MTKBDAProgrammer.OnTestResult += new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);
            }
            else if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestProgramAll")
            {
                MTKTestProgram.TestProgram[CurrentTest].OnTestStatusUpdate -= new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKTestProgram.TestProgram[CurrentTest].OnTestStatusUpdate += new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKTestProgram.TestProgram[CurrentTest].OnTestResult -= new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);
                MTKTestProgram.TestProgram[CurrentTest].OnTestResult += new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);


                ((MTKTestProgramAll)MTKTestProgram.TestProgram[CurrentTest]).OnProgramAllComplete -= new MTKTestProgramAll.ProgramAllCompleteEventHandler(CyBLE_MTK_OnProgComplete);
                ((MTKTestProgramAll)MTKTestProgram.TestProgram[CurrentTest]).OnProgramAllComplete += new MTKTestProgramAll.ProgramAllCompleteEventHandler(CyBLE_MTK_OnProgComplete);
                ((MTKTestProgramAll)MTKTestProgram.TestProgram[CurrentTest]).OnNumTestStatusUpdate -= new MTKTestProgramAll.NumTestStatusUpdateEventHandler(CyBLE_MTK_OnNumTestStatusUpdate);
                ((MTKTestProgramAll)MTKTestProgram.TestProgram[CurrentTest]).OnNumTestStatusUpdate += new MTKTestProgramAll.NumTestStatusUpdateEventHandler(CyBLE_MTK_OnNumTestStatusUpdate);
            }
            else
            {
                if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestI2C")
                {
                    ((MTKTestI2C)MTKTestProgram.TestProgram[CurrentTest]).Programmer = DUTProgrammers[MTKTestProgram.CurrentDUT];
                }

                if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKPSoCProgrammer")
                {
                    DUTProgrammers[MTKTestProgram.CurrentDUT].OnTestStatusUpdate -= new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                    DUTProgrammers[MTKTestProgram.CurrentDUT].OnTestStatusUpdate += new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                }

                MTKTestProgram.TestProgram[CurrentTest].OnTestStatusUpdate -= new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKTestProgram.TestProgram[CurrentTest].OnTestResult -= new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);

                MTKTestProgram.TestProgram[CurrentTest].OnTestStatusUpdate += new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
                MTKTestProgram.TestProgram[CurrentTest].OnTestResult += new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);
            }

            if (((MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestAnritsu") ||
                (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestXOCalibration")) &&
                ((CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs > 1) || DUTSelectSerialPort.DeviceSerialPort.IsOpen))
            {
                bool IsMuxPresent = true;
                if (!DUTSelectSerialPort.DeviceSerialPort.IsOpen)
                {
                    Logger.PrintLog(this, "DUT Multiplexer not connected.", LogDetailLevel.LogRelevant);
                    SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(DUTSelectSerialPort);
                    TempDialog.Text = "DUT Multiplexer Serial Port Setting";
                    TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                    if (TempDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        StopTests();
                        IsMuxPresent = false;
                    }
                    else
                    {
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTMultiplexerSerialPort = DUTSelectSerialPort.DeviceSerialPort.PortName;
                        CyBLE_MTK_Application.Properties.Settings.Default.Save();
                    }
                }

                if (IsMuxPresent)
                {
                    if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestAnritsu")
                    {
                        bool SwitchResult;
                        int TryCount = 0;
                        do
                        {
                            SwitchResult = ((MTKTestAnritsu)MTKTestProgram.TestProgram[CurrentTest]).SwitchToAnritsu();
                            Thread.Sleep(10);
                            TryCount++;
                        } while (!SwitchResult && (TryCount < 3));
                    }
                    int output = 0;
                    string temp = MTKTestProgram.CurrentDUT.ToString("X1");
                    this.Invoke(new MethodInvoker(() => DUTSelectSerialPort.DeviceSerialPort.Write(temp)));
                    try
                    {
                        Thread.Sleep(10);
                        this.Invoke(new MethodInvoker(() => output = DUTSelectSerialPort.DeviceSerialPort.ReadChar()));
                    }
                    catch
                    {
                    }
                    if ((char)output == 'g')
                    {
                        MTKTestProgram_OnOverallFail();
                        StopTests();
                    }

                    if (MTKTestProgram.TestProgram[CurrentTest].ToString() == "MTKTestAnritsu")
                    {
                        ((MTKTestAnritsu)MTKTestProgram.TestProgram[CurrentTest]).TestScriptID = CyBLE_MTK_Application.Properties.Settings.Default.AnritsuScriptID;
                        ((MTKTestAnritsu)MTKTestProgram.TestProgram[CurrentTest]).TXPowerOffset = decimal.Parse(CyBLE_MTK_Application.Properties.Settings.Default.AnritsuTXPower[MTKTestProgram.CurrentDUT]);
                        ((MTKTestAnritsu)MTKTestProgram.TestProgram[CurrentTest]).OutputPowerOffset = decimal.Parse(CyBLE_MTK_Application.Properties.Settings.Default.AnritsuOutputPower[MTKTestProgram.CurrentDUT]);
                    }
                }
            }

            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "test")
            {
                TestResults.Clear();
            }


        }

        //private void CyBLE_MTK_OnProgramAll()
        //{
        //    if (MTKTestProgram.CurrentDUT == 0)
        //    {
        //        ProgramAllDUTs();
        //    }
        //}

        private void CyBLE_MTK_OnBDAProgram(SerialPort DUTPort, MTKTestErrorEventArgs e)
        {
            bool UseGlobalProgrammerFlag = false;
            if (MTKBDAProgrammer.BDAProgrammer.GlobalProgrammerSelected)
            {
                UseGlobalProgrammerFlag = true;
                MTKBDAProgrammer.BDAProgrammer = DUTProgrammers[MTKTestProgram.CurrentDUT];
            }
            Logger.PrintLog(this, "Writing BDA via: " + MTKBDAProgrammer.BDAProgrammer.GetDisplayText(), LogDetailLevel.LogRelevant);
            MTKBDAProgrammer.UpdateDUTPort(DUTPort);
            //BDACurrentDUT = BitConverter.ToString(MTKBDAProgrammer.BDAddress);
            e.ReturnValue = MTKBDAProgrammer.WriteBDA();
            MTKBDAProgrammer.OnTestStatusUpdate -= new MTKTest.TestStatusUpdateEventHandler(CyBLE_MTK_OnTestStatusUpdate);
            MTKBDAProgrammer.OnTestResult -= new MTKTest.TestResultEventHandler(CyBLE_MTK_OnTestResult);
            if (UseGlobalProgrammerFlag)
            {
                MTKBDAProgrammer.BDAProgrammer = new MTKPSoCProgrammer(Logger);
                MTKBDAProgrammer.BDAProgrammer.GlobalProgrammerSelected = true;
            }
        }

        private void CyBLE_MTK_OnTestResult(MTKTestResult TestResult)
        {
            
            TestResults.Add(TestResult);



            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "day")
            {
                Logger.TestLogWriteString("Batch#," + RunCount.ToString() + ",DUT#," + MTKTestProgram.CurrentDUT.ToString() + ",");
                Logger.WriteTestLog(TestResults);
                TestResults.Clear();
            }
        }

        public void MTKTestProgram_OnTestComplete()
        {
            if (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "test")
            {
                string ResultLogFileName = "Test#" + (MTKTestProgram.CurrentTestIndex + 1).ToString() + "_" +
                    MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex].ToString() + "_Results";
                ResultLogFileName = Logger.GenerateTestResultsFileName(ResultLogFileName);
                Logger.LogTestResults(ResultLogFileName, TestResults);
                TestResults.Clear();
            }

            if (((MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex].ToString() == "MTKTestAnritsu") ||
                (MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex].ToString() == "MTKTestXErrOCalibration")) &&
                ((CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs > 1) || DUTSelectSerialPort.DeviceSerialPort.IsOpen))
            {
                bool IsMuxPresent = true;
                if (!DUTSelectSerialPort.DeviceSerialPort.IsOpen)
                {
                    Logger.PrintLog(this, "DUT Multiplexer not connected.", LogDetailLevel.LogRelevant);
                    SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(DUTSelectSerialPort);
                    TempDialog.Text = "DUT Multiplexer Serial Port Setting";
                    TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                    if (TempDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        StopTests();
                        IsMuxPresent = false;
                    }
                    {
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTMultiplexerSerialPort = DUTSelectSerialPort.DeviceSerialPort.PortName;
                        CyBLE_MTK_Application.Properties.Settings.Default.Save();
                    }
                }

                if (IsMuxPresent)
                {
                    int output = 0;
                    //string temp = CurrentDUT.ToString("X1");
                    this.Invoke(new MethodInvoker(() => DUTSelectSerialPort.DeviceSerialPort.Write("G")));
                    try
                    {
                        Thread.Sleep(10);
                        this.Invoke(new MethodInvoker(() => output = DUTSelectSerialPort.DeviceSerialPort.ReadChar()));
                    }
                    catch
                    {
                    }
                    if ((char)output != 'g')
                    {
                        MTKTestProgram_OnOverallFail();
                        StopTests();
                    }
                    if (MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex].ToString() == "MTKTestAnritsu")
                    {
                        bool SwitchResult = ((MTKTestAnritsu)MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex]).SwitchToMTK();
                    }
                }
            }
        }

        public void CyBLE_MTK_OnNumTestStatusUpdate(int index, string Message)
        {
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[index].Cells["Status"].Value = Message));
        }

        private void TestStatusUpdate(int TestIndex, MTKTestMessageType MessageType, string Message)
        {
            
            if (MessageType == MTKTestMessageType.Failure)
            {
                this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[TestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));
            }
            else if (MessageType == MTKTestMessageType.Success)
            {
                this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[TestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.LightGreen }));
            }
            else if (MessageType == MTKTestMessageType.Information)
            {
                this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[TestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.LightGray }));
            }
            else if (MessageType == MTKTestMessageType.Complete)
            {
                this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[TestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.LightGray }));
            }

            this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[TestIndex].Cells["Status"].Value = Message));
        }

        public void CyBLE_MTK_OnTestStatusUpdate(MTKTestMessageType MessageType, string Message)
        {
            //MTKTestProgramAll at begin
            if (MTKTestProgram.CurrentMTKTestType == MTKTestType.MTKTestProgramAll && (!MTKTestProgramAll.MTKTestProgramAllAtEnd))
            {
                for (int i = 0; i < DUTInfoDataGridView.RowCount; i++)
                {
                    DUT_TestStatus[i][MTKTestProgram.CurrentTestIndex].MessageType = MessageType;
                    DUT_TestStatus[i][MTKTestProgram.CurrentTestIndex].Message = Message;
                    TestStatusUpdate(MTKTestProgram.CurrentTestIndex, MessageType, Message);
                }
                
            }
            else
            {
                DUT_TestStatus[MTKTestProgram.CurrentDUT][MTKTestProgram.CurrentTestIndex].MessageType = MessageType;
                DUT_TestStatus[MTKTestProgram.CurrentDUT][MTKTestProgram.CurrentTestIndex].Message = Message;

                TestStatusUpdate(MTKTestProgram.CurrentTestIndex, MessageType, Message);
            }


            //if (MessageType == MTKTestMessageType.Failure)
            //{
            //    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));
            //}
            //else if (MessageType == MTKTestMessageType.Success)
            //{
            //    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.LightGreen }));
            //}
            //else if (MessageType == MTKTestMessageType.Information)
            //{
            //    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black }));
            //}
            //else if (MessageType == MTKTestMessageType.Complete)
            //{
            //    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.LightGray }));
            //}

            //this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = Message));
        }

        private void MTKTestProgram_OnTestError(MTKTestError Error, string Message)
        {
            if (Error == MTKTestError.MissingMTKSerialPort)
            {
                Logger.PrintLog(this, "Cannot communicate with MTK serial port.", LogDetailLevel.LogRelevant);
                Logger.PrintLog(this, "Disconnecting MTK serial port.", LogDetailLevel.LogRelevant);
                MTKSerialPortDialog.DeviceSerialPort.Close();
                //TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink };
                //this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = "FAIL"));

                try
                {
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = "ERROR"));   //cysp
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));   //cysp
                }
                catch
                {

                }


                StopTests();
            }

            if (Error == MTKTestError.MissingDUTSerialPort)
            {
                if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
                {
                    Logger.PrintLog(this, "Cannot communicate with DUT serial port.", LogDetailLevel.LogRelevant);
                    Logger.PrintLog(this, "Disconnecting DUT serial port.", LogDetailLevel.LogRelevant);
                    try
                    {
                        //cysp: MissingDUTSerialPort Event Invoke here
                        this.Invoke(new MethodInvoker(() => DUTSerialPorts[MTKTestProgram.CurrentDUT].DiscardInBuffer()));
                        this.Invoke(new MethodInvoker(() => DUTSerialPorts[MTKTestProgram.CurrentDUT].DiscardOutBuffer()));
                        this.Invoke(new MethodInvoker(() => DUTSerialPorts[MTKTestProgram.CurrentDUT].Open()));
                        this.Invoke(new MethodInvoker(() => DUTSerialPorts[MTKTestProgram.CurrentDUT].Close()));
                        this.Invoke(new MethodInvoker(() => DUTSerialPorts[MTKTestProgram.CurrentDUT].Dispose()));
                        TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink };
                        //this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = "FAIL"));
                        this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = "ERROR"));   //cysp
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception of Re-open MissingDUTSerialPort DUTSerialPorts #" + MTKTestProgram.CurrentDUT + ". \n Reason: " + ex.ToString());
                    }


                    StopTests();
                }
            }
        }

        private bool IsProgAllPresent()
        {
            for (int i = 0; i < MTKTestProgram.TestProgram.Count; i++)
            {
                if (MTKTestProgram.TestProgram[i].ToString() == "MTKTestProgramAll" && ((MTKTestProgramAll)MTKTestProgram.TestProgram[i]).ProgramCompleted)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// cysp: 2017-07-12
        /// </summary>
        private void MTKTestProgram_OnShopfloorPermissionCheckFail(int CurrentDUT)
        {
            ProgramStatus[MTKTestProgram.CurrentDUT] = MTKTestError.TestFailed;
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[CurrentDUT].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.DarkRed, BackColor = Color.Pink }));
            this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[CurrentDUT].Cells["Status"].Value = "PROCESS FAIL"));

            this.Invoke(new MethodInvoker(() => TestStatusLabel.ForeColor = Color.Red));
            this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = "FAIL"));
        }



        private void MTKTestProgram_OnOverallFail()
        {



            if ((CyBLE_MTK_Application.Properties.Settings.Default.PauseTestsOnFailure == true) && (MTKTestProgram.DeviceTestingComplete == false))
            {
                MTKTestProgram.PauseTestProgram();
                BackupAndApplyAppStatus("Pausing...");
            }

            this.Invoke(new MethodInvoker(() => TestStatusLabel.ForeColor = Color.Red));
            this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = "FAIL"));


            if (IsProgAllPresent() && (ProgAllErr.Count > 0))
            {
                if ((MTKTestProgram.TestProgram[MTKTestProgram.CurrentTestIndex].ToString() != "MTKTestProgramAll") && MTKTestProgram.TestRunning)
                {
                    ProgramStatus[MTKTestProgram.CurrentDUT] = MTKTestError.TestFailed;
                }

                for (int i = 0; i < ProgAllErr.Count; i++)
                {
                    if ((ProgAllErr[i] == MTKTestError.TestFailed || ProgAllErr[i] == MTKTestError.NotAllDevicesProgrammed)&&DUTsTestFlag[i])
                    {
                        ProgramStatus[i] = MTKTestError.TestFailed;
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "FAIL"));
                    }

                    if (ProgAllErr[i] == MTKTestError.IgnoringDUT || ProgAllErr[i] == MTKTestError.ProgrammerNotConfigured)
                    {
                        ProgramStatus[i] = MTKTestError.IgnoringDUT;
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Gray, BackColor = Color.LightGray }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "IGNORE"));
                    }
                }

                SetProgAllErr(ProgramStatus);
            }
            else
            {

                for (int i = 0; i < ProgramStatus.Count(); i++)
                {
                    if (ProgramStatus[i] == MTKTestError.TestFailed && DUTsTestFlag[i] && ProgramStatus[i] != MTKTestError.IgnoringDUT)
                    {


                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Red, BackColor = Color.Pink }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "FAIL"));
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Gray, BackColor = Color.LightGray }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "IGNORE"));
                    }

                }
            }

            this.Invoke(new MethodInvoker(() => FlashWindowEx(this)));
        }

        private void MTKTestProgram_OnOverallPass()
        {
            if (IsProgAllPresent())
            {
                SetProgAllErr(ProgramStatus);
                bool ProgErrPresent = false;

                for (int i = 0; i < ProgAllErr.Count; i++)
                {
                    if (ProgAllErr[i] == MTKTestError.TestFailed || ProgAllErr[i] == MTKTestError.NotAllDevicesProgrammed && DUTsTestFlag[i])
                    {
                        ProgramStatus[i] = MTKTestError.TestFailed;
                    }

                    if (ProgAllErr[i] == MTKTestError.IgnoringDUT || ProgAllErr[i] == MTKTestError.ProgrammerNotConfigured || !DUTsTestFlag[i])
                    {
                        ProgramStatus[i] = MTKTestError.IgnoringDUT;
                    }
                }
                for (int i = 0; i < ProgramStatus.Count(); i++)
                {
                    

                    if ((ProgramStatus[i] == MTKTestError.TestFailed))
                    {
                        ProgErrPresent = true;
                        break;
                    }
                }
                if (ProgErrPresent)
                {


                    this.Invoke(new MethodInvoker(() => TestStatusLabel.ForeColor = Color.Red));
                    this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = "FAIL"));



                }
                else
                {
                    this.Invoke(new MethodInvoker(() => TestStatusLabel.ForeColor = Color.Green));
                    this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = "PASS"));

                }

                for (int i = 0; i < ProgramStatus.Count(); i++)
                {
                    if (ProgramStatus[i] == MTKTestError.NoError)
                    {
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.LightGreen }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "PASS"));

                        TestStatusUpdate(MTKTestProgram.CurrentTestIndex, MTKTestMessageType.Complete,"Done");


                    }

                    if (ProgramStatus[i] == MTKTestError.IgnoringDUT)
                    {
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Gray, BackColor = Color.LightGray }));
                        this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "IGNORE"));

                        //this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Style =
                        //            new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.Gray }));
                        //this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[MTKTestProgram.CurrentTestIndex].Cells["Status"].Value = "Ignored"));
                    }
                }



            }
            else
            {
                this.Invoke(new MethodInvoker(() => TestStatusLabel.ForeColor = Color.Green));
                this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = "PASS"));

                if (ProgramStatus[MTKTestProgram.CurrentDUT] == MTKTestError.NoError)
                {
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Green, BackColor = Color.LightGreen }));
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Value = "PASS"));


                }

                if (ProgramStatus[MTKTestProgram.CurrentDUT] == MTKTestError.IgnoringDUT)
                {
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Gray, BackColor = Color.LightGray }));
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Value = "IGNORE"));
                }
            }

        }



        private void MTKTestProgram_OnTestPaused()
        {
            this.Invoke(new MethodInvoker(() => TestRunStopButton.Text = "&Continue"));
            this.Invoke(new MethodInvoker(() => TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Go));
            this.Invoke(new MethodInvoker(() => ResetTestButton.Enabled = true));
            this.Invoke(new MethodInvoker(() => StopButton.Enabled = true));
            BackupAndApplyAppStatus("Test program paused");
        }

        //private void OpenMTKPort()
        //{


        //    try
        //    {


        //        ClosePortStatus close_status = MTKSerialPort.ClosePort();
        //        OpenPortStatus open_status = OpenPortStatus.NotOpen;

        //        if (MTKSerialPort.OpenPortByName(CyBLE_MTK_Application.Properties.Settings.Default.MTKHostSerialPort))
        //        {
        //            open_status = OpenPortStatus.Open;
        //            Logger.PrintLog(this, "OpenMTKPort successfully", LogDetailLevel.LogRelevant);
        //        }
        //        else
        //        {
        //            open_status = OpenPortStatus.NotOpen;
        //            Logger.PrintLog(this, "OpenMTKPort failure", LogDetailLevel.LogRelevant);

        //        }

        //        if (open_status != OpenPortStatus.Open)
        //        {

        //            if (MessageBox.Show("MTK host port not open. Do you want to open a port?",
        //                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)        //cysp
        //            {
        //                //if (MTKSerialPortDialog.ShowDialog() == DialogResult.Cancel)
        //                //{
        //                //    StopTests();
        //                //}
        //                SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(MTKSerialPort);
        //                TempDialog.Text = "MTK Serial Port Setting";
        //                TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
        //                TempDialog.ShowDialog();
        //            }

        //            if (open_status != OpenPortStatus.Open)
        //            {
        //                MessageBox.Show("Unable to open MTK Host COM Port", "Error", MessageBoxButtons.OK);
        //                //cysp                                                                                                                    
        //                //cysp: fix the problem -- whenever COM port missing, user has to disable and enable the COM in device manager
        //            }


        //        }


        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show(err.ToString());

        //    }

        //}



        private void OpenMTKPort()
        {
            if (MTKSerialPortDialog.DeviceSerialPort.IsOpen)
            {
                Logger.PrintLog(this, "MTKSerialPort is already open : " + MTKSerialPortDialog.DeviceSerialPort.PortName, LogDetailLevel.LogRelevant);
                return;
            }

            if (MessageBox.Show("MTK host port not open. Do you want to open a port?",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)        //cysp
            {

                MTKSerialPortDialog.Text = "MTK Serial Port Setting";
                MTKSerialPortDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                MTKSerialPortDialog.ShowDialog();
            }

        }

        private void MTKTestProgram_OnMTKPortOpen()
        {
            this.Invoke(new MethodInvoker(() => OpenMTKPort()));
        }

        private void OpenDUTPort()
        {


            //cysp: delete messageboxbutton.No so as to fix the problem of false-pass tests due to clicking button No
            if (MessageBox.Show("DUT port not open. Do you want to open a port?",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                CyBLEMTKSerialPort TempSP = new CyBLEMTKSerialPort(Logger);
                if (CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART")
                {
                    TempSP.CheckDUTPresence = true;
                }
                TempSP.SerialPortType = PortType.DUT;
                TempSP.ValidatePort = true;
                TempSP.OnDUTConnectionStatusChange += new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
                TempSP.DeviceSerialPort = DUTSerialPorts[MTKTestProgram.CurrentDUT];
                SerialPortSettingsDialog TempDialog = new SerialPortSettingsDialog(TempSP);
                TempDialog.Text = "DUT Serial Port Setting";
                TempDialog.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
                if (TempDialog.ShowDialog() == DialogResult.OK)
                {
                    DUTSerialPorts[MTKTestProgram.CurrentDUT] = TempDialog.DeviceSerialPort;
                    DUTInfoDataGridView[4, MTKTestProgram.CurrentDUT].Value = DUTSerialPorts[MTKTestProgram.CurrentDUT].PortName;
                    DUTToolStripMenuItem.DropDownItems[MTKTestProgram.CurrentDUT].Text = (MTKTestProgram.CurrentDUT + 1).ToString() + ": " + DUTSerialPorts[MTKTestProgram.CurrentDUT].PortName;
                    SaveDUTSerialPortSettings(MTKTestProgram.CurrentDUT, DUTSerialPorts[MTKTestProgram.CurrentDUT].PortName);
                }
                else
                {
                    DUTInfoDataGridView[4, MTKTestProgram.CurrentDUT].Value = "Configure...";
                    DUTToolStripMenuItem.DropDownItems[MTKTestProgram.CurrentDUT].Text = (MTKTestProgram.CurrentDUT + 1).ToString() + ": " + "Configure...";
                    SaveDUTSerialPortSettings(MTKTestProgram.CurrentDUT, "");
                }
                TempSP.StopCheckingConnectionStatus();
                TempSP.OnDUTConnectionStatusChange -= new CyBLEMTKSerialPort.ConnectionEventHandler(MTKSerialPortDialog_OnDUTConnectionStatusChange);
            }
        }



        private void MTKTestProgram_OnDUTPortOpen()
        {
            this.Invoke(new MethodInvoker(() => OpenDUTPort()));
        }

        private void OpenAnritsuPort()
        {
            if (MessageBox.Show("Anritsu port not open. Do you want to open a port?",
                "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //if (DUTSerialPortDialog.ShowDialog() == DialogResult.Cancel)
                //{
                //    StopTests();
                //}
                OpenAndReadyAnritsuPort();
            }
            AnritsuSerialPort.StopCheckingConnectionStatus();
            //else
            //{
            //    StopTests();
            //}
            //MTKTestProgram.ContinueTestProgram();
        }

        private void MTKTestProgram_OnAnritsuPortOpen()
        {
            this.Invoke(new MethodInvoker(() => OpenAnritsuPort()));
        }

        private void PostIgnoreDUT()
        {
            DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.LightGray };
            DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Cells["Status"].Value = "IGNORE";
        }

        private void MTKTestProgram_OnIgnoreDUT()
        {
            this.Invoke(new MethodInvoker(() => PostIgnoreDUT()));
        }

        private bool CheckForTestRun()
        {
            MTKTestProgram.DUTConnectionType = CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType;
            MTKTestProgram.PauseTestsOnFailure = CyBLE_MTK_Application.Properties.Settings.Default.PauseTestsOnFailure;

            //if (!CheckMTKPort())
            //{
            //    return false;
            //}
            
            if (TestStatusLabel.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => TestStatusLabel.Text = ""));

            }
            else
            {
                TestStatusLabel.Text = "";
            }



            for (int i = 0; i < DUTInfoDataGridView.Rows.Count; i++)
            {
                DUTInfoDataGridView.ClearSelection();
                DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White };
                DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "Queued";
            }

            return true;
        }

        


        private void TestRunStopButton_Click(object sender, EventArgs e)
        {
            if ((!CyBLE_MTK.DUTsTestFlag[0]) || DUTInfoDataGridView.Rows[0].Cells["Serial Port"].Value.ToString() == "Configure...")
            {
                MessageBox.Show("警告：DUT#1 不能被屏蔽，请修复完成后，再使用！测试将被终止！ \n\nCyBLE_MTK.DUTsTestFlag[0]: " + CyBLE_MTK.DUTsTestFlag[0].ToString() + "\n\nDUTSerialPorts[0].PortName: " + DUTInfoDataGridView.Rows[0].Cells["Serial Port"].Value.ToString());
                return;
            }

            RedirectTheFirstFinalRowIndexfromDUTInfoDataGridView();

            if (IndexConfiguredSerialPortfor1stRow < 0)
            {
                Logger.PrintLog(this, "Warning: There is no configured serial port in the DUTInfoDataGridView...", LogDetailLevel.LogRelevant);
                Logger.PrintLog(this, "Test won't be excuted until 1 or more than 1 serial port to be configured!", LogDetailLevel.LogRelevant);
                return;
            }
            else
            {
                Logger.PrintLog(this, "Info: The 1st configured port is DUT#" + (IndexConfiguredSerialPortfor1stRow + 1), LogDetailLevel.LogRelevant);
                Logger.PrintLog(this, "Info: The final configured port is DUT#" + (IndexConfiguredSerialPortforfinalRow + 1), LogDetailLevel.LogRelevant);

            }

            if (TestRunStopButton.Text == "&Start")
            {
                if (e != null && e != EventArgs.Empty && CyBLEMTKRobotServer.IsGlobalServerActive())
                {
                    Logger.PrintLog(this, "Test program will run after receiving robot message.", LogDetailLevel.LogRelevant);
                    return;
                }


                RunCount++;
                Logger.PrintLog(this, "Running test program.", LogDetailLevel.LogRelevant);
                BackupAndApplyAppStatus("Running test program...");

                TestRunStopButton.Text = "&Pause";
                TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Pause;

                ProgAllErr = new List<MTKTestError>();
                ProgramStatus = new List<MTKTestError>();

                ResetTestButton.Enabled = false;
                StopButton.Enabled = false;
                SerialPortMenuItem.Enabled = false;
                //ProgramAllButton.Enabled = false;

                TestProgramMenuItem.Enabled = false;
                PreferencesMenuItem.Enabled = false;

                //ProgrammingInterfaceGroupBox.Enabled = false;

                NewTestProgramMenuItem.Enabled = false;
                LoadTestMenuItem.Enabled = false;
                CloseTestProgramMenuItem.Enabled = false;
                SaveTestMenuItem.Enabled = false;
                SaveTestProgramAsMenuItem.Enabled = false;
                RecentMenuItem.Enabled = false;

                BDATextBox.Enabled = false;
                BDAWriteButton.Enabled = false;
                ConfigBDAButton.Enabled = false;


                importApplicationSettingsToolStripMenuItem.Enabled = false;
                exportApplicationSettingsToolStripMenuItem.Enabled = false;

                setHexFileForAllDUTProgrammersToolStripMenuItem.Enabled = false;

                //cysp: clear errcodes
                errcodes.Clear();
                DUTTestResults.Clear();
                MTKTestCUS.TmplSFCSErrCode = 0;


                for (int i = 0; i < (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
                {
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["DUT Programmer", i]).Enabled = false;
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["Serial Port", i]).Enabled = false;
                    DUTInfoDataGridView["Unique ID", i].ReadOnly = true;
                    if (DUT_TestStatus[i] == null)
                    {
                        DUT_TestStatus[i] = new List<MTKTestStatus>();
                    }
                    else
                    {
                        DUT_TestStatus[i].Clear();
                    }

                    for (int j = 0; j < MTKTestProgram.TestCaseCount; j++)
                    {
                        DUT_TestStatus[i].Add(new MTKTestStatus(MTKTestMessageType.Information, "Queued"));
                    }
                    ProgramStatus.Add(new MTKTestError());
                    ProgramStatus[i] = MTKTestError.NoError;
                }

                try
                {
                    DUTInfoDataGridView.Refresh();
                }
                catch (Exception)
                {

                    throw;
                }


                if (TestProgramGridView.RowCount > 0)
                {
                    TestProgramGridView.FirstDisplayedScrollingRowIndex = 0;
                }



                if ((CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "'Run' cycle") ||
                    (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "DUT") ||
                    (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "test"))
                {
                    TestResults.Clear();
                }

                //CheckAllSerialPorts((CyBLE_MTK_Application.Properties.Settings.Default.TestModeLongRun == 0 || CyBLEMTKRobotServer.IsGlobalServerActive()));

                //if (CheckForTestRun())
                if (CheckForTestRun())
                {
                    MTKSerialPortDialog.StopCheckingConnectionStatus();
                    DUTSerialPortDialog.StopCheckingConnectionStatus();
                    //AnritsuSerialPort.StopCheckingConnectionStatus();



                    ////shopfloor by pzho
                    if (!MTKTestProgram.SupervisorMode && (CyBLE_MTK_Application.Properties.Settings.Default.TestModeLongRun == 0 || CyBLEMTKRobotServer.IsGlobalServerActive()))
                    {

                        ////shopfloor plus MFGTestSanityCheck by cysp
                        int DutCount = DUTInfoDataGridView.RowCount;

                        DUTSerialNumbers = new List<string>();
                        DUTSerialPortsName = new List<string>();


                        for (int i = 0; i < DutCount; i++)
                        {
                            DUTSerialNumbers.Add(DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value.ToString());


                            DUTSerialPortsName.Add(DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString());

                        }

                        //SerialNumberUniqueCheck

                        string msg = "MFGTestSanityCheck for SerialNumberUniqueCheck: No Check Because No MFG mode selected!!!";

                        if ((SFCS.GetSFCS(Logger).GetType() == Type.GetType("CyBLE_MTK_Application.SFCS_SIGMA") || SFCS.GetSFCS(Logger).GetType() == Type.GetType("CyBLE_MTK_Application.SFCS_FITTEC"))&&
                                CyBLE_MTK_Application.Properties.Settings.Default.DUTSerialNumberDuplicationCheck)
                        {

                            msg = MFGTestSanityCheck.SerialNumberUniqueCheck(DUTSerialNumbers.ToArray(), DUTSerialPortsName.ToArray());

                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }
                        else
                        {
                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }


                        //SerialPortPredefineMappingCheck

                        CyBLE_MTK_Application.Properties.Settings.Default.Reset();
                        CyBLE_MTK_Application.Properties.Settings.Default.Reload();
                        CyBLE_MTK_Application.Properties.Settings.Default.Upgrade();

                        if (CyBLE_MTK_Application.Properties.Settings.Default.EnableComPortsPredefineCheck)
                        {
                            List<string> PredefineserialportsName = new List<string>();

                            PredefineserialportsName.Clear();

                            for (int i = 0; i < DutCount; i++)
                            {
                                PredefineserialportsName.Add(CyBLE_MTK_Application.Properties.Settings.Default.DUTPredefinedSerialPorts[i]);

                            }

                            msg = MFGTestSanityCheck.SerialPortPredefineMappingCheck(DUTSerialPortsName.ToArray(), PredefineserialportsName.ToArray());
                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }
                        else
                        {
                            Logger.PrintLog(this, "SerialPortPredefineMappingCheck is disabled !!!", LogDetailLevel.LogRelevant);
                        }


                        DUTSerialNumbers.Clear();
                        DUTSerialPortsName.Clear();

                        try
                        {
                            if (!CheckPemission())
                            {
                                StopTests();
                                /*
                                 * Continue here, otherwise have to pop an error dialog.
                                 */
                                //return;
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Permisson error: " + ex.Message);
                        }

                    }

                    MTKTestProgram.DUTProgrammers = DUTProgrammers;
                    MTKTestProgram.DUTSerialPorts = DUTSerialPorts;



                    TestThread = new Thread(() => MTKTestProgram.RunTestProgram(DUTInfoDataGridView.Rows.Count));
                    TestThread.Name = "RunTestProgram_" + DUTInfoDataGridView.Rows.Count.ToString() + "_" + RunCount.ToString();
                    TestThread.Start();
                }
            }
            else if (TestRunStopButton.Text == "&Pause")
            {
                MTKTestProgram.PauseTestProgram();
                BackupAndApplyAppStatus("Pausing...");
                TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Go;
            }
            else if (TestRunStopButton.Text == "&Continue")
            {
                TestRunStopButton.Text = "&Pause";
                TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Pause;
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
                this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Selected = true));
                UpdateTestInfoForDUT(MTKTestProgram.CurrentDUT, DUT_TestStatus[MTKTestProgram.CurrentDUT]);
                ResetTestButton.Enabled = false;
                StopButton.Enabled = false;
                PreferencesMenuItem.Enabled = false;
                MTKTestProgram.ContinueTestProgram();
                BackupAndApplyAppStatus("Running test program...");
            }
        }







        /// <summary>
        /// cysp: call in robot
        /// </summary>
        private void TestRunStop()
        {
            if (true)
            {
                while (_runLock == true)
                {
                    Thread.Sleep(100);
                }
                _runLock = true;

                RunCount++;





                Logger.PrintLog(this, "Running test program.", LogDetailLevel.LogRelevant);
                BackupAndApplyAppStatus("Running test program...");



                ProgAllErr = new List<MTKTestError>();
                ProgramStatus = new List<MTKTestError>();





                if (TestRunStopButton.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => TestRunStopButton.Text = "&Stop"));
                    this.Invoke(new MethodInvoker(() => TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Pause));

                }
                else
                {
                    TestRunStopButton.Text = "&Stop";
                    TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Pause;
                }




                //cysp: clear errcodes
                errcodes.Clear();
                DUTTestResults.Clear();
                MTKTestCUS.TmplSFCSErrCode = 0;

                for (int i = 0; i < (int)CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
                {
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["DUT Programmer", i]).Enabled = false;
                    ((DataGridViewDisableButtonCell)DUTInfoDataGridView["Serial Port", i]).Enabled = false;
                    DUTInfoDataGridView["Unique ID", i].ReadOnly = true;

                    if (DUT_TestStatus[i] == null)
                    {
                        DUT_TestStatus[i] = new List<MTKTestStatus>();
                    }
                    else
                    {
                        DUT_TestStatus[i].Clear();
                    }

                    for (int j = 0; j < MTKTestProgram.TestCaseCount; j++)
                    {
                        DUT_TestStatus[i].Add(new MTKTestStatus(MTKTestMessageType.Information, "Queued"));
                    }

                    ProgramStatus.Add(new MTKTestError());
                    ProgramStatus[i] = MTKTestError.NoError;
                }




                if (DUTInfoDataGridView.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Refresh()));

                }
                else
                {
                    DUTInfoDataGridView.Refresh();
                }

                if (TestProgramGridView.RowCount > 0)
                {
                    TestProgramGridView.FirstDisplayedScrollingRowIndex = 0;
                }

                if ((CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "'Run' cycle") ||
                    (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "DUT") ||
                    (CyBLE_MTK_Application.Properties.Settings.Default.AutoLogTestsSetting == "test"))
                {
                    TestResults.Clear();
                }

                if (CheckForTestRun())
                {
                    MTKSerialPortDialog.StopCheckingConnectionStatus();
                    DUTSerialPortDialog.StopCheckingConnectionStatus();
                    AnritsuSerialPort.StopCheckingConnectionStatus();

                    ////shopfloor plus MFGTestSanityCheck by cysp
                    if (!MTKTestProgram.SupervisorMode)
                    {
                        //MFGTestSanityCheck 


                        int DutCount = DUTInfoDataGridView.RowCount;

                        DUTSerialNumbers = new List<string>();
                        DUTSerialPortsName = new List<string>();


                        for (int i = 0; i < DutCount; i++)
                        {
                            DUTSerialNumbers.Add(DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value.ToString());


                            DUTSerialPortsName.Add(DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString());

                        }

                        //SerialNumberUniqueCheck

                        string msg = "MFGTestSanityCheck for SerialNumberUniqueCheck: No Check Because No MFG mode selected!!!";

                        if (SFCS.GetSFCS(Logger).GetType() == Type.GetType("CyBLE_MTK_Application.SFCS_SIGMA") || SFCS.GetSFCS(Logger).GetType() == Type.GetType("CyBLE_MTK_Application.SFCS_FITTEC"))
                        {
                            msg = MFGTestSanityCheck.SerialNumberUniqueCheck(DUTSerialNumbers.ToArray(), DUTSerialPortsName.ToArray());

                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }
                        else
                        {
                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }


                        //SerialPortPredefineMappingCheck

                        CyBLE_MTK_Application.Properties.Settings.Default.Reset();
                        CyBLE_MTK_Application.Properties.Settings.Default.Reload();
                        CyBLE_MTK_Application.Properties.Settings.Default.Upgrade();

                        if (CyBLE_MTK_Application.Properties.Settings.Default.EnableComPortsPredefineCheck)
                        {
                            List<string> PredefineserialportsName = new List<string>();

                            PredefineserialportsName.Clear();

                            for (int i = 0; i < DutCount; i++)
                            {
                                PredefineserialportsName.Add(CyBLE_MTK_Application.Properties.Settings.Default.DUTPredefinedSerialPorts[i]);

                            }

                            msg = MFGTestSanityCheck.SerialPortPredefineMappingCheck(DUTSerialPortsName.ToArray(), PredefineserialportsName.ToArray());
                            Logger.PrintLog(this, msg, LogDetailLevel.LogRelevant);
                        }
                        else
                        {
                            Logger.PrintLog(this, "SerialPortPredefineMappingCheck is disabled !!!", LogDetailLevel.LogRelevant);
                        }


                        DUTSerialNumbers.Clear();
                        DUTSerialPortsName.Clear();

                        try
                        {
                            if (!CheckPemission())
                            {
                                StopTests();
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Reason: " + ex.Message, "Permisson error");
                        }
                    }

                    MTKTestProgram.DUTProgrammers = DUTProgrammers;
                    MTKTestProgram.DUTSerialPorts = DUTSerialPorts;

                    //timer_cycleRunCount.Enabled = true;

                    //timer_cycleRunCount.Start();


                    TestThread = new Thread(() => MTKTestProgram.RunTestProgram(DUTInfoDataGridView.Rows.Count));
                    TestThread.Start();
                }
            }
            //else if (TestRunStopButton.Text == "&Pause")
            //{
            //    MTKTestProgram.PauseTestProgram();
            //    BackupAndApplyAppStatus("Pausing...");
            //    TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Go;
            //}
            //else if (TestRunStopButton.Text == "&Continue")
            //{
            //    TestRunStopButton.Text = "&Pause";
            //    TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Pause;
            //    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
            //    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[MTKTestProgram.CurrentDUT].Selected = true));
            //    UpdateTestInfoForDUT(MTKTestProgram.CurrentDUT, DUT_TestStatus[MTKTestProgram.CurrentDUT]);
            //    ResetTestButton.Enabled = false;
            //    StopButton.Enabled = false;
            //    PreferencesMenuItem.Enabled = false;
            //    MTKTestProgram.ContinueTestProgram();
            //    BackupAndApplyAppStatus("Running test program...");
            //}
        }







        private void TestRunStopButton_TextChanged(object sender, EventArgs e)
        {
            if ((UpdateTestInfo == true) && (TestRunStopButton.Text == "&Start"))
            {
                UpdateTestInfo = false;
                InitializeDUTInfo();
            }
        }

        private bool SaveTest(bool SaveAs)
        {
            BackupAndApplyAppStatus("Saving...");

            bool ReturnValue = MTKTestProgram.SaveTestProgram(SaveAs);

            if (ReturnValue == true)
            {
                this.Text = MTKTestProgram.TestFileName + " - " + BackupWindowText;
                SaveTestMenuItem.Enabled = false;
                if (SupervisorModeMenuItem.Checked == true)
                {
                    SaveTestProgramAsMenuItem.Enabled = true;
                }
            }

            RestoreAppStatus();
            return ReturnValue;
        }

        private void SaveTestMenuItem_Click(object sender, EventArgs e)
        {
            SaveTest(false);
        }

        private void SaveTestProgramAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveTest(true);
        }

        public void LoadTest(bool LoadFromPath, string LoadFilePath)
        {
            BackupAndApplyAppStatus("Loading...");

            if (MTKTestProgram.LoadTestProgram(LoadFromPath, LoadFilePath) == true)
            {
                PopulateTestInfo(MTKTestProgram.TestProgram);
                this.Text = MTKTestProgram.TestFileName + " - " + BackupWindowText;

                PushRecentlyOpened();

                CloseTestProgramMenuItem.Enabled = true;
                SaveTestMenuItem.Enabled = false;
                if (SupervisorModeMenuItem.Checked == true)
                {
                    TestProgramMenuItem.Enabled = true;
                    SaveTestProgramAsMenuItem.Enabled = true;
                }
                TestRunStopButton.Enabled = true;
            }

            RestoreAppStatus();
        }

        private void LoadTestMenuItem_Click(object sender, EventArgs e)
        {
            LoadTest(false, "");
        }

        private void AnyRecentFileMenuItem_Click(object sender, EventArgs e)
        {
            LoadTest(true, sender.ToString());
        }

        private void CloseTestProgram(out DialogResult retValue)
        {
            if (MTKTestProgram.CloseTestProgram(out retValue) == true)
            {
                PopulateTestInfo(MTKTestProgram.TestProgram);

                if (retValue != DialogResult.Cancel)
                {
                    this.Text = BackupWindowText;
                    CloseTestProgramMenuItem.Enabled = false;
                    SaveTestMenuItem.Enabled = false;
                    SaveTestProgramAsMenuItem.Enabled = false;
                    TestProgramMenuItem.Enabled = false;
                    TestRunStopButton.Enabled = false;
                }
            }
        }

        private void CloseTestProgramMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult retValue;
            CloseTestProgram(out retValue);
        }

        private void NewTestProgramMenuItem_Click(object sender, EventArgs e)
        {
            if (SupervisorModeMenuItem.Checked == true)
            {
                if (MTKTestProgram.CreateNewTestProgram() == true)
                {
                    this.Text = MTKTestProgram.TestFileName + "* - " + BackupWindowText;
                    CloseTestProgramMenuItem.Enabled = true;
                    SaveTestMenuItem.Enabled = true;
                    SaveTestProgramAsMenuItem.Enabled = true;
                    TestProgramMenuItem.Enabled = true;
                    TestRunStopButton.Enabled = true;
                    TestProgramMenuItem_Click(this, e);
                }
            }
        }

        private void ResetTestButton_Click(object sender, EventArgs e)
        {
            StopTests();
            ResetTestButton.Enabled = false;
            StopButton.Enabled = false;
            for (int i = 0; i < MTKTestProgram.TestProgram.Count; i++)
            {
                if (TestProgramGridView.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.ClearSelection()));
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Style =
                        new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White }));
                    this.Invoke(new MethodInvoker(() => TestProgramGridView.Rows[i].Cells["Status"].Value = "Queued"));
                }
                else
                {
                    TestProgramGridView.ClearSelection();
                    TestProgramGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White };
                    TestProgramGridView.Rows[i].Cells["Status"].Value = "Queued";
                }
            }

            for (int i = 0; i < DUTInfoDataGridView.Rows.Count; i++)
            {
                if (DUTInfoDataGridView.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.ClearSelection()));
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Style =
                        new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White }));
                    this.Invoke(new MethodInvoker(() => DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "Queued"));
                }
                else
                {
                    DUTInfoDataGridView.ClearSelection();
                    DUTInfoDataGridView.Rows[i].Cells["Status"].Style = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White };
                    DUTInfoDataGridView.Rows[i].Cells["Status"].Value = "Queued";
                }
            }

            Logger.PrintLog(this, "Test program reset.", LogDetailLevel.LogRelevant);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopTests();
        }

        private void UpdateBDA()
        {
            BDAConfigLabel.Text = MTKBDAProgrammer.GetDisplayText();
            BDATextBox.SetTextFromByteArray(MTKBDAProgrammer.BDAddress);

            if (MTKBDAProgrammer.BDAProgrammer.SelectedProgrammer != "")
            {
                BDAWriteButton.Enabled = true;
            }
        }

        private void ConfigBDAButton_Click(object sender, EventArgs e)
        {
            MTKTestBDADialog TempDialog = new MTKTestBDADialog(MTKBDAProgrammer);
            if (TempDialog.ShowDialog() == DialogResult.OK)
            {
                CyBLE_MTK_Application.Properties.Settings.Default.BDA = TempDialog.BDATextBox.GetTextWithoutDelimiters();
                CyBLE_MTK_Application.Properties.Settings.Default.BDAIncrement = MTKBDAProgrammer.AutoIncrementBDA;
                CyBLE_MTK_Application.Properties.Settings.Default.BDAUseProgrammer = MTKBDAProgrammer.UseProgrammer;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
            }

            UpdateBDA();
        }

        private void BDAWriteButton_Click(object sender, EventArgs e)
        {
            ConfigBDAButton.Enabled = false;
            BDAWriteButton.Enabled = false;

            BDAProgrammingThread = new Thread(() => BDAProgram());
            BDAProgrammingThread.Start();
        }

        private void BDAProgram()
        {
            this.Invoke(new MethodInvoker(() => MTKBDAProgrammer.BDAddress = BDATextBox.ToByteArray()));
            Logger.PrintLog(this, "Writing BDA via: " + MTKBDAProgrammer.BDAProgrammer.GetDisplayText(), LogDetailLevel.LogRelevant);
            MTKBDAProgrammer.WriteBDA();
            this.Invoke(new MethodInvoker(() => ConfigBDAButton.Enabled = true));
            this.Invoke(new MethodInvoker(() => BDAWriteButton.Enabled = true));
        }

        private void MTKBDAProgrammer_OnBDAChange(byte[] BDA)
        {
            this.Invoke(new MethodInvoker(() => BDATextBox.SetTextFromByteArray(BDA) ));
        }

        private void MTKSerialPortDialog_OnDUTConnectionStatusChange(string ConnStatus)
        {
            if (ConnStatus == "CONNECTED")
            {
                if (IsHandleCreated)
                {
                    this.Invoke(new MethodInvoker(() => DataBaseStatus.BackColor = Color.Green));
                }
            }
            else if (ConnStatus == "DISCONNECTED")
            {
                if ((CyBLE_MTK_Application.Properties.Settings.Default.ConnectionType == "UART") &&
                    DUTSerialPortDialog.DeviceSerialPort.IsOpen)
                {
                    //this.Invoke(new MethodInvoker(() => DUTSerialPortDialog.DisconnectSerialPort()));
                }
                if (IsHandleCreated)
                {
                    this.Invoke(new MethodInvoker(() => DataBaseStatus.BackColor = Color.Red));
                }
            }
        }

        private void MTKSerialPortDialog_OnHostConnectionStatusChange(string ConnStatus)
        {
            if (ConnStatus == "CONNECTED")
            {
                if (IsHandleCreated)
                {
                    this.Invoke(new MethodInvoker(() => HostStatus.BackColor = Color.Green));
                }
            }
            else if (ConnStatus == "DISCONNECTED")
            {
                if (MTKSerialPortDialog.DeviceSerialPort.IsOpen)
                {
                    this.Invoke(new MethodInvoker(() => MTKSerialPortDialog.DeviceSerialPort.Close()));
                }
                if (IsHandleCreated)
                {
                    this.Invoke(new MethodInvoker(() => HostStatus.BackColor = Color.Red));
                }
            }
        }

        private void TestProgramGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TestProgramGridView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Count() > 0)
            {
                LoadTest(true, files[0]);
            }
        }

        private void BDATextBox_TextChanged(object sender, EventArgs e)
        {
            CyBLE_MTK_Application.Properties.Settings.Default.BDA = BDATextBox.GetTextWithoutDelimiters();
            CyBLE_MTK_Application.Properties.Settings.Default.Save();
        }

        private void anritsuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAndReadyAnritsuPort();
        }

        private void OpenAndReadyAnritsuPort()
        {
            SerialPortSettingsDialog Temp = new SerialPortSettingsDialog(AnritsuSerialPort);
            Temp.Text = "Anritsu Serial Port Setting";
            Temp.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            Temp.ShowDialog();
            if (AnritsuSerialPort.DeviceSerialPort.IsOpen)
            {
                SetupAnritsu();

                CyBLE_MTK_Application.Properties.Settings.Default.AnritsuSerialPort = AnritsuSerialPort.DeviceSerialPort.PortName;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
            }
        }

        private void SetupAnritsu()
        {
            //MTKTestAnritsuDialog Temp = new MTKTestAnritsuDialog();
            //Temp.ShowDialog();
            char[] DelimiterChars = { ',', '\n' };
            int ScriptNum = CyBLE_MTK_Application.Properties.Settings.Default.AnritsuScriptID;//Temp.ScriptID;
            string AnritsuScriptCMD = "SCPTTSTGP " + ScriptNum.ToString() + ",BLETSTS ON";
            AnritsuSerialPort.DeviceSerialPort.WriteLine(AnritsuScriptCMD);
            AnritsuScriptCMD = "SCPTSEL " + ScriptNum.ToString();
            AnritsuSerialPort.DeviceSerialPort.WriteLine(AnritsuScriptCMD);
            Logger.PrintLog(this, "Setting up test script: '" + AnritsuScriptCMD + "' : DONE", LogDetailLevel.LogRelevant);

            Thread.Sleep(100);

            AnritsuSerialPort.DeviceSerialPort.WriteLine("LESSCFG " + ScriptNum.ToString() + ",NUMPKTS," +
                CyBLE_MTK_Application.Properties.Settings.Default.AnritsuNumPKTS.ToString());
            Thread.Sleep(20);
            AnritsuSerialPort.DeviceSerialPort.DiscardInBuffer();
            AnritsuSerialPort.DeviceSerialPort.DiscardOutBuffer();
            string OffsetOutput;
            string[] OffsetOutputBroke;
            int NumRetries = 0;
            do
            {
                AnritsuSerialPort.DeviceSerialPort.WriteLine("LESSCFG? " + ScriptNum.ToString() + ",NUMPKTS");
                Thread.Sleep(100);
                OffsetOutput = AnritsuSerialPort.DeviceSerialPort.ReadExisting();
                OffsetOutputBroke = OffsetOutput.Split(DelimiterChars);
                NumRetries++;
            }
            while ((OffsetOutputBroke.Count() < 3) && (NumRetries < 10));

            if (OffsetOutputBroke.Count() >= 3)
            {
                if (OffsetOutputBroke[2] != CyBLE_MTK_Application.Properties.Settings.Default.AnritsuNumPKTS.ToString())
                {
                    this.Logger.PrintLog(this, "Cannot change number of packets configuration for Anritsu.", LogDetailLevel.LogRelevant);
                }
                else
                {
                    this.Logger.PrintLog(this, "Number of packets for Anritsu configured to: " + CyBLE_MTK_Application.Properties.Settings.Default.AnritsuNumPKTS.ToString(), LogDetailLevel.LogRelevant);
                }
            }
        }

        private void dUTSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialPortSettingsDialog Temp = new SerialPortSettingsDialog(DUTSelectSerialPort);
            Temp.Text = "DUT Multiplexer Serial Port Setting";
            Temp.CloseOnConnect = CyBLE_MTK_Application.Properties.Settings.Default.CloseSerialDialog;
            Temp.ShowDialog();
            if (DUTSelectSerialPort.DeviceSerialPort.IsOpen)
            {
                CyBLE_MTK_Application.Properties.Settings.Default.DUTMultiplexerSerialPort = DUTSelectSerialPort.DeviceSerialPort.PortName;
                CyBLE_MTK_Application.Properties.Settings.Default.Save();
            }
        }

        private bool LoadSettParameters(MTKTest TempTest, XmlNode TestNode)
        {
            if (TempTest.TestParameterCount != Int32.Parse(TestNode["NumberOfParamerters"].InnerText))
            {
                //Logger.PrintLog(this, "Test " + TestNode["TestIndex"].InnerText + ": Number of parameters don't match.",
                //    LogDetailLevel.LogRelevant);
                Logger.PrintLog(this, "Cannot load from file.", LogDetailLevel.LogRelevant);
                return false;
            }

            for (int i = 0; i < TempTest.TestParameterCount; i++)
            {
                if (TempTest.SetTestParameter(i, TestNode[TempTest.GetTestParameterName(i)].InnerText) == false)
                {
                    Logger.PrintLog(this, "Test " + TestNode["TestIndex"].InnerText + ": Unexpected value \"" +
                        TestNode[TempTest.GetTestParameterName(i)].InnerText + "\" for parameter \"" +
                        TempTest.GetTestParameterName(i) + "\".", LogDetailLevel.LogEverything);
                    Logger.PrintLog(this, "Cannot load from file.", LogDetailLevel.LogRelevant);
                    return false;
                }
            }

            return true;
        }

        private void importApplicationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML File|*.xml|All Files|*.*";
            openFileDialog1.Title = "Import Application Settings";
            DialogResult RetVal = openFileDialog1.ShowDialog();

            if ((RetVal == DialogResult.OK) && (openFileDialog1.FileName != ""))
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    throw new FileNotFoundException();
                }

                string FullFileName = openFileDialog1.FileName;
                string NewTestFileName = Path.GetFileName(FullFileName);
                XmlTextReader reader = new XmlTextReader(FullFileName);
                reader.Read();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);

                XmlNodeList DUTNumber = xmlDoc.GetElementsByTagName("NumberOfDUTs");
                if (DUTNumber.Count != 1)
                {
                    Logger.PrintLog(this, "Corrupt file or wrong xml file format.", LogDetailLevel.LogEverything);
                    Logger.PrintLog(this, "Cannot load from file.", LogDetailLevel.LogRelevant);
                    return;
                }

                int NumOfDUTs = Int32.Parse(DUTNumber[0].InnerText);

                XmlNodeList xnl = xmlDoc.SelectNodes("CyBLEMTKAppSett/DUTProgrammer");
                if (xnl.Count != NumOfDUTs)
                {
                    Logger.PrintLog(this, "Corrupt file: DUT Programmer number mismatch.", LogDetailLevel.LogEverything);
                    Logger.PrintLog(this, "Cannot load from file.", LogDetailLevel.LogRelevant);
                    return;
                }

                XmlNodeList xnl1 = xmlDoc.SelectNodes("CyBLEMTKAppSett/DUTSerialPort");
                if (xnl1.Count != NumOfDUTs)
                {
                    Logger.PrintLog(this, "Corrupt file: DUT serial port number mismatch.", LogDetailLevel.LogEverything);
                    Logger.PrintLog(this, "Cannot load from file.", LogDetailLevel.LogRelevant);
                    return;
                }

                CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs = NumOfDUTs;
                //if (NumOfDUTs > 1)
                //{
                //    CyBLE_MTK_Application.Properties.Settings.Default.MultiDUTEnable = true;
                //}
                //else
                //{
                //    CyBLE_MTK_Application.Properties.Settings.Default.MultiDUTEnable = false;
                //}
                int i = 0;
                foreach (XmlNode DUTProg in xnl)
                {
                    if (DUTProg["Name"].InnerText == "MTKPSoCProgrammer")
                    {
                        if (LoadSettParameters(DUTProgrammers[i], DUTProg) == false)
                        {
                            return;
                        }

                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerName[i] = DUTProgrammers[i].SelectedProgrammer;
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVoltage[i] = DUTProgrammers[i].SelectedVoltageSetting;
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPM[i] = DUTProgrammers[i].SelectedAquireMode;
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerConn[i] = DUTProgrammers[i].SelectedConnectorType.ToString();
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerClock[i] = DUTProgrammers[i].SelectedClock;
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerPA[i] = DUTProgrammers[i].PAToString();
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerVerify[i] = DUTProgrammers[i].ValidateAfterProgramming.ToString();
                        CyBLE_MTK_Application.Properties.Settings.Default.DUTProgrammerHexPath[i] = DUTProgrammers[i].SelectedHEXFilePath;

                        i++;
                    }
                }

                i = 0;
                foreach (XmlNode DUTSP in xnl1)
                {
                    SaveDUTSerialPortSettings(i, DUTSP["Name"].InnerText);
                    i++;
                }

                SetupDUTRelated();
            }
        }

        private void exportApplicationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string AppSettFileName, FullAppSettFileName;
            Logger.PrintLog(this, "Saving application setting.", LogDetailLevel.LogRelevant);

            SaveFileDialog AppSettFileDialog = new SaveFileDialog();
            AppSettFileDialog.Filter = "xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
            AppSettFileDialog.FilterIndex = 1;
            //TestProgSaveFileDialog.FileName = "CyBLE_MTK_Settings";// FullFileName;

            //if ((File.Exists(FullFileName) == false) || (SaveAs == true) || (NoFileLoaded == true))
            if (AppSettFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                Logger.PrintLog(this, "Save operation cancelled.", LogDetailLevel.LogRelevant);
                return;
            }

            if (AppSettFileDialog.FilterIndex == 1)
            {
                AppSettFileName = Path.GetFileNameWithoutExtension(AppSettFileDialog.FileName) + ".xml";
            }
            else
            {
                AppSettFileName = Path.GetFileName(AppSettFileDialog.FileName);
            }
            FullAppSettFileName = Path.GetDirectoryName(AppSettFileDialog.FileName) + "\\" + AppSettFileName;

            XmlWriter writer;
            try
            {
                writer = XmlWriter.Create(FullAppSettFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            writer.WriteStartDocument(true);
            writer.WriteStartElement("CyBLEMTKAppSett");
            {
                writer.WriteElementString("Name", AppSettFileName);
                writer.WriteElementString("NumberOfDUTs", CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs.ToString());
                for (int i = 0; i < CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
                {
                    writer.WriteStartElement("DUTProgrammer");
                    {
                        writer.WriteElementString("ProgrammerIndex", i.ToString());
                        writer.WriteElementString("Name", DUTProgrammers[i].ToString());
                        int temp = DUTProgrammers[i].TestParameterCount;
                        writer.WriteElementString("NumberOfParamerters", temp.ToString());
                        for (int j = 0; j < DUTProgrammers[i].TestParameterCount; j++)
                        {
                            writer.WriteElementString(DUTProgrammers[i].GetTestParameterName(j), DUTProgrammers[i].GetTestParameter(j));
                        }
                    }
                    writer.WriteEndElement();
                }
                for (int i = 0; i < CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
                {
                    writer.WriteStartElement("DUTSerialPort");
                    {
                        writer.WriteElementString("SerialPortIndex", i.ToString());
                        writer.WriteElementString("Name", DUTSerialPorts[i].PortName);
                        //int temp = DUTProgrammers[i].TestParameterCount;
                        //writer.WriteElementString("NumberOfParamerters", temp.ToString());
                        //for (int j = 0; j < DUTProgrammers[i].TestParameterCount; j++)
                        //{
                        //    writer.WriteElementString(DUTProgrammers[i].GetTestParameterName(j), DUTProgrammers[i].GetTestParameter(j));
                        //}
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            writer.Close();
        }

        private void setHexFileForAllDUTProgrammersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog TestProgOpenFileDialog = new OpenFileDialog();
            TestProgOpenFileDialog.Filter = "HEX Files (*.hex)|*.hex|All Files (*.*)|*.*";
            TestProgOpenFileDialog.FilterIndex = 1;

            if (TestProgOpenFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }


            for (int i = 0; i < CyBLE_MTK_Application.Properties.Settings.Default.NumDUTs; i++)
            {
                DUTProgrammers[i].SelectedHEXFilePath = TestProgOpenFileDialog.FileName;
            }
        }



        private void sWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Switch_Config_Dialog sw_cfg = new Switch_Config_Dialog();
            sw_cfg.ShowDialog();
        }

        private void yieldToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FPYForm form = new FPYForm();
            form.ShowDialog();
        }

        private void CyBLE_MTK_Load(object sender, EventArgs e)
        {

        }

        private void CleanLogBtn_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.LogTextBox.InvokeRequired == true)
                {
                    this.Invoke(new MethodInvoker(() => this.LogTextBox.Clear()));

                }
                else
                {
                    this.LogTextBox.Clear();
                }
            }
            catch
            { }
        }

        private void SnipLogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.LogTextBox.InvokeRequired == true)
                {
                    this.Invoke(new MethodInvoker(() => this.LogTextBox.SelectAll()));
                    this.Invoke(new MethodInvoker(() => this.LogTextBox.Copy()));

                }
                else
                {
                    this.LogTextBox.SelectAll();
                    this.LogTextBox.Copy();

                    //this.DUTInfoDataGridView.Rows[1].Cells["Serial Port"].Value = "COM6";



                }
            }
            catch
            { }
        }

        private bool stopStats = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Refresh();
            RunCountLabel.Text = "---";
            PassRateLabel.Text = "---";
            CycleTimePerCountLabel.Text = "---";

            stopStats = true;
        }



        private void OnRobotMsgParsed()
        {
            //Logger.PrintLog(this, "OnRobotMsgParsed: " + Thread.CurrentThread.Name + " - " + Thread.CurrentThread.ManagedThreadId.ToString(), LogDetailLevel.LogEverything);
            this.Invoke(new MethodInvoker(() => FillDutInfosAndRunTest()));
        }


        private void _OnRobotServerStateChange(bool start)
        {
            startRobotServerToolStripMenuItem.Text = start ? "Stop &Robot Server" : "Start &Robot Server";
            this.RobotLabel.BackColor = start ? Color.LightGreen : Color.Gray;
        }

        private void OnRobotServerStateChange(bool on)
        {
            if (IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() => _OnRobotServerStateChange(on)));
            }
        }

        private void _OnRobotStateChange(bool on)
        {
            this.RobotLabel.BackColor = on ? Color.Green : Color.LightGreen;
        }

        private void OnRobotStateChange(bool connect)
        {
            if (IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() => _OnRobotStateChange(connect)));
            }
        }


        private void FillDutInfosAndRunTest()
        {
            Logger.PrintLog(this, "FillDutInfosAndRunTest: " + Thread.CurrentThread.Name + " - " + Thread.CurrentThread.ManagedThreadId.ToString(), LogDetailLevel.LogEverything);

            if (DUTsTestFlag.IsFixedSize)
            {
                for (int i = 0; i < DUTsTestFlag.Length; i++)
                {
                    DUTsTestFlag[i] = true;
                }
            }

            lock (CyBLEMTKRobotServer.gServer.PendingDUTInfos)
            {
                for (int i = 0; (i < DUTInfoDataGridView.Rows.Count) && ((i + 1) < CyBLEMTKRobotServer.gServer.PendingDUTInfos.Count()); i++)
                {
                    if (CyBLEMTKRobotServer.gServer.PendingDUTInfos[i + 1] != null && CyBLEMTKRobotServer.gServer.PendingDUTInfos[i + 1].IsValid
                        && CyBLEMTKRobotServer.gServer.PendingDUTInfos[i + 1].TestFlag)
                    {
                        DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value = CyBLEMTKRobotServer.gServer.PendingDUTInfos[i + 1].SerialNumber;
                        DUTsTestFlag[i] = true;
                    }
                    else
                    {
                        DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value = "";
                        DUTsTestFlag[i] = false;
                    }

                }
            }
            TestRunStopButton.PerformClick();
            if (TestRunStopButton.Text == "&Start")
            {
                CyBLEMTKRobotServer.gServer.TestResultsUploadedEvent.Set();
            }
        }

        private void startRobotServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startRobotServerToolStripMenuItem.Text.Contains("StartRobotServer"))
            {
                startRobotServerToolStripMenuItem.Text = "StopRobotServer";

                ///Start Robot Server
                ///Establish Server Socket
                ///Accept Client Socket
                ///Start Session
                ///Stop Session
                ///Disconnect Client Socket
                ///Wait for another Client Socket
                ///


                this.TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Refresh_32px_523850_easyicon_net;
                this.startRobotServerToolStripMenuItem.Text = "StopRobotServer";
                this.RobotLabel.ForeColor = Color.Black;
                this.RobotLabel.BackColor = Color.Gray;


            }
            else
            {
                startRobotServerToolStripMenuItem.Text = "StartRobotServer";

                ///Stop Robot Server
                ///Enter Manual mode
                ///

                this.TestRunStopButton.Image = CyBLE_MTK_Application.Properties.Resources.Go;
                this.startRobotServerToolStripMenuItem.Text = "StartRobotServer";
                this.RobotLabel.ForeColor = Color.White;
                this.RobotLabel.BackColor = Color.LightGray;

            }

            if (CyBLEMTKRobotServer.gServer == null)
            {
                CyBLEMTKRobotServer.gServer = new CyBLEMTKRobotServer(Logger);
                CyBLEMTKRobotServer.gServer.OnServerStateChange += OnRobotServerStateChange;
                CyBLEMTKRobotServer.gServer.OnRobotStateChange += OnRobotStateChange;
                CyBLEMTKRobotServer.gServer.OnRobotMsgParsed += OnRobotMsgParsed;
            }

            if (CyBLEMTKRobotServer.IsGlobalServerActive())
            {
                if (!CyBLEMTKRobotServer.gServer.Stop())
                {
                    MessageBox.Show("Failed to stop robot server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (!CyBLEMTKRobotServer.gServer.Start())
                {
                    MessageBox.Show("Failed to start robot server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dmmSwitchSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DmmSwitchSettingsDialog dialog = new DmmSwitchSettingsDialog(Logger);

            dialog.ShowDialog();

        }

        #region SFCS control
        //==============================
        //  SFCS control   shopfloor by pzho
        //==============================
        public bool CheckPemission()
        {
            bool retVal = false;
            CyBLE_MTK_Application.Properties.Settings.Default.Reset();
            CyBLE_MTK_Application.Properties.Settings.Default.Reload();
            CyBLE_MTK_Application.Properties.Settings.Default.Save();

            string SFCSInterface = CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface;


            //m_SFCS = new SFCS(Logger);
            SFCS m_SFCS = SFCS.GetSFCS(Logger);

            if (CyBLEMTKRobotServer.gServer != null)
            {
                m_SFCS = CyBLEMTKRobotServer.gServer;
                CyBLEMTKRobotServer.gIsSupervisorMode = MTKTestProgram.SupervisorMode;
            }
            else
            {
                m_SFCS = SFCS.GetSFCS(Logger);
            }


            Logger.PrintLog(this, "CheckPemission is in progress...Please wait...", LogDetailLevel.LogRelevant);

            if (shopfloor_permission == null || shopfloor_permission.Length != DUTInfoDataGridView.Rows.Count)
            {
                shopfloor_permission = new bool[DUTInfoDataGridView.Rows.Count];
            }


            

            for (int i = 0; i < DUTInfoDataGridView.Rows.Count; i++)
            {
                string SerialNumber = DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value.ToString();
                string SerialPort = DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString();
                //Model = SerialNumber.Substring(0, 8);

                //add permission info by cysp

                if (SerialNumber == null)
                {
                    SerialNumber = "DUMP_000000000" + i.ToString();
                }

                string permission_info = m_SFCS.PermissonCheck(SerialNumber, "123456", "12345", "MTK");

                int dut_index = i;

                shopfloor_permission[dut_index] = false;

                if (permission_info.ToLower().Contains("pass"))
                {
                    
                    shopfloor_permission[dut_index] = true;
                    retVal = true;
                }

                
                
                if (!shopfloor_permission[dut_index]&&(!DUTInfoDataGridView.Rows[i].Cells["Serial Port"].Value.ToString().Contains("Configure")))
                {
                    //cysp: fixed the display issue which was feedback from SWJ 
                    //MessageBox.Show(m_SFCS.connect_error, "Error:No permission from shopfloor" + SerailNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //MessageBox.Show("DUT　"+ (dut_index + 1).ToString() + " : " + SerialNumber + "\n\n Reason: "　+ permission_info, "Error : No permission from shopfloor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.PrintLog(this, "No permission from shopfloor: DUT　" + (dut_index + 1).ToString() + " : " + SerialNumber + "\n\n Reason: " + permission_info, LogDetailLevel.LogRelevant);



                }

            }

            return retVal;
        }

        private static UInt32 SerialNumberAuto = 1;

        private void manufacturingModeConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManufacturingModeSettings manufacturingModeSettings = new ManufacturingModeSettings();

            manufacturingModeSettings.ShowDialog();



        }

        public bool UploadResult()
        {
            m_SFCS = new SFCS(Logger);
            m_SFCS = SFCS.GetSFCS(Logger);

            if (CyBLE_MTK_Application.Properties.Settings.Default.ShopfloorDataBaseEnable)
            {
                SFCS_DB = new SFCS_DB_Helper(Logger);

            }







            try
            {
                if (CyBLEMTKRobotServer.IsGlobalServerActive())
                {
                    //Let MTKRobotServer.gServer determine whether upload or not tests results.
                    CyBLEMTKRobotServer.gIsSupervisorMode = MTKTestProgram.SupervisorMode;
                    m_SFCS = CyBLEMTKRobotServer.gServer;
                }
                else
                {
                    if (MTKTestProgram.SupervisorMode &&
                        SFCS.GetSFCS(Logger).GetType() != Type.GetType("CyBLE_MTK_Application.SFCS_LOCAL"))
                    {
                        Logger.PrintLog(this, "Test Result won't be upload to shopfloor under the supervisor mode.", LogDetailLevel.LogRelevant);
                        return false;
                    }
                    m_SFCS = SFCS.GetSFCS(Logger); ;

                }

                _UploadResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Shopfloor UploadResult Error");
            }
            finally
            {
                if (CyBLEMTKRobotServer.IsGlobalServerActive())
                {
                    CyBLEMTKRobotServer.gServer.TestResultsUploadedEvent.Set();
                }
            }



            return true;
        }

        private void _UploadResult()
        {
            string SerialNumber;
            string Model;
            int numOftest = TestProgramGridView.RowCount;          //indicate how many Tests
            int numOfDUTs = DUTInfoDataGridView.RowCount;            //indicate how many DUTs

            Logger.PrintLog(this, "UploadTestResult is in progress...Please wait...", LogDetailLevel.LogRelevant);

            //DUT one by one
            for (int i = 0; i < DUTInfoDataGridView.Rows.Count; i++)
            {
                SerialNumber = DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value.ToString();
                Model = "Unknown";



                string DUTTestResultToShopfloor = DUTInfoDataGridView.Rows[i].Cells["Status"].Value.ToString();

                if (SerialNumber.Length > 0)
                {
                    /*
                     * Truncate different model name from SN string for different factory.
                     */
                    int ModelLen = CyBLE_MTK_Application.Properties.Settings.Default.DUTModelLengthOverride;

                    if (ModelLen <= 0)
                    {
                        if (m_SFCS.GetType() == Type.GetType("CyBLE_MTK_Application.SFCS_SIGMA"))
                        {
                            ModelLen = 9;
                        }
                        else
                        {
                            ModelLen = 8;
                        }
                    }

                    if (SerialNumber.Length >= 9)
                    {
                        Model = SerialNumber.Substring(0, ModelLen);
                    }
                    else
                    {

                        Model = "Unknown";
                    }
                }
                else
                {
                    SerialNumber = "Auto-" + SerialNumberAuto.ToString("D15");
                    SerialNumberAuto++;
                }

                //cysp: uploadresult to shopfloor one by one
                string socket_no = (i + 1).ToString();

                int DUT_no = i;




                ECCS errcode_cal = new ECCS();

                errorcode = ECCS.TMPL_ERROR_CODE;

                if (SupervisorModeMenuItem.Checked||m_SFCS.GetType().ToString().ToUpper().Contains("LOCAL"))
                {
                    errorcode = errcode_cal.ReturnFinalDUTOverallSFCSErrCodeforDUT(ProgramStatus[DUT_no],MTKTestProgram.DUTOverallSFCSErrCode[DUT_no], true);

                }
                else
                {
                    errorcode = errcode_cal.ReturnFinalDUTOverallSFCSErrCodeforDUT(ProgramStatus[DUT_no], MTKTestProgram.DUTOverallSFCSErrCode[DUT_no], shopfloor_permission[DUT_no]);
                }


                if (errorcode == ECCS.ERRORCODE_SHOPFLOOR_PROCESS_ERROR)
                {
                    DUTTestResultToShopfloor = "Invalid";
                    //MTKTestProgram_OnIgnoreDUT();
                    MTKTestProgram_OnShopfloorPermissionCheckFail(DUT_no);

                }

                UpdateAllDutTestStatusAndOverallTestResultLabel(MTKTestProgram.DUTOverallSFCSErrCode, MTKTestProgramAll.MTKTestProgramAllTmplSFCSErrCodes, DUT_no);

                ReminderErrorCodeFindingErrorButTestResultPass(MTKTestProgram.DUTOverallSFCSErrCode[DUT_no], DUTTestResultToShopfloor,DUT_no);

                string MFI_ID = "NONE";

                ////for debug use
                //MessageBox.Show("DUT#" + (i+1).ToString() + "\nSN: " + SerialNumber + "\nModel: " + Model+ "\nTestID: "+ TesterID+ "\nErrCode: "+ errorcode + "\nSocket No: "+ socket_no + "\nTestResult: "+ TestResult+"\nStationID: "+ "MTK");
                //Logger.PrintLog(this, "SHOPFLOOR UPLOAD INFO : " + "DUT#" + (i + 1).ToString() + "  SN: " + SerialNumber + "\tModel: " + Model + "\tTesterID: " + TesterID + "\tErrCode: " + errorcode + "\tSocket#: " + socket_no + "\tTestResult: " + TestResult + "\tStationID: " + "MTK" + "\tMFI_ID: " + MFI_ID, LogDetailLevel.LogRelevant);

                //bool upload = m_SFCS.SFCS_UploadTestResult(SerialNumber, Model, TesterID, errorcode, socket_no, TestResult, "MTK", MFI_ID);

                CyBLE_MTK_Application.Properties.Settings.Default.Reset();
                CyBLE_MTK_Application.Properties.Settings.Default.Reload();
                CyBLE_MTK_Application.Properties.Settings.Default.Save();

                bool upload = m_SFCS.UploadTestResult(SerialNumber, Model, TesterID, errorcode, socket_no, DUTTestResultToShopfloor, "MTK", MFI_ID);
                //Logger.PrintLog(this, m_SFCS.GetType().ToString().Substring(22) + " UPLOAD INFO : " + "DUT#" + (i + 1).ToString() + "  SN: " + SerialNumber + "\tModel: " + Model + "\tTesterID: " + TesterID + "\tErrCode: " + errorcode.ToString("X4") + "\tSocket#: " + socket_no + "\tTestResult: " + DUTTestResultToShopfloor + "\t\tStationID: " + "MTK" + "\tMFI_ID: " + MFI_ID, LogDetailLevel.LogRelevant);
                Logger.PrintLog(this, m_SFCS.GetType().ToString().Substring(22) + " UPLOAD INFO : " + "DUT#" + (i + 1).ToString() + "  SN: " + SerialNumber + "    \tModel: " + Model + "       \tTesterID: " + TesterID + "       \tErrCode: " + errorcode.ToString("X4") + "       \tTestResult: " + DUTTestResultToShopfloor, LogDetailLevel.LogRelevant);


                #region ShopfloorAccessDatabase
                if (CyBLE_MTK_Application.Properties.Settings.Default.ShopfloorDataBaseEnable)
                {
                    //SFCS_DB = new SFCS_DB_Helper(Logger);


                    SFCS_DB.InsertRow(SerialNumber + "$" + Model + "$" + "MTK" + "$" + errorcode + "$" + TesterID + "$" + socket_no + "$" + MFI_ID + "$" + LogWriteLine + "$" + DUTTestResultToShopfloor);
                }

                #endregion


                COUNT_ALL++;

                if (DUTTestResultToShopfloor.ToUpper().Contains("PASS"))
                {
                    COUNT_PASS++;
                }
                else
                {
                    COUNT_FAIL++;
                }

                if (!stopStats)
                {
                    this.RunCountLabel.Text = TestProgramManager.RunCount.ToString();
                    this.RunCountLabel.Text = (TestProgramManager.RunCount * numOfDUTs).ToString();



                    //this.PassRateLabel.Text = COUNT_PASS.ToString() + " / " + COUNT_FAIL.ToString();

                    if (COUNT_ALL > 0)
                    {
                        this.PassRateLabel.Text = (Math.Round((((double)COUNT_PASS) / ((double)COUNT_ALL)) * 100, 3)).ToString() + " %";
                        this.CycleTimePerCountLabel.Text = Math.Round(TestProgramManager.stopwatch.Elapsed.TotalSeconds, 3).ToString() + " secs";
                        this.PassCountLabel.Text = COUNT_PASS.ToString();
                    }

                }


                DUTInfoDataGridView.Rows[i].Cells["Unique ID"].Value = "";//clear the SN for next test

            }


            DUTTestResults.Clear();
            errcodes.Clear();



            if (errcodes.Count > 0)
            {
                Logger.PrintLog(this, "Warning: List string[] errcodes is not cleaned and its count is: " + errcodes.Count.ToString(), LogDetailLevel.LogEverything);
                errcodes.Clear();
                Logger.PrintLog(this, "Info: List string[] errcodes has been cleaned and its count is: " + errcodes.Count.ToString(), LogDetailLevel.LogEverything);
            }
            Logger.PrintLog(this, "UploadTestResult is DONE", LogDetailLevel.LogRelevant);

            if (CyBLE_MTK_Application.Properties.Settings.Default.ShopfloorDataBaseEnable)
            {
                Logger.PrintLog(this, $"Update Database {SFCS_DB.MDBfileFullPathWithFileName} is DONE", LogDetailLevel.LogRelevant);
            }
        }

        private void UpdateAllDutTestStatusAndOverallTestResultLabel(ushort[] dUTOverallSFCSErrCode, ushort[] mTKTestProgramAllTmplSFCSErrCodes, int CurrentDut)
        {
            //Update dut test status by error code


        }



        private void ReminderErrorCodeFindingErrorButTestResultPass(int errorcode, string dut_testresult, int DUT_no)
        {
            if (errorcode != ECCS.ERRORCODE_ALL_PASS && dut_testresult.ToLower().Contains("pass"))
            {

                string err_message = "警告！！！ DUT#" + (DUT_no + 1) + "\nERRORCODE: " + errorcode.ToString() + "\nTestResult: " + dut_testresult
                    + "\n报错原因：在某些DUT的PASS测试结果中发现它们的ErrorCode中发现了非0。\n这可能是某些DUT的PORT没有被正确configure好，导致烧录或者某些测试步骤丢失。停止作业，请工程师确认！";
                //MessageBox.Show(err_message, "ErrorCodeFindingErrorButTestResultPass",MessageBoxButtons.OK,MessageBoxIcon.Error);

                Logger.PrintLog(this, err_message, LogDetailLevel.LogRelevant);


            }
        }


        #endregion

    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied. 
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell. 
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,   
            // background, and disabled button for the cell. 
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified. 
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified. 
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.                
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text.  
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class  
                // handle the painting. 
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }

    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class MTKTestStatus
    {
        private MTKTestMessageType messageType;
        public MTKTestMessageType MessageType
        {
            set
            {
                messageType = value;
            }
            get
            {
                return messageType;
            }
        }

        private string msg;
        public string Message
        {
            set
            {
                msg = value;
            }
            get
            {
                return msg;
            }
        }

        public MTKTestStatus()
        {
            MessageType = MTKTestMessageType.None;
            Message = "";
        }

        public MTKTestStatus(MTKTestMessageType msgType) : this()
        {
            MessageType = msgType;
        }

        public MTKTestStatus(string message) : this()
        {
            Message = message;
        }

        public MTKTestStatus(MTKTestMessageType msgType, string message) : this()
        {
            MessageType = msgType;
            Message = message;
        }
    }
}
