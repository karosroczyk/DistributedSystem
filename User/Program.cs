using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace User
{
    class Program
    {
        public static IPHostEntry host = Dns.GetHostEntry("localhost");
        public static string fileName = "C:\\Users\\Lenovo\\Desktop\\Magisterka\\Systemy rozproszone\\Systemy_rozproszone\\plik.txt";
        public static int Main(String[] args)
        {
            Console.WriteLine("Wysyłanie pliku do systemu Wirtualnego Dziekanatu");
            int portIP = 0;
            string data = StartClient(portIP);

            return 0;
        }
        public static string StartClient(int ipNr)
        {
            string data = null;
            try
            {
                IPAddress ipAddress = host.AddressList[ipNr];
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
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
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
            return data;
        }
    }
}
