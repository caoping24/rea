namespace NetToPLCSim
{
    partial class FormGetPort102
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetPort102));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatusmessage = new System.Windows.Forms.Label();
            this.lblResultmessage = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 60);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(487, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // lblStatusmessage
            // 
            this.lblStatusmessage.AutoSize = true;
            this.lblStatusmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusmessage.Location = new System.Drawing.Point(9, 9);
            this.lblStatusmessage.Name = "lblStatusmessage";
            this.lblStatusmessage.Size = new System.Drawing.Size(92, 13);
            this.lblStatusmessage.TabIndex = 3;
            this.lblStatusmessage.Text = "Statusmessage";
            // 
            // lblResultmessage
            // 
            this.lblResultmessage.AutoSize = true;
            this.lblResultmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultmessage.Location = new System.Drawing.Point(9, 31);
            this.lblResultmessage.Name = "lblResultmessage";
            this.lblResultmessage.Size = new System.Drawing.Size(43, 13);
            this.lblResultmessage.TabIndex = 4;
            this.lblResultmessage.Text = "Result";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(424, 89);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormGetPort102
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 124);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblResultmessage);
            this.Controls.Add(this.lblStatusmessage);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGetPort102";
            this.ShowInTaskbar = false;
            this.Text = "Get Port 102";
            this.Shown += new System.EventHandler(this.FormGetPort102_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatusmessage;
        private System.Windows.Forms.Label lblResultmessage;
        private System.Windows.Forms.Button btnOk;
    }
}