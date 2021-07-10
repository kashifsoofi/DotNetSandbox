namespace Communication.Api.Services
{
    using System;
    using System.Collections.Generic;
    using Communication.Api.Models;

    public interface IMessageStore
    {
        void AddMessage(Message message);

        IEnumerable<Message> GetMessages();
    }
}
