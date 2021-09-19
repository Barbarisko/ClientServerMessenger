using ClientHandler;
using System;
using System.Collections.Generic;
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
            Console.WriteLine("Write your client name:");

            var name = Console.ReadLine();
            try
            {
                Client client1 = new Client(name, "127.0.0.1", 8001);

                client1.Hello();

                var gen_list = client1.RequestListToSend();
                var list_to_send = new List<string>();

                for(int i=0; i< gen_list.Count; i++)
                {
                    Console.WriteLine("\n"+gen_list[i]);
                    if ((i + 1) % 2 == 0)
                        list_to_send.Add(gen_list[i]);
                }

                var message = Console.ReadLine();

                client1.SendMessage($"\nFrom {client1.Name}: \n {message}", list_to_send);

            }
            catch (Exception e)
            {
                Console.WriteLine("\nError..... " + e.StackTrace);
            }
        }
    }
}
