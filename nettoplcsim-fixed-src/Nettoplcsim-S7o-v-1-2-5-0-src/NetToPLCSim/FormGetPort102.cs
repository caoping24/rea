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
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using System.Media;

namespace NetToPLCSim
{
    public partial class FormGetPort102 : Form
    {
        public bool Success { get; private set; }
        public bool AutoCloseOnSuccess { get; set; }

        private string m_Servicename;
        private TcpListener m_Listener;

        public FormGetPort102()
        {
            InitializeComponent();
            m_Servicename = Tools.GetS7DOSHelperServiceName();
        }

        private void FormGetPort102_Shown(object sender, EventArgs e)
        {
            StartGetPortBack();
        }

        private bool Step1()
        {
            return Tools.StopService(m_Servicename, 20000, true);
        }

        private bool Step2()
        {
            try
            {
                m_Listener = new TcpListener(IPAddress.Any, 102);
                m_Listener.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Step3()
        {
            return Tools.StartService(m_Servicename, 20000, true, false);
        }

        private bool Step4()
        {
            try
            {
                m_Listener.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool Step5()
        {
            return Tools.IsTcpPortAvailable(102);
        }

        private void StartGetPortBack()
        {
            progressBar1.Maximum = 10;
            progressBar1.Step = 1;
            this.Success = false;
            lblResultmessage.Text = "";
            lblStatusmessage.Text = "Starting to get Port 102 back...";
            lblStatusmessage.Refresh();
            progressBar1.PerformStep();
            Thread.Sleep(50);
            this.Refresh();

            lblStatusmessage.Text = "Step 1) Stopping service '" + m_Servicename + "'...";
            lblStatusmessage.Refresh();
            progressBar1.PerformStep();
            Thread.Sleep(50);
            if (Step1())
            {
                lblStatusmessage.Text = "Step 2) Starting our own Server on TCP Port 102...";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                if (Step2() == false)
                {
                    SystemSounds.Exclamation.Play();
                    lblStatusmessage.ForeColor = Color.Red;
                    lblStatusmessage.Text += "FAILED!";
                    lblStatusmessage.Refresh();
                    progressBar1.Value = 0;
                    return;
                }
                lblStatusmessage.Text += "OK.";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                lblStatusmessage.Text = "Step 3) Starting service '" + m_Servicename + "'...";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                if (Step3())
                {
                    lblStatusmessage.Text += "OK.";
                    lblStatusmessage.Refresh();
                    progressBar1.PerformStep();
                    Thread.Sleep(50);
                }
                else
                {
                    Step4(); // stops the previous started TCP server
                    SystemSounds.Exclamation.Play();
                    lblStatusmessage.ForeColor = Color.Red;
                    lblStatusmessage.Text += "FAILED to start the service!";
                    lblStatusmessage.Refresh();
                    lblResultmessage.Text = "Remember that you need to start NetToPLCsim" + System.Environment.NewLine + "with administrative rights to stop the service!";
                    progressBar1.Value = 0;
                    return;
                }
                lblStatusmessage.Text += "OK.";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                lblStatusmessage.Text = "Step 4) Stopping our own Server...";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                if (Step4() == false)
                {
                    SystemSounds.Exclamation.Play();
                    lblStatusmessage.ForeColor = Color.Red;
                    lblStatusmessage.Text += "FAILED!";
                    lblStatusmessage.Refresh();
                    progressBar1.Value = 0;
                    return;
                }
                lblStatusmessage.Text += "OK.";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                lblStatusmessage.Text = "Step 5) Checking TCP Port 102...";
                lblStatusmessage.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(50);
                if (Step5())
                {
                    lblStatusmessage.Text += "OK. Port 102 is available.";
                    lblResultmessage.ForeColor = Color.Green;
                    lblResultmessage.Text = "Success! You are ready to use NetToPLCsim :)";
                    lblResultmessage.Refresh();
                    this.Success = true;
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    lblStatusmessage.ForeColor = Color.Red;
                    lblStatusmessage.Text += "FAILED. Port 102 is still not available :(";
                    lblStatusmessage.Refresh();
                    progressBar1.Value = 0;
                }                
            }
            else
            {
                SystemSounds.Exclamation.Play();
                lblStatusmessage.ForeColor = Color.Red;
                lblStatusmessage.Text += "FAILED to stop the service!";
                lblResultmessage.Text = "Remember that you need to start NetToPLCsim" + System.Environment.NewLine + "with administrative rights to stop the service!";
                lblStatusmessage.Refresh();
                progressBar1.Value = 0;
            }
            if (this.Success && AutoCloseOnSuccess)
            {
                this.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
