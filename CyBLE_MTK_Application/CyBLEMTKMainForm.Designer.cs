using System.Reflection;


namespace CyBLE_MTK_Application
{
    partial class CyBLE_MTK
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyBLE_MTK));
            this.MTKMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewTestProgramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseTestProgramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveTestProgramAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportApplicationSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importApplicationSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.RecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SupervisorModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.TestProgramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PreferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SerialPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MTKHostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DUTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anritsuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dUTSelectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uARTTerminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instrumentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dmmSwitchSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manufacturingModeConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startRobotServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MTKStatStrip = new System.Windows.Forms.StatusStrip();
            this.ApplicationMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.RobotLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.HostStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.DataBaseStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ApplicationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ApplicationStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.DUTInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.DUTInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.DUTcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setHexFileForAllDUTProgrammersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.BDAWriteButton = new System.Windows.Forms.Button();
            this.BDAConfigLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.ConfigBDAButton = new System.Windows.Forms.Button();
            this.BDATextBox = new HexadecimalTextBoxControl.HexadecimalTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.TestProgramGroupBox = new System.Windows.Forms.GroupBox();
            this.TestProgramGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.ResultGroupBox = new System.Windows.Forms.GroupBox();
            this.TestStatusLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.ResetTestButton = new System.Windows.Forms.Button();
            this.TestRunStopButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.label_appmode = new System.Windows.Forms.Label();
            this.groupBox_supervisorControlPanel = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PassCountLabel = new System.Windows.Forms.Label();
            this.CycleTimePerCountLabel = new System.Windows.Forms.Label();
            this.CycleTimeLabel = new System.Windows.Forms.Label();
            this.PassRateLabel = new System.Windows.Forms.Label();
            this.PassFailLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RunCountLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SnipLogBtn = new System.Windows.Forms.Button();
            this.CleanLogBtn = new System.Windows.Forms.Button();
            this.LogGroupBox = new System.Windows.Forms.GroupBox();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.MTKMenuStrip.SuspendLayout();
            this.MTKStatStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.DUTInformationGroupBox.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DUTInfoDataGridView)).BeginInit();
            this.DUTcontextMenuStrip.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.TestProgramGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TestProgramGridView)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.ResultGroupBox.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox_supervisorControlPanel.SuspendLayout();
            this.LogGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MTKMenuStrip
            // 
            this.MTKMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MTKMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.EditMenuItem,
            this.SerialPortMenuItem,
            this.toolsToolStripMenuItem,
            this.HelpMenuItem});
            this.MTKMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MTKMenuStrip.Name = "MTKMenuStrip";
            this.MTKMenuStrip.Size = new System.Drawing.Size(924, 24);
            this.MTKMenuStrip.TabIndex = 0;
            this.MTKMenuStrip.Text = "MTK_menu";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewTestProgramMenuItem,
            this.LoadTestMenuItem,
            this.CloseTestProgramMenuItem,
            this.toolStripSeparator1,
            this.SaveTestMenuItem,
            this.SaveTestProgramAsMenuItem,
            this.toolStripSeparator4,
            this.exportApplicationSettingsToolStripMenuItem,
            this.importApplicationSettingsToolStripMenuItem,
            this.toolStripSeparator5,
            this.RecentMenuItem,
            this.toolStripSeparator2,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "&File";
            // 
            // NewTestProgramMenuItem
            // 
            this.NewTestProgramMenuItem.Enabled = false;
            this.NewTestProgramMenuItem.Name = "NewTestProgramMenuItem";
            this.NewTestProgramMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewTestProgramMenuItem.Size = new System.Drawing.Size(219, 22);
            this.NewTestProgramMenuItem.Text = "&New Test Program";
            this.NewTestProgramMenuItem.Click += new System.EventHandler(this.NewTestProgramMenuItem_Click);
            // 
            // LoadTestMenuItem
            // 
            this.LoadTestMenuItem.Name = "LoadTestMenuItem";
            this.LoadTestMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.LoadTestMenuItem.Size = new System.Drawing.Size(219, 22);
            this.LoadTestMenuItem.Text = "&Load Test Program";
            this.LoadTestMenuItem.Click += new System.EventHandler(this.LoadTestMenuItem_Click);
            // 
            // CloseTestProgramMenuItem
            // 
            this.CloseTestProgramMenuItem.Enabled = false;
            this.CloseTestProgramMenuItem.Name = "CloseTestProgramMenuItem";
            this.CloseTestProgramMenuItem.Size = new System.Drawing.Size(219, 22);
            this.CloseTestProgramMenuItem.Text = "&Close Test Program";
            this.CloseTestProgramMenuItem.Click += new System.EventHandler(this.CloseTestProgramMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // SaveTestMenuItem
            // 
            this.SaveTestMenuItem.Enabled = false;
            this.SaveTestMenuItem.Name = "SaveTestMenuItem";
            this.SaveTestMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveTestMenuItem.Size = new System.Drawing.Size(219, 22);
            this.SaveTestMenuItem.Text = "&Save Test Program";
            this.SaveTestMenuItem.Click += new System.EventHandler(this.SaveTestMenuItem_Click);
            // 
            // SaveTestProgramAsMenuItem
            // 
            this.SaveTestProgramAsMenuItem.Enabled = false;
            this.SaveTestProgramAsMenuItem.Name = "SaveTestProgramAsMenuItem";
            this.SaveTestProgramAsMenuItem.Size = new System.Drawing.Size(219, 22);
            this.SaveTestProgramAsMenuItem.Text = "Save Test Program &As...";
            this.SaveTestProgramAsMenuItem.Click += new System.EventHandler(this.SaveTestProgramAsMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(216, 6);
            // 
            // exportApplicationSettingsToolStripMenuItem
            // 
            this.exportApplicationSettingsToolStripMenuItem.Enabled = false;
            this.exportApplicationSettingsToolStripMenuItem.Name = "exportApplicationSettingsToolStripMenuItem";
            this.exportApplicationSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportApplicationSettingsToolStripMenuItem.Text = "&Export Application Settings";
            this.exportApplicationSettingsToolStripMenuItem.Click += new System.EventHandler(this.exportApplicationSettingsToolStripMenuItem_Click);
            // 
            // importApplicationSettingsToolStripMenuItem
            // 
            this.importApplicationSettingsToolStripMenuItem.Enabled = false;
            this.importApplicationSettingsToolStripMenuItem.Name = "importApplicationSettingsToolStripMenuItem";
            this.importApplicationSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.importApplicationSettingsToolStripMenuItem.Text = "I&mport Application Settings";
            this.importApplicationSettingsToolStripMenuItem.Click += new System.EventHandler(this.importApplicationSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(216, 6);
            // 
            // RecentMenuItem
            // 
            this.RecentMenuItem.Name = "RecentMenuItem";
            this.RecentMenuItem.Size = new System.Drawing.Size(219, 22);
            this.RecentMenuItem.Text = "&Recent Test Program";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(219, 22);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SupervisorModeMenuItem,
            this.toolStripSeparator3,
            this.TestProgramMenuItem,
            this.PreferencesMenuItem});
            this.EditMenuItem.Name = "EditMenuItem";
            this.EditMenuItem.Size = new System.Drawing.Size(39, 20);
            this.EditMenuItem.Text = "&Edit";
            // 
            // SupervisorModeMenuItem
            // 
            this.SupervisorModeMenuItem.Name = "SupervisorModeMenuItem";
            this.SupervisorModeMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.SupervisorModeMenuItem.Size = new System.Drawing.Size(226, 22);
            this.SupervisorModeMenuItem.Text = "&Supervisor Mode";
            this.SupervisorModeMenuItem.Click += new System.EventHandler(this.SupervisorModeMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(223, 6);
            // 
            // TestProgramMenuItem
            // 
            this.TestProgramMenuItem.Enabled = false;
            this.TestProgramMenuItem.Name = "TestProgramMenuItem";
            this.TestProgramMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.T)));
            this.TestProgramMenuItem.Size = new System.Drawing.Size(226, 22);
            this.TestProgramMenuItem.Text = "&Test Program";
            this.TestProgramMenuItem.Click += new System.EventHandler(this.TestProgramMenuItem_Click);
            // 
            // PreferencesMenuItem
            // 
            this.PreferencesMenuItem.Enabled = false;
            this.PreferencesMenuItem.Name = "PreferencesMenuItem";
            this.PreferencesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.P)));
            this.PreferencesMenuItem.Size = new System.Drawing.Size(226, 22);
            this.PreferencesMenuItem.Text = "&Preferences";
            this.PreferencesMenuItem.Click += new System.EventHandler(this.PreferencesMenuItem_Click);
            // 
            // SerialPortMenuItem
            // 
            this.SerialPortMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MTKHostToolStripMenuItem,
            this.DUTToolStripMenuItem,
            this.anritsuToolStripMenuItem,
            this.dUTSelectorToolStripMenuItem});
            this.SerialPortMenuItem.Name = "SerialPortMenuItem";
            this.SerialPortMenuItem.Size = new System.Drawing.Size(72, 20);
            this.SerialPortMenuItem.Text = "&Serial Port";
            // 
            // MTKHostToolStripMenuItem
            // 
            this.MTKHostToolStripMenuItem.Name = "MTKHostToolStripMenuItem";
            this.MTKHostToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.MTKHostToolStripMenuItem.Text = "&MTK Host";
            this.MTKHostToolStripMenuItem.Click += new System.EventHandler(this.MTKHostToolStripMenuItem_Click);
            // 
            // DUTToolStripMenuItem
            // 
            this.DUTToolStripMenuItem.Name = "DUTToolStripMenuItem";
            this.DUTToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.DUTToolStripMenuItem.Text = "&DUT";
            // 
            // anritsuToolStripMenuItem
            // 
            this.anritsuToolStripMenuItem.Name = "anritsuToolStripMenuItem";
            this.anritsuToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.anritsuToolStripMenuItem.Text = "&Anritsu";
            this.anritsuToolStripMenuItem.Click += new System.EventHandler(this.anritsuToolStripMenuItem_Click);
            // 
            // dUTSelectorToolStripMenuItem
            // 
            this.dUTSelectorToolStripMenuItem.Name = "dUTSelectorToolStripMenuItem";
            this.dUTSelectorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.dUTSelectorToolStripMenuItem.Text = "DUT &Multiplexer";
            this.dUTSelectorToolStripMenuItem.Click += new System.EventHandler(this.dUTSelectorToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uARTTerminalToolStripMenuItem,
            this.dataStatisticsToolStripMenuItem,
            this.instrumentsToolStripMenuItem,
            this.manufacturingModeConfigToolStripMenuItem,
            this.startRobotServerToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // uARTTerminalToolStripMenuItem
            // 
            this.uARTTerminalToolStripMenuItem.Name = "uARTTerminalToolStripMenuItem";
            this.uARTTerminalToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.uARTTerminalToolStripMenuItem.Text = "&UART Terminal";
            // 
            // dataStatisticsToolStripMenuItem
            // 
            this.dataStatisticsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yieldToolStripMenuItem});
            this.dataStatisticsToolStripMenuItem.Name = "dataStatisticsToolStripMenuItem";
            this.dataStatisticsToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.dataStatisticsToolStripMenuItem.Text = "Data Statistics";
            // 
            // yieldToolStripMenuItem
            // 
            this.yieldToolStripMenuItem.Name = "yieldToolStripMenuItem";
            this.yieldToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.yieldToolStripMenuItem.Text = "Yield";
            this.yieldToolStripMenuItem.Click += new System.EventHandler(this.yieldToolStripMenuItem_Click);
            // 
            // instrumentsToolStripMenuItem
            // 
            this.instrumentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dmmSwitchSettingsToolStripMenuItem});
            this.instrumentsToolStripMenuItem.Name = "instrumentsToolStripMenuItem";
            this.instrumentsToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.instrumentsToolStripMenuItem.Text = "Instruments Setup";
            // 
            // dmmSwitchSettingsToolStripMenuItem
            // 
            this.dmmSwitchSettingsToolStripMenuItem.Name = "dmmSwitchSettingsToolStripMenuItem";
            this.dmmSwitchSettingsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.dmmSwitchSettingsToolStripMenuItem.Text = "Dmm&&Switch Settings";
            this.dmmSwitchSettingsToolStripMenuItem.Click += new System.EventHandler(this.dmmSwitchSettingsToolStripMenuItem_Click);
            // 
            // manufacturingModeConfigToolStripMenuItem
            // 
            this.manufacturingModeConfigToolStripMenuItem.Name = "manufacturingModeConfigToolStripMenuItem";
            this.manufacturingModeConfigToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.M)));
            this.manufacturingModeConfigToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.manufacturingModeConfigToolStripMenuItem.Text = "Manufacturing Mode Settings";
            this.manufacturingModeConfigToolStripMenuItem.Click += new System.EventHandler(this.manufacturingModeConfigToolStripMenuItem_Click);
            // 
            // startRobotServerToolStripMenuItem
            // 
            this.startRobotServerToolStripMenuItem.Name = "startRobotServerToolStripMenuItem";
            this.startRobotServerToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.startRobotServerToolStripMenuItem.Text = "StartRobotServer";
            this.startRobotServerToolStripMenuItem.Click += new System.EventHandler(this.startRobotServerToolStripMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "&Help";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this.AboutMenuItem.Text = "&About";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // MTKStatStrip
            // 
            this.MTKStatStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MTKStatStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ApplicationMode,
            this.RobotLabel,
            this.toolStripStatusLabel1,
            this.HostStatus,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.DataBaseStatus,
            this.toolStripStatusLabel3,
            this.ApplicationStatusLabel,
            this.ApplicationStatus});
            this.MTKStatStrip.Location = new System.Drawing.Point(0, 610);
            this.MTKStatStrip.Name = "MTKStatStrip";
            this.MTKStatStrip.Size = new System.Drawing.Size(924, 22);
            this.MTKStatStrip.TabIndex = 4;
            this.MTKStatStrip.Text = "statusStrip1";
            // 
            // ApplicationMode
            // 
            this.ApplicationMode.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.ApplicationMode.Name = "ApplicationMode";
            this.ApplicationMode.Size = new System.Drawing.Size(39, 17);
            this.ApplicationMode.Text = "Tester";
            // 
            // RobotLabel
            // 
            this.RobotLabel.BackColor = System.Drawing.Color.LightGray;
            this.RobotLabel.ForeColor = System.Drawing.Color.White;
            this.RobotLabel.Name = "RobotLabel";
            this.RobotLabel.Size = new System.Drawing.Size(39, 17);
            this.RobotLabel.Text = "Robot";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(4, 20);
            // 
            // HostStatus
            // 
            this.HostStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.HostStatus.Name = "HostStatus";
            this.HostStatus.Size = new System.Drawing.Size(32, 17);
            this.HostStatus.Text = "Host";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(4, 20);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(30, 17);
            this.toolStripStatusLabel4.Text = "DUT";
            // 
            // DataBaseStatus
            // 
            this.DataBaseStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.DataBaseStatus.Name = "DataBaseStatus";
            this.DataBaseStatus.Size = new System.Drawing.Size(110, 17);
            this.DataBaseStatus.Text = "Shopfloor Database";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(4, 20);
            // 
            // ApplicationStatusLabel
            // 
            this.ApplicationStatusLabel.Name = "ApplicationStatusLabel";
            this.ApplicationStatusLabel.Size = new System.Drawing.Size(45, 17);
            this.ApplicationStatusLabel.Text = "Status :";
            // 
            // ApplicationStatus
            // 
            this.ApplicationStatus.Name = "ApplicationStatus";
            this.ApplicationStatus.Size = new System.Drawing.Size(26, 17);
            this.ApplicationStatus.Text = "Idle";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.MainSplitContainer, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(924, 586);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel4);
            this.MainSplitContainer.Panel1MinSize = 231;
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.groupBox_supervisorControlPanel);
            this.MainSplitContainer.Panel2.Controls.Add(this.LogGroupBox);
            this.MainSplitContainer.Size = new System.Drawing.Size(924, 586);
            this.MainSplitContainer.SplitterDistance = 346;
            this.MainSplitContainer.TabIndex = 25;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.81385F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.18615F));
            this.tableLayoutPanel4.Controls.Add(this.DUTInformationGroupBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(924, 346);
            this.tableLayoutPanel4.TabIndex = 24;
            // 
            // DUTInformationGroupBox
            // 
            this.DUTInformationGroupBox.Controls.Add(this.tableLayoutPanel10);
            this.DUTInformationGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DUTInformationGroupBox.Location = new System.Drawing.Point(3, 3);
            this.DUTInformationGroupBox.Name = "DUTInformationGroupBox";
            this.DUTInformationGroupBox.Size = new System.Drawing.Size(481, 340);
            this.DUTInformationGroupBox.TabIndex = 0;
            this.DUTInformationGroupBox.TabStop = false;
            this.DUTInformationGroupBox.Text = "DUT Information";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.DUTInfoDataGridView, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel12, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(475, 321);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // DUTInfoDataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DUTInfoDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DUTInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DUTInfoDataGridView.ContextMenuStrip = this.DUTcontextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DUTInfoDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.DUTInfoDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DUTInfoDataGridView.Location = new System.Drawing.Point(0, 56);
            this.DUTInfoDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.DUTInfoDataGridView.Name = "DUTInfoDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DUTInfoDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DUTInfoDataGridView.Size = new System.Drawing.Size(475, 265);
            this.DUTInfoDataGridView.TabIndex = 37;
            // 
            // DUTcontextMenuStrip
            // 
            this.DUTcontextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.DUTcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setHexFileForAllDUTProgrammersToolStripMenuItem});
            this.DUTcontextMenuStrip.Name = "DUTcontextMenuStrip";
            this.DUTcontextMenuStrip.Size = new System.Drawing.Size(273, 26);
            // 
            // setHexFileForAllDUTProgrammersToolStripMenuItem
            // 
            this.setHexFileForAllDUTProgrammersToolStripMenuItem.Name = "setHexFileForAllDUTProgrammersToolStripMenuItem";
            this.setHexFileForAllDUTProgrammersToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.setHexFileForAllDUTProgrammersToolStripMenuItem.Text = "Set &Hex File For All DUT Programmers";
            this.setHexFileForAllDUTProgrammersToolStripMenuItem.Click += new System.EventHandler(this.setHexFileForAllDUTProgrammersToolStripMenuItem_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel11.Controls.Add(this.BDAWriteButton, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.BDAConfigLabel, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(475, 28);
            this.tableLayoutPanel11.TabIndex = 38;
            // 
            // BDAWriteButton
            // 
            this.BDAWriteButton.Location = new System.Drawing.Point(410, 3);
            this.BDAWriteButton.Name = "BDAWriteButton";
            this.BDAWriteButton.Size = new System.Drawing.Size(62, 21);
            this.BDAWriteButton.TabIndex = 38;
            this.BDAWriteButton.Text = "&Write";
            this.BDAWriteButton.UseVisualStyleBackColor = true;
            this.BDAWriteButton.Click += new System.EventHandler(this.BDAWriteButton_Click);
            // 
            // BDAConfigLabel
            // 
            this.BDAConfigLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BDAConfigLabel.AutoSize = true;
            this.BDAConfigLabel.Location = new System.Drawing.Point(3, 7);
            this.BDAConfigLabel.Name = "BDAConfigLabel";
            this.BDAConfigLabel.Size = new System.Drawing.Size(0, 13);
            this.BDAConfigLabel.TabIndex = 39;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 3;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel12.Controls.Add(this.ConfigBDAButton, 2, 0);
            this.tableLayoutPanel12.Controls.Add(this.BDATextBox, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(475, 28);
            this.tableLayoutPanel12.TabIndex = 39;
            // 
            // ConfigBDAButton
            // 
            this.ConfigBDAButton.Location = new System.Drawing.Point(410, 3);
            this.ConfigBDAButton.Name = "ConfigBDAButton";
            this.ConfigBDAButton.Size = new System.Drawing.Size(62, 21);
            this.ConfigBDAButton.TabIndex = 35;
            this.ConfigBDAButton.Text = "Op&tions";
            this.ConfigBDAButton.UseVisualStyleBackColor = true;
            this.ConfigBDAButton.Click += new System.EventHandler(this.ConfigBDAButton_Click);
            // 
            // BDATextBox
            // 
            this.BDATextBox.Delimiter = ":";
            this.BDATextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BDATextBox.Location = new System.Drawing.Point(75, 3);
            this.BDATextBox.MaxLength = 17;
            this.BDATextBox.Name = "BDATextBox";
            this.BDATextBox.ReadOnly = true;
            this.BDATextBox.Size = new System.Drawing.Size(329, 20);
            this.BDATextBox.TabIndex = 40;
            this.BDATextBox.Text = "00:00:00:00:00:00";
            this.BDATextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BDATextBox.TextChanged += new System.EventHandler(this.BDATextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "BD Address:";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.TestProgramGroupBox, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(487, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(437, 346);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // TestProgramGroupBox
            // 
            this.TestProgramGroupBox.Controls.Add(this.TestProgramGridView);
            this.TestProgramGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestProgramGroupBox.Location = new System.Drawing.Point(3, 125);
            this.TestProgramGroupBox.Name = "TestProgramGroupBox";
            this.TestProgramGroupBox.Size = new System.Drawing.Size(431, 218);
            this.TestProgramGroupBox.TabIndex = 24;
            this.TestProgramGroupBox.TabStop = false;
            this.TestProgramGroupBox.Text = "Test Program";
            // 
            // TestProgramGridView
            // 
            this.TestProgramGridView.AllowDrop = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TestProgramGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.TestProgramGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TestProgramGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.TestProgramGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestProgramGridView.Location = new System.Drawing.Point(3, 16);
            this.TestProgramGridView.Margin = new System.Windows.Forms.Padding(0);
            this.TestProgramGridView.Name = "TestProgramGridView";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TestProgramGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.TestProgramGridView.Size = new System.Drawing.Size(425, 199);
            this.TestProgramGridView.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.ResultGroupBox, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(437, 122);
            this.tableLayoutPanel6.TabIndex = 25;
            // 
            // ResultGroupBox
            // 
            this.ResultGroupBox.Controls.Add(this.TestStatusLabel);
            this.ResultGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultGroupBox.Location = new System.Drawing.Point(76, 3);
            this.ResultGroupBox.MinimumSize = new System.Drawing.Size(268, 113);
            this.ResultGroupBox.Name = "ResultGroupBox";
            this.ResultGroupBox.Size = new System.Drawing.Size(358, 116);
            this.ResultGroupBox.TabIndex = 23;
            this.ResultGroupBox.TabStop = false;
            this.ResultGroupBox.Text = "Result";
            // 
            // TestStatusLabel
            // 
            this.TestStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestStatusLabel.ForeColor = System.Drawing.Color.Green;
            this.TestStatusLabel.Location = new System.Drawing.Point(3, 16);
            this.TestStatusLabel.Name = "TestStatusLabel";
            this.TestStatusLabel.Size = new System.Drawing.Size(352, 97);
            this.TestStatusLabel.TabIndex = 1;
            this.TestStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.ResetTestButton, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.TestRunStopButton, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.StopButton, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label_appmode, 0, 3);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 4;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(73, 122);
            this.tableLayoutPanel7.TabIndex = 24;
            // 
            // ResetTestButton
            // 
            this.ResetTestButton.AutoSize = true;
            this.ResetTestButton.Enabled = false;
            this.ResetTestButton.Image = global::CyBLE_MTK_Application.Properties.Resources.reset;
            this.ResetTestButton.Location = new System.Drawing.Point(3, 82);
            this.ResetTestButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ResetTestButton.Name = "ResetTestButton";
            this.ResetTestButton.Size = new System.Drawing.Size(67, 24);
            this.ResetTestButton.TabIndex = 5;
            this.ResetTestButton.Text = "R&eset";
            this.ResetTestButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ResetTestButton.UseVisualStyleBackColor = true;
            this.ResetTestButton.Click += new System.EventHandler(this.ResetTestButton_Click);
            // 
            // TestRunStopButton
            // 
            this.TestRunStopButton.AutoSize = true;
            this.TestRunStopButton.Enabled = false;
            this.TestRunStopButton.Image = ((System.Drawing.Image)(resources.GetObject("TestRunStopButton.Image")));
            this.TestRunStopButton.Location = new System.Drawing.Point(3, 3);
            this.TestRunStopButton.Name = "TestRunStopButton";
            this.TestRunStopButton.Size = new System.Drawing.Size(67, 51);
            this.TestRunStopButton.TabIndex = 4;
            this.TestRunStopButton.Text = "&Start";
            this.TestRunStopButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.TestRunStopButton.UseVisualStyleBackColor = true;
            this.TestRunStopButton.Click += new System.EventHandler(this.TestRunStopButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.AutoSize = true;
            this.StopButton.Enabled = false;
            this.StopButton.Image = global::CyBLE_MTK_Application.Properties.Resources.Stop_red_Icon_16;
            this.StopButton.Location = new System.Drawing.Point(3, 58);
            this.StopButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(67, 24);
            this.StopButton.TabIndex = 6;
            this.StopButton.Text = "&Stop";
            this.StopButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // label_appmode
            // 
            this.label_appmode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_appmode.AutoSize = true;
            this.label_appmode.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label_appmode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_appmode.Location = new System.Drawing.Point(2, 107);
            this.label_appmode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_appmode.Name = "label_appmode";
            this.label_appmode.Size = new System.Drawing.Size(69, 13);
            this.label_appmode.TabIndex = 7;
            this.label_appmode.Text = "Undefined";
            this.label_appmode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_supervisorControlPanel
            // 
            this.groupBox_supervisorControlPanel.Controls.Add(this.label3);
            this.groupBox_supervisorControlPanel.Controls.Add(this.PassCountLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.CycleTimePerCountLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.CycleTimeLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.PassRateLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.PassFailLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.label2);
            this.groupBox_supervisorControlPanel.Controls.Add(this.RunCountLabel);
            this.groupBox_supervisorControlPanel.Controls.Add(this.button1);
            this.groupBox_supervisorControlPanel.Controls.Add(this.SnipLogBtn);
            this.groupBox_supervisorControlPanel.Controls.Add(this.CleanLogBtn);
            this.groupBox_supervisorControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox_supervisorControlPanel.Location = new System.Drawing.Point(554, 0);
            this.groupBox_supervisorControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_supervisorControlPanel.Name = "groupBox_supervisorControlPanel";
            this.groupBox_supervisorControlPanel.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_supervisorControlPanel.Size = new System.Drawing.Size(370, 236);
            this.groupBox_supervisorControlPanel.TabIndex = 24;
            this.groupBox_supervisorControlPanel.TabStop = false;
            this.groupBox_supervisorControlPanel.Text = "Supervisor Control Panel";
            this.groupBox_supervisorControlPanel.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 136);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Pass Rate:";
            // 
            // PassCountLabel
            // 
            this.PassCountLabel.AutoSize = true;
            this.PassCountLabel.Location = new System.Drawing.Point(150, 110);
            this.PassCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PassCountLabel.Name = "PassCountLabel";
            this.PassCountLabel.Size = new System.Drawing.Size(16, 13);
            this.PassCountLabel.TabIndex = 12;
            this.PassCountLabel.Text = "---";
            // 
            // CycleTimePerCountLabel
            // 
            this.CycleTimePerCountLabel.AutoSize = true;
            this.CycleTimePerCountLabel.Location = new System.Drawing.Point(150, 162);
            this.CycleTimePerCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CycleTimePerCountLabel.Name = "CycleTimePerCountLabel";
            this.CycleTimePerCountLabel.Size = new System.Drawing.Size(16, 13);
            this.CycleTimePerCountLabel.TabIndex = 11;
            this.CycleTimePerCountLabel.Text = "---";
            // 
            // CycleTimeLabel
            // 
            this.CycleTimeLabel.AutoSize = true;
            this.CycleTimeLabel.Location = new System.Drawing.Point(6, 162);
            this.CycleTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CycleTimeLabel.Name = "CycleTimeLabel";
            this.CycleTimeLabel.Size = new System.Drawing.Size(112, 13);
            this.CycleTimeLabel.TabIndex = 10;
            this.CycleTimeLabel.Text = "Cycle Time Per Count:";
            // 
            // PassRateLabel
            // 
            this.PassRateLabel.AutoSize = true;
            this.PassRateLabel.Location = new System.Drawing.Point(150, 136);
            this.PassRateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PassRateLabel.Name = "PassRateLabel";
            this.PassRateLabel.Size = new System.Drawing.Size(16, 13);
            this.PassRateLabel.TabIndex = 9;
            this.PassRateLabel.Text = "---";
            // 
            // PassFailLabel
            // 
            this.PassFailLabel.AutoSize = true;
            this.PassFailLabel.Location = new System.Drawing.Point(6, 110);
            this.PassFailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PassFailLabel.Name = "PassFailLabel";
            this.PassFailLabel.Size = new System.Drawing.Size(64, 13);
            this.PassFailLabel.TabIndex = 8;
            this.PassFailLabel.Text = "Pass Count:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total Test Count:";
            // 
            // RunCountLabel
            // 
            this.RunCountLabel.AutoSize = true;
            this.RunCountLabel.Location = new System.Drawing.Point(150, 83);
            this.RunCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RunCountLabel.Name = "RunCountLabel";
            this.RunCountLabel.Size = new System.Drawing.Size(16, 13);
            this.RunCountLabel.TabIndex = 6;
            this.RunCountLabel.Text = "---";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 19);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 41);
            this.button1.TabIndex = 3;
            this.button1.Text = "Clean Stats";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SnipLogBtn
            // 
            this.SnipLogBtn.Location = new System.Drawing.Point(95, 19);
            this.SnipLogBtn.Margin = new System.Windows.Forms.Padding(2);
            this.SnipLogBtn.Name = "SnipLogBtn";
            this.SnipLogBtn.Size = new System.Drawing.Size(83, 41);
            this.SnipLogBtn.TabIndex = 2;
            this.SnipLogBtn.Text = "Snip Log";
            this.SnipLogBtn.UseVisualStyleBackColor = true;
            this.SnipLogBtn.Click += new System.EventHandler(this.SnipLogBtn_Click);
            // 
            // CleanLogBtn
            // 
            this.CleanLogBtn.Location = new System.Drawing.Point(8, 19);
            this.CleanLogBtn.Margin = new System.Windows.Forms.Padding(2);
            this.CleanLogBtn.Name = "CleanLogBtn";
            this.CleanLogBtn.Size = new System.Drawing.Size(83, 41);
            this.CleanLogBtn.TabIndex = 0;
            this.CleanLogBtn.Text = "Clean Log";
            this.CleanLogBtn.UseVisualStyleBackColor = true;
            this.CleanLogBtn.Click += new System.EventHandler(this.CleanLogBtn_Click);
            // 
            // LogGroupBox
            // 
            this.LogGroupBox.Controls.Add(this.LogTextBox);
            this.LogGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogGroupBox.Location = new System.Drawing.Point(0, 0);
            this.LogGroupBox.Name = "LogGroupBox";
            this.LogGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 375, 3);
            this.LogGroupBox.Size = new System.Drawing.Size(924, 236);
            this.LogGroupBox.TabIndex = 23;
            this.LogGroupBox.TabStop = false;
            this.LogGroupBox.Text = "Log";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.LogTextBox.Location = new System.Drawing.Point(3, 16);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTextBox.Size = new System.Drawing.Size(921, 217);
            this.LogTextBox.TabIndex = 6;
            // 
            // CyBLE_MTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 632);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.MTKStatStrip);
            this.Controls.Add(this.MTKMenuStrip);
            this.MainMenuStrip = this.MTKMenuStrip;
            this.MinimumSize = new System.Drawing.Size(639, 478);
            this.Name = "CyBLE_MTK";
            this.Text = "CyBLE MTK Application";
            this.Load += new System.EventHandler(this.CyBLE_MTK_Load);
            this.MTKMenuStrip.ResumeLayout(false);
            this.MTKMenuStrip.PerformLayout();
            this.MTKStatStrip.ResumeLayout(false);
            this.MTKStatStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.DUTInformationGroupBox.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DUTInfoDataGridView)).EndInit();
            this.DUTcontextMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.TestProgramGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TestProgramGridView)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.ResultGroupBox.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.groupBox_supervisorControlPanel.ResumeLayout(false);
            this.groupBox_supervisorControlPanel.PerformLayout();
            this.LogGroupBox.ResumeLayout(false);
            this.LogGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MTKMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadTestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveTestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PreferencesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.StatusStrip MTKStatStrip;
        private System.Windows.Forms.ToolStripStatusLabel HostStatus;
        private System.Windows.Forms.ToolStripStatusLabel DataBaseStatus;
        private System.Windows.Forms.ToolStripStatusLabel ApplicationStatus;
        private System.Windows.Forms.ToolStripStatusLabel ApplicationStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem TestProgramMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem SerialPortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SupervisorModeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripStatusLabel ApplicationMode;
        private System.Windows.Forms.ToolStripMenuItem SaveTestProgramAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RecentMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CloseTestProgramMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem NewTestProgramMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MTKHostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DUTToolStripMenuItem;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox DUTInformationGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox TestProgramGroupBox;
        private System.Windows.Forms.DataGridView TestProgramGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.GroupBox ResultGroupBox;
        private System.Windows.Forms.Label TestStatusLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button ResetTestButton;
        private System.Windows.Forms.Button TestRunStopButton;
        private System.Windows.Forms.GroupBox LogGroupBox;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Button ConfigBDAButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Button BDAWriteButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private HexadecimalTextBoxControl.HexadecimalTextBox BDATextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BDAConfigLabel;
        private System.Windows.Forms.ToolStripMenuItem anritsuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dUTSelectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uARTTerminalToolStripMenuItem;
        private System.Windows.Forms.DataGridView DUTInfoDataGridView;
        private System.Windows.Forms.ToolStripMenuItem exportApplicationSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem importApplicationSettingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip DUTcontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setHexFileForAllDUTProgrammersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startRobotServerToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox_supervisorControlPanel;
        private System.Windows.Forms.Button CleanLogBtn;
        private System.Windows.Forms.Button SnipLogBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label CycleTimePerCountLabel;
        private System.Windows.Forms.Label CycleTimeLabel;
        private System.Windows.Forms.Label PassRateLabel;
        private System.Windows.Forms.Label PassFailLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label RunCountLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label PassCountLabel;
        private System.Windows.Forms.ToolStripStatusLabel RobotLabel;
        private System.Windows.Forms.Label label_appmode;
        private System.Windows.Forms.ToolStripMenuItem instrumentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dmmSwitchSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manufacturingModeConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
    }
}

