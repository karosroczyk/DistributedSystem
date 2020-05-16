using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace User
{
    class Program
    {
        public static IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");
        //public static IPEndPoint localEndPoint = new IPEndPoint(localIpAddress, port);
        public static IPAddress WDIpAddress = IPAddress.Parse("127.0.0.2");
        public static string fileName = Path.GetFullPath("plik.txt");
        public static int Main(String[] args)
        {
            Console.WriteLine("Wysyłanie pliku do systemu Wirtualnego Dziekanatu");
            SendMessageWithFile(WDIpAddress);
            ReceiveEcho(localIpAddress, 11000);
            ReceiveEcho(localIpAddress, 11001);
            return 0;
        }
        public static void ReceiveEcho(IPAddress ipAddress, int port)
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
                        Console.WriteLine(data_plik);
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
        }
        public static void SendMessageWithFile(IPAddress ipAddress)
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
    }
}
