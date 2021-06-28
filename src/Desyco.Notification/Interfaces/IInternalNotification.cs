using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IInternalNotificationProvider 
    {
        Dictionary<string, string> Connections { get; }
        Task Notify(NotificationMessage m);

    }
}