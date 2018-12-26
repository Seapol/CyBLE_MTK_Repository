using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyBLE_MTK_Application
{
    class MTKTestErrCode
    {

        //private UInt16 _errorcode;
        public const UInt16 ERRORCODE_TEST_ALL_PASS = 0x0000;
        public const UInt16 ERRORCODE_TEST_NOT_FINISHED = 0x1111;
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

        public string ReturnFinalErrCodeforDUT ()
        {
            return null;
        }


    }
}
