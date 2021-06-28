
// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public abstract class MessageEventArgs
    {

        public NotificationMessage Message { get; set; }
        public NotificationEventType EventType { get; set; }

    }
}