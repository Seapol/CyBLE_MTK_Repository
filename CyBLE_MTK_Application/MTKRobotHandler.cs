using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using CyBLE_MTK_Application;

namespace CyBLE_MTK_Application
{
    public class MTKRobotHandler
    {
        private int _connectedClients;
        private object _client;
        private TcpClient _tcpclient;
        //private NetworkStream _clientStream;
        //private int _tcpClient;
        //private Thread _listenThread;
        //private int _testRunning;
        //private int _IgnoreFlagIndex;
        private int _DUT_SN_StartIndex = 2;
        private int _DUT_SN_length = 20;
        private char _splitChar = '#';
        private int _tcpPort = 3000;
        public string MessageFromRobot;
        public string MessageToRobot;
        public List<string> SNFromRobot = new List<string>();
        private char _tester_id;
        private List<char> _socket_no = new List<char>();
        private List<char> _status = new List<char>();
        private List<char> _result = new List<char>();
        private bool tcp_terminated = false;
        private string MessageSendWrongBack;

        public bool startlistening = false;
        public bool MTKhandlemessagefromrobotfinished = false;

        public List<char> TestResults = new List<char>();

        public LogManager logger;

        private object sender;

        public MTKRobotHandler()
        {
            logger = new LogManager();
            logger.PrintLog(this, "ListenForClients() is running...", LogDetailLevel.LogRelevant);
            MessageSendWrongBack = "****SendWrongBack****";
        }


        public struct FormattedRobotMessage 
        {
            public char tester_id;
            public char[] socket_no;
            public string[] serialnumber;
            public char[] status;          //1: Test      0:  Ignore
            public char[] result;          //1: Pass      0:  Fail

            public char EndBit;                 //separator         e.g. #

            public FormattedRobotMessage(char testerID, char[] socketNo, string[] sn, char[] teststatus, char separator)
            {
                tester_id = testerID;
                EndBit = separator;

                this.socket_no = new char[socketNo.Length];
                this.serialnumber = new string[sn.Length];
                this.status = new char[teststatus.Length];
                this.result = new char[teststatus.Length];


                for (int i=0; i< socketNo.Length; i++ )
                {
                    socket_no[i] = socketNo[i];

                    //Init test result to N (None) before MTK testing
                    result[i] = 'N';
                }

                for (int i = 0; i < sn.Length; i++)
                {
                    serialnumber[i] = sn[i];
                }

                for (int i = 0; i < teststatus.Length; i++)
                {
                    status[i] = teststatus[i];



                }

            }

        };
        
        private List<string> _snArray = new List<string>();

        FormattedRobotMessage robotmessage = new FormattedRobotMessage();

        public int TcpPort
        {
            get => default(int);
            set
            {
                _tcpPort = TcpPort;
            }
        }



        /// <summary>
        /// Listen TCP client to connect TCP Server and start to HandleClientComm
        /// </summary>
        /// <remarks>
        /// 1.Start TCPListener to AcceptTCPClient()
        /// 2.HandleClientComm(client)
        /// </remarks>
        public void ListenForClients()
        {


            InitializeTCPConnectionSettings();
            TcpListener tcpListener = new TcpListener(IPAddress.Any, _tcpPort);

            Console.WriteLine("TcpListener IPAddress is set to " + IPAddress.Any.ToString());
            Console.WriteLine("TcpListener Port is set to " + _tcpPort.ToString());
            Console.WriteLine("TcpListener Server is " + tcpListener.Server.ToString());



            try
            {
                tcpListener.Start();
                Console.WriteLine("TcpListener is starting ...");

                try
                {
                    //blocks until a client has connected to the server
                    _client = tcpListener.AcceptTcpClient();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString(), "Fail to AcceptTcpClient");
                }

                Console.WriteLine("TcpListener is starting ...");

                //create a thread to handle communication 
                //with connected client
                _connectedClients++;

            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Exception of ListenForClients");
                MTKhandlemessagefromrobotfinished = false; //if exception, stop to run testing...
            }

