using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace CyBLE_MTK_Application
{
    public class MTKTestProgramAll : MTKTest
    {
        //public int CurrentDUT;
        //public int NumberOfDUTs;
        public List<MTKPSoCProgrammer> DUTProgrammers;
        public bool ProgramAllAtEnd = false;

        public static bool MTKTestProgramAllAtEnd = false;
        public string SelectedHEXFilePath;

        //public string[] ErrCodes_Tmpl_for_Allprogram;

        //public List<SerialPort> DUTSerialPorts;


        private bool programCompleted;
        public bool ProgramCompleted
        {
            get
            {
                return programCompleted;
            }
        }

        public bool EnableModuleVerification, ApplicationVersionEnable, BLEStackVersionEnable, ProtocolVersionEnable, BootCauseEnable, MACAddressEnable, HWIDEnable;
        public string EventType, ApplicationVersion, BLEStackVersion, ProtocolVersion, BootCause, MACAddress, HWIDValue;
        public int UARTCaptureDelay;

        public event ProgramAllCompleteEventHandler OnProgramAllComplete;
        public event NumTestStatusUpdateEventHandler OnNumTestStatusUpdate;

        public delegate void ProgramAllCompleteEventHandler(List<MTKTestError> err);
        public delegate void NumTestStatusUpdateEventHandler(int Num, string Message);

        public MTKTestProgramAll()
            : base()
        {
            Init();
        }

        public MTKTestProgramAll(LogManager Logger)
            : base(Logger)
        {
            Init();
        }

        public MTKTestProgramAll(LogManager Logger, SerialPort MTKPort, SerialPort DUTPort)
            : base(Logger, MTKPort, DUTPort)
        {
            Init();
        }

        private void Init()
        {
            

            SelectedHEXFilePath = "";
            EnableModuleVerification = false;
            ApplicationVersionEnable = false;
            BLEStackVersionEnable = false;
            ProtocolVersionEnable = false;
            BootCauseEnable = false;
            MACAddressEnable = false;
            HWIDEnable = false;
            EventType = "BOOT";
            ApplicationVersion = "";
            BLEStackVersion = "";
            ProtocolVersion = "";
            BootCause = "";
            MACAddress = "";
            HWIDValue = "";
            UARTCaptureDelay = 50;
            programCompleted = false;
            TestParameterCount = 17;
            NumberOfDUTs = 0;
            CurrentDUT = 0;

        }

        public override string GetDisplayText()
        {
            string temp = (ProgramAllAtEnd)?"at the end.":"at the begning.";
            return "Program all devices " + temp;
        }

        public override string GetTestParameter(int TestParameterIndex)
        {
            switch (TestParameterIndex)
            {
                case 0:
                    return ProgramAllAtEnd.ToString();
                case 1:
                    return SelectedHEXFilePath;
                case 2:
                    return EnableModuleVerification.ToString();
                case 3:
                    return UARTCaptureDelay.ToString();
                case 4:
                    return EventType;
                case 5:
                    return ApplicationVersionEnable.ToString();
                case 6:
                    return ApplicationVersion;
                case 7:
                    return BLEStackVersionEnable.ToString();
                case 8:
                    return BLEStackVersion;
                case 9:
                    return ProtocolVersionEnable.ToString();
                case 10:
                    return ProtocolVersion;
                case 11:
                    return HWIDEnable.ToString();
                case 12:
                    return HWIDValue;
                case 13:
                    return BootCauseEnable.ToString();
                case 14:
                    return BootCause;
                case 15:
                    return MACAddressEnable.ToString();
                case 16:
                    return MACAddress;
            }
            return base.GetTestParameter(TestParameterIndex);
        }

        public override string GetTestParameterName(int TestParameterIndex)
        {
            switch (TestParameterIndex)
            {
                case 0:
                    return "ProgramAllAtEnd";
                case 1:
                    return "SelectedHEXFilePath";
                case 2:
                    return "EnableModuleVerification";
                case 3:
                    return "UARTCaptureDelay";
                case 4:
                    return "EventType";
                case 5:
                    return "ApplicationVersionEnable";
                case 6:
                    return "ApplicationVersion";
                case 7:
                    return "BLEStackVersionEnable";
                case 8:
                    return "BLEStackVersion";
                case 9:
                    return "ProtocolVersionEnable";
                case 10:
                    return "ProtocolVersion";
                case 11:
                    return "HWIDEnable";
                case 12:
                    return "HWIDValue";
                case 13:
                    return "BootCauseEnable";
                case 14:
                    return "BootCause";
                case 15:
                    return "MACAddressEnable";
                case 16:
                    return "MACAddress";
            }
            return base.GetTestParameterName(TestParameterIndex);
        }

        public override bool SetTestParameter(int TestParameterIndex, string ParameterValue)
        {
            //if (ParameterValue == "")
            //{
            //    return false;
            //}
            switch (TestParameterIndex)
            {
                case 0:
                    return bool.TryParse(ParameterValue, out ProgramAllAtEnd);
                case 1:
                    SelectedHEXFilePath = ParameterValue;
                    return true;
                case 2:
                    return bool.TryParse(ParameterValue, out EnableModuleVerification);
                case 3:
                    return int.TryParse(ParameterValue, out UARTCaptureDelay);
                case 4:
                    EventType = ParameterValue;
                    return true;
                case 5:
                    return bool.TryParse(ParameterValue, out ApplicationVersionEnable);
                case 6:
                    ApplicationVersion = ParameterValue;
                    return true;
                case 7:
                    return bool.TryParse(ParameterValue, out BLEStackVersionEnable);
                case 8:
                    BLEStackVersion = ParameterValue;
                    return true;
                case 9:
                    return bool.TryParse(ParameterValue, out ProtocolVersionEnable);
                case 10:
                    ProtocolVersion = ParameterValue;
                    return true;
                case 11:
                    return bool.TryParse(ParameterValue, out HWIDEnable);
                case 12:
                    HWIDValue = ParameterValue;
                    return true;
                case 13:
                    return bool.TryParse(ParameterValue, out BootCauseEnable);
                case 14:
                    BootCause = ParameterValue;
                    return true;
                case 15:
                    return bool.TryParse(ParameterValue, out MACAddressEnable);
                case 16:
                    MACAddress = ParameterValue;
                    return true;
            }
            return false;
        }

        public override MTKTestError RunTest()
        {


            MTKTestError return_value = MTKTestError.NoError;

            

            programCompleted = true;

            //ProgramAllAtEnd == false : Begin
            if ((ProgramAllAtEnd == false) && (CurrentDUT == CyBLE_MTK.IndexConfiguredSerialPortfor1stRow))
            {
                InitializeTestResult();
                programCompleted = false;
            }
            //ProgramAllAtEnd == true : End
            if ((ProgramAllAtEnd == true) && (CurrentDUT == CyBLE_MTK.IndexConfiguredSerialPortforfinalRow))
            {
                InitializeTestResult();
                programCompleted = false;
            }


            
            if (programCompleted == false)
            {

                ProgramAll();


                TestResult.Result = "DONE";
                TestResultUpdate(TestResult);
                TestStatusUpdate(MTKTestMessageType.Complete, "DONE");




                //if (!ProgramAll())
                //{
                //    //return_value = MTKTestError.NotAllDevicesProgrammed;
                //    //TestResult.Result = "FAIL";
                //    //TestResultUpdate(TestResult);
                //    //TestStatusUpdate(MTKTestMessageType.Failure, "FAIL");
                //}
                //else
                //{
                //    TestResult.Result = "DONE";
                //    TestResultUpdate(TestResult);
                //    TestStatusUpdate(MTKTestMessageType.Complete, "DONE");
                //}

            }
            else
            {
                TestResult.Result = "DONE";
                TestResultUpdate(TestResult);
                TestStatusUpdate(MTKTestMessageType.Complete, "DONE");
            }



            return return_value;
        }

        private MTKTestError ProgrammingThread(int DeviceCount)
        {
            MTKTestError ErrDUT;

            if (EnableModuleVerification && DUTSerialPorts[DeviceCount].IsOpen)
            {
                DUTSerialPorts[DeviceCount].DiscardInBuffer();
            }

            ErrDUT = DUTProgrammers[DeviceCount].RunTest();

            if (EnableModuleVerification && DUTSerialPorts[DeviceCount].IsOpen)
            {
                bool EventMismatch = true;
                //Thread.Sleep(100);
                //DUTProgrammers[DeviceCount].ResetDevice();

                string ReceivedEvent = "";

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(UARTCaptureDelay);
                    ReceivedEvent = DUTSerialPorts[DeviceCount].ReadExisting();
                    if (ReceivedEvent.Length > 1)
                    {
                        Log.PrintLog(this, "DUT#" + (DeviceCount + 1).ToString() + ": UART Capture Dump successfully after tried: " + (i + 1).ToString() + " times.", LogDetailLevel.LogRelevant);
                        break;
                    }
                    
                    
                }



                Log.PrintLog(this, "DUT#" + (DeviceCount+1).ToString() + ": UART Capture Dump: " + ReceivedEvent, LogDetailLevel.LogRelevant);

                char[] DelimiterChars = { ',', '\r', '\n' };
                string[] RxEventSplit = ReceivedEvent.Split(DelimiterChars);

                if (RxEventSplit.Length > 9)
                {
                    EventMismatch = false;

                    if (RxEventSplit[2] != EventType)
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'Event Type' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[3].EndsWith(ApplicationVersion) == false) && (ApplicationVersionEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'Application Version' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[4].EndsWith(BLEStackVersion) == false) && (BLEStackVersionEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'BLE Stack Version' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[5].EndsWith(ProtocolVersion) == false) && (ProtocolVersionEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'Protocol Version' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[6].EndsWith(HWIDValue) == false) && (HWIDEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'Hardware ID Version' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[7].EndsWith(BootCause) == false) && (BootCauseEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'Boot Cause' mismatch.", LogDetailLevel.LogRelevant);
                    }

                    if ((RxEventSplit[8].EndsWith(MACAddress) == false) && (MACAddressEnable))
                    {
                        EventMismatch = true;
                        Log.PrintLog(this, "Module 'MAC Address' mismatch.", LogDetailLevel.LogRelevant);
                    }
                }
                else
                {
                    Log.PrintLog(this, "Cannot perform module checks, 'Event Parameter Length' mismatch.", LogDetailLevel.LogRelevant);
                }

                if (EventMismatch == true)
                {
                    ErrDUT = MTKTestError.NotAllDevicesProgrammed;
                }
                else
                {
                    Log.PrintLog(this, "Module checks completed successfully.", LogDetailLevel.LogRelevant);
                }
            }
            else if (EnableModuleVerification && (DUTSerialPorts[DeviceCount].IsOpen == false))
            {
                Log.PrintLog(this, "Cannot perform module checks, DUT serial port not open.", LogDetailLevel.LogRelevant);
            }

            return ErrDUT;
        }

        private bool ProgramAll()
        {
            int i;
            bool return_value = true;
            List<Thread> ProgDUT = new List<Thread>();
            List<MTKTestError> ErrDUT = new List<MTKTestError>();


            for (i = 0; i < NumberOfDUTs; i++)
            {
                DUTProgrammers[i].SelectedHEXFilePath = this.SelectedHEXFilePath;
                ErrDUT.Add(new MTKTestError());
                ErrDUT[i] = MTKTestError.ProgrammerNotConfigured;

                if ((DUTProgrammers[i].SelectedProgrammer != "") &&
                    (DUTProgrammers[i].SelectedHEXFilePath != ""))
                {
                    try
                    {
                        
                        ProgDUT.Add(new Thread(() => { ErrDUT[i] = ProgrammingThread(i); }));
                        DUTProgrammers[i].ProgrammerNumber = i;
                        DUTProgrammers[i].OnNumTestStatusUpdate -= new MTKPSoCProgrammer.NumTestStatusUpdateEventHandler(Ind_Prog_OnNumTestStatusUpdate);
                        DUTProgrammers[i].OnNumTestStatusUpdate += new MTKPSoCProgrammer.NumTestStatusUpdateEventHandler(Ind_Prog_OnNumTestStatusUpdate);
                        ProgDUT[i].Start();
                        Thread.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        Log.PrintLog(this, "Cannot create programming thread. Reason: " + ex.ToString(), LogDetailLevel.LogRelevant);
                        return false;
                    }
                }
                else
                {
                    //MTKTestError temp = new MTKTestError();
                    //temp = MTKTestError.ProgrammerNotConfigured;
                    ////ErrDUT.Add(temp);
                    
                    ProgDUT.Add(new Thread(() => { ErrDUT[i] = NotProgrammingThread(); }));
                    ProgDUT[i].Start();
                    Thread.Sleep(200);
                }
                //ProgDUT[i].Join();
            }

            for (i = 0; i < ProgDUT.Count(); i++)
            {
                ProgDUT[i].Join();
            }

            //this.InitializeTestResult();
            TestResult.Measured = "";
            for (i = 0; i < ErrDUT.Count(); i++)
            {
                if (i > 0)
                {
                    TestResult.Measured += "|";
                }
                //cysp
                if (ErrDUT[i] == MTKTestError.NotAllDevicesProgrammed && CyBLE_MTK.DUTsTestFlag[i])
                {
                    TestResult.Measured += "DUT#" + (i + 1).ToString() + ": FAIL";
                    //cysp
                    TestResult.Result = "FAIL";
                    //Set Error code for ALLPROG
                    if (ProgramAllAtEnd)
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i + ProgDUT.Count()] = ECCS.ERRORCODE_ALLPROG_AT_END_FAIL;
                    }
                    else
                    {
                        
                        MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.ERRORCODE_ALLPROG_AT_BEGIN_FAIL;
                    }
                    return_value = false;

                    //TestResultUpdate(TestResult);
                }
                else if (ErrDUT[i] == MTKTestError.NoError && CyBLE_MTK.DUTsTestFlag[i])
                {
                    TestResult.Measured += "DUT#" + (i + 1).ToString() + ": PASS";


                    if (ProgramAllAtEnd)
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i + ProgDUT.Count()] = ECCS.ERRORCODE_ALL_PASS;
                    }
                    else
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.ERRORCODE_ALL_PASS;
                    }
                    
                    TestResult.Result = "PASS";
                    //TestResultUpdate(TestResult);


                }
                else if(ErrDUT[i] == MTKTestError.ProgrammerNotConfigured || !CyBLE_MTK.DUTsTestFlag[i])
                {
                    TestResult.Measured += "DUT#" + (i + 1).ToString() + ": IGNORE";

                    if (ProgramAllAtEnd)
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i + ProgDUT.Count()] = ECCS.ERRORCODE_DUT_NOT_TEST;
                    }
                    else
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.ERRORCODE_DUT_NOT_TEST;
                    }

                    

                    TestResult.Result = "IGNORE";
                }
                else
                {
                    TestResult.Measured += "DUT#" + (i + 1).ToString() + ": " + ErrDUT[i].ToString();
                    TestResult.Result = "ERROR";
                    return_value = false;
                    

                    if (ProgramAllAtEnd)
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i + ProgDUT.Count()] = ECCS.ERRORCODE_ALLPROG_TEST_INIT_NA;
                    }
                    else
                    {
                        MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.ERRORCODE_ALLPROG_TEST_INIT_NA;
                    }
                    TestResult.Result = "Error";

                }
            }

            OnProgramAllComplete(ErrDUT);
            programCompleted = true;

            return return_value;
        }

        private MTKTestError NotProgrammingThread()
        {
            MTKTestError temp = new MTKTestError();
            temp = MTKTestError.ProgrammerNotConfigured;

            return temp;
        }

        public void Ind_Prog_OnNumTestStatusUpdate(int index, string Message)
        {
            NumTestStatusUpdateEventHandler handler = OnNumTestStatusUpdate;
            if (handler != null)
            {
                handler(index, Message);
            }
        }

        protected override void InitializeTestResult()
        {
            base.InitializeTestResult();
            TestResult.PassCriterion = "N/A";
            TestResult.Measured = "N/A";

            CurrentMTKTestType = MTKTestType.MTKTestProgramAll;

            try
            {
                if (MTKTestProgramAllTmplSFCSErrCodes == null)
                {
                    MTKTestProgramAllTmplSFCSErrCodes = new UInt16[NumberOfDUTs*2];

                }


                for (int i = 0; i < NumberOfDUTs; i++)
                {
                    if (DUTProgrammers[i].SelectedProgrammer != "")
                    {
                        if (ProgramAllAtEnd)
                        {
                            MTKTestProgramAllTmplSFCSErrCodes[i + NumberOfDUTs] = ECCS.TMPL_ERROR_CODE;
                        }
                        else
                        {
                            MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.TMPL_ERROR_CODE;
                            MTKTestProgramAllTmplSFCSErrCodes[i + NumberOfDUTs] = ECCS.TMPL_ERROR_CODE;
                        }
                        
                    }
                    else
                    {
                        if (ProgramAllAtEnd)
                        {
                            MTKTestProgramAllTmplSFCSErrCodes[i + NumberOfDUTs] = ECCS.ERRORCODE_DUT_NOT_TEST;
                        }
                        else
                        {
                            MTKTestProgramAllTmplSFCSErrCodes[i] = ECCS.ERRORCODE_DUT_NOT_TEST;
                        }
                        
                    }

                }



            }
            catch (Exception ex)
            {

                Log.PrintLog(this, "Exception from InitializeTestResult: " + ex.ToString(), LogDetailLevel.LogRelevant);
            }


        }

    }
}
