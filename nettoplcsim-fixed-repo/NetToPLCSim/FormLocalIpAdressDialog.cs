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
using System.Net.NetworkInformation;

namespace NetToPLCSim
{
    public partial class FormLocalIpAdressDialog : Form
    {
        public string ChosenIPaddress { get; private set; }

        public FormLocalIpAdressDialog()
        {
            InitializeComponent();
        }

        private void FormLocalIpAdressDialog_Load(object sender, EventArgs e)
        {
            fillAddressListBox();
        }
        
        private void fillAddressListBox()
        {
            listBoxLocalIpAddresses.AutoSize = true;
            listBoxLocalIpAddresses.Items.Clear();

            NetworkInterface[] intf = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface device in intf)
            {
                if (cbShowAllInterfaceTypes.Checked || device.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet) || device.NetworkInterfaceType.Equals(NetworkInterfaceType.Wireless80211))
                {
                    foreach (UnicastIPAddressInformation addressInformation in device.GetIPProperties().UnicastAddresses)
                    {
                        if (addressInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            listBoxLocalIpAddresses.Items.Add(String.Format("{0} - [{1}]", addressInformation.Address.ToString(), device.Description));
                        }
                    }
                }
            }
        }       

        private void btnOK_Click(object sender, EventArgs e)
        {
            OkClicked();
        }

        private void listBoxLocalIpAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLocalIpAddresses.SelectedItem != null)
                btnOK.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChosenIPaddress = "";
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void listBoxLocalIpAddresses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OkClicked();
        }

        private void OkClicked()
        {
            if (listBoxLocalIpAddresses.SelectedItem != null)
            {
                string sel = listBoxLocalIpAddresses.SelectedItem.ToString();
                ChosenIPaddress = sel.Substring(0, sel.IndexOf(" "));
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void cbShowAllInterfaceTypes_CheckedChanged(object sender, EventArgs e)
        {
            fillAddressListBox();
        }
    }
}
