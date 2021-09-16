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
                Client client1 = new Client("tuts", "192.168.56.1", 8001);

                client1.Hello();


            }
            catch (Exception e)
            {
                Console.WriteLine("\nError..... " + e.StackTrace);
            }
        }
    }
}
