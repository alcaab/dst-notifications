using System;
using System.Collections.Generic;
using System.Text;
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

        public List<NotificationSubject> GetSubjects(NotificationMessage message)
        {
            return new List<NotificationSubject>();
        }
    }
}
