using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Started");

            try
            {
               Server server =  Server.GetServer();
                server.Listen();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
