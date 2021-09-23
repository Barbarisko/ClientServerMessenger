
using System;
using System.Collections.Generic;
using System.IO;
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
            Clients = InitClients();
            MessageBox = InitMessageBox(Clients);
            IPAddress ipAd = IPAddress.Parse(Protocol.Protocol.IpAddress);

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

            Console.WriteLine($"The server is running at port {Protocol.Protocol.ServerPort}...");
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

        private List<string> InitClients()
        {
            var clientstring = File.ReadAllLines("Clients.txt");
            foreach(string s in clientstring)
            {
                s.Trim(new Char[] { ',', ' ', '\n' });
            }
            return clientstring.ToList();
        }
        private Dictionary<string, List<string>> InitMessageBox(List<string> names)
        {
            MessageBox = new Dictionary<string, List<string>>();

            foreach(var i in names)
            {
                MessageBox.Add(i, new List<string>());
            }
            return MessageBox;
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
