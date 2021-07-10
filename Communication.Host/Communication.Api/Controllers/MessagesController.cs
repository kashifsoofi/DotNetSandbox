namespace Communication.Api.Controllers
{
    using System;
    using Communication.Api.Models;
    using Communication.Api.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageStore _messageStore;

        public MessagesController(IMessageStore messageStore)
        {
            _messageStore = messageStore;
        }

        [HttpPost]
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
    }
}
