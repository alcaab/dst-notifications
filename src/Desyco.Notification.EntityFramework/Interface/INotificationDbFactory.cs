

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public interface INotificationDbFactory
    {

        NotificationDbContext Build();
    }
}
