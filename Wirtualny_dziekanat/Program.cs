using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Wirtualny_dziekanat
{
    public class Wirtualny_dziekanat
    {
        public static IPHostEntry host = Dns.GetHostEntry("localhost");
        public static string fileName = "C:\\Users\\Lenovo\\Desktop\\Magisterka\\Systemy rozproszone\\Systemy_rozproszone\\plik.txt";
        public static int Main(String[] args)
        {
            bool result = false;
            result = ReceiveUser();

            if (result)
            {
                Console.WriteLine("Wysyłanie pliku do systemu antyplagiatowego");
                int portIP = 1;
                IPAddress ipAddress = host.AddressList[portIP];
                string data_ap = StartClient(portIP, ipAddress);

                if (data_ap.IndexOf("ok") > -1)
                {
                    IPAddress ipAddress2 = IPAddress.Parse("127.0.0.2");
                    portIP = 0;
                    Console.WriteLine("Wysyłanie pliku do promotora");
                    string data_promotor = StartClient(portIP, ipAddress2);
                }
            }
            return 0;
        }

        public static bool ReceiveUser()
        {
            bool result = false;
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
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
                        Console.WriteLine("Odebranie pliku do użytkownika");
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
            result = true;
            return result;
        }
    
        public static string StartClient(int ipNr, IPAddress ipAddress)
        {
            string data = null;
            try
            {
                //IPAddress ipAddress = host.AddressList[ipNr];
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
