using System.Threading.Tasks;
using Microsoft.Extensions.Options;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class DefaultInternalNotificationProvider : InternalNotificationProvider
    {
        public DefaultInternalNotificationProvider(IStorageProvider storageProvider, INotificationEventHub eventHub, NotificationOptions options, ITemplateContentProvider templateContent) : base(storageProvider, eventHub, options, templateContent)
        {
        }

        protected override Task SendNotification(NotificationMessage m)
        {
            return Task.CompletedTask;
        }



    }
}