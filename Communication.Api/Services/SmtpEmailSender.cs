namespace Communication.Api.Services
{
    using System;
    using Communication.Api.Config;
    using Communication.Api.Models;
    using MailKit.Net.Smtp;
    using MimeKit;

    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _smtpOptions;

        public SmtpEmailSender(SmtpOptions smtpOptions)
        {
            _smtpOptions = smtpOptions;
        }

        public void Send(Message message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_smtpOptions.FromName, _smtpOptions.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(message.To, message.To));
            mimeMessage.Subject = "Test";

            mimeMessage.Body = new TextPart("plain")
            {
                Text = message.Text,
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpOptions.Host, _smtpOptions.Port, false);
                client.Authenticate(_smtpOptions.Username, _smtpOptions.Password);

                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
}
