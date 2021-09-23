using Newtonsoft.Json;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientHandler
{
    public class Client
    {
        public string Name;
        public TcpClient ClientSocket;

        string IpAddress;
        int port;

        Stream stm;
        ASCIIEncoding asen = new ASCIIEncoding();

        public Client(string _name, string _ipadress, int _port)
        {
            Name = _name;
            IpAddress = _ipadress;
            port = _port;
            ClientSocket = new TcpClient();
        }

        public void Hello()
        {
            ClientSocket.Connect(IpAddress, port);

            Message message = new MessageHello(Name);

            Send(message);

            byte[] accepted = new byte[100];
            stm.Read(accepted, 0, 100);

            var message_resp = JsonConvert.DeserializeObject<MessageHelloResponce>(asen.GetString(accepted));

            if (message_resp.Body != HelloAnswers.OK)
                throw new Exception("Server rejected.");                        
        }
        public List<string> CheckForMsg()
        {
            Message message = new MessageCheckBox();

            Send(message);

            byte[] accepted = new byte[100];
            stm.Read(accepted, 0, 100);

            var message_resp = JsonConvert.DeserializeObject<Message>(asen.GetString(accepted));

            if (message_resp.Command == Commands.BigFile)
            {
                var message_big = JsonConvert.DeserializeObject<MessageBig>(asen.GetString(accepted));
                accepted = new byte[message_big.Body];

                stm.Read(accepted, 0, message_big.Body);
            }

            var message_list = JsonConvert.DeserializeObject<MessageReturnList>(asen.GetString(accepted));

            return message_list.Body;            
        }

        public List<string> RequestListToSend()
        {
            Message message = new MessageRequestList();

            Send(message);

            byte[] accepted = new byte[100];
            int k = stm.Read(accepted, 0, 100);

            var responce = JsonConvert.DeserializeObject<MessageReturnList>(asen.GetString(accepted));

            return responce.Body;
        }

        public void BroadcastMessage(string msg, List<string> recipients)
        {
            Message message = new MessageSendMsg(recipients, msg);
            Send(message);

            stm.Flush();
        }

        private void Send(object message)
        {
            stm = ClientSocket.GetStream();
            string json = message.ToString();
            byte[] sent = asen.GetBytes(json);
            stm.Write(sent, 0, sent.Length);
        }

        ~Client()
        {
            ClientSocket.Close();
        }
    }
}
