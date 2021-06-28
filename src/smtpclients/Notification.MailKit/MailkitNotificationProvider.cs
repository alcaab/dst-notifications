using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Desyco.Notification.MailKitProvider
{
    public class MailkitNotificationProvider : EmailNotificationProvider<MailKitOptions>
    {
        private readonly MailKitOptions _options;


        public MailkitNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent) : base(storageProvider, eventHub, options, templateContent)
        {
            _options = options.GetExternalOptionsProvider<MailKitOptions>(NotificationConst.ExternalProviderType);
        }

        protected override Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m)
        {

            try
            {
                using (var emailClient = new SmtpClient())
                {

                    emailClient.DeliveryStatusNotificationType =  _options.DeliveryStatusNotificationType;
                    emailClient.Connect(parameters.SmtpServer, parameters.Port, parameters.EnableSsl);

                    if(!string.IsNullOrEmpty(parameters.UserName) && !string.IsNullOrEmpty(parameters.Password))
                       emailClient.Authenticate(parameters.UserName, parameters.Password);

                    emailClient.Send(PrepareMessage(m));
                    emailClient.Disconnect(true);
                }

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }


        }

        private MimeMessage PrepareMessage(NotificationMessage m)
        {

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(m.From.DisplayName, m.From.Address));

            //
            m.To.ForEach(t =>
            {
                var mails = t.Address.Split(',');
                foreach (var mail in mails)
                {
                    mimeMessage.To
                        .Add(new MailboxAddress(t.DisplayName, mail));
                }
            });

            mimeMessage.Cc
                .AddRange(m.Cc.Select(s => new MailboxAddress(s.DisplayName, s.Address))
                    .ToArray());

            mimeMessage.Bcc
                .AddRange(m.Bcc.Select(s => new MailboxAddress(s.DisplayName, s.Address))
                    .ToArray());

            mimeMessage.ResentCc
                .AddRange(m.ResentCc.Select(s => new MailboxAddress(s.DisplayName, s.Address))
                    .ToArray());

            mimeMessage.ReplyTo
                .AddRange(m.ReplyTo.Select(s => new MailboxAddress(s.DisplayName, s.Address))
                    .ToArray());

            mimeMessage.Subject = m.Subject;

            var body = new TextPart(TextFormat.Html)
            {
                Text = m.Body
            };

            var multipart = new Multipart("mixed") { body };

            m.Attachments.ForEach(attachment =>
            {
                multipart.Add(new MimePart(attachment.MediaType)
                {
                    Content = new MimeContent(File.OpenRead(attachment.FileName)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                });
            });

            mimeMessage.Body = multipart;


            return mimeMessage;
        }

    }
}