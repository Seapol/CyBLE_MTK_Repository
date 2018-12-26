using CypressSemiconductor.ChinaManufacturingTest;
using System;


namespace CyBLE_MTK_Application
{
    public class MTKInstruments
    {
        public static string DMM_alias = CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias;
        public static string SW_alias = CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias;
        public static int DelayBeforeMeasure = CyBLE_MTK_Application.Properties.Settings.Default.DelayInMSBeforeDUTCurrentMeasure;
        public static int DelayAfterMeasure = CyBLE_MTK_Application.Properties.Settings.Default.DelayInMSAfterDUTCurrentMeasure;



        public static Current DUTCurrent = new Current();
        public static int MeasUnitMultipler = 1000;

        public static LogManager Logger;

        public static bool AlldevReady = false;


        private static Agilent sw;
        private static MultiMeter dmm;


        private static bool dmmConnected;

        public static bool DmmConnected
        {
            get { return dmmConnected; }
            set { dmmConnected = value; }
        }

        private static bool swConnected;

        public static bool SwConnected
        {
            get { return swConnected; }
            set { swConnected = value; }
        }

        public static void ConnectDMM()
        {
            try
            {
                dmm = new MultiMeter();
                DmmConnected = dmm.InitializeU3606A(DMM_alias);
                
            }
            catch (Exception ex)
            {

                //Logger.PrintLog(this, "Fail to Connect DMM: " + DMM_alias, LogDetailLevel.LogRelevant);

            }
        }

        public static void ConnectSwitch()
        {
            try
            {
                sw = new Agilent();
                SwConnected = sw.InitializeU2751A_WELLA(SW_alias);
                sw.SetRelayWellA_ALLCLOSE();
                //sw.InitializeU2651A(SW_alias);
            }
            catch (Exception ex)
            {

                //Logger.PrintLog(this, "Fail to Connect Switch: " + SW_alias, LogDetailLevel.LogRelevant);
            }
        }

        public static void ConnectInstruments()
        {
            ConnectDMM();
            ConnectSwitch();

            if (dmmConnected&&swConnected)
            {
                AlldevReady = true;
            }

        }


        private static void init()
        {
            DUTCurrent.average = 0;
            DUTCurrent.max = 0;
            DUTCurrent.min = 0;
        }

        public static Current MeasureChannelCurrent(int ch_no)
        {

            init();

            if (!swConnected)
            {
                ConnectSwitch();
            }

            //Switch on dedicated DUT Power (switch off others)
            SwitchDutPower(ch_no, PowerSupplyState.PowerOn);

            if (!dmmConnected)
            {

                ConnectDMM();

            }

            if (swConnected && dmmConnected)
            {
                AlldevReady = true;
            }
            else
            {
                
            }

            if (AlldevReady)
            {
                try
                {
                    MultiMeter.current MEASCurrent = dmm.MeasureChannelCurrent(DelayBeforeMeasure, DelayAfterMeasure);

                    //Convert reading value to mA value
                    switch (DUTCurrent.unit)
                    {
                        case CurrentUnit.A:
                            MEASCurrent.average = Math.Abs(MEASCurrent.average);
                            MEASCurrent.max = Math.Abs(MEASCurrent.max);
                            MEASCurrent.min = Math.Abs(MEASCurrent.min);
                            DUTCurrent.average = (double)Math.Round((MEASCurrent.average) * MeasUnitMultipler, 2);
                            DUTCurrent.max = (double)Math.Round((MEASCurrent.max) * MeasUnitMultipler, 2);
                            DUTCurrent.min = (double)Math.Round((MEASCurrent.min) * MeasUnitMultipler, 2);
                            break;
                        case CurrentUnit.mA:
                            MEASCurrent.average = Math.Abs(MEASCurrent.average);
                            MEASCurrent.max = Math.Abs(MEASCurrent.max);
                            MEASCurrent.min = Math.Abs(MEASCurrent.min);
                            DUTCurrent.average = (double)Math.Round((MEASCurrent.average), 2);
                            DUTCurrent.max = (double)Math.Round((MEASCurrent.max), 2);
                            DUTCurrent.min = (double)Math.Round((MEASCurrent.min), 2);
                            break;
                        case CurrentUnit.uA:
                            MEASCurrent.average = Math.Abs(MEASCurrent.average);
                            MEASCurrent.max = Math.Abs(MEASCurrent.max);
                            MEASCurrent.min = Math.Abs(MEASCurrent.min);
                            DUTCurrent.average = (double)Math.Round((MEASCurrent.average) / MeasUnitMultipler, 2);
                            DUTCurrent.max = (double)Math.Round((MEASCurrent.max) / MeasUnitMultipler, 2);
                            DUTCurrent.min = (double)Math.Round((MEASCurrent.min) / MeasUnitMultipler, 2);
                            break;
                        case CurrentUnit.nA:
                            MEASCurrent.average = Math.Abs(MEASCurrent.average);
                            MEASCurrent.max = Math.Abs(MEASCurrent.max);
                            MEASCurrent.min = Math.Abs(MEASCurrent.min);
                            DUTCurrent.average = (double)Math.Round((MEASCurrent.average) / MeasUnitMultipler/ MeasUnitMultipler, 2);
                            DUTCurrent.max = (double)Math.Round((MEASCurrent.max) / MeasUnitMultipler / MeasUnitMultipler, 2);
                            DUTCurrent.min = (double)Math.Round((MEASCurrent.min) / MeasUnitMultipler / MeasUnitMultipler, 2);
                            break;
                        default:
                            break;
                    }



                }
                catch (Exception)
                {

                }
                finally
                {
                    //Switch On all DUTs
                    sw.SetRelayWellA_ALLCLOSE();
                }
                 




            }

            return DUTCurrent;

        }

        public static void SyncParams()
        {
            DMM_alias = CyBLE_MTK_Application.Properties.Settings.Default.DMM_Alias;
            SW_alias = CyBLE_MTK_Application.Properties.Settings.Default.Switch_Alias;
          

        }

        public static void SwitchDutPower(int channel_no, PowerSupplyState PowerState)
        {

            try
            {
                if (!swConnected)
                {
                    ConnectSwitch();
                }

                if (PowerState == PowerSupplyState.PowerOn)
                {
                    //close single channel
                    sw.SetRelayWellA_byCH(channel_no, true);
                }
                else
                {
                    //open single channel
                    sw.SetRelayWellA_byCH(channel_no, false);
                }
            }
            catch (Exception)
            {

                Logger.PrintLog(MTKInstruments.sw, "", LogDetailLevel.LogRelevant);
            }

        }

        public MTKInstruments(LogManager logger)
        {
            try
            {
                dmm = new MultiMeter();
                sw = new Agilent();

                logger = new LogManager();

                Logger = logger;
            }
            catch (Exception )
            {

                //throw;
            }
        }

        public static void SwitchAllDutOff()
        {
            try
            {
                if (!swConnected)
                {
                    ConnectSwitch();
                }

                sw.SetRelayWellA_ALLOPEN();

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static bool SwitchAllDutOn()
        {
            try
            {
                if (!swConnected)
                {
                    ConnectSwitch();
                }

                sw.SetRelayWellA_ALLCLOSE();

                return true;

            }
            catch (Exception)
            {

                return false;
            }

            
        }
    }

    public enum PowerSupplyState
    {
        PowerOff = 0,
        PowerOn = 1
    }
}