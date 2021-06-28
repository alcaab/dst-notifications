using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public class NetMailOptions : EmailOptions
    {
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
        public bool UseDefaultCredentials { get; set; } 
    }

    public class NetMailProvider : EmailNotificationProvider<NetMailOptions>
    {

        private readonly NetMailOptions _options;

        public NetMailProvider(
            IStorageProvider storageProvider, 
            INotificationEventHub eventHub,
            NotificationOptions options, 
            ITemplateContentProvider templateContent) : base(storageProvider, eventHub, options, templateContent)
        {
            _options = options.GetExternalOptionsProvider<NetMailOptions>(NotificationConst.ExternalProviderType);
        }

        protected override async Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m)
        {

                using (var smtp = new SmtpClient(parameters.SmtpServer, parameters.Port))
                {
                    smtp.EnableSsl = parameters.EnableSsl;
                    smtp.DeliveryMethod = _options.DeliveryMethod;
                    smtp.UseDefaultCredentials = _options.UseDefaultCredentials;
                    if (!string.IsNullOrEmpty(parameters.UserName) && !string.IsNullOrEmpty(parameters.Password))
                        smtp.Credentials = new NetworkCredential(parameters.UserName, parameters.Password);

                    ServicePointManager.ServerCertificateValidationCallback =
                        (sender, certificate, chain, sslPolicyErrors) => true;

                    await smtp.SendMailAsync(PrepareMessage(m));

                }

        }

        private MailMessage PrepareMessage(NotificationMessage message)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(message.From.Address, message.From.DisplayName)
            };

            message.To.ForEach(m => mail.To.Add(new MailAddress(m.Address, m.DisplayName)));
            message.ReplyTo.ForEach(m => mail.ReplyToList.Add(new MailAddress(m.Address, m.DisplayName)));

            mail.Subject = message.Subject;
            mail.IsBodyHtml = message.TextFormat == ExternalTextFormat.Html;
            mail.Body = message.Body;

            return mail;

        }


    }
}