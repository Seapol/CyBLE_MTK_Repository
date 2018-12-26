using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;

namespace CyBLE_MTK_Application
{
    public enum CommandResultOperator { NoOperator, Equal, NotEqual, Less, LessOrEqual, Greater, GreaterOrEqual };

    public class MTKTestCUS : MTKTest
    {

        /*
            * Defined in mtk_cus_cmd.h of firmware mtk test firmware.
        */
        public const byte CUSTOM_CMD_READ_GPIO = 1;
        public const byte CUSTOM_CMD_READ_OPEN_GPIO = 2;
        public const byte CUSTOM_CMD_READ_UNIQUE_ID = 3;
        public const byte CUSTOM_CMD_SYNC_TIMER_COUNT = 9; /* mtk_host uses this command to restart timer  */
        public const byte CUSTOM_CMD_READ_MFIID = 10; /* read 4 byte mfid */
        public const byte CUSTOM_CMD_READ_FW_VERSION = 11; /* read 4 byte firmware */
        public const byte CUSTOM_CMD_READ_HOMEKIT_SELFTEST_RESULT = 12; /* read 1 byte Homekit self-test result */


        public static UInt16 TmplSFCSErrCode = 0;

        public string CustomCommandName;
        public byte CustomCommand;
        public int CustomCommandParamNum;
        public byte[] CustomCommandParam;
        public int CustomCommandResultNum;
        public byte[] CustomCommandResult;
        public CommandResultOperator[] ResultOperation;
        public int CommandDelay;
        public int DelayPerCommand = 20 + CyBLE_MTK_Application.Properties.Settings.Default.DelayPerCommandCUS_Additional;//, msPerSecond = 1000;



        private int ResultByteCount = 20;

        public MTKTestCUS() : base()
        {
            Init();
        }

        public MTKTestCUS(LogManager Logger)
            : base(Logger)
        {
            Init();
        }

        public MTKTestCUS(LogManager Logger, SerialPort MTKPort, SerialPort DUTPort)
            : base(Logger, MTKPort, DUTPort)
        {
            Init();
        }

        void Init()
        {
            TestParameterCount = 47;
            CustomCommandName = "";
            CommandDelay = 1000;
            CustomCommandParam = new byte[2];
            CustomCommandParamNum = 0;
            CustomCommandParam[0] = 0x00;
            CustomCommandParam[1] = 0x00;
            CustomCommandResultNum = 0;
            CustomCommandResult = new byte[ResultByteCount];
            ResultOperation = new CommandResultOperator[ResultByteCount];
            for (int i = 0; i < ResultByteCount; i++)
            {
                ResultOperation[i] = CommandResultOperator.NoOperator;
                CustomCommandResult[i] = 0x00;
            }
        }

        //protected override int GetTestParameterCount()
        //{
        //    return base.GetTestParameterCount();
        //}

        private string OprationString(CommandResultOperator InputOperation)
        {
            if (InputOperation == CommandResultOperator.Equal)
            {
                return " == ";
            }
            else if (InputOperation == CommandResultOperator.NotEqual)
            {
                return " != ";
            }
            else if (InputOperation == CommandResultOperator.Greater)
            {
                return " > ";
            }
            else if (InputOperation == CommandResultOperator.GreaterOrEqual)
            {
                return " >= ";
            }
            else if (InputOperation == CommandResultOperator.Less)
            {
                return " < ";
            }
            else if (InputOperation == CommandResultOperator.LessOrEqual)
            {
                return " <= ";
            }

            return "";
        }

        public override string GetDisplayText()
        {
            string temp = "Custom Command | " + CustomCommandName + ": " + CustomCommand.ToString("x2").ToUpper();
            if (CustomCommandParamNum >= 1)
            {
                temp += ", " + CustomCommandParam[0].ToString("x2").ToUpper();
            }

            if (CustomCommandParamNum == 2)
            {
                temp += ", " + CustomCommandParam[1].ToString("x2").ToUpper();
            }

            temp += " | Delay: " + CommandDelay.ToString() + "ms";
            temp += " | DelayPerCommand: " + DelayPerCommand.ToString() + "ms";

            for (int i = 0; i < CustomCommandResultNum; i++)
            {
                if (i == 0)
                {
                    temp += " | ";
                }
                else
                {
                    temp += ", ";
                }
                temp += "RetVal[" + i.ToString() + "]" + OprationString(ResultOperation[i]) + CustomCommandResult[i].ToString("x2").ToUpper();
            }

            return temp;
        }

