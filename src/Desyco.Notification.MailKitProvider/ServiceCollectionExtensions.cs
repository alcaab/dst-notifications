using System;
using Desyco.Notification;
using Desyco.Notification.MailKitProvider;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MailKitCollectionExtensions
    {
        public static NotificationOptions UseMailKitProvider(this NotificationOptions options, Action<MailKitOptions> configAction = null)
        {

            var config = new MailKitOptions();
            configAction?.Invoke(config);

            options.ConfigureProviderOptions(NotificationConst.ExternalProviderType, config);

            options.UseExternalProvider(sp => new MailkitNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>()
                ));

            return options;
        }


    }
}