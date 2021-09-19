using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerHandler
{
  public class Server
    {
        public List<string> Clients;
        public Dictionary<string, List<string>> MessageBox;
        TcpListener listener;
        
        private static Server server;

        private Server()
        {
            Clients = new List<string>() {"tuts", "tuts2" };
            MessageBox = new Dictionary<string, List<string>>();

            IPAddress ipAd = IPAddress.Parse("127.0.0.1");

            listener = new TcpListener(ipAd, 8001);

        }

        public static Server GetServer()
        {
            if (server == null)
                server = new Server();
            return server;
        }

        public void Listen()
        {
            /* Start Listeneting at the specified port */
            listener.Start();

            Console.WriteLine("The server is running at port 8001...");
            Console.WriteLine("The local End point is  :" +
                              listener.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");

            while (true)
            {
                Socket s = listener.AcceptSocket();
                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                var connection = new ServerConnection(s);
                Thread thread = new Thread(connection.StartProcess);

                thread.Start();
            }
        }

        public bool ClientExists(string name)
        {
            return Clients.Contains(name);
        }

        ~Server()
        {
            listener.Stop();
        }
    }
}
