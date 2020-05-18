using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Wirtualny_dziekanat
{
    public class Wirtualny_dziekanat
    {
        public static IPAddress localIpAddress = IPAddress.Parse("127.0.0.2");
        public static IPAddress UserIpAddress = IPAddress.Parse("127.0.0.1");
        public static IPAddress SystemAntyplagiatowyIpAddress = IPAddress.Parse("127.0.0.3");
        public static IPAddress PromotorIpAddress = IPAddress.Parse("127.0.0.4");
        public static IPEndPoint localEndPoint = new IPEndPoint(localIpAddress, 11000);
		
        public static Socket listener = new Socket(localIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		
        public static byte[] data_plik = new byte[160];
        public static int Main(String[] args)
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            for (int i = 0; i < 10; i++)
            {
                bool result = ReceiveFile();

                if (result)
                {
                    Console.WriteLine("Wysyłanie pliku do systemu antyplagiatowego");
                    string data_ap = SendMessage(SystemAntyplagiatowyIpAddress, 11000, data_plik, true);

                    SendMessage(UserIpAddress, 11000, Encoding.ASCII.GetBytes("Informacja zwrotna\nSystem antyplagiatowy: " + data_ap), false);

                    if (data_ap.IndexOf("ok") > -1)
                    {
                        Console.WriteLine("Wysyłanie pliku do promotora");
                        string data_pr = SendMessage(PromotorIpAddress, 11000, data_plik, true);
                        SendMessage(UserIpAddress, 11001, Encoding.ASCII.GetBytes(data_pr), false);
                    }
                }
            }
			Console.ReadLine();
            return 0;
        }
        public static bool ReceiveFile()
        {
            bool result = false;
            try
            {
                    Console.WriteLine("Waiting for a connection...\n");
                    Socket handler = listener.Accept();

                    // File receiver
                    try
                    {
                        Console.WriteLine("Odebranie pliku od użytkownika");
                        handler.Receive(data_plik);
                        Console.WriteLine("File received\nSending result \n");
                        handler.Send(Encoding.ASCII.GetBytes("Result: File received"));
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        handler.Send(Encoding.ASCII.GetBytes("Result: File not received"));
                        Console.WriteLine(ex.ToString());
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }  
		
        public static string SendMessage(IPAddress ipAddress, int port, byte[] message, bool echo)
        {
            string data = null;
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);
                try
                {
                    Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    sender.Connect(remoteEndPoint);

                    // Message sender                       
                    sender.Send(message);

                    if(echo)
                    {
                        // Echo receiver 
                        Console.WriteLine("Sending file");
                        byte[] bytes = new byte[1024];
                        int bytesRec = sender.Receive(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Console.WriteLine(data + "\n");
                    }

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
