using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace CyBLE_MTK_Application
{
    public class CyBLEMTKRobotServer : SFCS
    {
        /// <summary>
        /// 
        /// private variables
        /// 
        /// </summary>
        private Socket ClientSocket = null;
        private Socket ServerSocket = null;
        /// <summary>
        /// Server variables
        /// </summary>
        private Thread AcceptThread;
        private volatile int ThreadRunningStage;
        private volatile bool IsStopFlag;
        private volatile bool IsSuppressDbgInThread;

        /// <summary>
        /// Robot Message Handler variables
        /// </summary>
        public SFCS SFCSImp;

        /// <summary>
        /// 
        /// public variables
        /// 
        /// </summary>
        public int ValidDUTInfoCnt;
        static public int gAckMsgTimeoutMsec = -1;
        static public LogDetailLevel gDebugLevel = LogDetailLevel.LogEverything;
        static public int gNumDuts = 8;
        static public CyBLEMTKRobotServer gServer;
        public EventWaitHandle TestResultsUploadedEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        /// <summary>
        /// TCP Server Port e.g. 3000
        /// </summary>
        static public int gServerPort = CyBLE_MTK_Application.Properties.Settings.Default.RobotServerPort;
        static public bool gIsSupervisorMode;

        public class DUTTestInfo
        {
            protected LogManager Log;
            public UInt16 ErrCode; /* SFC Error */
            public string RawData;
            public int SocketID;
            public string TesterID;
            public string SerialNumber;
            public bool TestFlag;

            public DUTTestInfo(LogManager Logger, string RawDataFromRobot)
            {
                if (Logger == null)
                {
                    Logger = new LogManager();
                }
                Log = Logger;
                Initialize(RawDataFromRobot);
            }

            public bool IsValid
            {
                get
                {
                    return (SocketID > 0);
                }
            }

            public bool Initialize(string RawDataFromRobot)
            {
                int ParsedSocketID = 0;
                SocketID = 0;
                RawData = "";
                ErrCode = ECCS.ERRORCODE_DUT_NOT_TEST;
                TestFlag = false;

                if (RawDataFromRobot == null)
                {
                    return false;
                }

                RawData = RawDataFromRobot.Trim(new char[] { '\n', '\r', ' ', '\0', '#' });

                //Include TestID(1) + SocketID(1) + ModuleName(6) + id(1) + TestFlag(1) at least
                if (RawData.Length < 10)
                {
                    Log.PrintLog(this, "ReceviedRobotMessage is invalid due to lenght: " + RawData.Length.ToString(), LogDetailLevel.LogRelevant);
                    return false;
                }

                //Test ID
                TesterID = RawData.Substring(0, 1); //Char[0]

                //Socket ID
                try
                {
                    ParsedSocketID = int.Parse(RawData.Substring(1, 1)); //Char[1]
                    if (ParsedSocketID <= 0 || ParsedSocketID > CyBLEMTKRobotServer.gNumDuts)
                    {
                        Log.PrintLog(this, "ReceviedRobotMessage is invalid due to the socket# out of range: " + ParsedSocketID.ToString(), LogDetailLevel.LogRelevant);
                        return false;
                    }
                }
                catch
                {
                    Log.PrintLog(this, "ReceviedRobotMessage is invalid due to parsing socket# error : " + RawData, LogDetailLevel.LogRelevant);
                    return false;
                }

                //Serial Number
                SerialNumber = RawData.Substring(2, RawData.Length - 3).Trim(new char[] { '\n', '\r', ' ', '\0', '@', '$', '%', '^', '&', '*', '+', '-' });

                //TestFlag
                if (RawData.EndsWith("1"))
                {
                    TestFlag = true;
                }
                else if (RawData.EndsWith("0"))
                {
                    TestFlag = false;
                }
                else
                {
                    Log.PrintLog(this, "ReceviedRobotMessage is invalid due to TestFlag : " + RawData.Last().ToString(), LogDetailLevel.LogRelevant);
                    return false;
                }

                SocketID = ParsedSocketID;

                return true;
            }

            public string GenerateAckMsg()
            {
                string ack = RawData.Substring(0, RawData.Length - 1);

                if (!TestFlag)
                {
                    return ack + "N";
                }

                switch (ErrCode)
                {
                    case ECCS.ERRORCODE_DUT_NOT_TEST:
                        return ack + "N";
                    case ECCS.ERROR_CODE_CAUSED_BY_MTK_TESTER:
                        return ack + "N";
                    case ECCS.ERRORCODE_ALL_PASS:
                        return ack + "P";
                }
                return ack + "F";
            }

            public override string ToString()
            {
                return "RobotDUT#" + SocketID.ToString();
            }
        }


        public CyBLEMTKRobotServer(LogManager Logger):base(Logger)
        {
            SFCSImp = new SFCS(Logger);
            SFCSImp = SFCS.GetSFCS(Logger);

        }

        #region SFCS Interface

        public override string PermissonCheck(string SerialNumber, string Model, string WorkerID, string Station)
        {
            return SFCSImp.PermissonCheck(SerialNumber, Model, WorkerID, Station);
        }

        public override bool UploadTestResult(string SerialNumber, string Model, string TesterID, UInt16 errorcode, string SocketId, string TestResult, string TestStation, string MFI_ID)
        {
            bool IsRequestedResult = false;

            //Socket#'i'      
            int _socketid = int.Parse(SocketId);

            lock (PendingDUTInfos)
            {
                if (_socketid > 0 && _socketid < PendingDUTInfos.Count() && PendingDUTInfos[_socketid] != null)
                {
                    Log.PrintLog(this, "Fill test result of Socket# " + SocketId.ToString(), LogDetailLevel.LogRelevant);
                    PendingDUTInfos[_socketid].ErrCode = errorcode;
                    IsRequestedResult = true;
                }
            }

            if (IsRequestedResult)
            {
                Log.PrintLog(this, "Robot didn't request Socket#" + SocketId.ToString(), LogDetailLevel.LogRelevant);
            }

            if (CyBLEMTKRobotServer.gIsSupervisorMode&&SFCSImp.GetType() != Type.GetType("CyBLE_MTK_Application.SFCS_LOCAL"))
            {
                return true;
            }
            else
            {
                return SFCSImp.UploadTestResult(SerialNumber, Model, TesterID, errorcode, SocketId, TestResult, TestStation, MFI_ID);
            }
        }


        #endregion


        #region Server Functions

        private void AcceptThreadFunc()
        {
            ThreadRunningStage = 1;

            if (!IsSuppressDbgInThread)
            {
                if (OnServerStateChange != null)
                {
                    OnServerStateChange(true);
                }
            }

            while (!IsStopFlag)
            {
                try
                {
                    ThreadRunningStage = 2;

                    if (!IsSuppressDbgInThread)
                    {
                        Log.PrintLog(this, "Accepting ...", gDebugLevel);
                    }

                    ClientSocket = ServerSocket.Accept();

                    ThreadRunningStage = 3;

                    if (!IsSuppressDbgInThread)
                    {
                        Log.PrintLog(this, "Client connected: " + ClientSocket.RemoteEndPoint, gDebugLevel);

                        if (OnRobotStateChange != null)
                        {
                            OnRobotStateChange(true);
                        }
                    }

                    //Talk with Accepted Client
                    TalkWithAcceptedClient(ClientSocket);

                    ThreadRunningStage = 4;

                    try
                    {
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        ClientSocket = null;
                    }

                    ThreadRunningStage = 5;

                    if (!IsSuppressDbgInThread)
                    {
                        Log.PrintLog(this, "Client disconnected: " + ClientSocket.RemoteEndPoint, gDebugLevel);
                        if (OnRobotStateChange != null)
                        {
                            OnRobotStateChange(false);
                        }
                    }

                }
                catch (Exception ex)
                {

                    ThreadRunningStage = 6;
                    IsStopFlag = true;
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "Exception in accept thread " + ex.ToString(), LogDetailLevel.LogRelevant);
                }
            }

        }

        private void TalkWithAcceptedClient(Socket cs)
        {
            byte[] buffer = new byte[1024];
            int recvCnt = 0;

            ThreadRunningStage = 10;
            if (!IsSuppressDbgInThread)
                Log.PrintLog(this, "Session established: " + cs.RemoteEndPoint.ToString(), LogDetailLevel.LogRelevant);

            ClearPendingTests();

            while (IsStopFlag == false)
            {
                try
                {
                    ThreadRunningStage = 11;
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "Waiting robot message ....", LogDetailLevel.LogRelevant);

                    recvCnt = cs.Receive(buffer);

                    if (recvCnt == 0)
                    {
                        ThreadRunningStage = 12;
                        if (!IsSuppressDbgInThread)
                            Log.PrintLog(this, "Session is terminated by remote.", LogDetailLevel.LogRelevant);
                        break;
                    }

                    string msg = Encoding.ASCII.GetString(buffer, 0, recvCnt).Trim(new char[] { '\n', '\r', ' ', '\0' }); ;

                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "RobotMessage: " + msg, LogDetailLevel.LogRelevant);

                    int ret = QueueRobotMessage(msg);

                    if (ret == 0)
                    {
                        if (OnRobotMsgParsed != null)
                            OnRobotMsgParsed();

                        if (!IsSuppressDbgInThread)
                            Log.PrintLog(this, "Waiting test results...", LogDetailLevel.LogRelevant);

                        bool TestResultsUploaded = false;
                        try
                        {
                            TestResultsUploadedEvent.Reset();
                            TestResultsUploaded = TestResultsUploadedEvent.WaitOne(gAckMsgTimeoutMsec);
                        }
                        catch
                        {
                        }

                        if (!TestResultsUploaded)
                        {
                            if (!IsSuppressDbgInThread)
                                Log.PrintLog(this, "Waiting test results timeout.", LogDetailLevel.LogRelevant);
                        }

                        if (IsStopFlag != false)
                        {
                            ThreadRunningStage = 16;
                            if (!IsSuppressDbgInThread)
                                Log.PrintLog(this, "Session was terminated by user.", LogDetailLevel.LogRelevant);
                            break;
                        }

                        //todo 
                        string ackMsg = GenerateAckMessageToRobot();

                        if (!IsSuppressDbgInThread)
                            Log.PrintLog(this, "RobotMessageAck: " + ackMsg, LogDetailLevel.LogRelevant);

                        byte[] ackBytes = Encoding.ASCII.GetBytes(ackMsg.ToArray());
                        if (cs.Send(ackBytes) != ackBytes.Count())
                        {
                            ThreadRunningStage = 13;
                            if (!IsSuppressDbgInThread)
                                Log.PrintLog(this, "Session was terminated duo to sending ack message error.", LogDetailLevel.LogRelevant);
                            break;
                        }
                        ClearPendingTests();
                        continue;
                    }
                    if (ret == 1)
                    {
                        //Message is not completed.
                        continue;
                    }
                    else if (ret == -1)
                    {
                        ThreadRunningStage = 14;
                        if (!IsSuppressDbgInThread)
                            Log.PrintLog(this, "Terminate the session duo to message parsing error.", LogDetailLevel.LogRelevant);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ThreadRunningStage = 15;
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "Exception on receiving message: " + ex.ToString(), LogDetailLevel.LogRelevant);
                        Log.PrintLog(this, "ThreadRunningStage: " + ThreadRunningStage.ToString(), LogDetailLevel.LogRelevant);

                    break;
                }
            }

            if (!IsSuppressDbgInThread)
                Log.PrintLog(this, "Session exit: " + cs.RemoteEndPoint.ToString() + ". " + ThreadRunningStage.ToString(), LogDetailLevel.LogRelevant);
        }

        private string GenerateAckMessageToRobot()
        {
            string ack = "";
            lock (PendingDUTInfos)
            {
                for (int i = 1; i < PendingDUTInfos.Count(); i++)
                {
                    if (PendingDUTInfos[i] != null)
                        ack += (PendingDUTInfos[i].GenerateAckMsg() + "#");
                }
            }
            return ack;
        }



        //Process message from Robot
        private string PendingRobotMsg;
        public DUTTestInfo[] PendingDUTInfos = new DUTTestInfo[gNumDuts + 1];
        public int ValidDUTInfoCount = 0;

        /*
         * Return 
         *  0  : The message is completed.
         *  -1 : Error on parsing message.
         *  1  : Message is not completed.
         */
        private int QueueRobotMessage(string Msg)
        {
            lock (PendingDUTInfos)
            {
                if (ValidDUTInfoCount > 0)
                {
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "Test is in progress. Ignore this Robot message.", LogDetailLevel.LogRelevant);

                    return -1;
                }

                PendingRobotMsg += Msg;

                if (!PendingRobotMsg.EndsWith("#"))
                {
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "RobotMessage is not completed. Wait for teminated char: #", LogDetailLevel.LogRelevant);

                    return 1;
                }

                //Remove the latest '#' char.
                PendingRobotMsg = PendingRobotMsg.Remove(PendingRobotMsg.Length - 1);

                string[] DutStrs = PendingRobotMsg.Split('#');
                if (DutStrs.Count() < gNumDuts)
                {
                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "RobotMessage is not completed. Wait for remaining DUTInfo: " + (gNumDuts - DutStrs.Count()).ToString(), LogDetailLevel.LogRelevant);

                    return 1;
                }

                if (DutStrs.Count() > gNumDuts)
                {
                    ClearPendingTests();

                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "RobotMessage format error. Received DUT info count is out of range: " + DutStrs.Count().ToString(), LogDetailLevel.LogRelevant);

                    return -1;
                }

                ValidDUTInfoCount = 0;
                foreach (string s in DutStrs)
                {
                    DUTTestInfo info = new DUTTestInfo(Log, s);
                    if (info.IsValid && info.SocketID > 0 && info.SocketID <= gNumDuts)
                    {
                        PendingDUTInfos[info.SocketID] = info;
                        ValidDUTInfoCount++;
                    }

                }

                if (ValidDUTInfoCount != gNumDuts)
                {
                    ClearPendingTests();

                    if (!IsSuppressDbgInThread)
                        Log.PrintLog(this, "RobotMessage format error. Valid DUT info count: " + ValidDUTInfoCount.ToString(), LogDetailLevel.LogRelevant);

                    return -1;
                }
            }

            return 0;
        }



        private void ClearPendingTests()
        {
            lock (PendingDUTInfos)
            {
                PendingRobotMsg = "";
                for (int i = 0; i < PendingDUTInfos.Count(); i++)
                {
                    PendingDUTInfos[i] = null;
                }
                ValidDUTInfoCount = 0;
            }
            TestResultsUploadedEvent.Reset();
        }

        static public bool IsGlobalServerActive()
        {

            if (gServer != null)
                return gServer.IsActive();
            return false;
        }

        public bool IsActive()
        {
            return (ThreadRunningStage > 0);
        }


        #endregion



        public bool Start()
        {
            if (IsActive() || AcceptThread != null)
            {
                Log.PrintLog(this, "Server is running now. ThreadState = " + ThreadRunningStage.ToString(), gDebugLevel);
                return false;
            }

            IsStopFlag = false;
            IsSuppressDbgInThread = false;
            ThreadRunningStage = 0;

            try
            {
                if (ServerSocket != null)
                {
                    ServerSocket.Close();
                }

                if (ClientSocket != null)
                {
                    ClientSocket.Close();
                }

                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, gServerPort));
                ServerSocket.Listen(1);

                Log.PrintLog(this, "Start listening ...", gDebugLevel);
            }
            catch (Exception ex)
            {
                Log.PrintLog(this, "Exception on server socket start listening: " + ex.ToString(), LogDetailLevel.LogRelevant);
                return false;
            }

            if (AcceptThread == null)
            {
                Log.PrintLog(this, "Starting RobotThread accept thread ...", gDebugLevel);
                ThreadRunningStage = 0;
                AcceptThread = new Thread(() => AcceptThreadFunc());
                AcceptThread.Name = "MTKRobotSvrThread";
                AcceptThread.Start();
            }

            return true;
        }

        public bool Stop()
        {
            int i;
            bool aRobotConnected = false;
            try
            {
                if (AcceptThread != null && IsStopFlag == false)
                {
                    IsStopFlag = true;
                    IsSuppressDbgInThread = true;
                    aRobotConnected = (ClientSocket != null);
                    if (ClientSocket != null)
                    {
                        try
                        {
                            ClientSocket.Close();
                        }
                        catch (Exception ex)
                        {
                            Log.PrintLog(this, "Exception on Close ClientSocket: " + ex.ToString(), LogDetailLevel.LogRelevant);
                        }
                    }

                    if (ServerSocket != null)
                    {
                        try
                        {
                            //ServerSocket.Shutdown(SocketShutdown.Both);
                            ServerSocket.Close();
                        }
                        catch (Exception ex)
                        {
                            Log.PrintLog(this, "Exception on Close ServerSocket: " + ex.ToString(), LogDetailLevel.LogRelevant);
                        }
                    }

                    TestResultsUploadedEvent.Set();

                    for (i = 0; ((i < 20) && ThreadRunningStage > 0); i++)
                    {
                        Log.PrintLog(this, "Stopping MTKRobotServer retry " + i.ToString() + " IsThreadRunning = " + ThreadRunningStage.ToString(), gDebugLevel);

                        Thread.Sleep(100);
                    }

                    if (i >= 20)
                    {
                        /* Sometimes DevicePollThread can't be scheduled (One case, it is blocked in SerialPort Write/Read) even wait about 3 seconds here */
                        Log.PrintLog(this, AcceptThread.Name + " will be forcedly terminated. IsThreadRunning: " + ThreadRunningStage.ToString(), LogDetailLevel.LogRelevant);
                    }
                    else
                    {
                        Log.PrintLog(this, AcceptThread.Name + " is terminated. IsThreadRunning: " + ThreadRunningStage.ToString(), LogDetailLevel.LogRelevant);
                    }

                    AcceptThread.Abort();

                    AcceptThread = null;

                }
            }
            finally
            {
                ThreadRunningStage = 0;

                IsSuppressDbgInThread = false;

                if (OnRobotStateChange != null && aRobotConnected)
                {
                    OnRobotStateChange(false);
                }

                if (OnServerStateChange != null)
                {
                    OnServerStateChange(false);
                }
            }

            Log.PrintLog(this, "Robot Server is stopped.", LogDetailLevel.LogRelevant);

            return true;
        }


        public event AllDUTAreReadyEventHandler OnRobotMsgParsed;
        public event RobotStateChangeEventHandler OnRobotStateChange;
        public event RobotStateChangeEventHandler OnServerStateChange;

        public delegate void RobotStateChangeEventHandler(bool on);

        public delegate void AllDUTAreReadyEventHandler();
    }


}