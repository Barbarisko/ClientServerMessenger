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

            Stream stm = ClientSocket.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();
            Message message = new MessageHello(Name); 

            string json = message.ToString();
            byte[] sent = asen.GetBytes(json);

            Console.WriteLine("\nTransmitting.....\n");
            stm.Write(sent, 0, sent.Length);

            byte[] accepted = new byte[100];
            int k = stm.Read(accepted, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(accepted[i]));

            var message_resp = JsonConvert.DeserializeObject<MessageHelloResponce>(asen.GetString(accepted));

            if (message_resp.Body != HelloAnswers.OK)
            {
                throw new Exception("Server rejected.");
            }             
        }

        public List<string> RequestListToSend()
        {
            Stream stm = ClientSocket.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            Message message = new MessageRequestList();

            string json = message.ToString();
            byte[] sent = asen.GetBytes(json);

            Console.WriteLine("\nRequesting.....");

            stm.Write(sent, 0, sent.Length);

            byte[] accepted = new byte[100];
            int k = stm.Read(accepted, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(accepted[i]));

            var responce = JsonConvert.DeserializeObject<MessageReturnList>(asen.GetString(accepted));

            return responce.Body;
        }

        public void SendMessage(string msg, List<string> recipients)
        {
            Stream stm = ClientSocket.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            Message message = new MessageSendMsg(recipients, msg);

            //sending
            string json = message.ToString();

            byte[] sent = asen.GetBytes(json);
            Console.WriteLine("Sending.....");


            stm.Write(sent, 0, sent.Length);

            //recieving
            byte[] accepted = new byte[100];
            int k = stm.Read(accepted, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(accepted[i]));

            var responce = JsonConvert.DeserializeObject<MessageReturnList>(asen.GetString(accepted));
        }


        ~Client()
        {
            ClientSocket.Close();
        }
    }
}
