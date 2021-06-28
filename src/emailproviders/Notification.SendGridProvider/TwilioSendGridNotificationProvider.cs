
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Desyco.Notification.SendGridProvider
{
    public class TwilioSendGridNotificationProvider : EmailNotificationProvider<TwilioSendGridOptions>
    {
        private readonly TwilioSendGridOptions _options;


        public TwilioSendGridNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent) : base(storageProvider, eventHub, options, templateContent)
        {
            _options = options.GetExternalOptionsProvider<TwilioSendGridOptions>(NotificationConst.ExternalProviderType);
        }

        protected override async Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m)
        {

            var client = new SendGridClient(_options.ApiKey);
            var response = await client.SendEmailAsync(PrepareMimeMessage(m));
            if (((int)response.StatusCode).ToString()[0] != '2')
                throw new Exception(await response.Body.ReadAsStringAsync());

        }

        private SendGridMessage PrepareMimeMessage(NotificationMessage m)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(m.From.Address, m.From.DisplayName));

            var recipients = new List<EmailAddress>();

            m.To.ForEach(t =>
            {
                var mails = t.Address.Split(',');
                recipients.AddRange(mails.Select(mail => new EmailAddress(mail, t.DisplayName)));
            });

            msg.AddTos(recipients);
            msg.AddBccs(m.Bcc.Select(s => new EmailAddress(s.Address, s.DisplayName)).ToList());
            msg.SetSubject(m.Subject);
            switch (m.TextFormat)
            {
                case ExternalTextFormat.Html:
                    msg.AddContent(MimeType.Html, m.Body);
                    break;
                default:
                    msg.AddContent(MimeType.Text, m.Body);
                    break;
            }

            return msg;


        }

    }
}