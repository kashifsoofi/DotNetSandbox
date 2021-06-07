namespace BatchRequestClient
{
    using BatchRequest.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using HttpMethod = System.Net.Http.HttpMethod;

    class Program
    {
        public static Uri baseUri = new Uri("http://localhost:27738/");

        static async Task Main(string[] args)
        {
            var count = 5;
            var requestInfos = new List<RequestInfo>();

            for (var i = 0; i < count; i++)
            {
                var requestInfo = new RequestInfo
                {
                    RelativeUri = $"/api/values/{i}",
                    Method = "Get",
                    ContentType = "application/json",
                    Body = "",
                };
                requestInfos.Add(requestInfo);
            }

            var responseCount = await SendBatchRequestAsync(requestInfos);
            Console.WriteLine($"Response Count: {responseCount}");
        }

        private static async Task<int> SendBatchRequestAsync(IEnumerable<RequestInfo> requestInfos)
        {
            var client = new HttpClient();
            var batchUri = new Uri(baseUri, "api/batch");

            var batchRequest = new HttpRequestMessage(HttpMethod.Post, batchUri);
            batchRequest.Content = new StringContent(JsonConvert.SerializeObject(requestInfos), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(batchRequest);
            var responseJson = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<List<RequestResult>>(responseJson);
            foreach (var result in results)
            {
                Console.WriteLine(result.Body);
            }

            return results.Count;
        }
    }
}
