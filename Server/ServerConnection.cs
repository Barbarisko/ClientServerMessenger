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

        Message message;
        ASCIIEncoding asen = new ASCIIEncoding();

        public ServerConnection(Socket socket)
        {
            this.socket = socket;
        }

        public void StartProcess()
        {
            try
            {
                while (true)
                {
                    byte[] b = new byte[Protocol.Protocol.StandartBufSize];
                    int k = socket.Receive(b);
                    Console.WriteLine("\nRecieved...");

                    for (int i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(b[i]));

                    var str = asen.GetString(b);
                    message = JsonConvert.DeserializeObject<Message>(str);

                    object msg;
                    switch (message.Command)
                    {
                        case Commands.Hello:
                            msg = JsonConvert.DeserializeObject<MessageHello>(str);

                            ClientName = ((MessageHello)msg).Body;

                            if (!Server.GetServer().ClientExists(ClientName)) 
                            {
                                message = new MessageHelloResponce(HelloAnswers.NE_OK);
                                socket.Send(asen.GetBytes(message.ToString()));
                                //socket.Close();
                                return;
                            }
                            message = new MessageHelloResponce(HelloAnswers.OK);
                            break;

                        case Commands.GiveList:
                            message = new MessageReturnList(Server.GetServer().Clients);
                            break;

                        case Commands.SendMsg:
                            msg = JsonConvert.DeserializeObject<MessageSendMsg>(str);

                            var msgBox = Server.GetServer().MessageBox;
                            var list_to_send = ((MessageSendMsg)msg).Body.recepients;

                            foreach (var recipient in list_to_send)
                            {
                                if (!msgBox.ContainsKey(recipient))
                                    msgBox.Add(recipient, new List<string>());     

                                msgBox[recipient].Add(((MessageSendMsg)msg).Body.message);
                            }
                            message = null;

                            break;

                        case Commands.CheckMsgBox:
                            message = new MessageReturnList(Server.GetServer().MessageBox[ClientName]);
                            break;
                    }

                    if (message == null)
                    {
                        continue;
                    }

                    var bytestosend = asen.GetBytes(message.ToString());
                    if (bytestosend.Length > 100)
                    {
                        socket.Send(asen.GetBytes(new MessageBig(bytestosend.Length).ToString()));

                    }
                    socket.Send(bytestosend);
                    Console.WriteLine("\nSent response\n");
                }
            }
            catch(Exception e)
            {
                socket.Close();
                Console.WriteLine($"Client {ClientName} disconnected");
            }           
        }

        ~ServerConnection()
        {
            socket.Close();
        }
    }
}
