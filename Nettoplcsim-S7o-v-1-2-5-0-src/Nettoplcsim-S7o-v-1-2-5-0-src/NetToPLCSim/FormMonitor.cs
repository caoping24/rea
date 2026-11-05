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
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using IsoOnTcp;

namespace NetToPLCSim
{
    public partial class FormMonitor : Form
    {
        int m_lineNr;
        bool m_capture_active;
        IsoToS7online m_eventServer;

        const byte PDU_TYPE_REQUEST = 0x01;
        const byte PDU_TYPE_RESPONSE = 0x03;
        const byte PDU_TYPE_USERDATA = 0x07;
        const byte FUNC_READ = 0x04;
        const byte FUNC_WRITE = 0x05;
        const byte FUNC_SETUP_COMM = 0xf0;

        const byte S7COMM_AREA_P = 0x80;
        const byte S7COMM_AREA_INPUTS = 0x81;
        const byte S7COMM_AREA_OUTPUTS = 0x82;
        const byte S7COMM_AREA_FLAGS = 0x83;
        const byte S7COMM_AREA_DB = 0x84;
        const byte S7COMM_AREA_DI = 0x85;
        const byte S7COMM_AREA_LOCAL = 0x86;
        const byte S7COMM_AREA_V = 0x87;
        const byte S7COMM_AREA_COUNTER = 28;
        const byte S7COMM_AREA_TIMER = 29;

        const byte S7COMM_TRANSPORT_SIZE_BIT = 1;
        const byte S7COMM_TRANSPORT_SIZE_BYTE = 2;
        const byte S7COMM_TRANSPORT_SIZE_CHAR =	3;
	        /* types of 2 bytes length */
        const byte S7COMM_TRANSPORT_SIZE_WORD = 4;
        const byte S7COMM_TRANSPORT_SIZE_INT = 5;
	        /* types of 4 bytes length */
        const byte S7COMM_TRANSPORT_SIZE_DWORD = 6;
        const byte S7COMM_TRANSPORT_SIZE_DINT = 7;
        const byte S7COMM_TRANSPORT_SIZE_REAL = 8;
	        /* Timer or counter */
        const byte S7COMM_TRANSPORT_SIZE_TIMER = S7COMM_AREA_TIMER;
        const byte S7COMM_TRANSPORT_SIZE_COUNTER = S7COMM_AREA_COUNTER;

        public FormMonitor(IsoToS7online eventServer)
        {
            InitializeComponent();
            m_eventServer = eventServer;
            m_lineNr = 1;
            setCaptureActiveMode(true);
        }

        private void FormMonitor_Shown(object sender, EventArgs e)
        {
            m_eventServer.monitorDataReceived += new IsoToS7online.dataReceived(this.OnDataReceived);
            string msg = "Started monitoring server '" + m_eventServer.Name + "' on interface " + m_eventServer.NetworkIpAdress + ".";
            tbMonitor.AppendText(DateTime.Now.ToString("HH:mm:ss.fff ") + msg + Environment.NewLine);
        }

        public delegate void OnDataReceived_callback(string sourceIP, byte[] data, string message);

