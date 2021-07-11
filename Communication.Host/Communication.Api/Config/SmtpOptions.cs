namespace Communication.Api.Config
{
    using System;

    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string FromName { get; set; }
        public string FromAddress { get; set; }
    }
}
