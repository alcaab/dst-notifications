using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class DefaultExternalNotificationProvider: ExternalNotificationProvider
    {

        public DefaultExternalNotificationProvider(IStorageProvider storageProvider, INotificationEventHub eventHub, NotificationOptions options, ITemplateContentProvider templateContent, ILoggerFactory loggerFactory) : base(storageProvider, eventHub, options, templateContent)
        {
        }

        protected override Task SendNotificationAsync(NotificationMessage m)
        {
            return Task.CompletedTask;
        }
    }
}
