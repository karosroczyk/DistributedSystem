using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Promotor
{
    public class SocketListener
    {
        public static IPAddress localIpAddress = IPAddress.Parse("127.0.0.4");
        public static byte[] data_plik = new byte[160];
        public static int Main(String[] args)
        {
            ReceiveFile(localIpAddress);
            return 0;
        }

        public static void ReceiveFile(IPAddress ipAddress)
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
                try
                {
                    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...\n");
                    Socket handler = listener.Accept();

                    // File receiver
                    try
                    {
                        Console.WriteLine("Odbiór pliku z systemu Wirtualnego Dziekanatu\n");
                        handler.Receive(data_plik);
                        Console.WriteLine("File received \nSending result \n");
                        Random rnd = new Random();
                        int ocena = rnd.Next(2, 5);
						int ocenaRecenzent = rnd.Next(3, 5);
                        handler.Send(Encoding.ASCII.GetBytes(ocena.ToString()+ocenaRecenzent.ToString()));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File error");
                        throw ex;
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
