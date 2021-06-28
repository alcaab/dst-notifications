using System;
using System.Collections.Generic;
using System.Threading.Tasks;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class NotificationEventHub : INotificationEventHub
    {

        private readonly ICollection<Action<MessageEventArgs>> _subscriptions =
            new HashSet<Action<MessageEventArgs>>();

        public Task Emit(MessageEventArgs args)
        {
            Task.Run(() =>
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription(args);
                }
            });
            return Task.CompletedTask;
        }

        public void Subscribe(Action<MessageEventArgs> subscription)
        {
            _subscriptions.Add(subscription);
        }

        public Task Stop()
        {
            _subscriptions.Clear();
            return Task.CompletedTask;
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }
    }
}