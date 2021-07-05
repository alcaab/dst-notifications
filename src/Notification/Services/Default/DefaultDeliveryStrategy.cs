using System;
using System.Collections.Generic;
using System.Text;
using Desyco.Notification.Extensions;
using Desyco.Notification.Models;

namespace Desyco.Notification.Services.Default
{
    public class DefaultDeliveryStrategy: IDeliveryStrategy
    {
        private readonly NotificationOptions _options;

        public DefaultDeliveryStrategy(NotificationOptions options)
        {
            _options = options;
        }

        public NotificationContainer GetNotifications(PlainMessage message)
        {
            var notifications = new NotificationContainer();

            message.CopyTo(notifications);

            message.Subjects.ForEach(subject =>
            {
            });


            return notifications;
        }
    }
}
