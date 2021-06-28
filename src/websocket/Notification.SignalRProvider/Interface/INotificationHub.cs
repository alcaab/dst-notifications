using System.Threading.Tasks;

namespace Desyco.Notification.SignalRProvider.Interface
{
    public interface INotificationHub
    {
        Task Notify(NotificationMessage m);
    }
}