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

using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using LibUsbDotNet.LudnMonoLibUsb;
using LibUsbDotNet.DeviceNotify;
using EC = LibUsbDotNet.Main.ErrorCode;

namespace SuperVivi_Transfer_Tool
{
    public partial class SuperVivi_Transfer_Tool : Form
    {
        private UsbDevice mUsbDevice;
        private UsbEndpointReader mEpReader;
        private UsbEndpointWriter mEpWriter;
        private string filename = String.Empty;
        private int vid2440 = 0x5345;
        private int pid2440 = 0x1234;
        private int vid6410 = 0x0424;
        private int pid6410 = 0x2514;
        private IDeviceNotifier devNotifier;
        private string RxString = String.Empty;
        private uint dlAddr = 0x30000000;


        private static UsbDeviceFinder mini2440Finder = new UsbDeviceFinder(0x5345, 0x1234); //we select the mini2440 based on its VID & PID
        private static UsbDeviceFinder mini6410Finder = new UsbDeviceFinder(0x0424, 0x2514); //we select the mini6410 based on its VID & PID

        public SuperVivi_Transfer_Tool()
        {
            InitializeComponent(); // GUI
            t_log.AppendText("## Version: " + this.ProductVersion + "\r\n");

            usbConnect(); // We try to connect to the mini2440 if it is already plugged.
            fDeviceNotify(); // We start the usb notifier in case we plug/unplug the mini2440.
        }

        delegate void AppendNotifyDelegate();

        public void fDeviceNotify()
        {
            devNotifier = DeviceNotifier.OpenDeviceNotifier();
            devNotifier.OnDeviceNotify += onDevNotify;
        }

        // This function will be called each time we plug/unplug a usb device.
        private void onDevNotify(object sender, DeviceNotifyEventArgs e)
        {

            // If the mini2440 is disconnected, disable buttons.
            if ((e.EventType == EventType.DeviceRemoveComplete) && (((e.Device.IdProduct == pid2440) && (e.Device.IdVendor == vid2440))
                 || ((e.Device.IdProduct == pid6410) && (e.Device.IdVendor == vid6410)) ))
            {
                l_usbFound.Text = "Disconnected";
                t_log.AppendText("\r\n" + "## Device disconnected." + "\r\n");
                b_download.Enabled = false;
                b_upload.Enabled = false;
            }

            // If the mini2440 is plugged, connect to it.
            if ((e.EventType == EventType.DeviceArrival) && (((e.Device.IdProduct == pid2440) && (e.Device.IdVendor == vid2440))
                 || ((e.Device.IdProduct == pid6410) && (e.Device.IdVendor == vid6410))))
                Invoke(new AppendNotifyDelegate(usbConnect));

        }

        public void usbConnect() // The name is self-explanatory...
        {
            // Find and open the usb device.
            mUsbDevice = UsbDevice.OpenUsbDevice(mini2440Finder);
            if (mUsbDevice == null)
                mUsbDevice = UsbDevice.OpenUsbDevice(mini6410Finder);

            // If we fail to find or connect to the device
            if (mUsbDevice == null)
            {
                l_usbFound.Text = "Error";
                t_log.AppendText("\r\n" + "## Could not find or open device." + "\r\n"
                               + "Please plug the device, I will detect it." + "\r\n");
                b_download.Enabled = false;
                b_upload.Enabled = false;
            }
            // If the device is open and ready
            else
            {
                l_usbFound.Text = "Connected";
                if (mUsbDevice.UsbRegistryInfo.Pid == pid2440)
                    t_log.AppendText("\r\n" + "## Mini2440 connected." + "\r\n");
                if (mUsbDevice.UsbRegistryInfo.Pid == pid6410)
                    t_log.AppendText("\r\n" + "## Mini6410 connected." + "\r\n");
                b_download.Enabled = true;
                b_upload.Enabled = true;
                mEpReader = mUsbDevice.OpenEndpointReader((ReadEndpointID)(byte.Parse("1") | 0x80)); // = 81
                mEpWriter = mUsbDevice.OpenEndpointWriter((WriteEndpointID)byte.Parse("3"));
                if (mEpWriter.EndpointInfo == null) // Try opening endpoint 4 if endpoint 3 is unavailable (i.e. after usb reset when downloading firmware)
                    mEpWriter = mUsbDevice.OpenEndpointWriter((WriteEndpointID)byte.Parse("4"));

                //mEpReader.Flush();
                mEpWriter.Flush();

                IUsbDevice wholeUsbDevice = mUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    // This is a "whole" USB device. Before it can be used, 
                    // the desired configuration and interface must be selected.

                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    wholeUsbDevice.ClaimInterface(0);
                }
            }

        }

