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
        static List<string> registeredUsers;
        static void Main(string[] args)
        {
            Random r = new Random();
            Console.BackgroundColor = (ConsoleColor)r.Next(0, 16);
            Console.WriteLine("CLIENT SIDE");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\nWrite your client name:");

            var name = Console.ReadLine();
            try
            {
                Client client1 = new Client(name, "127.0.0.1", 8001);
                client1.Hello();

                while (true)
                {
                    Console.WriteLine("\nWelcome to the simple mailing. This is what you can do: " +
                                    "\n1 - Request list to choose recipients" +
                                    "\n2 - Check mailbox" +
                                    "\n \nWhat to do?(insert command number)");
                    var command = Console.ReadLine();//parse to enum

                    switch (command)
                    {
                        case "1":
                            registeredUsers = client1.RequestListToSend();

                            for (int i = 0; i < registeredUsers.Count; i++)
                                Console.WriteLine("\n" + registeredUsers[i]);

                            var list_to_send = new List<string>();

                            Console.WriteLine("\nSending to (write names separating with comma):");
                            args = Console.ReadLine().Split(", ");
                            foreach (var rec in args)
                            {
                                list_to_send.Add(rec);
                            }

                            Console.WriteLine("\nMessage:");
                            var message = Console.ReadLine();

                            client1.SendMessage($"\nFrom {client1.Name}: \n {message}", list_to_send);

                            break;

                        case "2":
                            var messages = client1.CheckForMsg();

                            if (messages.Count==0)
                                Console.WriteLine("0 new messages");
                            else
                                foreach (var m in messages) 
                                    Console.WriteLine(m);                  
                            break;
                    }
                }         
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError..... " + e.StackTrace);
            }
        }
    }
}
