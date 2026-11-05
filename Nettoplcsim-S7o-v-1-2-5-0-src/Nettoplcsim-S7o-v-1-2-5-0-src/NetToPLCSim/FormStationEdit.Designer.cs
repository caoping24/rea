namespace NetToPLCSim
{
    partial class FormStationEdit
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEnableTsapCheck = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSlotNr = new System.Windows.Forms.ComboBox();
            this.cbRackNr = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChosePlcsimIp = new System.Windows.Forms.Button();
            this.btnChoseNetworkIp = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLocalIpAddress = new System.Windows.Forms.TextBox();
            this.tbPlcsimIpAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(274, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(193, 236);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEnableTsapCheck);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbSlotNr);
            this.groupBox1.Controls.Add(this.cbRackNr);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnChosePlcsimIp);
            this.groupBox1.Controls.Add(this.btnChoseNetworkIp);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbLocalIpAddress);
            this.groupBox1.Controls.Add(this.tbPlcsimIpAddress);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 218);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Station Data";
            // 
            // cbEnableTsapCheck
            // 
            this.cbEnableTsapCheck.AutoSize = true;
            this.cbEnableTsapCheck.Location = new System.Drawing.Point(140, 132);
            this.cbEnableTsapCheck.Name = "cbEnableTsapCheck";
            this.cbEnableTsapCheck.Size = new System.Drawing.Size(123, 17);
            this.cbEnableTsapCheck.TabIndex = 51;
            this.cbEnableTsapCheck.Text = "Enable TSAP check";
            this.cbEnableTsapCheck.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 52);
            this.label6.TabIndex = 49;
            this.label6.Text = "Position of CPU\r\n- S7-300: Always 0/2\r\n- S7-400: 0/2 or from HWKonfig\r\n- S7-1200/" +
    "1500: Always 0/1\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = " / ";
            // 
            // cbSlotNr
            // 
            this.cbSlotNr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSlotNr.FormattingEnabled = true;
            this.cbSlotNr.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18"});
            this.cbSlotNr.Location = new System.Drawing.Point(212, 105);
            this.cbSlotNr.MaxDropDownItems = 3;
            this.cbSlotNr.Name = "cbSlotNr";
            this.cbSlotNr.Size = new System.Drawing.Size(44, 21);
            this.cbSlotNr.TabIndex = 7;
            // 
            // cbRackNr
            // 
            this.cbRackNr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRackNr.FormattingEnabled = true;
            this.cbRackNr.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cbRackNr.Location = new System.Drawing.Point(140, 105);
            this.cbRackNr.MaxDropDownItems = 3;
            this.cbRackNr.Name = "cbRackNr";
            this.cbRackNr.Size = new System.Drawing.Size(44, 21);
            this.cbRackNr.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "Plcsim Rack / Slot";
            // 
            // btnChosePlcsimIp
            // 
            this.btnChosePlcsimIp.Location = new System.Drawing.Point(265, 74);
            this.btnChosePlcsimIp.Name = "btnChosePlcsimIp";
            this.btnChosePlcsimIp.Size = new System.Drawing.Size(30, 23);
            this.btnChosePlcsimIp.TabIndex = 5;
            this.btnChosePlcsimIp.Text = "...";
            this.btnChosePlcsimIp.UseVisualStyleBackColor = true;
            this.btnChosePlcsimIp.Click += new System.EventHandler(this.btnChosePlcsimIp_Click);
            // 
            // btnChoseNetworkIp
            // 
            this.btnChoseNetworkIp.Location = new System.Drawing.Point(265, 46);
            this.btnChoseNetworkIp.Name = "btnChoseNetworkIp";
            this.btnChoseNetworkIp.Size = new System.Drawing.Size(30, 23);
            this.btnChoseNetworkIp.TabIndex = 3;
            this.btnChoseNetworkIp.Text = "...";
            this.btnChoseNetworkIp.UseVisualStyleBackColor = true;
            this.btnChoseNetworkIp.Click += new System.EventHandler(this.btnChoseNetworkIp_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(140, 19);
            this.tbName.MaxLength = 20;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(119, 20);
            this.tbName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Name";
            // 
            // tbLocalIpAddress
            // 
            this.tbLocalIpAddress.Location = new System.Drawing.Point(140, 48);
            this.tbLocalIpAddress.MaxLength = 20;
            this.tbLocalIpAddress.Name = "tbLocalIpAddress";
            this.tbLocalIpAddress.Size = new System.Drawing.Size(119, 20);
            this.tbLocalIpAddress.TabIndex = 2;
            // 
            // tbPlcsimIpAddress
            // 
            this.tbPlcsimIpAddress.Location = new System.Drawing.Point(140, 77);
            this.tbPlcsimIpAddress.MaxLength = 20;
            this.tbPlcsimIpAddress.Name = "tbPlcsimIpAddress";
            this.tbPlcsimIpAddress.Size = new System.Drawing.Size(119, 20);
            this.tbPlcsimIpAddress.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Plcsim IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Network IP Address";
            // 
            // FormStationEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 270);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStationEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Station";
            this.Load += new System.EventHandler(this.FormStationEdit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnChosePlcsimIp;
        private System.Windows.Forms.Button btnChoseNetworkIp;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLocalIpAddress;
        private System.Windows.Forms.TextBox tbPlcsimIpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSlotNr;
        private System.Windows.Forms.ComboBox cbRackNr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbEnableTsapCheck;
    }
}