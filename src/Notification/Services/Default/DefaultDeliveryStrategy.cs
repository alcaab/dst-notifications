using System;
using System.Collections.Generic;
using System.Text;
using Desyco.Notification.Extensions;
using Desyco.Notification.Models;

namespace Desyco.Notification.Services.Default
{
    public class DefaultDeliveryStrategy : IDeliveryStrategy
    {
        private readonly NotificationOptions _options;

        public DefaultDeliveryStrategy(NotificationOptions options)
        {
            _options = options;
        }

        public MessageContainer GetNotifications(PlainMessage message)
        {

            return message.CreateContainer(msg =>
            {
                if (string.IsNullOrEmpty(msg.Id))
                    msg.Id = Guid.NewGuid().ToString();

                msg.DeliveryAttempts++;
                msg.Status = MessageStatus.Pending;
            });

        }
    }
}
