/*  This file is part of SuperVivi Tranfert Tool.

    Copyright © 2010 Fabien Poussin. Mobyfab@netyxia.net

    SuperVivi Tranfert Tool is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    SuperVivi Tranfert Tool is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SuperVivi Tranfert Tool.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace SuperVivi_Transfer_Tool
{
    partial class SuperVivi_Transfer_Tool
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperVivi_Transfer_Tool));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.l_usbFound = new System.Windows.Forms.Label();
            this.l_usbStatus = new System.Windows.Forms.Label();
            this.b_upload = new System.Windows.Forms.Button();
            this.b_download = new System.Windows.Forms.Button();
            this.t_log = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Worker_download = new System.ComponentModel.BackgroundWorker();
            this.l_progress = new System.Windows.Forms.Label();
            this.Worker_upload = new System.ComponentModel.BackgroundWorker();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.g_serial = new System.Windows.Forms.GroupBox();
            this.l_serialPort = new System.Windows.Forms.Label();
            this.c_serialPort = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.num_dlAddr = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.b_options_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.b_options_Reset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.g_serial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_dlAddr)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.l_usbFound);
            this.groupBox1.Controls.Add(this.l_usbStatus);
            this.groupBox1.Controls.Add(this.b_upload);
            this.groupBox1.Controls.Add(this.b_download);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "USB";
            // 
            // l_usbFound
            // 
            this.l_usbFound.AutoSize = true;
            this.l_usbFound.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_usbFound.Location = new System.Drawing.Point(61, 21);
            this.l_usbFound.Name = "l_usbFound";
            this.l_usbFound.Size = new System.Drawing.Size(76, 13);
            this.l_usbFound.TabIndex = 4;
            this.l_usbFound.Text = "Searching...";
            // 
            // l_usbStatus
            // 
            this.l_usbStatus.AutoSize = true;
            this.l_usbStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_usbStatus.Location = new System.Drawing.Point(7, 21);
            this.l_usbStatus.Name = "l_usbStatus";
            this.l_usbStatus.Size = new System.Drawing.Size(48, 13);
            this.l_usbStatus.TabIndex = 3;
            this.l_usbStatus.Text = "Status:";
            // 
            // b_upload
            // 
            this.b_upload.Location = new System.Drawing.Point(238, 11);
            this.b_upload.Name = "b_upload";
            this.b_upload.Size = new System.Drawing.Size(100, 33);
            this.b_upload.TabIndex = 1;
            this.b_upload.Tag = "";
            this.b_upload.Text = "Send...";
            this.toolTip1.SetToolTip(this.b_upload, "Host PC -> Supervivi.");
            this.b_upload.UseVisualStyleBackColor = true;
            this.b_upload.Click += new System.EventHandler(this.b_upload_Click);
            // 
            // b_download
            // 
            this.b_download.Location = new System.Drawing.Point(344, 11);
            this.b_download.Name = "b_download";
            this.b_download.Size = new System.Drawing.Size(100, 33);
            this.b_download.TabIndex = 2;
            this.b_download.Text = "Receive...";
            this.toolTip1.SetToolTip(this.b_download, "Supervivi -> Host PC.");
            this.b_download.UseVisualStyleBackColor = true;
            this.b_download.Click += new System.EventHandler(this.b_download_Click);
            // 
            // t_log
            // 
            this.t_log.AcceptsReturn = true;
            this.t_log.AcceptsTab = true;
            this.t_log.BackColor = System.Drawing.Color.Black;
            this.t_log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.t_log.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_log.ForeColor = System.Drawing.Color.White;
            this.t_log.Location = new System.Drawing.Point(13, 120);
            this.t_log.Multiline = true;
            this.t_log.Name = "t_log";
            this.t_log.ReadOnly = true;
            this.t_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.t_log.Size = new System.Drawing.Size(659, 351);
            this.t_log.TabIndex = 1;
            this.t_log.Text = "Contact: mobyfab@netyxia.net\r\nhttp://mini2440.netyxia.net\r\nThis program uses LibU" +
                "sbDotNet.\r\n";
            this.toolTip1.SetToolTip(this.t_log, "Select this text box to input commands through the serial console.");
            this.t_log.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t_log_KeyPress);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 477);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(632, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Title = "Upload File to Device";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Title = "Save File From Device";
            // 
            // Worker_download
            // 
            this.Worker_download.WorkerReportsProgress = true;
            this.Worker_download.WorkerSupportsCancellation = true;
            this.Worker_download.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_download_DoWork);
            this.Worker_download.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_download_ProgressChanged);
            this.Worker_download.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_download_RunWorkerCompleted);
            // 
            // l_progress
            // 
            this.l_progress.AutoSize = true;
            this.l_progress.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_progress.Location = new System.Drawing.Point(651, 482);
            this.l_progress.Name = "l_progress";
            this.l_progress.Size = new System.Drawing.Size(26, 13);
            this.l_progress.TabIndex = 0;
            this.l_progress.Text = "0%";
            // 
            // Worker_upload
            // 
            this.Worker_upload.WorkerReportsProgress = true;
            this.Worker_upload.WorkerSupportsCancellation = true;
            this.Worker_upload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_upload_DoWork);
            this.Worker_upload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_upload_ProgressChanged);
            this.Worker_upload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_upload_RunWorkerCompleted);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // g_serial
            // 
            this.g_serial.Controls.Add(this.l_serialPort);
            this.g_serial.Controls.Add(this.c_serialPort);
            this.g_serial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_serial.Location = new System.Drawing.Point(469, 13);
            this.g_serial.Name = "g_serial";
            this.g_serial.Size = new System.Drawing.Size(203, 50);
            this.g_serial.TabIndex = 4;
            this.g_serial.TabStop = false;
            this.g_serial.Text = "Serial";
            // 
            // l_serialPort
            // 
            this.l_serialPort.AutoSize = true;
            this.l_serialPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_serialPort.Location = new System.Drawing.Point(6, 21);
            this.l_serialPort.Name = "l_serialPort";
            this.l_serialPort.Size = new System.Drawing.Size(35, 13);
            this.l_serialPort.TabIndex = 1;
            this.l_serialPort.Text = "Port:";
            // 
            // c_serialPort
            // 
            this.c_serialPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c_serialPort.FormattingEnabled = true;
            this.c_serialPort.Location = new System.Drawing.Point(47, 18);
            this.c_serialPort.Name = "c_serialPort";
            this.c_serialPort.Size = new System.Drawing.Size(149, 21);
            this.c_serialPort.TabIndex = 0;
            this.c_serialPort.Text = "Select...";
            this.toolTip1.SetToolTip(this.c_serialPort, "Serial port connected to the board running Supervivi.");
            this.c_serialPort.SelectedIndexChanged += new System.EventHandler(this.c_serialPort_SelectedIndexChanged);
            this.c_serialPort.MouseClick += new System.Windows.Forms.MouseEventHandler(this.c_serialPort_MouseClick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 0;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Information:";
            // 
            // num_dlAddr
            // 
            this.num_dlAddr.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_dlAddr.Hexadecimal = true;
            this.num_dlAddr.Location = new System.Drawing.Point(134, 15);
            this.num_dlAddr.Maximum = new decimal(new int[] {
            1342177280,
            0,
            0,
            0});
            this.num_dlAddr.Name = "num_dlAddr";
            this.num_dlAddr.Size = new System.Drawing.Size(150, 21);
            this.num_dlAddr.TabIndex = 0;
            this.toolTip1.SetToolTip(this.num_dlAddr, "Where the program will be transfered on the board.\r\n(When using automatic mode)");
            this.num_dlAddr.Value = new decimal(new int[] {
            805306368,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.b_options_Reset);
            this.groupBox2.Controls.Add(this.b_options_OK);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.num_dlAddr);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(659, 44);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // b_options_OK
            // 
            this.b_options_OK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_options_OK.Location = new System.Drawing.Point(578, 15);
            this.b_options_OK.Name = "b_options_OK";
            this.b_options_OK.Size = new System.Drawing.Size(75, 23);
            this.b_options_OK.TabIndex = 2;
            this.b_options_OK.Text = "OK";
            this.b_options_OK.UseVisualStyleBackColor = true;
            this.b_options_OK.Click += new System.EventHandler(this.b_options_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Address:";
            // 
            // b_options_Reset
            // 
            this.b_options_Reset.Location = new System.Drawing.Point(497, 15);
            this.b_options_Reset.Name = "b_options_Reset";
            this.b_options_Reset.Size = new System.Drawing.Size(75, 23);
            this.b_options_Reset.TabIndex = 3;
            this.b_options_Reset.Text = "Reset";
            this.b_options_Reset.UseVisualStyleBackColor = true;
            this.b_options_Reset.Click += new System.EventHandler(this.b_options_Reset_Click);
            // 
            // SuperVivi_Transfer_Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 512);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.g_serial);
            this.Controls.Add(this.l_progress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.t_log);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 580);
            this.MinimumSize = new System.Drawing.Size(800, 580);
            this.Name = "SuperVivi_Transfer_Tool";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SuperVivi USB Transfer Utility for Mini2440/6410";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Supervivi_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.g_serial.ResumeLayout(false);
            this.g_serial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_dlAddr)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_upload;
        private System.Windows.Forms.Button b_download;
        private System.Windows.Forms.Label l_usbStatus;
        private System.Windows.Forms.TextBox t_log;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker Worker_download;
        private System.Windows.Forms.Label l_progress;
        private System.ComponentModel.BackgroundWorker Worker_upload;
        private System.Windows.Forms.Label l_usbFound;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox g_serial;
        private System.Windows.Forms.Label l_serialPort;
        private System.Windows.Forms.ComboBox c_serialPort;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown num_dlAddr;
        private System.Windows.Forms.Button b_options_OK;
        private System.Windows.Forms.Button b_options_Reset;
    }
}