        public void OnDataReceived(string sourceIP, byte[] data, string message)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    OnDataReceived_callback callback = new OnDataReceived_callback(OnDataReceived);
                    this.Invoke(callback, new object[] { sourceIP, data, message });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                if (tbMonitor.IsDisposed) return;
                try
                {
                    if (m_capture_active)
                    {
                        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff ");
                        tbMonitor.AppendText(timestamp + "[" + sourceIP + "] ");
                        if (message != String.Empty)
                        {
                            tbMonitor.AppendText(message + Environment.NewLine);
                        }
                        if (data != null)
                        {
                            DissectProtocol(data, sourceIP);
                        }
                        m_lineNr++;
                    }
                }
                catch (ObjectDisposedException) { } // Zwischen invoke und dem Aufruf kann das Fenster geschlossen worden sein.
            }
        }

        private void FormMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_eventServer.monitorDataReceived -= new IsoToS7online.dataReceived(this.OnDataReceived);
        }

        private void DissectProtocol(byte[] data, string sourceIP)
        {
            int lengthComplete;
            int headerLen = 10;
            int paramLen = 0;
            int dataLen = 0;
            int pos = 0;
            byte pduType = 0;
            byte func = 0;
            bool decoded = false;

            string sPduType;
            string sParamType;

            sPduType = "unknown";
            sParamType = "unknown";
            lengthComplete = data.Length;
            if (lengthComplete > 10)
            {
                // Header
                if (data[0] == 0x32)
                {
                    pduType = data[1];
                    switch (pduType)
                    {
                        case PDU_TYPE_REQUEST:
                            sPduType = "Request";
                            paramLen = GetInt16at(data, 6);
                            dataLen = GetInt16at(data, 8);
                            headerLen = 10;
                            break;
                        case PDU_TYPE_RESPONSE:
                            sPduType = "Response";
                            headerLen = 12;
                            break;
                        case PDU_TYPE_USERDATA:
                            sPduType = "Userdata";
                            paramLen = GetInt16at(data, 6);
                            dataLen = GetInt16at(data, 8);
                            headerLen = 10;
                            break;
                    }

                    if (pduType == PDU_TYPE_REQUEST || pduType == PDU_TYPE_RESPONSE)
                    {
                        func = data[headerLen];
                        switch (func)
                        {
                            case FUNC_SETUP_COMM:
                                sParamType = "Setup communication";
                                break;
                            case FUNC_READ:
                                sParamType = "Read";
                                break;
                            case FUNC_WRITE:
                                sParamType = "Write";
                                break;
                        }
                        tbMonitor.AppendText(sParamType + "-" + sPduType);
                        decoded = true;
                    }
                    else if (pduType == PDU_TYPE_USERDATA)
                    {
                        // Parameter header for request
                        if (data[headerLen] == 0x00 && data[headerLen + 1] == 0x01 && data[headerLen + 2] == 0x12 &&
                            data[headerLen + 3] == 0x04 && data[headerLen + 4] == 0x11)
                        {
                            // SZL Request
                            if (data[headerLen + 5] == 0x44 && data[headerLen + 6] == 0x01)
                            {
                                pos = headerLen + paramLen; // set index to start of data part
                                if (data[pos] == 0xff && data[pos + 1] == 0x09 && data[pos + 2] == 0x00 && data[pos + 3] == 0x04)
                                {
                                    pos += 4;
                                    int szl_id = GetInt16at(data, pos);
                                    pos += 2;
                                    int szl_index = GetInt16at(data, pos);
                                    tbMonitor.AppendText(string.Format("Request read SZL-ID: 0x{0:X4} Index: 0x{1:X4}", szl_id, szl_index));
                                    decoded = true;
                                }
                            }
                            // Request subscribe cyclic data
                            else if (data[headerLen + 5] == 0x42 && data[headerLen + 6] == 0x01)
                            {
                                pos = headerLen + paramLen; // set index to start of data part
                                pos += 4; // skip returncode, transportsize and length
                                int item_count = GetInt16at(data, pos);
                                pos += 2;
                                tbMonitor.AppendText(string.Format("Request subscribe cyclic data. Items: {0:D} Timebase: {1:D} Time: {2:D}", item_count, data[pos], data[pos+1]));
                                pos += 2;
                                string itemInformation = String.Empty;
                                for (int i = 1; i <= item_count; i++)
                                {
                                    itemInformation += Environment.NewLine + "             Item [" + i.ToString().PadLeft(2) + "]: ";
                                    itemInformation += DecodeItem(i, data, ref pos, sourceIP);
                                }
                                tbMonitor.AppendText(itemInformation);
                                decoded = true;
                            }
                            // Request unsubscribe cyclic data
                            else if (data[headerLen + 5] == 0x42 && data[headerLen + 6] == 0x04)
                            {
                                tbMonitor.AppendText(string.Format("Request unsubscribe cyclic data"));
                                decoded = true;
                            }
                            // Request CPU message service
                            else if (data[headerLen + 5] == 0x44 && data[headerLen + 6] == 0x02)
                            {
                                tbMonitor.AppendText(string.Format("Request CPU message service (subscribe/unsubscribe for events)"));
                                decoded = true;
                            }
                        }
                    }

                    if (pduType == PDU_TYPE_REQUEST && (func == FUNC_READ || func == FUNC_WRITE))
                    {
                        pos = headerLen + 1;
                        int item_count = data[pos];
                        pos += 1;
                        tbMonitor.AppendText(" of " + item_count);
                        if (item_count > 1)
                            tbMonitor.AppendText(" items.");
                        else
                            tbMonitor.AppendText(" item.");

                        string itemInformation = String.Empty;
                        for (int i = 1; i <= item_count; i++)
                        {
                            itemInformation += Environment.NewLine + "             Item [" + i.ToString().PadLeft(2) + "]: ";
                            itemInformation += DecodeItem(i, data, ref pos, sourceIP);
                        }
                        tbMonitor.AppendText(itemInformation);
                    }
                }
            }
            if (decoded == false)
            {
                tbMonitor.AppendText("Undecoded telegram data" + Environment.NewLine);
            }
            else
            {
                tbMonitor.AppendText(Environment.NewLine);
            }
        }

        private string DecodeItem(int itemNr, byte[] data, ref int pos, string sourceIP)
        {
            string sItemName = "Unknown";
            byte var_spec_type;
            byte var_spec_len;
            byte var_spec_syntax_id;

            int t_size;
            int len;
            int db;
            int area;
            Int32 address;
            int bytepos;
            int bitpos;

            var_spec_type = data[pos];
            pos += 1;
            var_spec_len = data[pos];
            pos += 1;
            var_spec_syntax_id = data[pos];
            pos += 1;

            t_size = data[pos];
            pos += 1;
            len = GetInt16at(data, pos);
            pos += 2;
            db = GetInt16at(data, pos);
            pos += 2;
            area = data[pos];
            pos += 1;

            if (var_spec_syntax_id == 0x10)
            {
                // 3 bytes address
                byte[] aa = new byte[4];
                aa[0] = 0;
                aa[1] = data[pos];
                aa[2] = data[pos + 1];
                aa[3] = data[pos + 2];
                pos += 3;
                if (BitConverter.IsLittleEndian)
                    address = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(aa, 0));
                else
                    address = BitConverter.ToInt32(aa, 0);

                bytepos = address / 8;
                bitpos = address % 8;

                switch (area)
                {
                    case (S7COMM_AREA_P):
                        sItemName = "P";
                        break;
                    case (S7COMM_AREA_INPUTS):
                        sItemName = "I";
                        break;
                    case (S7COMM_AREA_OUTPUTS):
                        sItemName = "Q";
                        break;
                    case (S7COMM_AREA_FLAGS):
                        sItemName = "M";
                        break;
                    case (S7COMM_AREA_DB):
                        sItemName = "DB" + db + ".DBX";
                        break;
                    case (S7COMM_AREA_DI):
                        sItemName = "DI" + db + ".DIX";
                        break;
                    case (S7COMM_AREA_LOCAL):
                        sItemName = "L";
                        break;
                    case (S7COMM_AREA_COUNTER):
                        sItemName = "C";
                        break;
                    case (S7COMM_AREA_TIMER):
                        sItemName = "T";
                        break;
                    default:
                        sItemName = "unknown area";
                        break;
                }
                if (area == S7COMM_AREA_TIMER || area == S7COMM_AREA_COUNTER)
                {
                    sItemName += " " + address;
                }
                else
                {
                    sItemName += " " + bytepos + "." + bitpos + " ";
                    string t_size_name = "Unknown transport size";
                    switch (t_size)
                    {
                        case S7COMM_TRANSPORT_SIZE_BIT:
                            t_size_name = "BIT";
                            break;
                        case S7COMM_TRANSPORT_SIZE_BYTE:
                            t_size_name = "BYTE";
                            break;
                        case S7COMM_TRANSPORT_SIZE_CHAR:
                            t_size_name = "CHAR";
                            break;
                        case S7COMM_TRANSPORT_SIZE_WORD:
                            t_size_name = "WORD";
                            break;
                        case S7COMM_TRANSPORT_SIZE_INT:
                            t_size_name = "INT";
                            break;
                        case S7COMM_TRANSPORT_SIZE_DWORD:
                            t_size_name = "DWORD";
                            break;
                        case S7COMM_TRANSPORT_SIZE_DINT:
                            t_size_name = "DINT";
                            break;
                        case S7COMM_TRANSPORT_SIZE_REAL:
                            t_size_name = "REAL";
                            break;
                        case S7COMM_TRANSPORT_SIZE_TIMER:
                            t_size_name = "TIMER";
                            break;
                        case S7COMM_TRANSPORT_SIZE_COUNTER:
                            t_size_name = "COUNTER";
                            break;
                    }
                    sItemName += t_size_name;
                    sItemName += " " + len;
                }
            }
            else
            {
                sItemName = "Address is not a S7 ANY-Pointer";
                pos += (var_spec_len - 2);  // 1 Byte header and 1 Byte len
            }
            return sItemName;
        }

        private Int16 GetInt16at(byte[] value, int startindex)
        {
            if (BitConverter.IsLittleEndian)
                return IPAddress.HostToNetworkOrder(BitConverter.ToInt16(value, startindex));
            return BitConverter.ToInt16(value, startindex);
        }

        private void tbMonitor_TextChanged(object sender, EventArgs e)
        {
            // if more than 5000 lines, remove the last 1000
            int maxLines = 5000;
            int linesToRemove = 1000;
            if (tbMonitor.Lines.Length > maxLines)
            {
                int selectionstart = tbMonitor.SelectionStart;
                int firstlinelength = tbMonitor.Lines[0].Length;
                int lentoremove = 0;
                for (int i = 0; i < linesToRemove; i++)
                {
                    lentoremove += tbMonitor.Lines[i].Length + 2;   // 2= \r\n
                }
                tbMonitor.Text = tbMonitor.Text.Remove(0, lentoremove);
                tbMonitor.SelectionStart = selectionstart - lentoremove;
                tbMonitor.ScrollToCaret();
            }
        }

        private void toolStripStatusLabelCapturing_Click(object sender, EventArgs e)
        {
            setCaptureActiveMode(!m_capture_active);
        }

        private void setCaptureActiveMode(bool value)
        {
            m_capture_active = value;
            if (m_capture_active)
            {
                toolStripStatusLabelCapturing.BackColor = Color.LimeGreen;
                toolStripStatusLabelCapturing.Text = ">>>Capturing<<<";
                toolStripStatusLabelCapturing.ToolTipText = "Click to pause";
            }
            else
            {
                toolStripStatusLabelCapturing.BackColor = Color.Yellow;
                toolStripStatusLabelCapturing.Text = "---Paused---";
                toolStripStatusLabelCapturing.ToolTipText = "Click to resume";
            }
        }
    }
}
