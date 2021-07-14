namespace Communication.ApiClient
{
    using System.Threading.Tasks;
    using Communication.Contracts.Requests;

    public interface ICommunicationApiClient
    {
        Task<bool> SendAsync(MessageRequest request);
        Task<bool> QueueAsync(MessageRequest request);
    }
}
