using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public enum Commands
    {
        Hello,
        HelloResponse, 
        GiveList,
        ReturnList,
        SendMsg,
        CheckMsgBox,
        BigFile
    }
    public enum HelloAnswers
    {
        OK, 
        NE_OK
    }
    public class Message
    {       
        public Commands Command;
        public object Body;

        public Message(Commands command)
        {
            Command = command;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }

        public byte[] ToBytes()
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(this.ToString());
        }
    }
    public class MessageHello : Message
    {
        public new string Body;

        public MessageHello(string body) : base(Commands.Hello)
        {
            Body = body;
        }
    }
    
    public class MessageHelloResponce : Message
    {
        public new HelloAnswers Body;

        public MessageHelloResponce(HelloAnswers body) : base(Commands.HelloResponse)
        {
            Body = body;
        }
    }
    public class MessageRequestList : Message
    {
        public MessageRequestList() : base(Commands.GiveList)
        {
        }
    }
    public class MessageCheckBox : Message
    {
        public MessageCheckBox() : base(Commands.CheckMsgBox)
        {
        }
    }
    public class MessageReturnList : Message
    {
        public new List<string> Body;

        public MessageReturnList(List<string> clients) : base(Commands.ReturnList)
        {
            Body = clients;
        }
    }
    public class MessageBig : Message
    {
        public new int Body;

        public MessageBig(int bodysize) : base(Commands.BigFile)
        {
            Body = bodysize;
        }
    }
    public class MessageSendMsg : Message
    {
        public struct Data
        {
            public List<string> recepients;
            public string message;
        }
        public new Data Body;

        public MessageSendMsg(List<string> recepients, string message) : base(Commands.SendMsg)
        {
            Body = new Data();
            Body.recepients = recepients;
            Body.message = message;
        }
    }
}
