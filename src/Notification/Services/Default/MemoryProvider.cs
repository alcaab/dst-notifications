using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public class MemoryProvider : IMemoryStorageProvider
    {
        private readonly ILogger _logger;
        private readonly NotificationOptions _options;

        private readonly List<NotificationMessage> _messages;


        public MemoryProvider(NotificationOptions  options, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MemoryProvider>();
            _options = options;
            _messages = new List<NotificationMessage>();
        }
        public  Task<string> AddOrUpdateAsync(NotificationMessage m)
        {
            lock (_messages)
            {
                var exist = _messages.FirstOrDefault(x => x.Id == m.Id);
                if(exist != null)
                    _messages.Remove(exist);
                
                _messages.Add(m);
                return Task.FromResult(m.Id);
            }
        }

        public Task<List<NotificationMessage>> GetFailedNotificationsAsync()
        {
            var data = _messages
                .Where(w => w.Status == MessageStatus.Error && (w.DeliveryAttempts < _options.MaxDeliveryAttempts || _options.MaxDeliveryAttempts ==0))
                .ToList();

            return Task.FromResult(data);
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

        public  Task<NotificationMessage> GetNotification(string id)
        {
            lock (_messages)
            {
                return Task.FromResult(_messages.First(f => f.Id == id)) ;
            }
        }

        public void EnsureStoreExists()
        {
            throw new NotImplementedException();
        }

    }
}