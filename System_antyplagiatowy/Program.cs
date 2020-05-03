using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace System_antyplagiatowy
{
    public class SystemAntyplagiatowy
    {
        public static int Main(String[] args)
        {
            StartSystemAntyplagiatowy();
            return 0;
        }
        public static void StartSystemAntyplagiatowy()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[1];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                try
                {
                    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...\n");
                    Socket handler = listener.Accept();

                    // File receiver
                    string data_plik = null;
                    byte[] buffer = new byte[160];

                    try
                    {
                        int received = handler.Receive(buffer);
                        data_plik += Encoding.ASCII.GetString(buffer, 0, received);
                        Console.WriteLine("File received \nSending result \n");
                        handler.Send(Encoding.ASCII.GetBytes("Result: ok"));
                    }
                    catch (Exception ex)
                    {
                        handler.Send(Encoding.ASCII.GetBytes("Result: bad"));
                        Console.WriteLine(ex.ToString());
                    }

                    // Echo sender
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