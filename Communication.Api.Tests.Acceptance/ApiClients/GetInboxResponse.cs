namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using System.Collections.Generic;

    public class GetInboxResponse
    {
        public string status { get; set; }
        public List<MailMessage> data { get; set; }
    }
}
