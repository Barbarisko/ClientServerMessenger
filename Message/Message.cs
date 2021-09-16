using Newtonsoft.Json;
using System;

namespace Protocol
{
    public enum Commands
    {
        Hello,
        HelloResponse
    }
    public enum HelloAnswers
    {
        OK, 
        jopa
    }
    public class Message
    {
        public Commands Command;
        public string Body;

        public Message(Commands command, string body)
        {
            Command = command;
            Body = body;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }
    }
}
