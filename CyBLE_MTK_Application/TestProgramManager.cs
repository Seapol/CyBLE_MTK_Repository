using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Xml;
using System.Diagnostics;

namespace CyBLE_MTK_Application
{
    public enum TestProgramState { Run, Running, Pause, Pausing, Paused, Stop, Stopping, Stopped };
    public enum TestManagerError { NoError, TestProgramEmpty, MTKPortClosed, DUTPortClosed };

    public class TestProgramManager
    {
        private static bool NoFileLoaded;
        private static bool FileNotSaved;
        private int NewFileCounter;
        private LogManager Log;
        private SerialPort MTKSerialPort;
        public SerialPort DUTSerialPort;
        private EventWaitHandle PauseTestEvent;
        private static TestProgramState CurrentTestStatus;

        public List<MTKPSoCProgrammer> DUTProgrammers;
        public List<SerialPort> DUTSerialPorts;
        private int NumberOfDUTs;
        public string TestFileName;
        public string FullFileName;
        public List<MTKTest> TestProgram;
        public bool SupervisorMode;
        public MTKTestType CurrentMTKTestType;


        public static Stopwatch stopwatch = new Stopwatch();


        public TestProgramState TestProgramStatus
        {
            get { return CurrentTestStatus; }
        }
        public bool IsFileLoaded
        {
            get { return !NoFileLoaded; }
        }
        public bool IsFileSaved
        {
            get { return !FileNotSaved; }
        }
        public string DUTConnectionType;
        public bool PauseTestsOnFailure;
        private int _CurrentDUT;
        public int CurrentDUT
        {
            get { return _CurrentDUT; }
        }
        private int _CurrentTestIndex;
        public int CurrentTestIndex
        {
            get { return _CurrentTestIndex; }
        }
        private bool _IgnoreDUT;
        public bool IgnoreDUT
        {
            get { return _IgnoreDUT; }
            set { _IgnoreDUT = value; }
        }
        private bool TestStart;
        private bool devTestComplete;
        public bool DeviceTestingComplete
        {
            get
            {
                return devTestComplete;
            }
        }

        private bool testRunning;
        public bool TestRunning
        {
            get
            {
                return testRunning;
            }
        }

        public int TestCaseCount
        {
            get
            {
                return TestProgram.Count();
            }
        }




        private SerialPort AnritsuSerialPort;

        public event TestProgramRunErrorEventHandler OnTestProgramRunError;
        public event TestProgramNextIterationEventHandler OnNextIteration;
        public event TestProgramCurrentIterationEventHandler OnCurrentIterationComplete;
        public event TestProgramNextTestEventHandler OnNextTest;
        public event TestRunErrorEventHandler OnTestError;
        public event TestRunFailEventHandler OnOverallFail;
        public event TestRunPassEventHandler OnOverallPass;
        public event TestProgramPausedEventHandler OnTestPaused;
        public event TestProgramStoppedEventHandler OnTestStopped;
        public event SerialPortEventHandler OnMTKPortOpen;
        public event SerialPortEventHandler OnDUTPortOpen;
        public event SerialPortEventHandler OnAnritsuPortOpen;
        public event TestCompleteEventHandler OnTestComplete;
        public event IgnoreDUTEventHandler OnIgnoreDUT;

        public delegate void TestProgramRunErrorEventHandler(TestManagerError Error, string Message);
        public delegate void TestProgramNextIterationEventHandler(int CurrentIteration);
        public delegate void TestProgramCurrentIterationEventHandler(int CurrentIteration, bool Ignore);
        public delegate void TestProgramNextTestEventHandler(int CurrentTest);
        public delegate void TestRunErrorEventHandler(MTKTestError Error, string Message);
        public delegate void TestRunFailEventHandler();
        public delegate void TestProgramPausedEventHandler();
        public delegate void TestRunPassEventHandler();
        public delegate void TestProgramStoppedEventHandler();
        public delegate void SerialPortEventHandler();
        public delegate void TestCompleteEventHandler();
        public delegate void IgnoreDUTEventHandler();

