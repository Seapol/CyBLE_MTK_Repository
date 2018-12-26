using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;
using System.Reflection;

namespace CyBLE_MTK_Application
{
    [Flags]
    public enum LogDetailLevel {NoLogs = 1, LogRelevant, LogEverything};

    public class LogManager
    {
        private StreamWriter LogFile;

        public string ApplicationLogFileName;
        public LogDetailLevel LogDetails;
        public bool EnableTestResultLogging;
        public TextBox LogTextBox;
        private bool CanFileBeCreated;
        private string _TestResultsLogPath;
        public string TestResultsLogPath
        {
            get { return _TestResultsLogPath; }
            set
            {
                _TestResultsLogPath = (value == "") ? value : (value + "\\");
            }
        }

        private StreamWriter TestLogFile;
        private string TestLogFileName;

        public bool IsTestLogFileOpen
        {
            get
            {
                return (LogFile != null);
            }
        }


        delegate void SetTextCallback(string text);

        public LogManager()
        {
            LogDetails = LogDetailLevel.LogRelevant;
            LogTextBox = new TextBox();
            ApplicationLogFileName = "";
            _TestResultsLogPath = "";
            LogFile = null;
            TestLogFileName = "";
        }

        public LogManager(LogDetailLevel SetLogDetails) : this()
        {
            LogDetails = SetLogDetails;
        }

        public LogManager(LogDetailLevel SetLogDetails, TextBox SetLogTextBox) : 
            this(SetLogDetails)
        {
            LogTextBox = SetLogTextBox;
            this.PrintLog(this, "Started", LogDetailLevel.LogRelevant);
        }

        public LogManager(LogDetailLevel SetLogDetails, TextBox SetLogTextBox, string LogFilePath) : 
            this(SetLogDetails, SetLogTextBox)
        {
            LogFilePath = (LogFilePath == "") ? LogFilePath : (LogFilePath + "\\");
            ApplicationLogFileName = LogFilePath + "CyBLE_MTK_Log_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".txt";
            CanFileBeCreated = true;
            CreateLogFile();
            this.PrintLog(this, "Started", LogDetailLevel.LogRelevant);
        }

        private void CreateLogFile()
        {
            bool temp = true;
            try
            {
                if (LogDetails != LogDetailLevel.NoLogs)
                {
                    LogFile = new StreamWriter(ApplicationLogFileName);
                    LogFile.AutoFlush = true;
                }
                else
                {
                    LogFile = null;
                }
                temp = false;
            }
            catch
            {
                temp = true;
            }
            if (temp == true)
            {
                CanFileBeCreated = false;
                this.PrintLog(this, "Unable to open log file for write. Use \"Edit > Preferences > Logs >" +
                    "Application Logs\" to select a location.", LogDetailLevel.LogRelevant);
            }
            else
            {
                this.PrintLog(this, "Logs save to: " + ApplicationLogFileName, LogDetailLevel.LogEverything);
            }
        }

        public bool PrintLog(object Sender, string Message, LogDetailLevel PrintDetailLevel)
        {
            if (LogDetails >= PrintDetailLevel)
            {
                string TimeStamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");

                if ((CanFileBeCreated == true) && (LogFile == null))
                {
                    CreateLogFile();
                }

                if (this.LogTextBox.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(LogTextBox.AppendText);
                    this.LogTextBox.Invoke(d, new object[] { TimeStamp + ": "
                            + Sender.GetType().Name + ": " + Message + Environment.NewLine });
                }
                else
                {
                    LogTextBox.AppendText(TimeStamp + ": "
                        + Sender.GetType().Name + ": " + Message + Environment.NewLine);

                }

                if (LogFile != null)
                {
                    LogFile.WriteLine(TimeStamp + ": "
                        + Sender.GetType().Name + ": " + Message);
                }

                return true;
            }
            return false;
        }

        public void StopLogging()
        {
            this.PrintLog(this, "Stopped", LogDetailLevel.LogRelevant);
            LogTextBox = new TextBox();
            StopWritingToLogFile();
        }

        private void StopWritingToLogFile()
        {
            if (LogFile != null)
            {
                LogFile.Close();
                LogFile = null;
            }
        }

        public bool OpenTestLogFile(string FileName)
        {
            if (TestLogFile != null)
            {
                TestLogFile.Close();
            }

            TestLogFile = null;
            try
            {
                TestLogFile = new StreamWriter(FileName);
                TestLogFile.AutoFlush = true;
            }
            catch
            {
                this.PrintLog(this, "Unable to open log file for write. Use \"Edit > Preferences > Logs >" +
                    "Test Logs\" to select a location.", LogDetailLevel.LogRelevant);
                return false;
            }

            TestLogFileName = FileName;
            return true;
        }

        public void CloseTestLogFile()
        {
            TestLogFile.Close();
        }

        private void WriteTestLogCSV(List<MTKTestResult> TestResults)
        {
            this.PrintLog(this, "Logging test results to: " + TestLogFileName, LogDetailLevel.LogRelevant);

            string LogWriteLine = "";

            for (int i = 0; i < TestResults.Count; i++)
            {
                LogWriteLine = "Test Name," + TestResults[i].TestName + "," + "Pass Criterion," + TestResults[i].PassCriterion + "," +
                    "Measured Value," + TestResults[i].Measured + "," + "Overall Test Result," + TestResults[i].Result;
                for (int j = 0; j < TestResults[i].Parameters.Count(); j++)
                {
                    LogWriteLine += "," + TestResults[i].Parameters[j] + "," + TestResults[i].Value[j];
                }

                CyBLE_MTK.LogWriteLine = LogWriteLine;
                TestLogFile.WriteLine(LogWriteLine);
            }
        }

        public void WriteTestLog(List<MTKTestResult> TestResults)
        {
            WriteTestLogCSV(TestResults);
        }

        private bool LogTestResultsCSV(string FileName, List<MTKTestResult> TestResults)
        {
            if (OpenTestLogFile(FileName) == false)
            {
                return false;
            }

            WriteTestLog(TestResults);

            CloseTestLogFile();
            return true;
        }

        public bool LogTestResults(string FileName, List<MTKTestResult> TestResults)
        {
            if (!EnableTestResultLogging)
            {
                return true;
            }
            return LogTestResultsCSV(FileName, TestResults);
        }

        // save log for each day by cysp
        public string GenerateTestResultsFileName(string Prefix)
        {
            string datestamp = DateTime.Now.ToString("yyyy-MM-dd");
            //default testlogPath is under C:\\BLE_TestLogs\\yyyy-MM-dd\\
            string testlogPath = CyBLE_MTK_Application.Properties.Settings.Default.TestLogPath + datestamp + "\\";

            DirectoryInfo di = new DirectoryInfo(testlogPath);
            if (!di.Exists)
            {
                di.Create();
            }


            Prefix = (Prefix == "") ? Prefix : Prefix + "_";
            return _TestResultsLogPath + datestamp + "\\" + Prefix + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".csv";
        }

        public void TestLogWriteString(string LogString)
        {
            TestLogFile.Write(LogString);
        }

        public void TestLogWriteLine(string LogString)
        {
            TestLogFile.WriteLine(LogString);
        }
    }
}
