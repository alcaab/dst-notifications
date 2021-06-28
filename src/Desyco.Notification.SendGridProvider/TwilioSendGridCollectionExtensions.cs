using System;
using Desyco.Notification;
using Desyco.Notification.SendGridProvider;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TwilioSendGridCollectionExtensions
    {
        public static NotificationOptions UseTwilioSendGridProvider(this NotificationOptions options, Action<TwilioSendGridOptions> configAction = null)
        {

            var config = new TwilioSendGridOptions();
            configAction?.Invoke(config);

            options.ConfigureProviderOptions(NotificationConst.ExternalProviderType, config);

            options.UseExternalProvider(sp => new TwilioSendGridNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>()
                ));

            return options;
        }


    }
}