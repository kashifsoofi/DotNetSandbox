namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMail7Client
    {
        Task<IEnumerable<MailMessage>> GetInboxAsync(string emailAddress);

        Task<bool> DeleteEmailAsync(string emailAddress, string messageId);
    }
}
