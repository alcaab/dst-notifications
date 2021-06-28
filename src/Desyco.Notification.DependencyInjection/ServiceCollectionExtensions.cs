using System;
using Desyco.Notification;
using Microsoft.AspNetCore.Builder;


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class NotificationServiceCollectionExtensions
    {

        public static IApplicationBuilder UseNotification(this IApplicationBuilder app)
        {

            var notificationHost = app.ApplicationServices.GetService<INotificationHost>();

            notificationHost.Start();
            notificationHost.OnNotifyEvent += async delegate (MessageEventArgs eventArgs)
            {
                switch (eventArgs.EventType)
                {
                    case NotificationEventType.Start:
                        await notificationHost.Options.Events.NotificationStart(eventArgs as MessageStart);
                        break;
                    case NotificationEventType.Sending:
                        await notificationHost.Options.Events.NotificationSending(eventArgs as MessageSending);
                        break;
                    case NotificationEventType.Delivered:
                        await notificationHost.Options.Events.NotificationDelivered(eventArgs as MessageDelivered);
                        break;
                    case NotificationEventType.Failed:
                        await notificationHost.Options.Events.NotificationFailed(eventArgs as MessageFailed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };



            return app;

        }




    }
}
