using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Postal.Contracts;

namespace Ubik.Postal.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfig _config;

        public async Task SendSingleMail(MailAddress sender, MailAddress recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender.DisplayName, sender.Email));
            message.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;

            message.Body = builder.ToMessageBody();

            await SendMessage(message);
        }




        private  async Task SendMessage(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_config.Host, _config.Port, (_config.SSL)? SecureSocketOptions.SslOnConnect: SecureSocketOptions.Auto);

                client.Authenticate(_config.UseName, _config.Password);

                await client.SendAsync(message);

                client.Disconnect(true);
            }
        }
    }
}