        public TestProgramManager()
        {
            _CurrentDUT = 0;
            _CurrentTestIndex = 0;
            NumberOfDUTs = 0;
            SupervisorMode = false;
            CurrentTestStatus = TestProgramState.Stopped;
            TestProgram = new List<MTKTest>();
            NoFileLoaded = true;
            FileNotSaved = false;
            NewFileCounter = 1;
            TestFileName = "NewTestProgram" + NewFileCounter.ToString();
            FullFileName = TestFileName + ".xml";
            DUTConnectionType = "BLE";
            PauseTestsOnFailure = true;
            PauseTestEvent = new AutoResetEvent(false);
            Log = new LogManager();
        }

        public TestProgramManager(LogManager Logger)
            : this()
        {
            Log = Logger;
        }

        public TestProgramManager(LogManager Logger, SerialPort MTKPort, SerialPort DUTPort, SerialPort AnritsuPort)
            : this(Logger)
        {
            MTKSerialPort = MTKPort;
            DUTSerialPort = DUTPort;
            AnritsuSerialPort = AnritsuPort;
        }

        public TestProgramManager(LogManager Logger, SerialPort MTKPort, SerialPort DUTPort)
            : this(Logger)
        {
            MTKSerialPort = MTKPort;
            DUTSerialPort = DUTPort;
            
        }