        // This class is calculating the checksum before uploading. The checksum is sent after the file itself. (2 last bytes)
        // We stream the file to calculate it. This avoids loading the whole file into memory which could be enormous.
        private static UInt16 csum_upload(FileStream fileStream, uint fileSize)
        {
            UInt16 csum = 0;
            int j;
            int step = 1024 * 4;
            byte[] data = new byte[step];

            BinaryReader binReader = new BinaryReader(fileStream);

            for (j = 0; j <= (fileSize - step); j += step)
            {
                data = binReader.ReadBytes(step);
                foreach (byte value in data)
                {
                    csum += value;
                }
            }

            // Last step
            if (j <= fileSize)
            {
                byte[] lastdata = new byte[fileSize - j];
                lastdata = binReader.ReadBytes((int)(fileSize - j));
                foreach (byte value in lastdata)
                {
                    csum += value;
                }
            }

            return csum;
        }

        private void b_upload_Click(object sender, EventArgs e)
        {

            if (b_upload.Text == "Cancel")
            {
                Worker_upload.CancelAsync();
                if (Worker_upload.CancellationPending)
                    t_log.AppendText("## Cancelling..." + "\r\n");
                return;
            }

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                openFileDialog1.Dispose();
                t_log.AppendText("## Opening " + filename + "\r\n");
                byte[] infoBuffer = new byte[8];
                byte[] csumBuffer = new byte[2];
                byte[] bstart = new byte[4];
                byte[] bsize = new byte[4];

                // Create a FileStream object to write a stream to a file
                FileInfo file = new FileInfo(filename);
                uint fileSize = (uint)file.Length;
                uint fullFileSize = (uint)file.Length + (uint)infoBuffer.Length + (uint)csumBuffer.Length;
                //uint start = 0x30000000; // Kernel ram base
                uint start = dlAddr; // We start where we transfered the program
                FileStream fileStream = file.OpenRead();
                uint end = (start + fileSize);

                t_log.AppendText("## Calculating checksum... This might take some time on large files." + "\r\n");
                UInt16 csum = csum_upload(fileStream, fileSize);

                t_log.AppendText("## File Information: " + "\r\n"
                               + "## File Size        : " + fileSize.ToString() + " (" + (((fileSize) / 1024) / 1024).ToString() + "MB" 
                               + " - " + ((fileSize) / 1024).ToString() + "KB)" + "\r\n"
                               + "## Start Addr       : " + "0x" + start.ToString("x") + "\r\n"
                               + "## End Addr         : " + "0x" + end.ToString("x") + "\r\n");


                /* 4 bytes address, 4 bytes length, data, 2 bytes csum,
                 * we have to spread the ints across 4 bytes (an int32 is 4 bytes - 32 bits / 8) */

                bstart = BitConverter.GetBytes(start); // 1
                bsize = BitConverter.GetBytes(fullFileSize); // 2
                csumBuffer = BitConverter.GetBytes(csum); // 3

                infoBuffer[0] = bstart[0];
                infoBuffer[1] = bstart[1];
                infoBuffer[2] = bstart[2];
                infoBuffer[3] = bstart[3];

                infoBuffer[4] = bsize[0];
                infoBuffer[5] = bsize[1];
                infoBuffer[6] = bsize[2];
                infoBuffer[7] = bsize[3];

                t_log.AppendText("## Checksum        : 0x" + csum.ToString("x04") + "\r\n");
                t_log.AppendText("## Got the info we need, now uploading data..." + "\r\n");
                b_upload.Enabled = false;
                b_download.Enabled = false;


                // We load the data in our class.
                WorkerData workerData = new WorkerData
                {
                    total = fullFileSize,
                    start = start,
                    end = end,
                    info = infoBuffer,
                    csum = csumBuffer,
                    filename = filename
                };

//                fileStream.Unlock(0, fileStream.Length);
                fileStream.Flush();
                fileStream.Close();
                b_upload.Enabled = true;
                b_upload.Text = "Cancel";

                Worker_upload.RunWorkerAsync(workerData); // We spawn the thread.

            }
            else
            {
                openFileDialog1.Dispose(); // If no file is selected, free ressources.
            }
        }

        private void b_download_Click(object sender, EventArgs e)
        {
            if (b_download.Text == "Cancel")
            {
                Worker_download.CancelAsync();
                if (Worker_download.CancellationPending)
                    t_log.AppendText("Cancelling..." + "\r\n");
                return;
            }

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                saveFileDialog1.Dispose();
                t_log.AppendText("## Saving to " + filename + "\r\n");
                byte[] dlinfo = new byte[0x000010a0]; //this array has the size of the info that the mini2440 sends us.
                b_upload.Enabled = false;
                b_download.Enabled = false;

                int uiTransmitted;
                uint total;
                ErrorCode eReturn = mEpReader.Read(dlinfo, 1000, out uiTransmitted);
                if (eReturn == ErrorCode.None)
                {
                    // We gather the informations from the info buffer and display it.
                    total = BitConverter.ToUInt32(dlinfo, 0x30);
                    t_log.AppendText("Nand Flash Information: " + "\r\n"
                                    + "## type      : " + "0x" + BitConverter.ToInt32(dlinfo, 0x00).ToString("x08") + "\r\n"
                                    + "## flags     : " + "0x" + BitConverter.ToInt32(dlinfo, 0x04).ToString("x08") + "\r\n"
                                    + "## size      : " + ((BitConverter.ToInt32(dlinfo, 0x08) / 1024) / 1024).ToString() + "MB"
                                    + " - " + (BitConverter.ToInt32(dlinfo, 0x08) / 1024).ToString() + "KB" + "\r\n"
                                    + "## erasesize : " + (BitConverter.ToInt32(dlinfo, 0x0c) / 1024).ToString() + "KB" + "\r\n"
                                    + "## oobblock  : " + BitConverter.ToInt32(dlinfo, 0x10).ToString() + "\r\n"
                                    + "## oobsize   : " + BitConverter.ToInt32(dlinfo, 0x14).ToString() + "\r\n"
                                    + "## ecctype   : " + "0x" + BitConverter.ToInt32(dlinfo, 0x18).ToString("x") + "\r\n"
                                    + "## eccsize   : " + BitConverter.ToInt32(dlinfo, 0x1c).ToString() + "\r\n");

                    t_log.AppendText("Backup Information: " + "\r\n"
                                   + "## Start Addr       : " + "0x" + BitConverter.ToInt32(dlinfo, 0x20).ToString("x08") + "\r\n"
                                   + "## End Addr         : " + "0x" + BitConverter.ToInt32(dlinfo, 0x24).ToString("x08") + "\r\n"
                                   + "## bBackupOOB       : " + BitConverter.ToInt32(dlinfo, 0x28).ToString() + "\r\n"
                                   + "## bCheckBad        : " + BitConverter.ToInt32(dlinfo, 0x2c).ToString() + "\r\n"
                                   + "## dwBackupTotalLen : " + "0x" + total.ToString("x") + " (" + ((total / 1024) / 1024) + "MB)" + "\r\n"
                                   + "## dwReservedBlks   : " + BitConverter.ToInt32(dlinfo, 0x34).ToString() + "\r\n"
                                   + "## dwEPInPktSize    : " + BitConverter.ToInt32(dlinfo, 0x38).ToString() + "\r\n");

                    t_log.AppendText("## Got the info we need, now downloading data..." + "\r\n");

                    b_download.Enabled = true;
                    b_download.Text = "Cancel";
                }

                else
                {
                    t_log.AppendText("## No info to read! " + eReturn + "\r\n");
                    b_upload.Enabled = true;
                    b_download.Enabled = true;
                    return;
                }

                // We load the data in our class.
                WorkerData workerData = new WorkerData
                {
                    total = total,
                    start = BitConverter.ToUInt32(dlinfo, 0x20),
                    end = BitConverter.ToUInt32(dlinfo, 0x24)
                };

                Worker_download.RunWorkerAsync(workerData); // We spawn the thread.

            }

            else
            {
                saveFileDialog1.Dispose();
            }
        }


        #region Nested Types

        private delegate void OnDataReceivedDelegate(object sender, EndpointDataEventArgs e);

        private delegate void UsbErrorEventDelegate(object sender, UsbError e);

        #endregion


        private void Worker_download_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerData workerData = e.Argument as WorkerData; // We gather the info from our class
            uint offset = 0;
            uint total = workerData.total;
            int uiTransmitted;
            string file = workerData.filename;
            int progressnb;
            byte[] readBuffer = new byte[2048*8];
            ErrorCode eReturn;

            // Create a FileStream object to write a stream to a file
            FileStream fileStream = System.IO.File.Create(filename, readBuffer.Length);
            BinaryWriter binWriter = new BinaryWriter(fileStream);

            while ((offset <= (total - readBuffer.Length)) && !e.Cancel)
            {
                if (Worker_download.CancellationPending) e.Cancel = true;

                eReturn = mEpReader.Read(readBuffer, 1000, out uiTransmitted);
                if (eReturn == ErrorCode.None)
                {
                    binWriter.Write(readBuffer);
                    offset += (uint)readBuffer.Length;
                    progressnb = (int)((float)((float)offset / (float)total) * 100.0F);
                    Worker_download.ReportProgress(progressnb);
                }

                else
                {
                    binWriter.Close();
                    fileStream.Close();
                    mEpReader.Flush();
                    return;
                }

            }

            if ((offset >= (total - readBuffer.Length)) && !Worker_download.CancellationPending)
            {
                mEpReader.Flush();
                //we create a buffer sized to the remaining bytes. This is to avoid writing a file too big.
                byte[] endBuffer = new byte[total - offset];
                mEpReader.Read(endBuffer, 1000, out uiTransmitted);
                binWriter.Write(endBuffer);
                return;
            }
            mEpReader.Flush();
            binWriter.Flush();
            binWriter.Close();
            fileStream.Flush();
            fileStream.Unlock(0, fileStream.Length);
            fileStream.Close();
        }

        private void Worker_download_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            l_progress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void Worker_download_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            b_upload.Enabled = true;
            b_download.Enabled = true;
            b_download.Text = "Download...";

            if (e.Cancelled)
            {
                t_log.AppendText("## Canceled!" + "\r\n");
                return;
            }

            if (progressBar1.Value >= 90)
            {
                l_progress.Text = "Done.";
                t_log.AppendText("\r\n" + "## Done Downloading. " + "\r\n");
                progressBar1.Value = 100;
            }
            else
                t_log.AppendText("## Something went wrong... " + "\r\n");
        }

        private void Worker_upload_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerData workerData = e.Argument as WorkerData; // We gather the info from our class
            uint offset = 0;
            uint total = workerData.total;
            int uiTransmitted;
            string file = workerData.filename;
            int progressnb;
            byte[] writeBuffer = new byte[2048*8];
            byte[] info = workerData.info;
            byte[] csum = workerData.csum;
            bool small = false;
            ErrorCode eReturn;

            // Create a FileStream object to read a stream from a file
            FileStream fileStream = System.IO.File.OpenRead(file);
            BinaryReader binReader = new BinaryReader(fileStream);

            if (total > writeBuffer.Length) //if file is smaller then buffer, we'll skip this part.
            {

                // Filling the buffer with informative bytes...
                for (int i = 0; i < (int)info.Length; i++)
                {
                    writeBuffer[i] = info[i];
                }

                binReader.Read(writeBuffer, (int)info.Length, (int)(writeBuffer.Length - info.Length));
                bool infosent = false;

                while ((offset <= (total - writeBuffer.Length)) && !e.Cancel)
                {

                    if (Worker_upload.CancellationPending) e.Cancel = true;

                    if (infosent) //to avoid overwriting the infos
                        writeBuffer = binReader.ReadBytes(writeBuffer.Length);

                    eReturn = mEpWriter.Write(writeBuffer, 1000, out uiTransmitted);
                    if (eReturn == ErrorCode.None)
                    {
                        offset += (uint)writeBuffer.Length;
                        progressnb = (int)((float)((float)offset / (float)total) * 100.0F);
                        Worker_upload.ReportProgress(progressnb);
                        infosent = true;
                    }

                    else
                    {
                        binReader.Close();
                        fileStream.Close();
                        mEpWriter.Flush();
                        return;
                    }

                }

            }

            else //if file is smaller then buffer do this
            {
                small = true;
                t_log.AppendText("## Small file detected" + "\r\n");
                writeBuffer = new byte[total]; //buffer size = file size

                // Filling the buffer with informative bytes...
                for (int i = 0; i < (int)info.Length; i++)
                {
                    writeBuffer[i] = info[i];
                }

                binReader.Read(writeBuffer, (int)info.Length, (int)(writeBuffer.Length - info.Length));
                mEpWriter.Write(writeBuffer, 1000, out uiTransmitted);
                mEpWriter.Write(csum, 1000, out uiTransmitted);
                Worker_upload.ReportProgress(100);
                return;
            }

            if ((offset >= (total - writeBuffer.Length)) && !e.Cancel && !small)
            {

                //we create a buffer sized to the remaining bytes. This is to avoid writing a file too big.
                byte[] endBuffer = new byte[total - offset];
                endBuffer = binReader.ReadBytes(endBuffer.Length);
                mEpWriter.Write(endBuffer, 1000, out uiTransmitted);
                mEpWriter.Write(csum, 1000, out uiTransmitted);
                return;
            }
            mEpWriter.Flush();
            binReader.Close();
            fileStream.Flush();
            fileStream.Unlock(0, fileStream.Length);
            fileStream.Close();

        }

        private void Worker_upload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            l_progress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void Worker_upload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            b_upload.Enabled = true;
            b_download.Enabled = true;
            b_upload.Text = "Upload...";

            if (e.Cancelled)
            {
                t_log.AppendText("## Canceled!" + "\r\n");
                return;
            }

            if (progressBar1.Value >= 90)
            {
                l_progress.Text = "Done.";
                t_log.AppendText("\r\n" + "## Done uploading. " + "\r\n");
                progressBar1.Value = 100;
            }
            else
                t_log.AppendText("## Something went wrong... " + "\r\n");
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
        }

        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            //RxString = "Serial: Error!" + "\r\n";
            //this.Invoke(new EventHandler(DisplayText));
        }

        private void DisplayText(object sender, EventArgs e)
        {
            string text = t_log.Text; // We make a temporary copy of the textbox's content for faster processing.
            string buff = RxString;
            char[] buffer = new char[RxString.Length];
            buffer = RxString.ToCharArray();
            RxString = "";

            if (buff == "\033[2K\r") text = text.Substring(0, text.Length - 10);
            else text += buff;

            foreach (char word in buffer)
            {
                if (word == '\b')
                {
                    text = text.Substring(0, text.Length - 1); // If backspace char detected, we delete one char from our temp string
                }
             //   else
            //        text += word;
            }

            t_log.Text = text;
            t_log.SelectionStart = t_log.Text.Length;
            t_log.SelectionLength = 0;
            t_log.ScrollToCaret();

        }

        private void c_serialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            // We open the previously selected port.
            if (serialPort1.IsOpen)
                serialPort1.Close();

            t_log.AppendText("## Serial port selected: " + c_serialPort.SelectedItem.ToString() + "\r\n");
            serialPort1.PortName = c_serialPort.SelectedItem.ToString();
            serialPort1.Open();
        }

        private void c_serialPort_MouseClick(object sender, MouseEventArgs e)
        {
            // Each time we click on the combobox, we refresh the list of available serial ports
            c_serialPort.Items.Clear();
            string[] sPorts = SerialPort.GetPortNames();
            foreach (string port in sPorts)
                c_serialPort.Items.Add(port);
        }


        private void Supervivi_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close(); // We close the serial port when quiting.
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (t_log.Focused && ( (keyData == Keys.Right) || (keyData == Keys.Left) ||
                (keyData == Keys.Up) || (keyData == Keys.Down) ) )
            {
                // If the port is closed, don't try to send a character.
                if (!serialPort1.IsOpen) return true;

                // If the port is Open, declare a char[] array with one element.
                switch (keyData)
                {
                    case Keys.Up:
                        sendKeyToBoard((char)0x1b);
                        sendKeyToBoard((char)0x5b);
                        sendKeyToBoard((char)0x41);
                        break;
                    case Keys.Down:
                        sendKeyToBoard((char)0x1b);
                        sendKeyToBoard((char)0x5b);
                        sendKeyToBoard((char)0x42);
                        break;
                    case Keys.Left:
                        sendKeyToBoard((char)0x1b);
                        sendKeyToBoard((char)0x5b);
                        sendKeyToBoard((char)0x44);
                        break;
                    case Keys.Right:
                        sendKeyToBoard((char)0x1b);
                        sendKeyToBoard((char)0x5b);
                        sendKeyToBoard((char)0x43);
                        break;
                    default:
                        break;
                }
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void t_log_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the port is closed, don't try to send a character.
            if (!serialPort1.IsOpen) return;


            // If the port is Open, declare a char[] array with one element.
            switch ((int)e.KeyChar)
            {
                case 22: // Case: STRG+V (Paste)
                        // Send the ClipboardText to Board
                        foreach (Char i in Clipboard.GetText().Replace("\n", ""))
                        {
                            sendKeyToBoard(i);
                        }
                        break;

                default:
                    sendKeyToBoard(e.KeyChar);
                    break;
            }

            e.Handled = true;
        }

        /// <summary>
        /// sendKeyToBoard(char()) is sending char
        /// </summary>
        /// <param name="c">Char</param>
        private void sendKeyToBoard(char c)
        {
            char[] buff = new char[1];
            // Load element 0 with the key character.
            buff[0] = c;

            // Send the one character buffer.
            serialPort1.Write(buff, 0, 1);
        }

        private void sendKeyToBoard(byte[] c)
        {
            serialPort1.Write(c, 0, c.Count());
        }

        private void b_options_OK_Click(object sender, EventArgs e)
        {
            dlAddr = (uint)this.num_dlAddr.Value;
            t_log.AppendText("## Download Address Changed: 0x" + dlAddr.ToString("x08") + "\r\n");
        }

        private void b_options_Reset_Click(object sender, EventArgs e)
        {
            this.num_dlAddr.Value = 0x30000000;
            dlAddr = (uint)0x30000000;
            t_log.AppendText("## Download Address Changed: 0x" + dlAddr.ToString("x08") + "\r\n");
        }

    }
}

class WorkerData // The class we use to provide data to the asynchronous threads.
{
    public uint total { get; set; }
    public uint start { get; set; }
    public uint end { get; set; }
    public byte[] info { get; set; }
    public byte[] csum { get; set; }
    public byte[] data { get; set; }
    public string filename { get; set; }

}