        public override string GetTestParameter(int TestParameterIndex)
        {
            switch (TestParameterIndex)
            {
                case 0:
                    return CustomCommandName;
                case 1:
                    return CustomCommand.ToString();
                case 2:
                    return CustomCommandParamNum.ToString();
                case 3:
                    return CustomCommandParam[0].ToString();
                case 4:
                    return CustomCommandParam[1].ToString();
                case 5:
                    return CommandDelay.ToString();
                case 6:
                    return CustomCommandResultNum.ToString();
                default:
                    int ResultByteIndex = TestParameterIndex - 7;
                    if ((ResultByteIndex >= 0) && (ResultByteIndex < (ResultByteCount * 2)))
                    {
                        if ((ResultByteIndex % 2) == 0)
                        {
                            return CustomCommandResult[ResultByteIndex / 2].ToString();
                        }
                        else
                        {
                            return ResultOperation[ResultByteIndex / 2].ToString();
                        }
                    }
                    else
                    {
                        return base.GetTestParameter(TestParameterIndex);
                    }
            }
        }

        public override string GetTestParameterName(int TestParameterIndex)
        {
            switch (TestParameterIndex)
            {
                case 0:
                    return "CustomCommandName";
                case 1:
                    return "CustomCommand";
                case 2:
                    return "CustomCommandParamNum";
                case 3:
                    return "CommandParam1";
                case 4:
                    return "CommandParam2";
                case 5:
                    return "CommandDelayInMS";
                case 6:
                    return "CustomCommandResultNum";
                default:
                    int ResultByteIndex = TestParameterIndex - 7;
                    if ((ResultByteIndex >= 0) && (ResultByteIndex < (ResultByteCount * 2)))
                    {
                        if ((ResultByteIndex % 2) == 0)
                        {
                            return "CommandResult" + (ResultByteIndex / 2);
                        }
                        else
                        {
                            return "CommandOperation" + (ResultByteIndex / 2);
                        }
                    }
                    else
                    {
                        return base.GetTestParameterName(TestParameterIndex);
                    }
            }
        }

