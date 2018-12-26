Release notes:
2018-12-24 (v4.0.8)
1.Add new feature of SWJ shopfloor Access Database to backup the test information and test log onto server with mdb file.


2018-11-06 (v4.0.7)
1.Push to GitHub Repository of CyBLE_MTK_Development_Repository-master
2.Move AppReleaseNotes to Readme.txt
3.Truncate different model name from SN string for different factory
4.Degrade .NET Framework Platform from 4.7.1 to 4.0 so as to be compatible with old computers
5.DUTCurrentMeasureTest is mature
6.DUT Test log is good

2018-10-25 (v4.0.6)
1.change the length of Model from 8 to 9.
2.When IgnoreDUT by robot, skip to RunAllTests()

2018-08-02 (v4.0.5)
1.Improve the display on the TestProgramDataGridView status when running test.


2018-08-01 (v4.0.4)
1.Replace Permission error Messagebox with log print
2.fix the error code assignment for 9100 permission error
3.Trim (remove) all the useless characters from the robot message to get the serial numbers but return the message to robot with original format (including useful and useless characters).
	a. useless characters could be { '\n', '\r', ' ', '\0', '@', '$', '%', '^', '&', '*', '+', '-' }.

2018-07-27 (v4.0.3)
1. Add the new feature of ReopenDutSerialPortIfMissing
2. Add the new feature of ReopenMTKHostSerialPortIfMissing

2018-07-26 (v4.0.2)
1. fix the bug about the error code assignment incorrectly for ProgramAllatEnd Failure and test result wrong.
2. Add retry up to 10 times for UART Capture Dump for check information after programming

2018-07-23 (v4.0.1)
1. fix the bug about error code for delay is not ALL_PASS but test ignored which casued the final error code is 0001
2. Add RedirectTheFirstFinalRowIndexfromDUTInfoDataGridView() in the CyBLEMTKMainForm whenever click TestRunStopButton before everything test
	a.To get the first and final row form DUTInfoDataGridView
	b.Let ProgramAllatBegin and ProgramAllatEnd work correctly
	c.This added feature allows user to no need to configure actual 1st row and final row in the DUTInfoDataGridView in some scenario
3. Add a reminder Messagebox for missing test althrough Test result pass

2017-11-15 (v1.4.2):
1.Add DUTCurrentMeasureDialog
	a. Able to config settings of DUTCurrentMeasureDialog
2.Add Switch_Config_Dialog in the CyBLEMTKMainForm's menustrip --> Tool --> Devices Setup for DUTCurrentMeasure
	a. Able to config Switch Matrix's alias and try to connect
	b. Able to save the alias
2.Add DMM_Config_Dialog in the CyBLEMTKMainForm's menustrip --> Tool --> Devices Setup for DUTCurrentMeasure
	a. Able to config DMM's alias and try to connect
	b. Able to save the alias





Release version: 3.0.0.*				
1.Support tripple mode shopfloor i.e. sigma, fittec and local
	a.config SFCSInterface in the application test settings file to switch mode
	b.when local, it support test long run mode and record the test log into local.csv
2.Add new test of DUTCurrentMeasure which is using U3606A and U2751A and related wiring on fixture
3.Support bi-mode for manual and auto (robot TCP communication)
4.Support the functionality of "save test log for each day" on the method of GenerateTestResultsFileName
5.Offer pass yield chart for test data anaylsis


What's Ok but no try in actual environment
1.Support tripple mode shopfloor i.e. sigma, fittec and local
4.Support the functionality of "save test log for each day" on the method of GenerateTestResultsFileName
5.Offer pass yield chart for test data anaylsis


What's in progress
2.Add new test of DUTCurrentMeasure which is using U3606A and U2751A and related wiring on fixture
3.Support bi-mode for manual and auto (robot TCP communication)
	a.able to get message from TCP tool
	b.able to send back message to TCP tool
	c.able to auto-loop


Known issue:
 3.Support bi-mode for manual and auto (robot TCP communication)
	a.unable to set the ignored DUT according to the TCP message (flag status)
	b.hasn't compatible with manual mode (need keep original functions)



Inheritage from:
CyBLE MTK Application (manual) version v1.4.2.*

======================================================
=============	Definitions&Agreements	 =============
======================================================

TesterID Format

ComputerName(Prefix 6 chars + 1 char)	+	"_"		+	TestHeadID(e.g.fixture ID or/and POGO module ID)

Example: TesterA_01


MessageFromRobot has the following constraints
1.Single string message without any space or special char (except for separated char #)
	For example: A1B012011000317261G1GK1#A2B012011000317261G1GA0#A3B012011000317251EI0A0#A4B012011000317261G1G71#A5B012011000317261G1D61#A6B012011000317261G1G60#A7B012011000317251EHI41#A8B012011000317261G1DE1#


2.The length of DUT serial number is variable. Able to compatible with different module with different length (but same module should has same length of SN)
3.1st char of each segment indicates the tester ID
4.2nd char of each segment indicates the socket#
5.Last 2nd char of each segment indicates the dedicated socket is ok for test or ignore test result
	a.if ignore test result, please set all 1s (i.e. 111111111111111111) for the serial number (the length should be same as others or predefined)


MessageToRobot has the following constraints
1.Same length with MessageFromRobot if non SendWrongBack
2.If robot received no message exceeds timeout, it means MTK is not started due to some reasons. 
Robot just wait some time for retry. If retry failure, just set alarm to inform machine stop.
Need engineer to handle it.
3.If robot received the message of SendWrongBack (i.e.****SendWrongBack****), it means the MessageFromRobot is invalid which could voliate the above constraints.
Robot can resend the MessageFromRobot. if still get the message of SendWrongBack, just set alarm to inform machine stop. 
Need engineer to handle it.

======================================================
=============			END	     		 =============
======================================================


