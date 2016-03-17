using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Postal.Contracts;

namespace Ubik.Postal.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfig _config;

        public async Task SendSingleMail(MailAddress sender, MailAddress recipient, string subject, string body, IEnumerable<IAttachmentInfo> attachments)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender.DisplayName, sender.Email));
            message.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;

            var multipart = new Multipart("alternative");
            var plain = new TextPart();
            plain.SetText(Encoding.UTF8, "");
            var html = new TextPart("html");
            html.SetText(Encoding.UTF8, body);
            multipart.Add(plain); // always add plain forst
            multipart.Add(html);

            if (attachments.Any())
            {
                multipart = new Multipart("mixed");
                foreach (var info in attachments)
                {
                    var attachment = new MimePart(info.MediaType, info.MediaTSubType)
                    {
                        ContentObject = new ContentObject(info.GetStream(), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = info.FileName
                    };
                }
            }

            message.Body = multipart;
            await SendMessage(message);
        }




        private async Task SendMessage(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_config.Host, _config.Port, (_config.SSL) ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);

                client.Authenticate(_config.UseName, _config.Password);

                await client.SendAsync(message);

                client.Disconnect(true);
            }
        }
    }
}
