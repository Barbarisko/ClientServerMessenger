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
                Console.WriteLine("\nRecieved...\n");

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));

                ASCIIEncoding asen = new ASCIIEncoding();
                var str = asen.GetString(b);
                Message message = JsonConvert.DeserializeObject<Message>(str);
                
                object msg;
                switch (message.Command)
                {    
                    case Commands.Hello:
                        msg = JsonConvert.DeserializeObject<MessageHello>(str);
                        ClientName = ((MessageHello)msg).Body;

                        if (Server.GetServer().ClientExists(ClientName))
                            message = new MessageHelloResponce(HelloAnswers.OK);
                        else
                        {
                            message = new MessageHelloResponce(HelloAnswers.jopa);
                            socket.Send(asen.GetBytes(message.ToString()));
                            socket.Close();
                            return;
                        }
                        break;

                    case Commands.GiveList:
                        msg = JsonConvert.DeserializeObject<MessageRequestList>(str);
                        message = new MessageReturnList(Server.GetServer().Clients);
                        break;

                    case Commands.SendMsg:
                        msg = JsonConvert.DeserializeObject<MessageSendMsg>(str);

                        var msgBox = Server.GetServer().MessageBox;
                        var list_to_send = ((MessageSendMsg)msg).Body.recepients;

                        foreach(var recipient in list_to_send)
                        {
                            if (!msgBox.ContainsKey(recipient))
                            {
                                msgBox.Add(recipient, new List<string>());
                            }

                            msgBox[recipient].Add(((MessageSendMsg)msg).Body.message);

                            message = new MessageHello(msgBox[recipient].LastOrDefault()+"\n sent a message.");
                        }
                        break;
                }

                socket.Send(asen.GetBytes(message.ToString()));
                Console.WriteLine("\nSent Acknowledgement\n");
            }
        }


        ~ServerConnection()
        {
            socket.Close();
        }
    }
}
