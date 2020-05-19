using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System_antyplagiatowy
{
    public class SystemAntyplagiatowy
    {
        public static IPAddress localIpAddress = IPAddress.Parse("127.0.0.3");
        public static IPEndPoint localEndPoint = new IPEndPoint(localIpAddress, 11000);
        public static Socket listener = new Socket(localIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public static byte[] data_plik = new byte[160];
        public static int Main(String[] args)
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            for (int i = 0; i < 10; i++)
            {
                ReceiveFile();
            }
            return 0;
        }
        public static void ReceiveFile()
        {
            try
            {
                try
                {
                    Console.WriteLine("Waiting for a connection...\n");
                    Socket handler = listener.Accept();

                    // File receiver
                    try
                    {
                        Console.WriteLine("Odbiór pliku z systemu Wirtualnego Dziekanatu\n");
                        handler.Receive(data_plik);
                        Thread.Sleep(10000);
                        Console.WriteLine("File received \nSending result \n");
                        handler.Send(Encoding.ASCII.GetBytes("Result: ok"));
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
    }
}