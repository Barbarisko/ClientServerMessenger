using Newtonsoft.Json;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerHandler
{
   public  class ServerConnection
    {
        Socket socket;

        string ClientName;
        public ServerConnection(Socket socket)
        {
            this.socket = socket;
        }

        public void StartProcess()
        {
            while (true)
            {
                byte[] b = new byte[100];
                int k = socket.Receive(b);
                Console.WriteLine("Recieved...");

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));

                ASCIIEncoding asen = new ASCIIEncoding();
                var str = asen.GetString(b);
                Message message = JsonConvert.DeserializeObject<Message>(str);

                switch (message.Command)
                {
                    case Commands.Hello:
                        ClientName = message.Body;

                        if (Server.GetServer().ClientExists(ClientName))
                            message = new Message(Commands.HelloResponse, HelloAnswers.OK.ToString());

                        else
                        {
                            message = new Message(Commands.HelloResponse, HelloAnswers.jopa.ToString());
                            socket.Send(asen.GetBytes(message.ToString()));
                            socket.Close();
                            return;
                        }

                        break;
                }


                socket.Send(asen.GetBytes(message.ToString()));
                Console.WriteLine("\nSent Acknowledgement");
            }
        }

        ~ServerConnection()
        {
            socket.Close();
        }
    }
}
