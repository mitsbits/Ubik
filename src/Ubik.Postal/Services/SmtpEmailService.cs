using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Postal.Contracts;
using Microsoft.Extensions.OptionsModel;
using MailKit.Net.Imap;
using MailKit;
using System.Diagnostics;

namespace Ubik.Postal.Services
{
    public class SmtpImapEmailService : IEmailService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly ImapConfig _imapConfig;

        public SmtpImapEmailService(IOptions<PostalSettings> postalOptions) { _smtpConfig = postalOptions.Value.Smtp; _imapConfig = postalOptions.Value.Imap; }

        public async Task SendSingleMail(MailAddress sender, MailAddress recipient, string subject, string body, IEnumerable<IAttachmentInfo> attachments)
        {
            if (!recipient.Email.WellFormedMail()) throw new EmailFormatException(recipient.Email);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender.DisplayName, sender.Email));
            message.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;

            var multipart = attachments.Any() ? new Multipart("alternative") : new Multipart("alternative");
            var plain = new TextPart();
            plain.SetText(Encoding.UTF8, "");
            var html = new TextPart("html");
            html.SetText(Encoding.UTF8, body);
            multipart.Add(plain); // always add plain first
            multipart.Add(html);

            if (attachments.Any())
            {

                foreach (var info in attachments)
                {
                    var attachment = new MimePart(info.MediaType, info.MediaTSubType)
                    {
                        ContentObject = new ContentObject(info.GetStream(), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = info.FileName
                    };
                    multipart.Add(attachment);
                }
            }

            message.Body = multipart;
            await SendMessage(new[] { message });
        }

        private async Task SendMessage(MimeMessage[] messages)
        {

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, (_smtpConfig.SSL) ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);

                await client.AuthenticateAsync(_smtpConfig.UseName, _smtpConfig.Password);
                foreach (var message in messages)
                {
                    await client.SendAsync(message);
                }
                await client.DisconnectAsync(true);
            }

        }


        public void Main()
        {
            using (var client = new ImapClient())
            {
                client.Connect(_imapConfig.Host, _imapConfig.Port, _imapConfig.SSL);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_imapConfig.UseName, _imapConfig.Password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                Debug.WriteLine("Total messages: {0}", inbox.Count);
                Debug.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    if (i > 100) break;
                    var message = inbox.GetMessage(i);
                    Debug.WriteLine("Subject: {0}", message.Subject);
                }

                client.Disconnect(true);
            }
        }
    }
}