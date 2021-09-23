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
            CreateColorLabel("CLIENT SIDE");
            SendSystemMessage("\nWrite your client name:");

            var name = Console.ReadLine();
            try
            {
                Client client1 = new Client(name, Protocol.Protocol.IpAddress, Protocol.Protocol.ServerPort);
                client1.Hello();

                while (true)
                {
                    Console.Clear();
                    SendSystemMessage(PrintMenu(client1.Name));

                    var command = Console.ReadLine();//parse to enum
                    switch (command)
                    {
                        case "1":
                            registeredUsers = client1.RequestListToSend();

                            for (int i = 0; i < registeredUsers.Count; i++)
                                Console.WriteLine("\n" + registeredUsers[i]);

                            var list_to_send = new List<string>();

                            SendSystemMessage("\nSending to (write names separating with comma):");
                            args = Console.ReadLine().Split(", ");
                            foreach (var rec in args)
                            {
                                list_to_send.Add(rec);
                            }

                            SendSystemMessage("\nMessage:");
                            var msg_to_send = Console.ReadLine();

                            client1.BroadcastMessage($"From {client1.Name}:\n\t{msg_to_send}\n", list_to_send);

                            break;

                        case "2":
                            var messages = client1.CheckForMsg();

                            if (messages.Count == 0)
                                CreateColorLabel("0 new messages");

                            else
                                foreach (var m in messages) 
                                    Console.WriteLine(m);
                            Console.ReadKey();

                            break;
                        default:
                            SendSystemMessage("No other options");
                            Console.ReadKey();
                            break;
                    }
                }         
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void CreateColorLabel(string text)
        {
            Random r = new Random();
            Console.BackgroundColor = (ConsoleColor)r.Next(0, 16);
            Console.WriteLine($"{text}");
            Console.BackgroundColor = ConsoleColor.Black;            
        }
        private static void SendSystemMessage(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{text}");
            Console.ResetColor();           
        }

        private static string PrintMenu(string name)
        {
            return $"\n{name}, welcome to the simple mailing. This is what you can do: " +
                "\n1 - Request list to choose recipients" +
                "\n2 - Check mailbox" +
                "\n \nWhat to do?(insert command number)";
        }
    }
}
