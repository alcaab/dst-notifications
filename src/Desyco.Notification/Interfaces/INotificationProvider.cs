using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{



    public interface INotificationProvider
    {

        Task Notify(NotificationMessage m);

    }
}