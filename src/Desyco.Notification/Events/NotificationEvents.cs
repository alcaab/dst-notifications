using System;
using System.Threading.Tasks;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class NotificationEvents
    {


        public Func<MessageStart, Task> OnNotificationStart { get; set; } = args => Task.CompletedTask;

        public Func<MessageSending, Task> OnNotificationSending { get; set; } = args => Task.CompletedTask;

        public Func<MessageFailed, Task> OnNotificationFailed { get; set; } = args => Task.CompletedTask;

        public Func<MessageDelivered, Task> OnNotificationDelivered { get; set; } = args => Task.CompletedTask;


        public virtual Task NotificationStart(MessageStart args) => OnNotificationStart(args);

        public virtual Task NotificationSending(MessageSending args) => OnNotificationSending(args);

        public virtual Task NotificationFailed(MessageFailed args) => OnNotificationFailed(args);

        public virtual Task NotificationDelivered(MessageDelivered args) => OnNotificationDelivered(args);


    }
}
