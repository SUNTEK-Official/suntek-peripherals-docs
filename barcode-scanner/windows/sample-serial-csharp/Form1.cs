using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SUNTEK.Sample.BarcodeSerial
{
    public partial class Form1 : Form
    {
        [DllImport("BarcodeScanner.dll", EntryPoint = "OpenPort", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr OpenPort(IntPtr idx);

        [DllImport("BarcodeScanner.dll", EntryPoint = "ClosePort", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClosePort(IntPtr com_port);

        [DllImport("BarcodeScanner.dll", EntryPoint = "SetPortBoudRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPortBoudRate(IntPtr com_port, int rate);

        [DllImport("BarcodeScanner.dll", EntryPoint = "SetPortDataBits", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPortDataBits(IntPtr com_port, int bits);

        [DllImport("BarcodeScanner.dll", EntryPoint = "SetPortStopBits", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPortStopBits(IntPtr com_port, int bits);

        [DllImport("BarcodeScanner.dll", EntryPoint = "SetPortParity", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPortParity(IntPtr com_port, int parity);

        [DllImport("BarcodeScanner.dll", EntryPoint = "SendData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendData(IntPtr com_port, byte[] data);

        [DllImport("BarcodeScanner.dll", EntryPoint = "ReciveData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReciveData(IntPtr com_port, byte[] data, int len, int TimeOut);

        [DllImport("BarcodeScanner.dll", EntryPoint = "Trigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Trigger(IntPtr com_port);

        [DllImport("BarcodeScanner.dll", EntryPoint = "UnTrigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UnTrigger(IntPtr com_port);

        [DllImport("BarcodeScanner.dll", EntryPoint = "ReadBarcode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadBarcode(IntPtr com_port, byte[] revivedata, int len, int TimeOut);

        [DllImport("BarcodeScanner.dll", EntryPoint = "DefaultControl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DefaultControl(IntPtr com_port);

        [DllImport("BarcodeScanner.dll", EntryPoint = "EditTriggerMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EditTriggerMode(IntPtr com_port, int type);

        public int baudRate = 115200;
        private readonly RichTextBox box = new RichTextBox();
        private readonly RichTextBox box1 = new RichTextBox();
        private readonly RichTextBox box2 = new RichTextBox();
        private readonly RichTextBox box3 = new RichTextBox();

        public static class GlobalFunAndVar
        {
            public static IntPtr fd;
            public const int Len = 1000000;
            public static int nread = 0;
            public static readonly byte[] ReviveData = new byte[Len];
            public static readonly byte[] PictureBuffer = new byte[Len];
            public const int Timeout = 600;
        }

        public Form1()
        {
            InitializeComponent();

            // Configure the command button properties.
            box.Location = new Point(50, 100);
            box.Size = new Size(700, 700);
            Controls.Add(box);

            box1.Location = new Point(140, 10);
            box1.Size = new Size(25, 25);
            Controls.Add(box1);

            box2.Location = new Point(50, 800);
            box2.Size = new Size(700, 50);
            Controls.Add(box2);

            Button btnSend = new Button
            {
                Location = new Point(750, 800),
                Size = new Size(80, 50),
                Text = "Send"
            };
            btnSend.Click += btnSend_Click_button;
            Controls.Add(btnSend);

            Button openPort = new Button
            {
                Location = new Point(50, 10),
                Size = new Size(80, 25),
                Text = "Open Port"
            };
            openPort.Click += openport_button;
            Controls.Add(openPort);

            box3.Location = new Point(450, 10);
            box3.Size = new Size(50, 25);
            Controls.Add(box3);

            Button setBaudRate = new Button
            {
                Location = new Point(350, 10),
                Size = new Size(80, 25),
                Text = "Baud Rate"
            };
            setBaudRate.Click += setBandRate_button;
            Controls.Add(setBaudRate);

            Button closePort = new Button
            {
                Location = new Point(50, 50),
                Size = new Size(80, 25),
                Text = "Close Port"
            };
            closePort.Click += closeport_button;
            Controls.Add(closePort);

            Button trigger = new Button
            {
                Location = new Point(650, 50),
                Size = new Size(80, 25),
                Text = "Trigger Decode"
            };
            trigger.Click += Trigger_button;
            Controls.Add(trigger);

            Button untrigger = new Button
            {
                Location = new Point(350, 50),
                Size = new Size(80, 25),
                Text = "Stop Decode"
            };
            untrigger.Click += UnTrigger_button;
            Controls.Add(untrigger);

            Button readBarcode = new Button
            {
                Location = new Point(650, 10),
                Size = new Size(80, 25),
                Text = "Read Data"
            };
            readBarcode.Click += ReadBarcode_button;
            Controls.Add(readBarcode);
        }

        private void openport_button(object sender, EventArgs e)
        {
            // Example: COM12 at 115200, no parity, 8 data bits, 1 stop bit.
            string str = box1.Text;
            if (str.Length == 0)
            {
                return;
            }

            GlobalFunAndVar.fd = OpenPort((IntPtr)int.Parse(str));
            SetPortBoudRate(GlobalFunAndVar.fd, baudRate);
            SetPortDataBits(GlobalFunAndVar.fd, 8);
            SetPortParity(GlobalFunAndVar.fd, 'n');
            SetPortStopBits(GlobalFunAndVar.fd, 1);
            MessageBox.Show("Port opened.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void setBandRate_button(object sender, EventArgs e)
        {
            string str = box3.Text;
            if (str.Length == 0)
            {
                return;
            }

            int testBaud = int.Parse(str);
            if (testBaud == 9600)
            {
                MessageBox.Show("Baud rate set to 9600.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                baudRate = 9600;
            }
            else if (testBaud == 115200)
            {
                MessageBox.Show("Baud rate set to 115200.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                baudRate = 115200;
            }
        }

        private void closeport_button(object sender, EventArgs e)
        {
            ClosePort(GlobalFunAndVar.fd);
            MessageBox.Show("Port closed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReadBarcode_button(object sender, EventArgs e)
        {
            box.Text = "";
            ReadBarcode(GlobalFunAndVar.fd, GlobalFunAndVar.ReviveData, GlobalFunAndVar.Len, GlobalFunAndVar.Timeout);
            string str = Encoding.Default.GetString(GlobalFunAndVar.ReviveData);
            box.Text += str;
        }

        private void Trigger_button(object sender, EventArgs e)
        {
            Trigger(GlobalFunAndVar.fd);
        }

        private void UnTrigger_button(object sender, EventArgs e)
        {
            UnTrigger(GlobalFunAndVar.fd);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void btnSend_Click_button(object sender, EventArgs e)
        {
            // Read the hex string from the input box and send it to the scanner.
            string inputMessage = box2.Text;
            byte[] messageBytes = StringToByteArray(inputMessage);
            SendData(GlobalFunAndVar.fd, messageBytes);

            box.Text = "";
            int len = ReadBarcode(GlobalFunAndVar.fd, GlobalFunAndVar.ReviveData, GlobalFunAndVar.Len, GlobalFunAndVar.Timeout);
            box.Text += ByteArrayToHexString(GlobalFunAndVar.ReviveData, len);
        }

        public static string ByteArrayToHexString(byte[] bytes, int length)
        {
            StringBuilder hex = new StringBuilder(length * 2);
            for (int i = 0; i < length; i++)
            {
                hex.AppendFormat("{0:X2}", bytes[i]);
            }
            return hex.ToString().ToLower();
        }
    }
}
