using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Cypress_Link_SQL2005;
using GeneralLinkSql;
using GeneralLinkSqlLocal;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace CyBLE_MTK_Application
{
    public class SFCS
    {
        protected LogManager Log;
        

        private string _LastError;

        public string LastError
        {
            get
            {
                return _LastError;
            }
            set
            {
                _LastError = value;
                Log.PrintLog(this, "Error! " + _LastError, LogDetailLevel.LogRelevant);
            }
        }

        public SFCS(LogManager Logger)
        {
            if (Logger == null)
            {
                Logger = new LogManager();
            }
            Log = Logger;
            _LastError = "";
        }

        public virtual string PermissonCheck(string SerialNumber, string Model, string WorkerID, string Station)
        {
            return "Virtual Permission Check but no directive object.";
        }

        public virtual bool UploadTestResult(string SerialNumber, string Model, string TesterID, UInt16 errorcode, string SocketId, string TestResult, string TestStation, string MFI_ID)
        {
            return false;
        }

        public virtual bool IsValidSerialNumber(string DbgTag, string SerialNumber)
        {
            if (SerialNumber.Length >= 15)
                return true;
            LastError = DbgTag + "Invalid Serial Number: " + SerialNumber;
            return false;
        }

        private static string _SFCSName = "";
        private static SFCS _SFCS = null;
        public static SFCS GetSFCS(LogManager Logger)
        {
            string name = CyBLE_MTK_Application.Properties.Settings.Default.SFCSInterface.ToLower();

            if (_SFCS != null && name == _SFCSName)
            {
                return _SFCS;
            }

            _SFCSName = name;

            switch (_SFCSName)
            {
                case "":
                    _SFCS = new SFCS(Logger);
                    break;
                case "fittec":
                    _SFCS = new SFCS_FITTEC(Logger);
                    break;
                case "sigma":
                    _SFCS = new SFCS_SIGMA(Logger);
                    break;
                default:
                    _SFCS = new SFCS_LOCAL(Logger, "SFCS_" + name + ".csv");
                    break;
            }

            return _SFCS;
        }
    }

    public class SFCS_LOCAL : SFCS
    {
        string FileName;

        public SFCS_LOCAL(LogManager Logger, string SavedFileName)
            : base(Logger)
        {
            if (SavedFileName != null && SavedFileName.Length > 0)
            {
                FileName = SavedFileName;
            }
            else
            {
                FileName = "SFCS.csv";
            }

            Logger.PrintLog(this, "SFCS_LOCAL was initiated.", LogDetailLevel.LogRelevant);
        }


        public override string PermissonCheck(string SerialNumber, string Model, string WorkerID, string Station)
        {
            return "PASS: SFCSInterface is SFCS_LOCAL";

            
        }

        public override bool UploadTestResult(string SerialNumber, string Model, string TesterID, UInt16 errorcode, string SocketId, string TestResult, string TestStation, string MFI_ID)
        {

            SocketId = SocketId + "Socket#";
            try
            {
                string current_timestamp = System.DateTime.Now.ToShortDateString() + "," + System.DateTime.Now.ToShortTimeString();
                StreamWriter sw = new StreamWriter(FileName, true, Encoding.ASCII);
                sw.AutoFlush = true;
                sw.WriteLine(current_timestamp+ "," + SerialNumber + "," + Model + "," + TesterID + "," + errorcode + "," + SocketId + "," + TestResult + "," + TestStation + "," + MFI_ID);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                LastError = "Failed to save test result to local file. (" + ex.Message + ")";
            }

            return true;
        }
    }

    public class SFCS_SIGMA : SFCS
    {
        protected bool SqlConnected;

        

        protected GeneralLinkSql.GeneralLinkSql SFCSconnection;
        protected GeneralLinkSqlLocal.GeneralLinkSqlLocal SFCSconnectionLocal;


        public SFCS_SIGMA(LogManager Logger)
            : base(Logger)
        {
            SqlConnected = false;
            Logger.PrintLog(this, "SFCS_SIGMA was initiated.", LogDetailLevel.LogRelevant);
            Logger.PrintLog(this, "The setting of GeneralLinkSqlLocalDebug is: " + CyBLE_MTK_Application.Properties.Settings.Default.GeneralLinkSqlLocalDebug.ToString(), LogDetailLevel.LogRelevant);

        }

        /// <summary>
        /// Setup connection to the SFCS traceability system.
        /// </summary>
        /// <returns></returns>true: sucessfully connected. false: failed to connect.
        protected bool Connect()
        {


            SFCSconnection = new GeneralLinkSql.GeneralLinkSql();
            SFCSconnectionLocal = new GeneralLinkSqlLocal.GeneralLinkSqlLocal();

            int connectionInfo = 99;

            try
            {
                if (!CyBLE_MTK_Application.Properties.Settings.Default.GeneralLinkSqlLocalDebug)
                {
                    connectionInfo = SFCSconnection.IsConnect();
                }
                else
                {
                    connectionInfo = SFCSconnectionLocal.IsConnect();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SFCSconnection Error");
            }

            switch (connectionInfo)
            {
                case 0:
                    SqlConnected = false;
                    LastError = "SFCS: SFCS cannot login.";
                    break;
                case 1:
                    SqlConnected = true;
                    break;
                //case 9:
                //    connected = false;
                //    connect_error = "SFCS: Cannot find SFCS database.";
                //    break;
                //case 10:
                //    connected = false;
                //    connect_error = "SFCS: SFCS connection time out.";
                //    break;
                //case 11:
                //    connected = false;
                //    connect_error = "SFCS: Failed to connect SFCS.";
                //    break;
                default:
                    SqlConnected = false;
                    LastError = "SFCS: Unkown error when trying to connect SFCS.";
                    break;
            }
            return SqlConnected;
        }

        /// <summary>
        /// Check test permission of current station by searching the serial number of pre-station status.
        /// </summary>
        /// <param name="SerialNumber"></param> serial number of BLE Device.
        /// <param name="Model"></param> part number of BLE Device.
        /// <param name="Station"></param> current station ID.
        /// <returns></returns> true: get permission; false: no permission.
        public override string PermissonCheck(string SerialNumber, string Model, string WorkerID, string Station)
        {

            if (Connect() != true)
            {
                
                return "FAIL: " + LastError;
            }

            if (!IsValidSerialNumber("PermissonCheck ", SerialNumber))
            {
                return "FAIL: Serialnumber : " + SerialNumber +" is invalid!!!";
            }


            string permisson_info = "No Directive GeneralLinkSql found.";

            if (!CyBLE_MTK_Application.Properties.Settings.Default.GeneralLinkSqlLocalDebug)
            {
                permisson_info = SFCSconnection.Check_Route(SerialNumber, Model, Station);
            }
            else
            {
                permisson_info = SFCSconnectionLocal.Check_Route(SerialNumber, Model, Station);
            }

            //MessageBox.Show(permisson_info);
            if (permisson_info.ToLower().Contains("pass"))
            {
                return permisson_info;
            }
            else
            {
                
                LastError = "Permission denied.";
                return permisson_info;
            }
            //switch (permisson_info)
            //{
            //    case 0:
            //        permisson = true;
            //        break;
            //    case 1:
            //        permisson = false;
            //        connect_error = "SFCS: " + SerialNumber.ToString() + " failed at SMT.";
            //        break;
            //    case 2:
            //        permisson = false;
            //        connect_error = "SFCS: " + SerialNumber.ToString() + " failed at AOI.";
            //        break;
            //    case 3:
            //        permisson = false;
            //        connect_error = "SFCS: " + SerialNumber.ToString() + " failed at TPT.";
            //        break;
            //    case 4:
            //        permisson = false;
            //        connect_error = "SFCS: " + SerialNumber.ToString() + " does not exsist.";
            //        break;
            //    case 9:
            //        permisson = false;
            //        connect_error = "SFCS: Cannot find SFCS database.";
            //        break;
            //    case 10:
            //        permisson = false;
            //        connect_error = "SFCS: SFCS connection time out.";
            //        break;
            //    case 11:
            //        permisson = false;
            //        connect_error = "SFCS: Failed to connect SFCS.";
            //        break;
            //    default:
            //        permisson = false;
            //        connect_error = "SFCS: Unkown error when trying to get permission.";
            //        break;
            //}
            
        }

        /// <summary>
        /// Upload the test result of current station to SFCS by serial number.
        /// </summary>
        /// <param name="SerialNumber"></param> serial number of BLE Device.
        /// <param name="Model"></param> part number of BLE Device.
        /// <param name="WorkerID"></param> worker ID.
        /// <param name="Station"></param> current station ID.
        /// <param name="ErrorCode"></param> errorcode of BLE Device.
        /// <param name="TestResult"></param> "Pass" or "Fail".
        /// <returns></returns> true: upload successfully. false: failed to upload the test result.
        public override bool UploadTestResult(string SerialNumber, string Model, string TesterID, UInt16 errorcode, string SocketId, string TestResult, string TestStation, string MFI_ID
)
        {
            bool resultUploaded = false;
            string upload_info;
            SocketId = SocketId + "Socket#";

            if (Connect()!= true)
            {
                MessageBox.Show(LastError, "Shopfloor Error");
                return false;
            }



            if (!IsValidSerialNumber("UploadTestResult ", SerialNumber) || (!SqlConnected && !Connect()))
            {
                return false;
            }


            if (!CyBLE_MTK_Application.Properties.Settings.Default.GeneralLinkSqlLocalDebug)
            {
                upload_info = SFCSconnection.Save_Result(SerialNumber, Model, TesterID, errorcode.ToString("X4"),
                SocketId, TestResult, TestStation, MFI_ID);
            }
            else
            {
                upload_info = SFCSconnectionLocal.Save_Result(SerialNumber, Model, TesterID, errorcode.ToString("X4"),
                SocketId, TestResult, TestStation, MFI_ID);
            }
            

            if (upload_info.Contains("Pass"))
            {
                resultUploaded = true;
            }
            else
            {
                LastError = "Failed to save result to server: " + upload_info;
            }
            //switch (upload_info)
            //{
            //    case 0:
            //        resultUploaded = true;
            //        break;
            //    case 9:
            //        resultUploaded = false;
            //        connect_error = "SFCS: Cannot find SFCS database.";
            //        break;
            //    case 10:
            //        resultUploaded = false;
            //        connect_error = "SFCS: SFCS connection time out.";
            //        break;
            //    case 11:
            //        resultUploaded = false;
            //        connect_error = "SFCS: Failed to connect SFCS.";
            //        break;
            //    case 12:
            //        resultUploaded = false;
            //        connect_error = "SFCS: Not enough database space for uploading the result.";
            //        break;
            //    case 13:
            //        resultUploaded = false;
            //        connect_error = "SFCS: infomation for uploading cannot meet database integrity.";
            //        break;
            //    default:
            //        resultUploaded = false;
            //        connect_error = "SFCS: Unkown error when trying to upload the test result.";
            //        break;
            //}
            return resultUploaded;
        }
    }

    public class StrOperator
    {
        #region AES Encrypt
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion AES Encrypt

        #region AES Decrypt
        public static string Decrypt(string toDecrypt)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion AES Decrypt
    }

    public class SFCS_FITTEC : SFCS
    {
        public SFCS_FITTEC(LogManager Logger)
            : base(Logger)
        {
            Logger.PrintLog(this, "SFCS_FITTEC was initiated.", LogDetailLevel.LogRelevant);
        }

        public override bool UploadTestResult(string SerialNumber, string Model, string TesterID, UInt16 errorcode, string SocketId, string TestResult, string TestStation, string MFI_ID
)
        {
            SqlConnection Con;
            string adoCon;
            string adoConEncrypted;
            string sql;
            string site;
            int IntErrorCdoe = errorcode;

            SocketId = SocketId + "Socket#";

            if (!IsValidSerialNumber("UploadTestResult " + GetType().ToString(), SerialNumber))
            {
                return false;
            }

            try
            {
#if SAVE_AdoCon_In_AppSettings
                site = ConfigurationManager.AppSettings["Site"];
                //MessageBox.Show(site);
                adoConEncrypted = ConfigurationManager.AppSettings["AdoCon"]
#else
                site = CyBLE_MTK_Application.Properties.Settings.Default.Site;
                adoConEncrypted = CyBLE_MTK_Application.Properties.Settings.Default.AdoCon;
#endif
                adoCon = StrOperator.Decrypt(adoConEncrypted);
                //MessageBox.Show(adoCon);
                Con = new SqlConnection(adoCon);
                Con.Open();

                sql = "INSERT INTO if_check(if_site,if_barcode,if_result,if_mfi_id) VALUES (@Site,@BarCode,@Result,@MFiID)";

                SqlCommand sqlCmd = new SqlCommand(sql, Con);
                sqlCmd.Parameters.AddWithValue("@Site", site);
                sqlCmd.Parameters.AddWithValue("@BarCode", SerialNumber);
                sqlCmd.Parameters.AddWithValue("@Result", IntErrorCdoe);
                sqlCmd.Parameters.AddWithValue("@MFiID", MFI_ID);
                sqlCmd.ExecuteNonQuery();
                Con.Close();
                return true;
            }
            catch (Exception ex)
            {
                LastError = "Failed to connect log server. (" + ex.Message + ")";
                MessageBox.Show(LastError, "Shopfloor error");
            }

            return false;
        }

        public override string PermissonCheck(string SerialNumber, string Model, string WorkerID, string Station)
        {
            return "PASS: SFCSInterface is SFCS_FITTEC that doesn't have PermissonCheck";


        }

    }

}