            //wait for client
            while (true)
            {
                System.Threading.Thread.Sleep(100);
                try
                {

                    if (startlistening)
                    {
                        //init();
                        HandleClientComm(_client);

                    }


                }

                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString(), "Exception of ListenForClients");
                    MTKhandlemessagefromrobotfinished = false; //if exception, stop to run testing...
                }






            }



        }

        /// <summary>
        /// Handler TCP Client stream to string
        /// </summary>
        /// <remarks>GetStream-&gt;byte buffer-&gt; encoder string -&gt; Handle Message</remarks>
        private void HandleClientComm(object client)
        {
            _tcpclient = (TcpClient)client;
            NetworkStream clientStream = _tcpclient.GetStream();

            Console.WriteLine("GetNetworkStream: "+ clientStream);
            logger.PrintLog(this, "GetNetworkStream: " + clientStream, LogDetailLevel.LogRelevant);




            //init();

            //while (true)
            //{

            //}

            byte[] message = new byte[4096];
            int bytesRead = 0;

            try
            {
                bytesRead = clientStream.Read(message, 0, 192);

            }
            catch
            {

            }

            if (bytesRead == 0)
            {
                _connectedClients--;
                

            }
            else
            {
                MessageFromRobot = "";
                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();

                // Convert the Bytes received to a string and display it on the Server Screen
                MessageFromRobot = encoder.GetString(message, 0, bytesRead);

                Console.WriteLine("");
                Console.WriteLine("ReadClientStream to message: ");
                Console.WriteLine(MessageFromRobot);
                Console.WriteLine("");

                logger.PrintLog(this, "TCPServer: ReadClientStream to message: " + MessageFromRobot, LogDetailLevel.LogRelevant);
                //MessageBox.Show("TCPServer: ReadClientStream to message: " + MessageFromRobot);

                string[] SeparatedMessage = HandleReceivingMessage(MessageFromRobot);

                if (SeparatedMessage == null)
                {
                    MessageToRobot = MessageSendWrongBack;
                    MTKhandlemessagefromrobotfinished = true;
                    //MessageBox.Show("Error!!! "+"\n注意已停机，机器人发送消息不合法\n"+"MessageFromRobot:" + MessageFromRobot, MessageToRobot);
                    return;
                }

                robotmessage.tester_id = GetTesterIDByReceivingMessage(SeparatedMessage);
                robotmessage.socket_no = GetSocketNoByReceivingMessage(SeparatedMessage, 1);
                robotmessage.status = GetTestStatusByReceivingMessage(SeparatedMessage);
                robotmessage.serialnumber = GetSerialNumberByReceivingMessage(SeparatedMessage, _DUT_SN_StartIndex, _DUT_SN_length);
                robotmessage.EndBit = _splitChar;
                Console.WriteLine("Retrieve Serial Numbers and test information from message as below: ");
                Console.WriteLine("TesterID\t" + "Socket#\t\t" + "SerialNumber\t\t" + "\tFlag\t\t" + "EndBit#");


                _tester_id = robotmessage.tester_id;



                for (int i = 0; i < robotmessage.serialnumber.Length; i++)
                {
                    SNFromRobot.Add(robotmessage.serialnumber[i]);
                    _socket_no.Add(robotmessage.socket_no[i]);
                    _status.Add(robotmessage.status[i]);
                    Console.WriteLine(robotmessage.tester_id + "\t\t" + robotmessage.socket_no[i] + "\t\t" + robotmessage.serialnumber[i] + "\t\t" + robotmessage.status[i] + "\t\t" + robotmessage.EndBit);

                }

                if (SNFromRobot.ToArray().Length > 0 )
                {
                    MTKhandlemessagefromrobotfinished = true;
                }

                Console.WriteLine("");


            }




            //_tcpclient.Close();

        }

        private void HandleClientCommBack(object client, string Message)
        {


            try
            {
                _tcpclient = (TcpClient)client;
                NetworkStream clientStream = _tcpclient.GetStream();

                Console.WriteLine("GetNetworkStream: " + clientStream);
                byte[] message = new byte[4096];


                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();



                // Convert the Bytes received to a string and display it on the Server Screen
                encoder.GetBytes(Message, 0, Message.Length, message, 0);

                Console.WriteLine("Trying to write Message to Robot:");
                Console.WriteLine(Message);
                Console.WriteLine("");
                Console.WriteLine("");

                clientStream.Write(message, 0, 192);

                if(MessageToRobot != MessageSendWrongBack)
                {
                    Console.WriteLine("Send Back Serial Numbers and test information to Robot as below: ");
                    Console.WriteLine("TesterID\t" + "Socket#\t\t" + "SerialNumber\t\t" + "\tResult\t\t" + "EndBit#");
                    for (int i = 0; i < robotmessage.serialnumber.Length; i++)
                    {

                        Console.WriteLine(robotmessage.tester_id + "\t\t" + robotmessage.socket_no[i] + "\t\t" + robotmessage.serialnumber[i] + "\t\t" + _result[i] + "\t\t" + robotmessage.EndBit);

                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("HandleClientCommBack Exception: " + ex.ToString());
                return;
            }






            //_tcpclient.Close();
        }


        /// <summary>
        /// Handle Message from Robot
        /// </summary>
        private string[] HandleReceivingMessage(string message)
        {
            
            if (!message.Contains('#'))
            {

                MessageToRobot = MessageSendWrongBack;

                MessageBox.Show("Error!!! " + "\n注意已停机，机器人发送消息不合法\n" + "MessageFromRobot: " + MessageFromRobot, 
                    MessageToRobot,MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly);
                MTKhandlemessagefromrobotfinished = true;

                return null;
            }

            string[] SeparatedMessage = SplitMessageBySeparator(message);

            List<string> SeparatedMessages = new List<string>();

            foreach (var val in SeparatedMessage)
            {
                if (val.Length > 0)
                {
                    SeparatedMessages.Add(val);
                }
            }

            List<string> serialnumbers = new List<string>();

            if (!SendWrongBack(SeparatedMessages.ToArray()))
            {

                foreach (var val in SeparatedMessages.ToArray())
                {
                    if (val.Length >8)
                    {
                        serialnumbers.Add(val);

                    }
                }

                

                return serialnumbers.ToArray();
            }
            else
            {
                MessageToRobot = HandleSendingWrongBackMessage();
                return null;
            }

            

        }

        private void init()
        {
            SNFromRobot.Clear();
            _socket_no.Clear();
            _status.Clear();
            _result.Clear();
            TestResults.Clear();
            MessageFromRobot = "";
            MessageFromRobot = "";
            //robotmessage.tester_id = System.Net.Dns.GetHostName().ToCharArray().ElementAt<char>(6);
            robotmessage.tester_id = 'A';
            robotmessage.EndBit = _splitChar;

            if (robotmessage.serialnumber != null)
            {
                robotmessage.serialnumber.Initialize();

            }
            if (robotmessage.result != null)
            {
                robotmessage.result.Initialize();

            }
            if (robotmessage.socket_no != null)
            {
                robotmessage.socket_no.Initialize();

            }
            if (robotmessage.status != null)
            {
                robotmessage.status.Initialize();

            }



        }

        public void CleanUpMTKRobotHandlerVariable()
        {
            SNFromRobot.Clear();
            _socket_no.Clear();
            _status.Clear();
            _result.Clear();
            TestResults.Clear();
            MessageFromRobot = "";
            MessageFromRobot = "";
            //robotmessage.tester_id = System.Net.Dns.GetHostName().ToCharArray().ElementAt<char>(6);
            robotmessage.tester_id = 'A';
            robotmessage.EndBit = _splitChar;

            if (robotmessage.serialnumber != null)
            {
                robotmessage.serialnumber.Initialize();

            }
            if (robotmessage.result != null)
            {
                robotmessage.result.Initialize();

            }
            if (robotmessage.socket_no != null)
            {
                robotmessage.socket_no.Initialize();

            }
            if (robotmessage.status != null)
            {
                robotmessage.status.Initialize();

            }



        }


        private string HandleSendingMessage(FormattedRobotMessage robotmessage, char[] testresults)
        {
            string message = "";

            if (MessageToRobot == MessageSendWrongBack)
            {
                return MessageSendWrongBack;
            }



            //Last Char as Tester ID
            //Example: Tester_A ==> A
            robotmessage.tester_id = _tester_id;
            robotmessage.EndBit = _splitChar;

            for (int i=0; i < SNFromRobot.ToArray().Length; i++)
            {
                robotmessage.socket_no[i] = _socket_no[i];


                if (_status[i] == '1')
                {
                    //message += robotmessage.tester_id.ToString() + robotmessage.socket_no[i].ToString()
                    //    + SNFromRobot[i].ToString() + TestResults.ToArray()[i].ToString() + robotmessage.EndBit;

                    _result.Add(TestResults.ToArray()[i]);
                }
                else
                {
                    //if robot tells MTK ignore test, set result to "N" and then robot won't count it into yield
                    TestResults.Insert(i,'N');
                    TestResults.RemoveAt(i+1);
                    //message += robotmessage.tester_id.ToString() + robotmessage.socket_no[i].ToString()
                    //    + SNFromRobot[i].ToString() + "N" + robotmessage.EndBit;

                    _result.Add(TestResults.ToArray()[i]);


                }

                //MTK return message to Robot
                message += robotmessage.tester_id.ToString() + robotmessage.socket_no[i].ToString()
                    + SNFromRobot[i].ToString() + TestResults.ToArray()[i].ToString() + robotmessage.EndBit;


            }






            return message;
            


        }

        private string HandleSendingWrongBackMessage()
        {
            string message = MessageSendWrongBack;



            return message;



        }


        private string[] GetSNFromRobot(string[] message_array, int start, int length)
        {

            for (int i = 0; i < message_array.Length; i++)
            {
                _snArray[i] = message_array[i].Substring(start, length);
            }

            return _snArray.ToArray();

        }

        public bool SendMessageToRobot(char[] testresults)
        {
            bool RetVal = false;

            if (MessageToRobot != MessageSendWrongBack  
                && robotmessage.status.Length > 0)
            {
                //for (int i = 0; i < robotmessage.status.Length; i++)
                //{
                //    if (_status[i] == '1')
                //    {
                        
                //        _result.Add(testresults[i]);
                //    }
                //    else
                //    {
                        
                //        _result.Add('N');


                //    }

                //}

                MessageToRobot = HandleSendingMessage(robotmessage, TestResults.ToArray());

            }
            else
            {
                //MessageBox.Show("MessageToRobot: "+ MessageToRobot, "MessageToRobot Invalid Error");
                
            }




            try
            {
                HandleClientCommBack(_client, MessageToRobot);
                RetVal = true;
            }
            catch
            {
                Console.WriteLine(_client.ToString()+ ": has exception for HandleClientCommBack...");
                Console.WriteLine("MessageToRobot: " + MessageToRobot);
                //MessageBox.Show(_client.ToString() + ": has exception for HandleClientCommBack...", "Exception of SendingMessageToRobot");
                RetVal = false;
            }


            //init();

            return RetVal;

        }

        private string[] SplitMessageBySeparator(string message)
        {
            string[] sArray = message.Split(_splitChar);
            List<string> TmpSplitStringArray = new List<string>();
            foreach(string temp in sArray)
            {
                TmpSplitStringArray.Add(temp);
            }

            return TmpSplitStringArray.ToArray();
        }

        private void CombineMessageSendingToRobot()
        {
            throw new System.NotImplementedException();
        }



        private void InitializeTCPConnectionSettings()
        {
            _tcpPort = 3000;

        }

        private char GetTesterIDByReceivingMessage(string[] separatedmessage)
        {
            char tester_id = '0';

            try
            {
                tester_id = separatedmessage[0].ToCharArray().First();

            }
            catch
            {
                tester_id = '?';
            }
            return tester_id;

        }

        private char[] GetSocketNoByReceivingMessage(string[] separatedmessage, int index)
        {
            List<char> socket_no = new List<char>();
            
            foreach (var val in separatedmessage)
            {
                socket_no.Add(val.ToCharArray().ElementAt(index));
            }

            return socket_no.ToArray();

        }

        private char[] GetTestStatusByReceivingMessage(string[] separatedmessage)
        {
            List<char> status = new List<char>();

            foreach (var val in separatedmessage)
            {
                status.Add(val.ToCharArray().Last());
            }

            return status.ToArray();
        }

        private string[] GetSerialNumberByReceivingMessage(string[] separatedmessage, int StartIndex, int length)
        {
            List<string> serialnumbers = new List<string>();

            try
            {
                foreach (var val in separatedmessage)
                {
                    serialnumbers.Add(val.Substring(StartIndex, length));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            return serialnumbers.ToArray();
        }

        public int DUT_SN_StartIndex
        {
            get => default(int);
            set
            {
                this._DUT_SN_StartIndex = this.DUT_SN_StartIndex;
            }
        }

        public int DUT_SN_Length
        {
            get => default(int);
            set
            {
                this._DUT_SN_length = this.DUT_SN_Length;
            }
        }

        /// <param name="separatedmessages">
        /// Check if Robot Message is valid, if no, send wrong back to robot
        /// Check if each separatedmessage is longer than 8 chars
        /// Check if the last char of each separatedmessage has #
        /// Check if the number of separatedmessages is 8
        /// Check if the 2nd char (socket#) of each separatedmessage is within 0 to 8
        /// Check if the last 2nd char (status bit) of each separatedmessage is 1 or 0
        /// </param>
        private bool SendWrongBack(string[] separatedmessages)
        {
            bool RetVal = false;

            foreach (var val in separatedmessages)
            {
                if (val.Length < 8)
                {
                    Console.WriteLine("ReceviedRobotMessage is invalid due to the length: " + val.Length);
                    //MessageBox.Show("ReceviedRobotMessage is invalid due to the length: " + val.Length, "SendWrongBack");
                    RetVal = true;

                }
                if (val.Last().ToString().EndsWith("1") || val.Last().ToString().EndsWith("0"))
                {
                }
                else
                {
                    Console.WriteLine("ReceviedRobotMessage is invalid due to the status bit out of range: "
                        + val.Last<char>().ToString());
                    //MessageBox.Show("ReceviedRobotMessage is invalid due to the status bit out of range: "
                    //    + val.Last<char>().ToString(), "SendWrongBack");
                    RetVal = true;
                }
                if (int.Parse(val.ElementAt(1).ToString()) < 0 || int.Parse(val.ElementAt(1).ToString()) > 8)
                {
                    Console.WriteLine("ReceviedRobotMessage is invalid due to the socket# out of range: " 
                        + val.ElementAt(1).ToString());
                    //MessageBox.Show("ReceviedRobotMessage is invalid due to the socket# out of range: "
                    //    + val.ElementAt(1).ToString(), "SendWrongBack");

                    RetVal = true;
                }


            }

            if (separatedmessages.Length != 8)
            {
                RetVal = true;
            }



            return RetVal;
        }




    }

    public delegate void WriteSNToGridDelegate(string SN, int row);

    public delegate void WriteDategridDelegate(string msg);

    public delegate void updateLableDelegate(string msg);
}