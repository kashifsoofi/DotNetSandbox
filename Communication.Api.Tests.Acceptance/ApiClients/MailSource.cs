namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using System;
    using System.Collections.Generic;

    public class MailSource
    {
        public List<object> attachments { get; set; }
        public string html { get; set; }
        public string text { get; set; }
        public string textAsHtml { get; set; }
        public string subject { get; set; }
        public string references { get; set; }
        public DateTime date { get; set; }
        public MailAddress to { get; set; }
        public MailAddress from { get; set; }
        public string messageId { get; set; }
        public string inReplyTo { get; set; }
    }
}
