using ClientHandler;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerMessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Started");
            try
            {
                Client client1 = new Client("tuts", "127.0.0.1", 8001);

                client1.Hello();

                foreach(var i in client1.RequestListToSend())
                {
                    Console.WriteLine("\n"+i);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\nError..... " + e.StackTrace);
            }
        }
    }
}
