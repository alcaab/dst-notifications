

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Message was successfully delivered
    /// </summary>
    public class MessageDelivered : MessageEventArgs
    {
        public MessageDelivered()
        {
            EventType = NotificationEventType.Delivered;
        }
    }
}