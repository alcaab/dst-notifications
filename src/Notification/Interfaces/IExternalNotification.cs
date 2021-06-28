using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IExternalNotificationProvider 
    {
        Task Notify(NotificationMessage m);

    }
}