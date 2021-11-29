namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using System;

    public class MailMessage
    {
        public string _id { get; set; }
        public string to_username { get; set; }
        public string to_domain { get; set; }
        public MailSource mail_source { get; set; }
        public bool is_deleted { get; set; }
        public bool is_read { get; set; }
        public DateTime sender_time { get; set; }
    }
}
