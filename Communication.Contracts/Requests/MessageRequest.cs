namespace Communication.Contracts.Requests
{
    using System;

    public class MessageRequest
    {
        public string To { get; set; }
        public string Text { get; set; }
    }
}
