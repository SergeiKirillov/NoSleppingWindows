namespace ActivePC
{
    partial class frmSetting
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
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.lblAutoStart = new System.Windows.Forms.Label();
            this.chk5Brigada = new System.Windows.Forms.CheckBox();
            this.chkSmena = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(13, 13);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(325, 17);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "Автоматичский запускпрограммы после запуска Windows";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            // 
            // lblAutoStart
            // 
            this.lblAutoStart.AutoSize = true;
            this.lblAutoStart.Location = new System.Drawing.Point(12, 37);
            this.lblAutoStart.Name = "lblAutoStart";
            this.lblAutoStart.Size = new System.Drawing.Size(266, 13);
            this.lblAutoStart.TabIndex = 2;
            this.lblAutoStart.Tag = "Путь где находиться программа для автозапуска. ";
            this.lblAutoStart.Text = "Путь где находиться программа для автозапуска. ";
            // 
            // chk5Brigada
            // 
            this.chk5Brigada.AutoSize = true;
            this.chk5Brigada.Location = new System.Drawing.Point(15, 64);
            this.chk5Brigada.Name = "chk5Brigada";
            this.chk5Brigada.Size = new System.Drawing.Size(224, 17);
            this.chk5Brigada.TabIndex = 3;
            this.chk5Brigada.Text = "Автоматическое отключение после 17.";
            this.chk5Brigada.UseVisualStyleBackColor = true;
            this.chk5Brigada.CheckedChanged += new System.EventHandler(this.chk5Brigada_CheckedChanged);
            // 
            // chkSmena
            // 
            this.chkSmena.AutoSize = true;
            this.chkSmena.Location = new System.Drawing.Point(15, 88);
            this.chkSmena.Name = "chkSmena";
            this.chkSmena.Size = new System.Drawing.Size(359, 17);
            this.chkSmena.TabIndex = 4;
            this.chkSmena.Text = "Автоматическое отключение после окончания смены(7-00, 19-00)";
            this.chkSmena.UseVisualStyleBackColor = true;
            this.chkSmena.CheckedChanged += new System.EventHandler(this.chkSmena_CheckedChanged);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 263);
            this.Controls.Add(this.chkSmena);
            this.Controls.Add(this.chk5Brigada);
            this.Controls.Add(this.lblAutoStart);
            this.Controls.Add(this.chkAutoStart);
            this.Name = "frmSetting";
            this.ShowIcon = false;
            this.Text = "Настройка программы";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.Label lblAutoStart;
        private System.Windows.Forms.CheckBox chk5Brigada;
        private System.Windows.Forms.CheckBox chkSmena;
    }
}