using System;
using Desyco.Notification;
using Desyco.Notification.Twilio;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TwilioCollectionExtensions
    {
        public static NotificationOptions UseTwilio(this NotificationOptions options, Action<TwilioOptions> configAction = null)
        {

            var config = new TwilioOptions();
            configAction?.Invoke(config);

            options.ConfigureProviderOptions(NotificationConst.ExternalProviderType, config);

            options.UseExternalProvider(sp => new TwilioNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>()
                ));

            return options;
        }


    }
}