        public override bool SetTestParameter(int TestParameterIndex, string ParameterValue)
        {
            if (ParameterValue == "")
            {
                return false;
            }
            switch (TestParameterIndex)
            {
                case 0:
                    CustomCommandName = ParameterValue;
                    return true;
                case 1:
                    CustomCommand = byte.Parse(ParameterValue);
                    CustomCommand = byte.Parse(ParameterValue);
                    switch (CustomCommand)
                    {
                        case CUSTOM_CMD_READ_GPIO:
                            //TmplSFCSErrCode = ECCS.ERRORCODE_GPIO_CONTINUITY_TEST_FAIL;
                            break;
                        case CUSTOM_CMD_READ_OPEN_GPIO:
                            //TmplSFCSErrCode = ECCS.ERRORCODE_GPIO_OPENSHORTS_TEST_FAIL;
                            break;
                        case CUSTOM_CMD_READ_UNIQUE_ID:
                            //TmplSFCSErrCode = ECCS.ERRORCODE_SILICON_UNIQUENUMBER_TEST_FAIL;
                            break;
                        //case CUSTOM_CMD_READ_MFIID:
                        //    TmplSFCSErrCode = ECCS.ERROR_CODE_READ_MFIID;
                        //    break;
                        //case CUSTOM_CMD_READ_FW_VERSION:
                        //    TmplSFCSErrCode = SFCS.ERROR_CODE_FW_VERSION;
                        //    break;
                        //case CUSTOM_CMD_READ_HOMEKIT_SELFTEST_RESULT:
                        //    TmplSFCSErrCode = SFCS.ERROR_CODE_APPLE_AUTO_CHIP;
                        //    break;
                    }
                    return true;
                case 2:
                    CustomCommandParamNum = int.Parse(ParameterValue);
                    return true;
                case 3:
                    CustomCommandParam[0] = byte.Parse(ParameterValue);
                    return true;
                case 4:
                    CustomCommandParam[1] = byte.Parse(ParameterValue);
                    return true;
                case 5:
                    CommandDelay = int.Parse(ParameterValue);
                    return true;
                case 6:
                    CustomCommandResultNum = int.Parse(ParameterValue);
                    return true;
                default:
                    int ResultByteIndex = TestParameterIndex - 7;
                    if ((ResultByteIndex >= 0) && (ResultByteIndex < (ResultByteCount * 2)))
                    {
                        if ((ResultByteIndex % 2) == 0)
                        {
                            CustomCommandResult[ResultByteIndex / 2] = byte.Parse(ParameterValue);
                        }
                        else
                        {
                            ResultOperation[ResultByteIndex / 2] = (CommandResultOperator)Enum.Parse((Type)typeof(CommandResultOperator), ParameterValue);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        private MTKTestError RunTestBLE()
        {/*
            int PercentageComplete = 0;
            int DelayPerCommand = 20, msPerSecond = 1000;
            int TimeForEachPacket = 700;
            int TotalEstTime = ((int)((int)this.NumberOfPackets * TimeForEachPacket) / msPerSecond);
            int TimeSlice = (int)Math.Ceiling((double)TotalEstTime / 100.00);

            MTKTestError CommandRetVal;

            this.Log.PrintLog(this, GetDisplayText(), LogDetailLevel.LogRelevant);

            TestStatusUpdate(MTKTestMessageType.Information, PercentageComplete.ToString() + "%");

            string Command = "DUT 0";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #1
            Command = "RRS";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #1
            CommandRetVal = SearchForDUT();
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #2
            Command = "DUT 1";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #3
            Command = "SPL " + PacketLength.ToString();
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #4
            Command = "SPT " + GetPacketType(PacketType).ToString();
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #5
            Command = "RXP " + this.ChannelNumber.ToString() + " " + this.NumberOfPackets.ToString();
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #6
            Command = "DUT 0";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #7
            Command = "DCW " + this.NumberOfPackets.ToString();
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Delay
            for (int i = 0; i <= 100; i++)
            {
                TestStatusUpdate(MTKTestMessageType.Information, PercentageComplete.ToString() + "%");
                Thread.Sleep(TimeSlice);
                PercentageComplete++;
            }

            //  Command #8
            CommandRetVal = SearchForDUT();
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #9
            Command = "DUT 1";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            //  Command #10
            Command = "PST";
            CommandRetVal = SendCommand(MTKSerialPort, Command, 200);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }
            if (CommandResult == "")
            {
                CommandResult = "0";
            }
            TestResult.Result = CommandResult;
            this.Log.PrintLog(this, "Number of packets received: " + this.CommandResult, LogDetailLevel.LogRelevant);


            //  Command #11
            Command = "DUT 0";
            CommandRetVal = SendCommand(MTKSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            TestStatusUpdate(MTKTestMessageType.Complete, "DONE");
            this.Log.PrintLog(this, "Result: DONE", LogDetailLevel.LogRelevant);
*/
            return MTKTestError.NoError;
        }

        private MTKTestError RunTestUART()
        {
            int PercentageComplete = 0;
            //int DelayPerCommand = 20;//, msPerSecond = 1000;

            //int TimeForEachPacket = 700;
            //int TotalEstTime = ((int)((int)this.NumberOfPackets * TimeForEachPacket) / msPerSecond);
            //int TimeSlice = (int)Math.Ceiling((double)TotalEstTime / 100.00);

            MTKTestError CommandRetVal;

            this.Log.PrintLog(this, GetDisplayText(), LogDetailLevel.LogRelevant);

            TestStatusUpdate(MTKTestMessageType.Information, PercentageComplete.ToString() + "%");

            //  Command #1
            string Command = "RRS";
            //Read Response 
            //no return if failure 
            //by cysp

            int loops = CyBLE_MTK_Application.Properties.Settings.Default.CUSTestRRSRetryInTheBeginning;

            if (loops < 0)
            {
                loops = 0;
            }

            for (int i=0; i < loops; i++)
            {
                CommandRetVal = SendCommand(DUTSerialPort, Command, DelayPerCommand);
                if (CommandRetVal == MTKTestError.NoError)
                {
                    break;          //break if SendCommand NoError by cysp
                }
                TestStatusUpdate(MTKTestMessageType.Information, "RRS Retry: "+(i+1).ToString());
                this.Log.PrintLog(this, "RRS Retry: " + (i + 1).ToString() + " |DelayPerCommand: "+ DelayPerCommand.ToString(), LogDetailLevel.LogRelevant);
            }

            // end by cysp

            //  Command #2
            Command = "CUS " + CustomCommand.ToString() + " " + CustomCommandParam[0].ToString() + " " + CustomCommandParam[1].ToString();
            CommandRetVal = SendCommand(DUTSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            TestStatusUpdate(MTKTestMessageType.Information, "Waiting...");
            this.Log.PrintLog(this, "Waiting for: " + CommandDelay.ToString(), LogDetailLevel.LogRelevant);
            Thread.Sleep(CommandDelay);

            //  Command #3
            Command = "RRS";
            CommandRetVal = SendCommand(DUTSerialPort, Command, DelayPerCommand);
            if (CommandRetVal != MTKTestError.NoError)
            {
                return CommandRetVal;
            }

            bool FailedOnce = false;

            if (CustomCommandResultNum == -1)
            {
                string[] TempValue = new string[CommandResults.Count()];
                string[] TempParameter = new string[CommandResults.Count()];
                for (int i = 0; i < CommandResults.Count(); i++)
                {
                    TempValue[i] = int.Parse(CommandResults[i]).ToString("x2").ToUpper();
                    TempParameter[i] = "Received Byte#" + i.ToString();
                    this.Log.PrintLog(this, "0x" + int.Parse(CommandResults[i]).ToString("x2").ToUpper(), LogDetailLevel.LogRelevant);
                }
                TestResult.Value = TempValue;
                TestResult.Parameters = TempParameter;
            }
            else
            {
                if (CommandResult == "")
                {
                    this.Log.PrintLog(this, Command + ": No results returned from DUT.", LogDetailLevel.LogRelevant);
                    TestStatusUpdate(MTKTestMessageType.Failure, "FAIL");
                    return MTKTestError.MissingDUT;
                }

                if (CommandResults.Count() != CustomCommandResultNum)
                {
                    this.Log.PrintLog(this, "Output count mismatch.", LogDetailLevel.LogRelevant);
                    
                    ////cysp: display actual values for GPIOs test
                    foreach (var val in CommandResults)
                    {
                        this.Log.PrintLog(this, "Actual byte value:" + val.ToString(), LogDetailLevel.LogRelevant);
                    }

                    
                    TestStatusUpdate(MTKTestMessageType.Failure, "FAIL");
                    return MTKTestError.TestFailed;
                }

                string[] TempValue = new string[CustomCommandResultNum * 4];
                string[] TempParameter = new string[CustomCommandResultNum * 4];

                for (int i = 0; i < CustomCommandResultNum; i++)
                {
                    TempValue[i * 4] = int.Parse(CommandResults[i]).ToString("x2").ToUpper();
                    TempParameter[i * 4] = "Received Byte#" + i.ToString();
                    TempValue[(i * 4) + 1] = OprationString(ResultOperation[i]);
                    TempParameter[(i * 4) + 1] = "Operation#" + i.ToString();
                    TempValue[(i * 4) + 2] = CustomCommandResult[i].ToString("x2").ToUpper();
                    TempParameter[(i * 4) + 2] = "Compare Value#" + i.ToString();
                    TempParameter[(i * 4) + 3] = "Result#" + i.ToString();

                    if (!CheckOutput(byte.Parse(CommandResults[i]), ResultOperation[i], CustomCommandResult[i]))
                    {
                        FailedOnce = true;
                        this.Log.PrintLog(this, int.Parse(CommandResults[i]).ToString("x2").ToUpper() + " " + OprationString(ResultOperation[i]) + " " + CustomCommandResult[i].ToString("x2").ToUpper() + ": FAIL", LogDetailLevel.LogEverything);
                        TempValue[(i * 4) + 3] = "FAIL";
                    }
                    else
                    {
                        this.Log.PrintLog(this, int.Parse(CommandResults[i]).ToString("x2").ToUpper() + " " + OprationString(ResultOperation[i]) + " " + CustomCommandResult[i].ToString("x2").ToUpper() + ": PASS", LogDetailLevel.LogEverything);
                        TempValue[(i * 4) + 3] = "PASS";
                    }
                }
                TestResult.Value = TempValue;
                TestResult.Parameters = TempParameter;
            }


            if (FailedOnce)
            {
                this.Log.PrintLog(this, "Result: FAIL", LogDetailLevel.LogRelevant);
                this.Log.PrintLog(this, "Output count mismatch.", LogDetailLevel.LogRelevant);

                ////cysp: display actual values for GPIOs test
                for (int i=0; i< CommandResults.Length;i++)
                {
                    this.Log.PrintLog(this, "Actual RetVal[" + i.ToString() + "]" + OprationString(ResultOperation[i]) + int.Parse(CommandResults[i]).ToString("x2").ToUpper(), LogDetailLevel.LogRelevant);

                }



                TestStatusUpdate(MTKTestMessageType.Failure, "FAIL");
                TestResult.Result = "FAIL";
                return MTKTestError.TestFailed;
            }

            TestStatusUpdate(MTKTestMessageType.Success, "PASS");
            this.Log.PrintLog(this, "Result: PASS", LogDetailLevel.LogRelevant);
            TestResult.Result = "PASS";
            return MTKTestError.NoError;
        }

        private bool CheckOutput(byte ReceivedOutput, CommandResultOperator InputOperation, byte ExpectedResult)
        {
            if (InputOperation == CommandResultOperator.Equal)
            {
                if (ReceivedOutput == ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (InputOperation == CommandResultOperator.NotEqual)
            {
                if (ReceivedOutput != ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (InputOperation == CommandResultOperator.Greater)
            {
                if (ReceivedOutput > ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (InputOperation == CommandResultOperator.GreaterOrEqual)
            {
                if (ReceivedOutput >= ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (InputOperation == CommandResultOperator.Less)
            {
                if (ReceivedOutput < ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (InputOperation == CommandResultOperator.LessOrEqual)
            {
                if (ReceivedOutput <= ExpectedResult)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public override MTKTestError RunTest()
        {
            MTKTestError RetVal = MTKTestError.NoError;

            this.InitializeTestResult();

            //TmplSFCSErrCode = 0;

            if (this.DUTConnectionMode == DUTConnMode.BLE)
            {
                RetVal = RunTestBLE();
            }
            else if (this.DUTConnectionMode == DUTConnMode.UART)
            {
                RetVal = RunTestUART();
            }
            else
            {
                return MTKTestError.NoConnectionModeSet;
            }


            //if (TestResult.Result != "PASS" && TmplSFCSErrCode == 0)
            if (TestResult.Result != "PASS")
            {
                switch (CustomCommand)
                {
                    case CUSTOM_CMD_READ_GPIO:
                        TmplSFCSErrCode = (ECCS.ERRORCODE_GPIO_CONTINUITY_TEST_FAIL);
                        MTKTestTmplSFCSErrCode = ECCS.ERRORCODE_GPIO_CONTINUITY_TEST_FAIL;
                        break;
                    case CUSTOM_CMD_READ_OPEN_GPIO:
                        TmplSFCSErrCode = (ECCS.ERRORCODE_GPIO_OPENSHORTS_TEST_FAIL);
                        MTKTestTmplSFCSErrCode = ECCS.ERRORCODE_GPIO_OPENSHORTS_TEST_FAIL;
                        break;
                    case CUSTOM_CMD_READ_UNIQUE_ID:
                        TmplSFCSErrCode = (ECCS.ERRORCODE_SILICON_UNIQUENUMBER_TEST_FAIL);
                        MTKTestTmplSFCSErrCode = ECCS.ERRORCODE_SILICON_UNIQUENUMBER_TEST_FAIL;
                        break;
                        //case CUSTOM_CMD_READ_MFIID:
                        //    TmplSFCSErrCode = ECCS.ERROR_CODE_READ_MFIID;
                        //    break;
                        //case CUSTOM_CMD_READ_FW_VERSION:
                        //    TmplSFCSErrCode = SFCS.ERROR_CODE_FW_VERSION;
                        //    break;
                        //case CUSTOM_CMD_READ_HOMEKIT_SELFTEST_RESULT:
                        //    TmplSFCSErrCode = SFCS.ERROR_CODE_APPLE_AUTO_CHIP;
                        //    break;
                }
            }
            else
            {
                MTKTestTmplSFCSErrCode = ECCS.ERRORCODE_ALL_PASS;
            }

            TestResultUpdate(TestResult);

            return RetVal;
        }

        protected override void InitializeTestResult()
        {
            base.InitializeTestResult();
            TestResult.PassCriterion = "N/A";
            TestResult.Measured = "N/A";

            MTKTestTmplSFCSErrCode = ECCS.ERRORCODE_CUS_TEST_FAILURE_BUT_UNKNOWN;

            CurrentMTKTestType = MTKTestType.MTKTestCUS;
        }
    }
}
