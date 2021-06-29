using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class DefaultStorageProvider : IStorageProvider
    {
        private readonly IMemoryStorageProvider _storageProvider;

        public DefaultStorageProvider(
            IMemoryStorageProvider storageProvider
        )
        {
            _storageProvider = storageProvider;
        }

        public Task<string> AddOrUpdateAsync(NotificationMessage m)
        {
            return _storageProvider.AddOrUpdateAsync(m);
        }

        public Task<List<NotificationMessage>> GetFailedNotificationsAsync()
        {
            return _storageProvider.GetFailedNotificationsAsync();
        }

        public Task<List<NotificationMessage>> GetNotificationsByUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsReadAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void EnsureStoreExists()
        {
       
        }

        //public Task<NotiticationServerOptions> GetServerOptions(string id = null) => _storageProvider.GetServerOptions(id);
        //public Task<string> GetTemplate(string templateKey) => _storageProvider.GetTemplate(templateKey);

    }
}