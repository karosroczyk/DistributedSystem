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

        public static IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");
        public static IPAddress WDIpAddress = IPAddress.Parse("127.0.0.2");

        public Form1()
        {
            InitializeComponent();
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
                SendMessageWithFile(WDIpAddress, openFileDialog1.FileName);

                //tu otzymujef
                //ReceiveEcho(localIpAddress, 11000);

                string score = ReceiveEcho(localIpAddress, 11001);
                label7.Text = score.Substring(0, 1);
                if (score.Substring(0, 1) == '2'.ToString())
                    label7.ForeColor = Color.Red;
                else
                    label7.ForeColor = Color.Lime;

                label8.Text = score.Substring(1, 1);
                label8.ForeColor = Color.Lime;
            }
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

        public static void SendMessageWithFile(IPAddress ipAddress, string fileName)
        {
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 11000);
                try
                {
                    Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    sender.Connect(remoteEndPoint);

                    // File sender    
                    Console.WriteLine("Sending file");
                    sender.SendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread);

                    // Echo receiver 
                    byte[] bytes = new byte[1024];
                    int bytesRec = sender.Receive(bytes);
                    string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Console.WriteLine(data + "\n");

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine(ane.ToString());
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static string ReceiveEcho(IPAddress ipAddress, int port)
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
                try
                {
                    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    listener.Bind(localEndPoint);
                    listener.Listen(10);
                    Socket handler = listener.Accept();

                    // File receiver
                    try
                    {
                        byte[] buffer = new byte[160];
                        int received = handler.Receive(buffer);
                        string data_plik = Encoding.ASCII.GetString(buffer, 0, received);
                        return data_plik;
                        //Console.WriteLine(data_plik);
                    }
                    catch (Exception ex)
                    {
                        handler.Send(Encoding.ASCII.GetBytes("Result: bad"));
                        Console.WriteLine(ex.ToString());
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return "---";
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }
    }
}
