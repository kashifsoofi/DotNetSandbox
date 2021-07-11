namespace Communication.Api.Services
{
    using System;
    using Communication.Api.Models;

    public interface IEmailSender
    {
        void Send(Message message);
    }
}
