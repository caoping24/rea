namespace NetToPLCSim
{
    partial class FormLocalIpAdressDialog
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
            this.listBoxLocalIpAddresses = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbShowAllInterfaceTypes = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBoxLocalIpAddresses
            // 
            this.listBoxLocalIpAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLocalIpAddresses.FormattingEnabled = true;
            this.listBoxLocalIpAddresses.Location = new System.Drawing.Point(12, 26);
            this.listBoxLocalIpAddresses.Name = "listBoxLocalIpAddresses";
            this.listBoxLocalIpAddresses.Size = new System.Drawing.Size(307, 199);
            this.listBoxLocalIpAddresses.TabIndex = 1;
            this.listBoxLocalIpAddresses.SelectedIndexChanged += new System.EventHandler(this.listBoxLocalIpAddresses_SelectedIndexChanged);
            this.listBoxLocalIpAddresses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxLocalIpAddresses_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available network IP addresses";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(163, 231);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(244, 231);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbShowAllInterfaceTypes
            // 
            this.cbShowAllInterfaceTypes.AutoSize = true;
            this.cbShowAllInterfaceTypes.Location = new System.Drawing.Point(12, 235);
            this.cbShowAllInterfaceTypes.Name = "cbShowAllInterfaceTypes";
            this.cbShowAllInterfaceTypes.Size = new System.Drawing.Size(138, 17);
            this.cbShowAllInterfaceTypes.TabIndex = 5;
            this.cbShowAllInterfaceTypes.Text = "Show all interface types";
            this.cbShowAllInterfaceTypes.UseVisualStyleBackColor = true;
            this.cbShowAllInterfaceTypes.CheckedChanged += new System.EventHandler(this.cbShowAllInterfaceTypes_CheckedChanged);
            // 
            // FormLocalIpAdressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(331, 261);
            this.Controls.Add(this.cbShowAllInterfaceTypes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxLocalIpAddresses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLocalIpAdressDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Network IP Addresses";
            this.Load += new System.EventHandler(this.FormLocalIpAdressDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLocalIpAddresses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbShowAllInterfaceTypes;
    }
}