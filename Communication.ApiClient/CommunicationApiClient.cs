namespace Communication.ApiClient
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Communication.Contracts.Requests;

    public class CommunicationApiClient : ICommunicationApiClient
    {
        private const string SendEndpoint = "api/messages/send";
        private const string QueueEndpoint = "api/messages/queue";

        private readonly HttpClient httpClient;

        public CommunicationApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> SendAsync(MessageRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, SendEndpoint)
            {
                Content = JsonContent.Create(request),
            };

            var response = await this.httpClient.SendAsync(httpRequest);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> QueueAsync(MessageRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, QueueEndpoint)
            {
                Content = JsonContent.Create(request),
            };

            var response = await this.httpClient.SendAsync(httpRequest);
            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}
