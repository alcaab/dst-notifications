using System;
using Desyco.Notification;
using Desyco.Notification.MailJet;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MailJetCollectionExtensions
    {
        public static NotificationOptions UseMailJetProvider(this NotificationOptions options, Action<MailJetOptions> configAction = null)
        {

            var config = new MailJetOptions();
            configAction?.Invoke(config);

            options.ConfigureProviderOptions(NotificationConst.ExternalProviderType, config);

            options.UseExternalProvider(sp => new MailJetNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>()
                ));

            return options;
        }


    }
}