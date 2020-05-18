using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MultiClient
{
    class Program
    {
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private static readonly Socket ClientSocket2 = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        private const int PORT = 100;
		private const int PORT2 = 103;

        static void Main()
        {
            Console.Title = "Client";
            ConnectToServer();
            RequestLoop();
            //Exit();
        }

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
					ClientSocket2.Connect(IPAddress.Loopback, PORT2);
                }
                catch (SocketException) 
                {
                    Console.Clear();
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }

        private static void RequestLoop()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

            while (true)
            {
                SendRequest();
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        /*private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }*/

        private static void SendRequest()
        {
            Console.Write("Send a request: ");
            string request = Console.ReadLine();
			//tu jakas flaga
			
			if(request == "promotor")
			{
				SendString(request); //mulServ
				ReceiveResponse();
			}
			else
			{
				SendString2(request); //mulServ
				ReceiveResponse2();
			}

            /*if (request.ToLower() == "exit")
            {
                Exit();
            }*/
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

        private static void ReceiveResponse()
        {
            byte[] buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine(text);
		
        }
		
		private static void ReceiveResponse2()
        {
            byte[] buffer = new byte[2048];
            int received = ClientSocket2.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine(text);
		
        }
    }
}