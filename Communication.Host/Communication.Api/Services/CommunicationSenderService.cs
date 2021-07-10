namespace Communication.Api.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class CommunicationSenderService : IHostedService, IDisposable
    {
        private readonly ILogger<CommunicationSenderService> _logger;
        private readonly IMessageStore _messageStore;
        private Timer _timer;

        public CommunicationSenderService(ILogger<CommunicationSenderService> logger, IMessageStore messageStore)
        {
            _logger = logger;
            _messageStore = messageStore;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var messages = _messageStore.GetMessages();
            _logger.LogInformation("Processing {Count} messages.", messages.Count);

            foreach (var message in messages)
            {
                _logger.LogInformation("Message: {Message}", message.Text);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
