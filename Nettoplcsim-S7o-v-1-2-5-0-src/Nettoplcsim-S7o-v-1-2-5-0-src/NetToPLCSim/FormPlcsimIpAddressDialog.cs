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
using System.Windows.Forms;
using PlcsimS7online;
using System.Net;

namespace NetToPLCSim
{
    public partial class FormPlcsimIpAddressDialog : Form
    {
        public string ChosenIPaddress { get; private set; }

        public FormPlcsimIpAddressDialog()
        {
            InitializeComponent();
        }

        private void FormPlcsimIpAddressDialog_Load(object sender, EventArgs e)
        {
            listBoxPlcsimIpAddresses.AutoSize = true;
            listBoxPlcsimIpAddresses.Items.Clear();
            try
            {
                PlcS7onlineMsgPumpS7 s7o = new PlcS7onlineMsgPumpS7(IPAddress.None, 0, 0);
                List<string> partners;
                int n = 0;

                partners = s7o.ListReachablePartners();
                foreach (string p in partners)
                {
                    n++;
                    listBoxPlcsimIpAddresses.Items.Add(p);
                }

                if (n == 0)
                {
                    MessageBox.Show("No TCP/IP reachable PLC detected." + Environment.NewLine +
                        "Please check if Plcsim is running.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    ChosenIPaddress = "";
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Error when connecting S7online interface!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                ChosenIPaddress = "";
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OkClicked();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChosenIPaddress = "";
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void listBoxPlcsimIpAddresses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OkClicked();
        }

        private void OkClicked()
        {
            if (listBoxPlcsimIpAddresses.SelectedItem != null)
            {
                string sel = listBoxPlcsimIpAddresses.SelectedItem.ToString();
                ChosenIPaddress = sel.Substring(0, sel.IndexOf(" "));
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void listBoxPlcsimIpAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlcsimIpAddresses.SelectedItem != null)
            {
                btnOK.Enabled = true;
            }
        }
    }
}
