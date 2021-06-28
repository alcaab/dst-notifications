using System;
using System.Threading.Tasks;

namespace Desyco.Notification.Twilio
{
    public class TwilioNotificationProvider : IExternalNotificationProvider
    {
        private readonly TwilioOptions _options;


        public TwilioNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent)
        {
           _options = options.GetExternalOptionsProvider<TwilioOptions>(NotificationConst.ExternalProviderType);
        }

        protected Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m)
        {
            try
            {

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }

        }

        public Task Notify(NotificationMessage m)
        {
            throw new NotImplementedException();
        }
    }
}