        public bool SaveTestProgram(bool SaveAs)
        {
            Log.PrintLog(this, "Saving test program.", LogDetailLevel.LogRelevant);

            SaveFileDialog TestProgSaveFileDialog = new SaveFileDialog();
            TestProgSaveFileDialog.Filter = "xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
            TestProgSaveFileDialog.FilterIndex = 1;
            TestProgSaveFileDialog.FileName = TestFileName;// FullFileName;

            if ((File.Exists(FullFileName) == false) || (SaveAs == true) || (NoFileLoaded == true))
            {
                if (TestProgSaveFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    Log.PrintLog(this, "Save operation cancelled.", LogDetailLevel.LogRelevant);
                    return false;
                }

                if (TestProgSaveFileDialog.FilterIndex == 1)
                {
                    TestFileName = Path.GetFileNameWithoutExtension(TestProgSaveFileDialog.FileName) + ".xml";
                }
                else
                {
                    TestFileName = Path.GetFileName(TestProgSaveFileDialog.FileName);
                }
                FullFileName = Path.GetDirectoryName(TestProgSaveFileDialog.FileName) + "\\" + TestFileName;
            }
            XmlWriter writer;
            try
            {
                writer = XmlWriter.Create(FullFileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "File operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            writer.WriteStartDocument(true);
            writer.WriteStartElement("CyBLEMTKTestProgram");
            {
                writer.WriteElementString("Name", TestFileName);
                writer.WriteElementString("NumberOfTests", TestProgram.Count.ToString());
                for (int i = 0; i < TestProgram.Count; i++)
                {
                    writer.WriteStartElement("Test");
                    {
                        writer.WriteElementString("TestIndex", i.ToString());
                        writer.WriteElementString("Name", TestProgram[i].ToString());
                        int temp = TestProgram[i].TestParameterCount;
                        writer.WriteElementString("NumberOfParamerters", temp.ToString());
                        for (int j = 0; j < TestProgram[i].TestParameterCount; j++)
                        {
                            writer.WriteElementString(TestProgram[i].GetTestParameterName(j), TestProgram[i].GetTestParameter(j));
                        }
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            writer.Close();

            if (Path.GetFileNameWithoutExtension(TestFileName) == ("NewTestProgram" + NewFileCounter.ToString()))
            {
                NewFileCounter++;
            }
            NoFileLoaded = false;
            FileNotSaved = false;

            Log.PrintLog(this, "Test program successfully saved to " + FullFileName, LogDetailLevel.LogRelevant);
            return true;
        }

        private bool LoadTestParameters(MTKTest TempTest, XmlNode TestNode)
        {
            if (TempTest.TestParameterCount != Int32.Parse(TestNode["NumberOfParamerters"].InnerText))
            {
                Log.PrintLog(this, "Test " + TestNode["TestIndex"].InnerText + ": Number of parameters don't match.",
                    LogDetailLevel.LogRelevant);
                Log.PrintLog(this, "Cannot load file.", LogDetailLevel.LogRelevant);
                return false;
            }

            for (int i = 0; i < TempTest.TestParameterCount; i++)
            {
                if (TempTest.SetTestParameter(i, TestNode[TempTest.GetTestParameterName(i)].InnerText) == false)
                {
                    Log.PrintLog(this, "Test " + TestNode["TestIndex"].InnerText + ": Unexpected value \"" +
                        TestNode[TempTest.GetTestParameterName(i)].InnerText + "\" for parameter \"" +
                        TempTest.GetTestParameterName(i) + "\".", LogDetailLevel.LogEverything);
                    Log.PrintLog(this, "Cannot load file.", LogDetailLevel.LogRelevant);
                    return false;
                }
            }

            return true;
        }

        public bool LoadTestProgram(bool LoadFromPath, string LoadFilePath)
        {
            Log.PrintLog(this, "Loading test program.", LogDetailLevel.LogRelevant);

            if ((FileNotSaved == true) && (SupervisorMode == true) && (NoFileLoaded == false))
            {
                if (MessageBox.Show("Do you want to save - " + TestFileName + "?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    SaveTestProgram(true);
                    if (FileNotSaved == true)
                    {
                        return false;
                    }
                }
            }

            OpenFileDialog TestProgOpenFileDialog = new OpenFileDialog();
            TestProgOpenFileDialog.Filter = "xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
            TestProgOpenFileDialog.FilterIndex = 1;

            if ((File.Exists(LoadFilePath) == false) && (LoadFromPath == true))
            {
                if (MessageBox.Show("File does not exist! Do you want to browse for it?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return false;
                }
            }

            if (LoadFromPath == false)
            {
                if (TestProgOpenFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    Log.PrintLog(this, "Test program load cancelled.", LogDetailLevel.LogRelevant);
                    return false;
                }
                FullFileName = TestProgOpenFileDialog.FileName;
            }
            else
            {
                FullFileName = LoadFilePath;
            }

            string NewTestFileName = Path.GetFileName(FullFileName);
            XmlTextReader reader = new XmlTextReader(FullFileName);
            reader.Read();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            XmlNodeList TestNum = xmlDoc.GetElementsByTagName("NumberOfTests");
            if (TestNum.Count != 1)
            {
                Log.PrintLog(this, "Corrupt file or wrong xml file format.", LogDetailLevel.LogEverything);
                Log.PrintLog(this, "Cannot load file.", LogDetailLevel.LogRelevant);
                return false;
            }

            int NumberOfTests = Int32.Parse(TestNum[0].InnerText);

            XmlNodeList xnl = xmlDoc.SelectNodes("CyBLEMTKTestProgram/Test");
            if (xnl.Count != NumberOfTests)
            {
                Log.PrintLog(this, "Corrupt file: Incorrect number of tests.", LogDetailLevel.LogEverything);
                Log.PrintLog(this, "Cannot load file.", LogDetailLevel.LogRelevant);
                return false;
            }

            List<MTKTest> NewTestProgram = new List<MTKTest>();
            foreach (XmlNode TestNode in xnl)
            {
                if (TestNode["Name"].InnerText == "MTKTestRXPER")
                {
                    MTKTestRXPER TempTest = new MTKTestRXPER(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }

                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestTXPER")
                {
                    MTKTestTXPER TempTest = new MTKTestTXPER(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestTXCW")
                {
                    MTKTestTXCW TempTest = new MTKTestTXCW(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestTXP")
                {
                    MTKTestTXP TempTest = new MTKTestTXP(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestRXP")
                {
                    MTKTestRXP TempTest = new MTKTestRXP(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKPSoCProgrammer")
                {
                    MTKPSoCProgrammer TempTest = new MTKPSoCProgrammer(Log);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestDelay")
                {
                    MTKTestDelay TempTest = new MTKTestDelay(Log);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestBDAProgrammer")
                {
                    if (!IsBDAProgrammerPresent(NewTestProgram))
                    {
                        MTKTestBDAProgrammer TempTest = new MTKTestBDAProgrammer(Log);
                        if (LoadTestParameters(TempTest, TestNode) == false)
                        {
                            return false;
                        }
                        NewTestProgram.Add(TempTest);
                    }
                }
                else if (TestNode["Name"].InnerText == "MTKTestAnritsu")
                {
                    MTKTestAnritsu TempTest = new MTKTestAnritsu(Log);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestSTC")
                {
                    MTKTestSTC TempTest = new MTKTestSTC(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestDUTCurrentMeasure")
                {
                    MTKTestDUTCurrentMeasure TempTest = new MTKTestDUTCurrentMeasure(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestCUS")
                {
                    MTKTestCUS TempTest = new MTKTestCUS(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestProgramAll")
                {
                    MTKTestProgramAll TempTest = new MTKTestProgramAll(Log, MTKSerialPort, DUTSerialPort);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestI2C")
                {
                    MTKTestI2C TempTest = new MTKTestI2C(Log);
                    TempTest.TestParameterCount = int.Parse(TestNode["NumberOfParamerters"].InnerText);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestRSX")
                {
                    MTKTestRSX TempTest = new MTKTestRSX(Log);
                    TempTest.TestParameterCount = int.Parse(TestNode["NumberOfParamerters"].InnerText);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestRBA")
                {
                    MTKTestRBA TempTest = new MTKTestRBA(Log);
                    TempTest.TestParameterCount = int.Parse(TestNode["NumberOfParamerters"].InnerText);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
                else if (TestNode["Name"].InnerText == "MTKTestXOCalibration")
                {
                    MTKTestXOCalibration TempTest = new MTKTestXOCalibration(Log);
                    TempTest.TestParameterCount = int.Parse(TestNode["NumberOfParamerters"].InnerText);
                    if (LoadTestParameters(TempTest, TestNode) == false)
                    {
                        return false;
                    }
                    NewTestProgram.Add(TempTest);
                }
            }
            TestProgram.Clear();
            TestProgram = NewTestProgram;

            TestFileName = NewTestFileName;

            NoFileLoaded = false;
            FileNotSaved = false;

            Log.PrintLog(this, "Test program successfully loaded from " + FullFileName, LogDetailLevel.LogRelevant);
            return true;
        }

        private bool IsBDAProgrammerPresent(List<MTKTest> TP)
        {
            for (int i = 0; i < TP.Count; i++)
            {
                if (TP[i].ToString() == "MTKTestBDAProgrammer")
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnritsuTestPresent()
        {
            for (int i = 0; i < TestProgram.Count; i++)
            {
                if (TestProgram[i].ToString() == "MTKTestAnritsu")
                {
                    return true;
                }
            }
            return false;
        }

        public bool CloseTestProgram(out DialogResult retValue)
        {
            retValue = DialogResult.Yes;

            if ((FileNotSaved == true) && (SupervisorMode == true) && (NoFileLoaded == false))
            {
                retValue = MessageBox.Show("Do you want to save - " + TestFileName + "?", "Information",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (retValue == DialogResult.Yes)
                {
                    SaveTestProgram(true);
                    if (FileNotSaved == true)
                    {
                        return false;
                    }
                }
            }

            if (retValue != DialogResult.Cancel)
            {
                TestProgram.Clear();
                if (NoFileLoaded == false)
                {
                    Log.PrintLog(this, TestFileName + " - test program closed.", LogDetailLevel.LogRelevant);
                }
                NoFileLoaded = true;
            }

            return true;
        }

        public void TestProgramEdited()
        {
            FileNotSaved = true;
            NoFileLoaded = false;
        }

        public bool CreateNewTestProgram()
        {
            DialogResult retValue;
            if (CloseTestProgram(out retValue) == true)
            {
                if ((FileNotSaved == false) && (NoFileLoaded == false))
                {
                    NewFileCounter++;
                }
                TestFileName = "NewTestProgram" + NewFileCounter.ToString();
                FullFileName = TestFileName + ".xml";
                FileNotSaved = true;
                NoFileLoaded = false;
                Log.PrintLog(this, TestFileName + " - test program created.", LogDetailLevel.LogRelevant);
                return true;
            }

            return false;
        }

        public void StopTestProgram()
        {
            if ((CurrentTestStatus != TestProgramState.Stopped)
                && (CurrentTestStatus != TestProgramState.Stopping)
                && (CurrentTestStatus != TestProgramState.Stop))
            {
                CurrentTestStatus = TestProgramState.Stop;
            }

            PauseTestEvent.Set();
        }

        public void PauseTestProgram()
        {
            CurrentTestStatus = TestProgramState.Pause;
            PauseTestEvent.Reset();
            Log.PrintLog(this, "Pausing test program.", LogDetailLevel.LogRelevant);
        }

        public void ContinueTestProgram()
        {
            CurrentTestStatus = TestProgramState.Run;
            Log.PrintLog(this, "Continuing test program.", LogDetailLevel.LogRelevant);
            PauseTestEvent.Set();
        }

        public void TestRunViability()
        {
            if (TestProgram.Count <= 0)
            {
                OnTestProgramRunError(TestManagerError.TestProgramEmpty, "Test program empty.");
                StopTestProgram();
                return;
            }
        }





        public MTKTestError RunTest(int TestIndex)
        {
            Log.PrintLog(this, "Running test program " + (TestIndex + 1).ToString() + "/" +
                TestProgram.Count.ToString(), LogDetailLevel.LogRelevant);

            TestProgram[TestIndex].UpdateDUTPort(DUTSerialPort);
            TestProgram[TestIndex].DUTConnectionMode = MTKTest.GetConnectionModeFromText(DUTConnectionType);

            string TestType = TestProgram[TestIndex].ToString();

            CurrentMTKTestType = MTKTestType.MTKTest;

            MTKTestError ReturnError;
            if (CheckPorts(TestType, out ReturnError) == false)
            {
                return ReturnError;
            }

            if (TestType == "MTKTestProgramAll")
            {
                ((MTKTestProgramAll)TestProgram[TestIndex]).NumberOfDUTs = NumberOfDUTs;
                ((MTKTestProgramAll)TestProgram[TestIndex]).CurrentDUT = _CurrentDUT;
                ((MTKTestProgramAll)TestProgram[TestIndex]).DUTProgrammers = DUTProgrammers;
                ((MTKTestProgramAll)TestProgram[TestIndex]).DUTSerialPorts = DUTSerialPorts;

                CurrentMTKTestType = MTKTestType.MTKTestProgramAll;
            }

            TestProgram[TestIndex].CurrentDUT = _CurrentDUT;
            if (TestType == "MTKTestDUTCurrentMeasure")
            {
                ((MTKTestDUTCurrentMeasure)TestProgram[TestIndex]).NumberOfDUTs = NumberOfDUTs;
                ((MTKTestDUTCurrentMeasure)TestProgram[TestIndex]).DUTSerialPorts = DUTSerialPorts;
                ((MTKTestDUTCurrentMeasure)TestProgram[TestIndex]).CurrentDUT = _CurrentDUT + 1;

                CurrentMTKTestType = MTKTestType.MTKTestDUTCurrentMeasure;
            }

            if (TestType == "MTKTestAnritsu")
            {
                ((MTKTestAnritsu)TestProgram[TestIndex]).AnritsuPort = AnritsuSerialPort;

                CurrentMTKTestType = MTKTestType.MTKTestAnritsu;

            }

            MTKTestError RetVal;

            OnNextTest(TestIndex);
            if (TestType == "MTKPSoCProgrammer")
            {
                CurrentMTKTestType = MTKTestType.MTKPSoCProgrammer;

                if (((MTKPSoCProgrammer)TestProgram[TestIndex]).GlobalProgrammerSelected)
                {
                    RetVal = DUTProgrammers[_CurrentDUT].RunTest();
                }
                else
                {
                    RetVal = TestProgram[TestIndex].RunTest();
                }
            }
            else
            {
                if (DUTSerialPorts[_CurrentDUT].IsOpen)
                {
                    RetVal = TestProgram[TestIndex].RunTest();
                }
                else
                {
                    RetVal = MTKTestError.IgnoringDUT;
                }
            }

            

            OnTestComplete();
            return RetVal;
        }

        private MTKTestError RunAllTests()
        {
            bool FailedOnce = false;

            testRunning = true;



            for (int i = 0; i < TestProgram.Count; i++)
            {
                _CurrentTestIndex = i;
                if (CurrentTestStatus == TestProgramState.Stop)
                {
                    CurrentTestStatus = TestProgramState.Stopping;
                    StopTestProgram();
                    CurrentTestStatus = TestProgramState.Stopped;
                    OnTestStopped();
                    break;
                }

                TestStart = true;

                MTKTestError TestResult = RunTest(i);




                if (TestResult != MTKTestError.NoError)
                {
                    if (TestResult == MTKTestError.IgnoringDUT|| TestResult == MTKTestError.ProgrammerNotConfigured && CyBLE_MTK.DUTsTestFlag[_CurrentDUT])
                    {
                        OnIgnoreDUT();
                        return MTKTestError.IgnoringDUT;
                    }
                    else if (TestResult == MTKTestError.NotAllDevicesProgrammed)
                    {
                        return MTKTestError.NotAllDevicesProgrammed;
                    }
                    else
                    {
                        if ((CurrentTestStatus != TestProgramState.Pausing) &&
                            (CurrentTestStatus != TestProgramState.Paused) &&
                            (CurrentTestStatus != TestProgramState.Pause) &&
                            (PauseTestsOnFailure == true))
                        {
                            CurrentTestStatus = TestProgramState.Pause;
                        }
                        OnOverallFail();
                        FailedOnce = true;
                        OnTestError(TestResult, TestResult.ToString());
                    }
                }

                if (CurrentTestStatus == TestProgramState.Pause)
                {
                    CurrentTestStatus = TestProgramState.Paused;
                    Log.PrintLog(this, "Test program paused.", LogDetailLevel.LogRelevant);
                    OnTestPaused();
                    while (!PauseTestEvent.WaitOne(100)) ;
                    if (CurrentTestStatus == TestProgramState.Stop)
                    {
                        StopTestProgram();
                        testRunning = false;
                        if (FailedOnce)
                        {

                            return MTKTestError.TestFailed;
                        }
                        else
                        {
                            return MTKTestError.NoError;
                        }
                    }
                    CurrentTestStatus = TestProgramState.Running;
                }

                ///cysp: SetDUTOverallSFCSErrorCodeForUploadTestResult

                SetDUTTmplSFCSErrorCode(_CurrentDUT, _CurrentTestIndex);
            }


            testRunning = false;





            if (FailedOnce)
            {
                return MTKTestError.TestFailed;
            }


            else
            {
                return MTKTestError.NoError;
            }


        }

        public int LongRunCnt = 0;

        public static int RunCount = 1;


        public void RunTestProgram(int NumIteration)
        {
            NumberOfDUTs = NumIteration;
            DUTOverallSFCSErrCode = new UInt16[NumberOfDUTs];



            DUTTmplSFCSErrCode = new UInt16[NumberOfDUTs, TestProgram.Count];

            if (!CyBLEMTKRobotServer.IsGlobalServerActive() && CyBLE_MTK_Application.Properties.Settings.Default.TestModeLongRun > 0)
            {
                for (int i = 0; i < CyBLE_MTK_Application.Properties.Settings.Default.TestModeLongRun; i++)
                {

                    Log.PrintLog(this, "###########LONG_RUN[" + LongRunCnt.ToString() + "]##############", LogDetailLevel.LogRelevant);


                    _RunTestProgram(NumIteration);
                    LongRunCnt++;
                    RunCount++;




                    if (SupervisorMode)
                    {
                        break;
                    }
                    else if (CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface.ToLower().Contains("sigma"))
                    {
                        break;
                    }
                    else if (CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface.ToLower().Contains("fittec"))
                    {
                        break;
                    }


                }
            }
            else
            {
                //Normal mode



                _RunTestProgram(NumIteration);
                
                RunCount++;

            }



        }




        public void _RunTestProgram(int NumIteration)
        {
            bool _Ignore;
            MTKTestError ReturnValue = MTKTestError.NoError;

            CurrentTestStatus = TestProgramState.Running;
            NumberOfDUTs = NumIteration;
            TestRunViability();

            TestStart = false;

            try
            {
                stopwatch.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "StopWatch of TestProgramManager");
            }



            for (int j = 0; j < NumIteration; j++)
            {
                devTestComplete = false;
                _Ignore = false;
                _CurrentDUT = j;
                Log.PrintLog(this, "Selecting DUT " + (j + 1).ToString() + "/" +
                    NumIteration.ToString() + " for tests", LogDetailLevel.LogRelevant);
                OnNextIteration(j);

                if (CyBLE_MTK.DUTsTestFlag[j])
                {
                    ReturnValue = RunAllTests();
                }
                else
                {
                    ReturnValue = MTKTestError.IgnoringDUT;
                }
                
                devTestComplete = true;

                if ((CurrentTestStatus == TestProgramState.Stop) ||
                    (CurrentTestStatus == TestProgramState.Stopped) ||
                    (CurrentTestStatus == TestProgramState.Stopping))
                {
                    break;
                }

                if (ReturnValue == MTKTestError.NoError && CyBLE_MTK.DUTsTestFlag[_CurrentDUT])
                {
                    if (TestStart)
                    {
                        OnOverallPass();
                        Log.PrintLog(this, "Overall test result: PASS", LogDetailLevel.LogRelevant);

                    }

                    //CyBLE_MTK.COUNT_PASS++;
                }
                else if (ReturnValue == MTKTestError.TestFailed||ReturnValue==MTKTestError.NotAllDevicesProgrammed && CyBLE_MTK.DUTsTestFlag[_CurrentDUT])
                {
                    OnOverallFail();
                    Log.PrintLog(this, "Overall test result: FAIL", LogDetailLevel.LogRelevant);

                    //CyBLE_MTK.COUNT_FAIL++;

                }
                else
                {
                    OnIgnoreDUT();
                    Log.PrintLog(this, "Ignoring DUT# " + _CurrentDUT.ToString(), LogDetailLevel.LogRelevant);
                    DUTOverallSFCSErrCode[_CurrentDUT] = ECCS.ERRORCODE_DUT_NOT_TEST;
                    _Ignore = true;
                }

                //CyBLE_MTK.COUNT_ALL++;


                OnCurrentIterationComplete(j, _Ignore);




            }



            SetDUTOverallSFCSErrCode();


            StopTestProgram();
            OnTestStopped();
            _CurrentDUT = 0;
            _CurrentTestIndex = 0;
            CurrentTestStatus = TestProgramState.Stopped;

            try
            {
                stopwatch.Stop();

            }
            catch
            {
                MessageBox.Show("Fail to stopwatch");

            }


        }

        private bool CheckPorts(string TestType, out MTKTestError err)
        {
            bool ReturnValue = true, CheckDUTPort = false;

            err = MTKTestError.NoError;

            if ((TestType == "MTKTestTXP") || (TestType == "MTKTestTXPER") || (TestType == "MTKTestRXP") ||
                            (TestType == "MTKTestRXPER") || (TestType == "MTKTestTXCW") || (TestType == "MTKTestSTC"))
            {
                if (MTKSerialPort.IsOpen == false)
                {
                    OnMTKPortOpen();
                    if (MTKSerialPort.IsOpen == false)
                    {
                        err = MTKTestError.MissingMTKSerialPort;
                        return false;
                    }
                }

                CheckDUTPort = true;
            }

            if ((TestType == "MTKTestCUS") || (TestType == "MTKTestXOCalibration") || (TestType == "MTKTestProgramAll"))
            {
                CheckDUTPort = true;
            }

            if (TestType == "MTKTestAnritsu")
            {
                if (AnritsuSerialPort.IsOpen == false)
                {
                    OnAnritsuPortOpen();
                    if (AnritsuSerialPort.IsOpen == false)
                    {
                        err = MTKTestError.MissingAnritsuSerialPort;
                        return false;
                    }
                }

                CheckDUTPort = true;
            }

            if (CheckDUTPort)
            {
                if ((DUTSerialPort.IsOpen == false) && (DUTConnectionType == "UART"))
                {
                    if (!_IgnoreDUT)
                    {
                        OnDUTPortOpen();
                    }
                    if ((DUTSerialPort.IsOpen == false) && (DUTConnectionType == "UART"))
                    {
                        if (_IgnoreDUT)
                        {
                            err = MTKTestError.IgnoringDUT;
                        }
                        else
                        {
                            err = MTKTestError.MissingDUTSerialPort;
                        }
                        return false;
                    }
                }
            }

            return ReturnValue;
        }


        //Added by cysp 2018-06-20

        #region TestErrorCodeAssignment

        /// <summary>
        /// DUTTmplSFCSErrCode is a two-dimension Uint16 Array
        /// [Dut_index , TestProgram_index]
        /// 
        /// 
        /// DUTOverallSFCSErrCode is a one-dimension Uint16 Array and it's public to let CyBLE Mainform call
        /// [Dut_index]
        /// 
        /// 
        /// </summary>
        public UInt16[,] DUTTmplSFCSErrCode;
        public UInt16[] DUTOverallSFCSErrCode;

        private void SetDUTTmplSFCSErrorCode(int Dut_Index ,int TestProgramIndex)
        {

            try
            {


                if (TestProgram[TestProgramIndex].CurrentMTKTestType != MTKTestType.MTKTestProgramAll)
                {
                    if (TestProgram[TestProgramIndex].CurrentMTKTestType == MTKTestType.MTKTestDelay)
                    {
                        DUTTmplSFCSErrCode[Dut_Index, TestProgramIndex] = ECCS.ERRORCODE_ALL_PASS;
                    }
                    else
                    {
                        DUTTmplSFCSErrCode[Dut_Index, TestProgramIndex] = TestProgram[TestProgramIndex].MTKTestTmplSFCSErrCode;
                    }                  

                }
                else
                {

                    for (int i = 0; i < NumberOfDUTs; i++)
                    {
                        if (CurrentTestIndex != 0)
                        {
                            DUTTmplSFCSErrCode[i, CurrentTestIndex] = MTKTestProgramAll.MTKTestProgramAllTmplSFCSErrCodes[i+ NumberOfDUTs];
                        }
                        else
                        {
                            DUTTmplSFCSErrCode[i, CurrentTestIndex] = MTKTestProgramAll.MTKTestProgramAllTmplSFCSErrCodes[i];
                        }

                        


                    }
                    

                }
            }
            catch (Exception ex)
            {
                Log.PrintLog(this, "Exception from SetDUTTmplSFCSErrorCode: " + ex.ToString(),LogDetailLevel.LogRelevant);
            }

            //fill in AllProg ErrorCode




        }

        private void SetDUTOverallSFCSErrCode()
        {

            try
            {
                if (DUTOverallSFCSErrCode == null)
                {
                    DUTOverallSFCSErrCode = new UInt16[NumberOfDUTs];

                }



                if (DUTTmplSFCSErrCode != null)
                {
                    for (int i = 0; i < NumberOfDUTs; i++)
                    {
                        for (int j = 0; j < TestProgram.Count; j++)
                        {
                            if (DUTTmplSFCSErrCode[i,j] != ECCS.ERRORCODE_ALL_PASS)
                            {
                                DUTOverallSFCSErrCode[i] = DUTTmplSFCSErrCode[i,j];
                                if (DUTTmplSFCSErrCode[i, j] == ECCS.ERRORCODE_DUT_NOT_TEST)
                                {
                                    //OnIgnoreDUT();
                                }
                                else
                                {
                                    OnOverallFail();
                                }
                                
                                break;
                            }
                            else
                            {
                                OnOverallPass();
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

    }
}
