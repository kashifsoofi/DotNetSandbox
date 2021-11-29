namespace Communication.Api.Tests.Acceptance.ApiClients
{
    using Flurl;
    using Flurl.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Mail7Client : IMail7Client
    {
        private class Endpoints
        {
            public const string Inbox = "inbox";
            public const string Email = "email";
            public const string Delete = "delete";
        }

        private class QueryStringParameterName
        {
            public const string ApiKey = "apikey";
            public const string ApiSecret = "apisecret";
        }

        private readonly Mail7Configuration _configuration;

        public Mail7Client(Mail7Configuration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<MailMessage>> GetInboxAsync(string emailAddress)
        {
            var parts = emailAddress.Split("@", StringSplitOptions.RemoveEmptyEntries);
            var response = await $"{_configuration.BaseUrl}"
                .AppendPathSegment(Endpoints.Inbox)
                .SetQueryParams(new { apikey = _configuration.ApiKey, apisecret = _configuration.ApiSecret, to = parts[0], domain = parts[1] })
                .GetJsonAsync<GetInboxResponse>();
            return response.data;
        }

        public async Task<bool> DeleteEmailAsync(string emailAddress, string messageId)
        {
            var parts = emailAddress.Split("@", StringSplitOptions.RemoveEmptyEntries);
            var response = await $"{_configuration.BaseUrl}"
                .AppendPathSegment(Endpoints.Delete)
                .SetQueryParams(new { apikey = _configuration.ApiKey, apisecret = _configuration.ApiSecret, mesid = messageId, domain = parts[1] })
                .GetJsonAsync<DeleteEmailResponse>();
            return response.status == "success";
        }
    }
}
