/*********************************************************************
 * NetToPLCsim, Netzwerkanbindung fuer PLCSIM
 * 
 * Copyright (C) 2011-2016 Thomas Wiens, th.wiens@gmx.de
 *
 * This file is part of NetToPLCsim.
 *
 * NetToPLCsim is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 /*********************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.ServiceProcess;
using IsoOnTcp;
using Ini;

namespace NetToPLCSim
{
    public partial class FormMain : Form
    {
        Config m_Conf = new Config();
        List<IsoToS7online> m_servers = new List<IsoToS7online>();

        string m_ConfigName = "";
        bool m_S7DOSServiceStopped = false;
        string m_IEPGhelperServiceName = String.Empty;

        CmdLineArgs StartArgs = new CmdLineArgs();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = "NetToPLCsim::s7o";
            m_IEPGhelperServiceName = Tools.GetS7DOSHelperServiceName();
            SetToolTipText();
            SetHelpShortcutKeys();
            InitStationDataGridView();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
            if (m_S7DOSServiceStopped == true)
            {
                DialogResult result = MessageBox.Show(
                     "You have stopped the 'SIMATIC S7DOS Help Service'.\n"
                    + "Remind to restart the service before using other Simatic software.\n\n"
                    + "Try to restart the service?",
                    "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Tools.StartService(m_IEPGhelperServiceName, 20000, false, true);
                    m_S7DOSServiceStopped = false;
                }
            }
        }

        private void SetToolTipText()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnStartServer, "Start NetToPLCsim server");
            toolTip.SetToolTip(btnStopServer, "Stop NetToPLCsim server");
            toolTip.SetToolTip(btnAdd, "Add a new station to configuration");
            toolTip.SetToolTip(btnModify, "Modify the selected station configuration");
            toolTip.SetToolTip(btnDelete, "Delete the selected station from list");
        }

        private void SetHelpShortcutKeys()
        {
            if (Application.CurrentCulture.Name == "de-DE")
            {
                manualdeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
                manualenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            }
            else
            {
                manualdeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
                manualenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            }
        }

        private void InitStationDataGridView()
        {
            dgvStations.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.Width = 110;

            DataGridViewTextBoxColumn netipColumn = new DataGridViewTextBoxColumn();
            netipColumn.DataPropertyName = "NetworkIpAddress";
            netipColumn.HeaderText = "Network address";
            netipColumn.Width = 120;

            DataGridViewTextBoxColumn plcsimipColumn = new DataGridViewTextBoxColumn();
            plcsimipColumn.DataPropertyName = "PlcsimIpAddress";
            plcsimipColumn.HeaderText = "Plcsim address";
            plcsimipColumn.Width = 120;

            DataGridViewTextBoxColumn rackSlotColumn = new DataGridViewTextBoxColumn();
            rackSlotColumn.DataPropertyName = "PlcsimRackSlot";
            rackSlotColumn.HeaderText = "Rack/Slot";
            rackSlotColumn.Width = 30;

            DataGridViewTextBoxColumn statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.DataPropertyName = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.Width = 111;

            dgvStations.Columns.Add(nameColumn);
            dgvStations.Columns.Add(netipColumn);
            dgvStations.Columns.Add(plcsimipColumn);
            dgvStations.Columns.Add(rackSlotColumn);
            dgvStations.Columns.Add(statusColumn);

            dgvStations.DataSource = m_Conf.Stations;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // parse optional command line arguments
            StartArgs.parseCmdLineArgs(Environment.GetCommandLineArgs());

            if (Tools.IsTcpPortAvailable(102) == false)
            {
                toolStripStatusLabel3.Text = "Port 102 not available!";

                if (String.IsNullOrEmpty(m_IEPGhelperServiceName))
                {
                    MessageBox.Show(
                        "Port 102 is in use!\n"
                        + "Before you can use NetToPLCsim you have to stop the program which uses this port.\n"
                        , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );
                }
                else
                {
                    if (StartArgs.AutoStopService == eAutoStopService.ASK)
                    {
                        DialogResult result = MessageBox.Show(
                             "Port 102 is in use!\n\n"
                            + "Before you can use NetToPLCsim you have to stop the program which uses this port. "
                            + "Seems to be the 'SIMATIC S7DOS Help Service' '" + m_IEPGhelperServiceName + "'\n\n"
                            + "If you have started NetToPLCsim with administrative rights, the service could automatically be stopped.\n\n"
                            + "Try to stop this service?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            m_S7DOSServiceStopped = getPort102back(false);
                        }
                    }
                    else if (StartArgs.AutoStopService == eAutoStopService.YES)
                    {
                        m_S7DOSServiceStopped = getPort102back(true);
                    }

                    if (Tools.IsTcpPortAvailable(102) == true)
                    {
                        toolStripStatusLabel3.Text = "Port 102 OK";
                    }
                }
            }
            else
            {
                toolStripStatusLabel3.Text = "Port 102 OK";
            }

            if (StartArgs.StartIni != String.Empty)
            {
                LoadIniFile(StartArgs.StartIni);
                if (StartArgs.AutoStart == eAutoStart.YES)
                {
                    btnStartServer_Click(null, null);
                }
            }
        }

        private void SetFormText(string file)
        {
            this.Text = "NetToPLCsim::s7o - [" + file + "]";
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            bool oneStationWasStarted = false;

            foreach (StationData station in m_Conf.Stations)
            {
                List<byte[]> tsaps = new List<byte[]>();
                byte tsap2 = (byte)(((station.PlcsimRackNumber << 4) | station.PlcsimSlotNumber));
                tsaps.Add(new byte[] { 0x01, tsap2 });
                tsaps.Add(new byte[] { 0x02, tsap2 });
                tsaps.Add(new byte[] { 0x03, tsap2 });

                IsoToS7online srv = new IsoToS7online(station.TsapCheckEnabled);
                m_servers.Add(srv);
                try
                {
                    srv.start(station.Name, station.NetworkIpAddress, tsaps, station.PlcsimIpAddress, station.PlcsimRackNumber, station.PlcsimSlotNumber);
                }
                catch
                {
                    MessageBox.Show("Error starting server on connection " + station.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    station.Connected = false;
                    station.Status = StationStatus.ERROR.ToString();
                }
                station.Connected = true;
                station.Status = StationStatus.RUNNING.ToString();
                oneStationWasStarted = true;
            }

            if (oneStationWasStarted)
            {
                ConfigFieldsEditable(false);
                monitorToolStripMenuItem.Enabled = true;
            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            foreach (IsoToS7online srv in m_servers)
            {
                srv.stop();
            }
            m_servers.Clear();

            foreach (StationData station in m_Conf.Stations)
            {
                station.Status = StationStatus.STOPPED.ToString();
            }
            ConfigFieldsEditable(true);
            monitorToolStripMenuItem.Enabled = false;
        }

        private void ConfigFieldsEditable(bool enable)
        {
            btnAdd.Enabled = enable;
            btnModify.Enabled = enable;
            btnDelete.Enabled = enable;
            addToolStripMenuItem.Enabled = enable;
            deleteToolStripMenuItem.Enabled = enable;
            modifyToolStripMenuItem.Enabled = enable;
            btnStartServer.Enabled = enable;
            btnStopServer.Enabled = !(enable);
        }

        private void btnNewStation_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            AddNewStation(c.Location);
        }

        private void AddNewStation(Point clickPoint)
        {
            FormStationEdit dlg = new FormStationEdit();
            Point parentPoint = this.Location;

            parentPoint.X += clickPoint.X;
            parentPoint.Y += clickPoint.Y;
            dlg.Location = parentPoint;

            string station_name;
            int station_nr = 1;
            bool found = false;
            do
            {
                // Enter a default Station dummy name
                station_name = String.Format("PLC#{0:000}", station_nr);
                if (m_Conf.IsStationNameUnique(station_name) == false)
                {
                    station_nr++;
                }
                else
                {
                    found = true;
                }
            } while (found == false);

            dlg.Station.Name = station_name;
            dlg.ShowDialog();

            if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (m_Conf.IsStationNameUnique(station_name) == false)
                {
                    MessageBox.Show("Station with name '" + dlg.Station.Name + "' already exists.\r\nPlease use a unique name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                StationData station = new StationData(dlg.Station.Name, dlg.Station.NetworkIpAddress, dlg.Station.PlcsimIpAddress,
                    dlg.Station.PlcsimRackNumber, dlg.Station.PlcsimSlotNumber, dlg.Station.TsapCheckEnabled);

                m_Conf.Stations.Add(station);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point loc = new Point();
            loc.X = this.Width / 4;
            loc.Y = this.Height / 4;
            AddNewStation(loc);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Point loc = new Point();
            loc.X = this.Width / 4;
            loc.Y = this.Height / 4;
            AddNewStation(loc);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedStations();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedStations();
        }

        private void DeleteSelectedStations()
        {
            int selectedRow = GetSelectedDgvRow(dgvStations);
            if (selectedRow >= 0)
            {
                m_Conf.Stations.RemoveAt(selectedRow);
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyStation();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            ModifyStation();
        }

        private void dgvStations_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnModify.Enabled == true)
                ModifyStation();
        }

        // Returns -1 when no row is selected
        private int GetSelectedDgvRow(DataGridView dgv)
        {
            int selectedRowCount = dgv.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 0)
            {
                return -1;
            }
            return dgv.SelectedRows[0].Index;
        }

        private void ModifyStation()
        {
            int selectedRow = GetSelectedDgvRow(dgvStations);

            if (selectedRow < 0)
            {
                return;
            }
            else
            {
                FormStationEdit dlg = new FormStationEdit();
                Point parentPoint = this.Location;
                Point loc = new Point();
                loc.X = this.Width / 4;
                loc.Y = this.Height / 4;

                parentPoint.X += loc.X;
                parentPoint.Y += loc.Y;
                dlg.Location = parentPoint;

                dlg.Station = m_Conf.Stations[selectedRow].ShallowCopy();

                dlg.ShowDialog();
                if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (m_Conf.IsStationNameUniqueExcept(dlg.Station.Name, selectedRow) == false)
                    {
                        MessageBox.Show("Station with name '" + dlg.Station.Name + "' already exists.\r\nPlease use a unique name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_Conf.Stations[selectedRow] = dlg.Station;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Conf.Stations.Count == 0)
                return;

            if (m_ConfigName == String.Empty)
                ChoseSaveFile();
            else
                SaveConfigToFile(m_ConfigName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Conf.Stations.Count == 0)
                return;
            ChoseSaveFile();
        }

        private void ChoseSaveFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "ini files (*.ini)|*.ini|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveConfigToFile(saveFileDialog1.FileName);
                m_ConfigName = saveFileDialog1.FileName;
                SetFormText(m_ConfigName);
            }
        }

        private void SaveConfigToFile(string file)
        {
            IniFile ini = new IniFile(file);
            ini.DeleteFileIfExists();

            int stationNr = 1;
            string section;

            foreach (StationData st in m_Conf.Stations)
            {
                section = "Station_" + stationNr;
                ini.IniWriteValue(section, "Name", st.Name);
                ini.IniWriteValue(section, "NetworkIpAddress", st.NetworkIpAddress.ToString());
                ini.IniWriteValue(section, "PlcsimIpAddress", st.PlcsimIpAddress.ToString());
                ini.IniWriteValue(section, "PlcsimRackNumber", st.PlcsimRackNumber.ToString());
                ini.IniWriteValue(section, "PlcsimSlotNumber", st.PlcsimSlotNumber.ToString());
                ini.IniWriteValue(section, "TsapCheckEnabled", st.TsapCheckEnabled.ToString());
                stationNr++;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "ini files (*.ini)|*.ini|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadIniFile(openFileDialog1.FileName);
            }
        }

        private void LoadIniFile(string file)
        {
            int stationNr = 1;
            string section;
            int rack = 0, slot = 2;
            bool firstOk = false;
            bool readOk = true;
            bool err;

            IniFile ini = new IniFile(file);
            while (readOk)
            {
                err = false;
                section = "Station_" + stationNr;
                StationData station = new StationData();
                station.Name = ini.IniReadValue(section, "Name");

                if (station.Name == String.Empty)
                {
                    readOk = false;
                    break;
                }

                try
                {
                    station.NetworkIpAddress = IPAddress.Parse(ini.IniReadValue(section, "NetworkIpAddress"));
                    station.PlcsimIpAddress = IPAddress.Parse(ini.IniReadValue(section, "PlcsimIpAddress"));
                    if (ini.IniReadValue(section, "TsapCheckEnabled") == "True")
                    {
                        station.TsapCheckEnabled = true;
                    }
                    else
                    {
                        station.TsapCheckEnabled = false;
                    }

                    rack = Convert.ToInt32(ini.IniReadValue(section, "PlcsimRackNumber"));
                    slot = Convert.ToInt32(ini.IniReadValue(section, "PlcsimSlotNumber"));
                }
                catch
                {
                    err = true;
                }

                if (firstOk == false && err == false)
                {
                    m_Conf.Stations.Clear();
                    firstOk = true;
                    m_ConfigName = file;
                    SetFormText(file);
                }

                if (err == false && station.Name != String.Empty && m_Conf.IsStationNameUnique(station.Name) &&
                    rack >= 0 && rack <= 3 && slot >= 0 && slot <= 18)
                {
                    station.PlcsimRackNumber = rack;
                    station.PlcsimSlotNumber = slot;
                    m_Conf.Stations.Add(station);
                }
                else
                {
                    MessageBox.Show("Error in station-configuration ini-file in section '" + section + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    readOk = false;
                }
                stationNr++;
            }
        }

        private void infoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormInfoDialog dlg = new FormInfoDialog();
            dlg.ShowDialog();
        }

        private void stopS7DOSHelperServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(m_IEPGhelperServiceName)))
            {
                if (Tools.StopService(m_IEPGhelperServiceName, 20000, false) == true)
                {
                    m_S7DOSServiceStopped = true;
                }
            }
        }

        private void startS7DOSHelperServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(m_IEPGhelperServiceName)))
            {
                if (Tools.StartService(m_IEPGhelperServiceName, 20000, false, false) == true)
                {
                    m_S7DOSServiceStopped = false;
                }
            }
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string servicename = Tools.GetS7DOSHelperServiceName();
            if (!(String.IsNullOrEmpty(servicename)))
            {
                ServiceController service = new ServiceController(servicename);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    stopS7DOSHelperServiceToolStripMenuItem.Enabled = true;
                    startS7DOSHelperServiceToolStripMenuItem.Enabled = false;
                }
                else
                {
                    stopS7DOSHelperServiceToolStripMenuItem.Enabled = false;
                    startS7DOSHelperServiceToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void monitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonitorServer();
        }

        private void MonitorServer()
        {
            int selectedRow = GetSelectedDgvRow(dgvStations);

            if (selectedRow < 0)
            {
                return;
            }

            foreach (IsoToS7online srv in m_servers)
            {
                if (srv.Name == m_Conf.Stations[selectedRow].Name)
                {
                    StartMonitoringServer(srv);
                    break;
                }
            }
        }

        private void StartMonitoringServer(IsoToS7online srv)
        {
            FormMonitor frmMon = new FormMonitor(srv);
            frmMon.Text = "Monitoring server '" + srv.Name + "' on local interface " + srv.NetworkIpAdress.ToString() + ".";
            frmMon.Show();
        }

        private void getPort102ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getPort102back(false);
        }

        private bool getPort102back(bool autoclose)
        {
            FormGetPort102 frmGetPort = new FormGetPort102();
            frmGetPort.AutoCloseOnSuccess = autoclose;
            frmGetPort.ShowDialog();
            if (Tools.IsTcpPortAvailable(102) == true)
            {
                toolStripStatusLabel3.Text = "Port 102 OK";
            }
            else
            {
                toolStripStatusLabel3.Text = "Port 102 not available!";
            }
            return frmGetPort.Success;
        }

        private void manualdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpFileName = @"NetToPLCsim-Manual-de.chm";
            if (System.IO.File.Exists(helpFileName))
            {
                Help.ShowHelp(this, helpFileName);
            }
            else
            {
                MessageBox.Show("Sorry, couldn't open the german NetToPLCsim helpfile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void manualenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpFileName = @"NetToPLCsim-Manual-en.chm";
            if (System.IO.File.Exists(helpFileName))
            {
                Help.ShowHelp(this, helpFileName);
            }
            else
            {
                MessageBox.Show("Sorry, couldn't open the english NetToPLCsim helpfile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStations_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            modifyButtonsCheck();
        }

        private void dgvStations_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            modifyButtonsCheck();
        }

        private void dgvStations_SelectionChanged(object sender, EventArgs e)
        {
            modifyButtonsCheck();
        }

        private void modifyButtonsCheck()
        {
            int selectetRow = GetSelectedDgvRow(dgvStations);
            if (dgvStations.RowCount == 0 || selectetRow < 0)
            {
                btnModify.Enabled = false;
                btnDelete.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                modifyToolStripMenuItem.Enabled = false;
            }
            else if (selectetRow >= 0)
            {
                bool enable = btnStartServer.Enabled;
                btnModify.Enabled = enable;
                btnDelete.Enabled = enable;
                deleteToolStripMenuItem.Enabled = enable;
                modifyToolStripMenuItem.Enabled = enable;
            }
        }
    }
}
