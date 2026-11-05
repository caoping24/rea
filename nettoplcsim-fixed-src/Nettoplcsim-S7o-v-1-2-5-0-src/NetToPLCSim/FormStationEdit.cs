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
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;

namespace NetToPLCSim
{
    public partial class FormStationEdit : Form
    {
        public StationData Station = new StationData();

        public FormStationEdit()
        {
            InitializeComponent();
        }

        private void FormStationEdit_Load(object sender, EventArgs e)
        {
            setToolTipText();
            tbName.Text = Station.Name;
            tbLocalIpAddress.Text = Station.NetworkIpAddress.ToString();
            tbPlcsimIpAddress.Text = Station.PlcsimIpAddress.ToString();
            cbRackNr.SelectedIndex = Station.PlcsimRackNumber;
            cbSlotNr.SelectedIndex = Station.PlcsimSlotNumber;
            cbEnableTsapCheck.Checked = Station.TsapCheckEnabled;
        }

        private void setToolTipText()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnChoseNetworkIp, "Browse available network interface IP addresses");
            toolTip.SetToolTip(btnChosePlcsimIp, "Browse available TCP/IP Plcsim Simulations");
        }

        private void btnChoseNetworkIp_Click(object sender, EventArgs e)
        {
            FormLocalIpAdressDialog dlg = new FormLocalIpAdressDialog();
            Button btn = (Button)sender;
            Point parentPoint = this.Location;

            parentPoint.X += btn.Location.X;
            parentPoint.Y += btn.Location.Y;
            dlg.Location = parentPoint;

            dlg.ShowDialog();
            if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                tbLocalIpAddress.Text = dlg.ChosenIPaddress;
            }
        }

        private void btnChosePlcsimIp_Click(object sender, EventArgs e)
        {
            FormPlcsimIpAddressDialog dlg = new FormPlcsimIpAddressDialog();
            Button btn = (Button)sender;
            Point parentPoint = this.Location;

            parentPoint.X += btn.Location.X;
            parentPoint.Y += btn.Location.Y;
            dlg.Location = parentPoint;

            dlg.ShowDialog();
            if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                tbPlcsimIpAddress.Text = dlg.ChosenIPaddress;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (checkTextBoxEntries() == true)
            {
                Station.Name = tbName.Text;
                Station.NetworkIpAddress = IPAddress.Parse(tbLocalIpAddress.Text);
                Station.PlcsimIpAddress = IPAddress.Parse(tbPlcsimIpAddress.Text);
                Station.PlcsimRackNumber = cbRackNr.SelectedIndex;
                Station.PlcsimSlotNumber = cbSlotNr.SelectedIndex;
                Station.TsapCheckEnabled = cbEnableTsapCheck.Checked;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private bool checkTextBoxEntries()
        {
            string ip;
            if (tbName.Text == String.Empty)
            {
                MessageBox.Show("Enter a unique name for this station.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                tbName.Focus();
                return false;
            }
            ip = tbLocalIpAddress.Text;
            if (!(IsValidIP(ip)))
            {
                MessageBox.Show("The entered IP-address is not valid!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                tbLocalIpAddress.Focus();
                return false;
            }
            ip = tbPlcsimIpAddress.Text;
            if (!(IsValidIP(ip)))
            {
                MessageBox.Show("The entered IP-address is not valid!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                tbPlcsimIpAddress.Focus();
                return false;
            }
            return true;
        }

        public bool IsValidIP(string addr)
        {
            string pattern = @"^(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d)$";
            Regex check = new Regex(pattern);
            bool valid = false;
            if (addr == String.Empty)
            {
                valid = false;
            }
            else
            {
                valid = check.IsMatch(addr, 0);
            }
            return valid;
        }
    }
}
