using System.Collections.Generic;

namespace Desyco.Notification.Services
{
    public interface IDeliveryStrategy
    {
        MessageContainer GetNotifications(PlainMessage msg);     
    }
}
