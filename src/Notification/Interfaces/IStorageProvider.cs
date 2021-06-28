using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IStorageProvider
    {

        /*create*/
        Task<string> AddOrUpdateAsync(NotificationMessage m);
        Task<List<NotificationMessage>> GetFailedNotificationsAsync();
        Task<List<NotificationMessage>> GetNotificationsByUserAsync(string userName);
        Task MarkAsReadAsync(string id);
        Task DeleteAsync(string id);

        void EnsureStoreExists();
    }
}