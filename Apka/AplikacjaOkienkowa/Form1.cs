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
		
		private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private static readonly Socket ClientSocket2 = new Socket
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

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    //Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
					ClientSocket2.Connect(IPAddress.Loopback, PORT2);
                }
                catch (SocketException) 
                {
                    // Console.Clear();
                }
            }

            //Console.Clear();
            //Console.WriteLine("Connected");
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

				string scores = SendRequest();
				setLabels(scores);
                string ok = SendRequest2();
                label10.Text = ok;
            }
        }

        private static string SendRequest()
        {
            SendString("praca");
			return ReceiveResponse();
		
        }

        private static string SendRequest2()
        {
            SendString2("cokolwiek"); //mulServ
            return ReceiveResponse2();
        }
        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
			ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
		 private static void SendString2(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
			ClientSocket2.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static string ReceiveResponse() //promotor
        {
            byte[] buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string score = Encoding.ASCII.GetString(data);
            return score;
            //setLabels(score);
      
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

		private static string ReceiveResponse2()
        {
            byte[] buffer = new byte[2048];
            int received = ClientSocket2.Receive(buffer, SocketFlags.None);
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            return Encoding.ASCII.GetString(data);
            //Console.WriteLine(text);
		
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
