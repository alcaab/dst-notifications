

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Message is been preparing to be sent
    /// </summary>
    public class MessageStart : MessageEventArgs
    {
        public MessageStart()
        {
            EventType = NotificationEventType.Start;
        }
    }
}