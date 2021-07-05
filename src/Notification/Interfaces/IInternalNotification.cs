using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IInternalNotificationProvider 
    {
        Dictionary<string, string> Connections { get; }
        [Obsolete]
        Task Notify(NotificationMessage m);
        Task Notify(List<NotificationBase> notificacions);

    }
}