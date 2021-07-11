namespace Communication.Api.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Communication.Api.Models;

    public class MessageStore : IMessageStore
    {
        private readonly ConcurrentQueue<Message> _queue;

        public MessageStore()
        {
            _queue = new ConcurrentQueue<Message>();
        }

        public void AddMessage(Message message)
        {
            _queue.Enqueue(message);
        }

        public List<Message> GetMessages()
        {
            var messages = _queue.ToArray();
            _queue.Clear();
            return messages.ToList();
        }
    }
}
