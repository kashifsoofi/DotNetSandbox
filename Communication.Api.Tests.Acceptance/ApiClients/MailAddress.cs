namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using System.Collections.Generic;

    public class MailAddress
    {
        public List<MailAddressValue> value { get; set; }
        public string html { get; set; }
        public string text { get; set; }
    }

    public class MailAddressValue
    {
        public string address { get; set; }
        public string name { get; set; }
    }
}
