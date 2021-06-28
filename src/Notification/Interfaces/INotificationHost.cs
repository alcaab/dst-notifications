using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface INotificationHost
    {
        void Start();
        void Stop();
        Task<string> Notify(NotificationMessage m);
        IStorageProvider StorageProvider { get; }
        ILogger Logger { get;  }
        event NotificationEventHandler OnNotifyEvent;
        NotificationOptions Options { get;  }
    }

    public delegate void NotificationEventHandler(MessageEventArgs args);
}