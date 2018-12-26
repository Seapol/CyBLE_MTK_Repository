using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace CyBLE_MTK_Application
{
    public class SFCS_DB_Helper
    {
        private string lastError;

        LogManager logger;

        OleDbConnection dbConnection;
        //OleDbCommandBuilder commandBuilder;
        OleDbCommand dbCommand;
        OleDbDataAdapter adapter;

        public string LastError
        {
            get { return lastError; }
            set { lastError = value; }
        }

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private DbTable table;

        public DbTable Table
        {
            get { return table; }
            set { table = value; }
        }

        private string sqlString;

        public string SqlString
        {
            get { return sqlString; }
            set { sqlString = value; }
        }


        public SFCS_DB_Helper(LogManager log)
        {
            logger = new LogManager();

            logger = log;

            init();
        }

        public SFCS_DB_Helper()
        {
            init();
        }


        private void init()
        {
            if (connectionString == null)
            {



                if (!System.IO.File.Exists(Application.StartupPath + @"\SWJshopfloorDB.mdb"))
                {
                    MessageBox.Show("SWJshopfloorDB.mdb is missing...", this.ToString());
                } 

                connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\SWJshopfloorDB.mdb";
                
                //logger.PrintLog(this,"Connecting to ... " + connectionString, LogDetailLevel.LogEverything);
            }

            if (table == null)
            {
                table = new DbTable();
                table.Name = "Sheet1";

                try
                {
                    table.ColTitles = new string[] { "Serial_NO", "Model_Name", "Test_Mstation", "Test_Code", "TesterID", "SocketNo", "MFI_ID", "Test_Log", "Test_Result" };
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }

        public bool InsertRow(string RowContentBySeparator)
        {
            bool retVal = false;

            sqlString = MakingInsertSQLString(RowContentBySeparator);

            if (sqlString == "" || sqlString == null)
            {
                return false;
            }

            try
            {
                using (dbConnection = new OleDbConnection(connectionString))
                {
                    adapter = new OleDbDataAdapter();
                    //adapter.SelectCommand = new OleDbCommand(sqlString, dbConnection);
                    //commandBuilder = new OleDbCommandBuilder(adapter);

                    dbCommand = new OleDbCommand(sqlString, dbConnection);

                    dbCommand.Connection = dbConnection;

                    dbConnection.Open();

                    if (dbConnection.State == ConnectionState.Open)
                    {


                        int new_row = dbCommand.ExecuteNonQuery();

                        if (new_row > 0)
                        {
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }

                    }
                    else
                    {
                        retVal = false;
                        return retVal;
                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), this.ToString());

                retVal = false;
            }









            return retVal;
        }

        private string MakingInsertSQLString(string rowContentBySeparator)
        {
            string retMsg = @"INSERT INTO " + table.Name + $"(";

            foreach (var item in table.ColTitles)
            {
                retMsg += item + ",";
            }

            retMsg = retMsg.Remove(retMsg.Length-1,1) + ") VALUES ";


            if (rowContentBySeparator == null)
            {
                return null;
            }

            string[] vs = rowContentBySeparator.Split('$');

            if (vs == null)
            {
                return null;
            }

            retMsg += "(";

            foreach (var item in vs)
            {
                retMsg += "'" + item + "',";
            }

            retMsg = retMsg.Remove(retMsg.Length-1, 1) + ")";

            return retMsg;

        }
    }
}