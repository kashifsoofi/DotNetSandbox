namespace Communication.Api.Controllers
{
    using System;
    using Communication.Api.Models;
    using Communication.Api.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageStore _messageStore;
        private readonly IEmailSender _emailSender;

        public MessagesController(
            ILogger<MessagesController> logger,
            IMessageStore messageStore,
            IEmailSender emailSender)
        {
            _logger = logger;
            _messageStore = messageStore;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("queue")]
        public IActionResult Queue([FromBody] MessageRequest request)
        {
            _messageStore.AddMessage(new Message
            {
                To = request.To,
                Text = request.Text,
                CreatedOn = DateTime.UtcNow,
            });

            return Accepted();
        }

        [HttpPost]
        [Route("send")]
        public IActionResult Send([FromBody] MessageRequest request)
        {
            try
            {
                _emailSender.Send(new Message
                {
                    To = request.To,
                    Text = request.Text,
                    CreatedOn = DateTime.UtcNow,
                });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error sending email");
                return new StatusCodeResult(500);
            }
        }
    }
}
