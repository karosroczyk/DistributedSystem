using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AplikacjaOkienkowa
{
    public partial class Form1 : Form
    {
		
		private static readonly Socket PromotorSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
		private static readonly Socket AntyplagiatSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        private const int PORT = 100;
		private const int PORT2 = 103;

        public Form1()
        {
			ConnectToServer();
            InitializeComponent();
        }
		
		
        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!PromotorSocket.Connected || !AntyplagiatSocket.Connected)
            {
                try
                {
                    attempts++;
                    PromotorSocket.Connect(IPAddress.Loopback, PORT);
                    AntyplagiatSocket.Connect(IPAddress.Loopback, PORT2);
                }
                catch (SocketException) 
                {
                    // Console.Clear();
                }
            }

        }


        private void PictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmaps|*.bmp|jpeps|*.jpg";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = "przesłana";
                label2.ForeColor = Color.Lime;
                label4.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;

                //tu zmiana
                string antyplagiatResponse = SendRequestToAntyplagiat(openFileDialog1);
                
                string scores = SendRequestToPromotor(openFileDialog1);
                if (antyplagiatResponse != "Result: ok")
                {
                    label10.Text = "WYKRYTO\n PLAGIAT";
                    label10.ForeColor = Color.Red;

                    label8.Text = "-";
                    label8.ForeColor = Color.Black;
                    label7.Text = "-";
                    label7.ForeColor = Color.Black;
                }
                else
                {
                    label10.Text = "OK";
                    label10.ForeColor = Color.Black;
                    setLabels(scores);
                }
                

                /*
                string scores = SendRequestToPromotor(openFileDialog1);
				setLabels(scores);
                string ok = SendRequestToAntyplagiat(openFileDialog1);
                label10.Text = ok;

                */
            }
        }

        private static string SendRequestToPromotor(OpenFileDialog ofd)
        {
            SendString(PromotorSocket, ofd.FileName);
			return ReceiveResponse(PromotorSocket);
		
        }

        private static string SendRequestToAntyplagiat(OpenFileDialog ofd)
        {
            SendString(AntyplagiatSocket, ofd.FileName);
            return ReceiveResponse(AntyplagiatSocket);
        }

        private static void SendString(Socket socket, string text)
        {
            socket.SendFile(text, null, null, TransmitFileOptions.UseDefaultWorkerThread);
            //byte[] buffer = Encoding.ASCII.GetBytes(text);
            //socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static string ReceiveResponse(Socket socket)
        {
            byte[] buffer = new byte[2048];
            int received = socket.Receive(buffer, SocketFlags.None);
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string score = Encoding.ASCII.GetString(data);
            return score;
        }

        private void setLabels(string score)
        {
            label7.Text = score.Substring(0, 1);
            if (score.Substring(0, 1) == '2'.ToString())
                label7.ForeColor = Color.Red;
            else
                label7.ForeColor = Color.Lime;

            label8.Text = score.Substring(1, 1);
            label8.ForeColor = Color.Lime;
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }
    }
}
