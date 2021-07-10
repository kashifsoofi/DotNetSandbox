using System;
using System.Collections.Generic;
using Communication.Api.Models;

namespace Communication.Api.Services
{
    public class MessageStore : IMessageStore
    {
        public MessageStore()
        {
        }

        public void AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
        }
    }
}
