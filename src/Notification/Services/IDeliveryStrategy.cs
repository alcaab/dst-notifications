
using System.Collections.Generic;
using Desyco.Notification.Models;

namespace Desyco.Notification.Services
{
    public interface IDeliveryStrategy
    {
        NotificationContainer GetNotifications(PlainMessage message);     
    }
}
