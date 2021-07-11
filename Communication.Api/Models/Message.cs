namespace Communication.Api.Models
{
    using System;

    public class Message
    {
        public string To { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
