using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IExternalNotificationProvider 
    {
        [Obsolete()]
        Task Notify(NotificationMessage m);

        Task Notify(List<NotificationBase> notifications);

    }
}