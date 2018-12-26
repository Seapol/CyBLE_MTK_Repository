using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyBLE_MTK_Application
{
    class TryLogPrinting
    {
        LogManager log;

        public TryLogPrinting(LogManager log)
        {

            log.PrintLog(this,"TryTryTry....", LogDetailLevel.LogRelevant);

            speakout(log);
        }

        private void speakout(LogManager log)
        {
            log.PrintLog(this, "SpeakSpeakTryTryTry....", LogDetailLevel.LogRelevant);
        }
    }
}
