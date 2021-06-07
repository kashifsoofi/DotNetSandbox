using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpBatchHandler.Multipart;
using Microsoft.AspNetCore.WebUtilities;

namespace BatchClient
{
    class Program
    {
        public static Uri baseUri = new Uri("http://localhost:6545/");

        static async Task Main(string[] args)
        {
            var count = 5;
            var requests = new List<HttpRequestMessage>();

            for (var i = 0; i < count; i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseUri, $"api/values/{i}"));
                requests.Add(request);
            }

            var responseCount = await SendBatchRequestAsync(requests);
            Console.WriteLine(responseCount);
        }


        private static async Task<int> SendBatchRequestAsync(IEnumerable<HttpRequestMessage> requests)
        {
            var client = new HttpClient();

            var batchUri = new Uri(baseUri, "api/values");
            var count = 0;

            using (var requestContent = new MultipartContent("batch", "batch_" + Guid.NewGuid()))
            {
                var multipartContent = requestContent;
                foreach (var request in requests)
                {
                    var content = new HttpApplicationContent(request);
                    multipartContent.Add(content);
                }

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, batchUri)
                {
                    Content = requestContent
                })
                {
                    using (var responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(false))
                    {
                        var reader = await HttpContentExtensions.ReadAsMultipartAsync(responseMessage.Content, CancellationToken.None).ConfigureAwait(false);
                        MultipartSection section;
                        while ((section = await reader.ReadNextSectionAsync()) != null)
                        {
                            count++;

                            Console.WriteLine(await section.ReadAsStringAsync());
                        }
                    }
                }
            }

            return count;
        }
    }
}
