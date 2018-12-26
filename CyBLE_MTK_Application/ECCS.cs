using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyBLE_MTK_Application;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{

//      Error code      Define
//      0100			First FW upload fail
//      0200			FW information not match
//      0300			Data transfer fail
//      04xx            GPIO continuity test fail
//				        "04" is main error, when this item fail the read the current value and return to SW
//                      For example: When GPIO continuity test fail and SW read the GPIO value is 51 then generate the error code "0451", etc.
//      05xx            GPIO open-short test fail
//				        "05" is main error, when this item fail the read the current value and return to SW
//                      For example: When GPIO open-short test fail and SW read the GPIO value is 51 then generate the error code "0551", etc.
//      0600			Silicon Unique Number test fail
//      0700			Apple Chip I2C Test failure
//      0800			Second FW programming fail
//      9100			Shopfloor Process Check fail
//      4100			First FW Programming verify fail
//      FFFF            Others

    class ECCS
    {

        

        private UInt16 _errorcode;
        public const UInt16 ERRORCODE_ALL_PASS = 0x0000;
        public const UInt16 ERRORCODE_DUT_NOT_TEST = 0x0001;
        public const UInt16 ERRORCODE_TESTNOTFINISHED = 0x1111;
        public const UInt16 ERRORCODE_ALLPROG_AT_BEGIN_FAIL = 0x0100;
        public const UInt16 ERRORCODE_ALLPROG_VERIFY_FAIL = 0x4100;
        public const UInt16 ERRORCODE_FW_INFORMATION_NOT_MATCH = 0x0200;
        public const UInt16 ERRORCODE_STC_DATA_TRANSFER_TEST_FAIL = 0x0300;
        public const UInt16 ERRORCODE_GPIO_CONTINUITY_TEST_FAIL = 0x0400;
        public const UInt16 ERRORCODE_GPIO_OPENSHORTS_TEST_FAIL = 0x0500;
        public const UInt16 ERRORCODE_SILICON_UNIQUENUMBER_TEST_FAIL = 0x0600;
        public const UInt16 ERRORCODE_APPLE_CHIPI2C_TEST_FAIL = 0x0700;
        public const UInt16 ERRORCODE_ALLPROG_AT_END_FAIL = 0x0800;
        public const UInt16 ERRORCODE_SHOPFLOOR_PROCESS_ERROR = 0x9100;
        public const UInt16 ERRORCODE_OTHERS_UNDEFINED_ERROR = 0xFFFF;
        public const UInt16 ERRORCODE_PENDING_FOR_ALLPROG_BEGIN_REWRITE = 0xFEFE;
        public const UInt16 ERRORCODE_PENDING_FOR_ALLPROG_END_REWRITE = 0xEFEF;
        public const UInt16 ERRORCODE_CUS_TEST_FAILURE_BUT_UNKNOWN = 0x0F0F;
        public const UInt16 ERRORCODE_ALLPROG_TEST_INIT_NA = 0xF0F0;
        public const UInt16 ERROR_CODE_READ_MFIID = 0x7777;
        public const UInt16 ERROR_CODE_DMM = 0x0A00;
        public const UInt16 ERROR_CODE_DMM_LOW = 0x0A01;
        public const UInt16 ERROR_CODE_DMM_HIGH = 0x0A02;
        public const UInt16 ERROR_CODE_CHECK_SN = 0x0B00;

        public static UInt16 TMPL_ERROR_CODE = 0xFFFF;

        public const UInt16 ERROR_CODE_CAUSED_BY_MTK_TESTER = 0xAAAA;



        /// <summary>
        /// ReturnErrCodeforAllProgram
        /// To process the Error Code for AllProgram
        /// this method is subject to ReturnErrCodeforTest
        /// </summary>
        /// <param name="TestResult"></param>
        /// <param name="NumOfDUTs"></param>
        /// <returns>string array</returns>


        public UInt16[] ReturnErrCodeforAllProgram (MTKTestResult TestResult, int NumOfDUTs)
        {


            List<UInt16> errorcodesforallprogram = new List<UInt16>();
            //List<string> DUT_PROG_FAIL = new List<string>();
            string[] ALL_DUT_PROG_RESULT;
            


            //init error code for AllProgram
            if (TestResult.Result.Contains("DONE") && (TestResult.Measured == "N/A"))
            {
                for (int i=0; i<NumOfDUTs;i++)
                {
                    errorcodesforallprogram.Add(ERRORCODE_ALLPROG_TEST_INIT_NA);
                }

                


            }
            else if (TestResult.Result.Contains("DONE")&&(TestResult.Measured!="N/A"))
            {
                ALL_DUT_PROG_RESULT = TestResult.Measured.Split('|');

                string DUT_PROG_FAILURE_MESSAGE = "";

                for (int i = 0; i < NumOfDUTs; i++)
                {
                    try
                    {
                        DUT_PROG_FAILURE_MESSAGE = "DUT#" + (i + 1).ToString() + ": FAIL";

                        if ((ALL_DUT_PROG_RESULT[i].ToLower() == DUT_PROG_FAILURE_MESSAGE.ToLower())&& (ALL_DUT_PROG_RESULT.Length == NumOfDUTs))
                        {
                            if (TestResult.Value[0] == "False")
                            {
                                //errorcodesforallprogram[i] = ERRORCODE_ALLPROG_AT_BEGIN_FAIL.ToString("X4");
                                errorcodesforallprogram.Insert(i, ERRORCODE_ALLPROG_AT_BEGIN_FAIL);
                            }
                            else
                            {
                                //errorcodesforallprogram[i] = ERRORCODE_ALLPROG_AT_END_FAIL.ToString("X4");
                                errorcodesforallprogram.Insert(i, ERRORCODE_ALLPROG_AT_END_FAIL);
                            }
                        }
                        else
                        {
                            //errorcodesforallprogram[i] = ERRORCODE_ALL_PASS.ToString("X4");
                            errorcodesforallprogram.Insert(i, ERRORCODE_ALL_PASS);

                        }


                    }
                    catch 
                    {
                        //MessageBox.Show(ex.ToString() + "\nALL_DUT_PROG_RESULT Length is " + ALL_DUT_PROG_RESULT.Length.ToString() + "\n\nerrorcodesforallprogram count is: " + errorcodesforallprogram.Count.ToString()); 
                        //errorcodesforallprogram.Add(ERRORCODE_ALL_PASS.ToString("X4"));
                    }

                   

                }

                UInt16[] ret_errorcodes = errorcodesforallprogram.ToArray();

                errorcodesforallprogram.Clear();
                return ret_errorcodes;

            }




            return errorcodesforallprogram.ToArray();
        }

 

        public UInt16 ReturnFinalDUTOverallSFCSErrCodeforDUT(MTKTestError mTKTestError, UInt16 _DUTOverallSFCSErrCode, bool shopfloorpermission)
        {

            try
            {

                if (!shopfloorpermission && _DUTOverallSFCSErrCode != ECCS.ERRORCODE_DUT_NOT_TEST && mTKTestError != MTKTestError.IgnoringDUT)
                {
                    return ECCS.ERRORCODE_SHOPFLOOR_PROCESS_ERROR;
                }
                else
                {
                    //for DUTOverallSFCSErrCode
                    return _DUTOverallSFCSErrCode;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Reason: \n" + ex.ToString(), "ReturnFinalDUTOverallSFCSErrCodeforDUT Error");
            }

            return ECCS.TMPL_ERROR_CODE;
        }


    }
}
