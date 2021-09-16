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
            Message message = new Message(Commands.Hello, Name); 

            string json = message.ToString();
            byte[] ba = asen.GetBytes(json);

            Console.WriteLine("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(bb[i]));

            message = JsonConvert.DeserializeObject<Message>(asen.GetString(bb));

            var result = Enum.Parse<HelloAnswers>(message.Body);
            if (result != HelloAnswers.OK)
            {
                throw new Exception("Server rejected.");
            }             
        }

        ~Client()
        {
            ClientSocket.Close();
        }
    }
}
