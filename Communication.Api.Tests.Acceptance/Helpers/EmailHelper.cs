namespace Communication.Api.Tests.Acceptance.Helpers
{
    using Communication.Api.Tests.Acceptance.ApiClients;
    using FluentAssertions;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class EmailHelper
    {
        private readonly ConcurrentDictionary<string, string> _trackedMessageIdEmailAddressMap = new ConcurrentDictionary<string, string>();
        private readonly Mail7Client _mail7Client;

        public EmailHelper(Mail7Client mail7Client)
        {
            _mail7Client = mail7Client;
        }

        private void TrackMessageId(string messageId, string emailAddress) => _trackedMessageIdEmailAddressMap.AddOrUpdate(messageId, emailAddress, (key, existingVal) => emailAddress);

        public async Task VerifyEmailAsync(string emailAddress, string expectedSubject = "Test")
        {
            var messages = await _mail7Client.GetInboxAsync(emailAddress);
            messages.Should().HaveCountGreaterOrEqualTo(1);

            var message = messages.First();
            TrackMessageId(message._id, emailAddress);

            message.mail_source.subject.Should().Be(expectedSubject);
        }

        public async Task CleanUpTrackedEntities(CancellationToken cancellationToken)
        {
            var messageIdEmailAddressMap = _trackedMessageIdEmailAddressMap.ToDictionary(x => x.Key, x => x.Value);
            _trackedMessageIdEmailAddressMap.Clear();

            foreach (var messageId in messageIdEmailAddressMap.Keys)
            {
                await _mail7Client.DeleteEmailAsync(messageIdEmailAddressMap[messageId], messageId);
            }
        }
    }
}
