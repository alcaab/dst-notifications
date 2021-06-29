
using System.Collections.Generic;
using Desyco.Notification.Models;

namespace Desyco.Notification.Services
{
    public interface IDeliveryStrategy
    {
        List<NotificationSubject> GetSubjects(NotificationMessage message);  
    }
}
