
// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Message is being sent
    /// </summary>
    public class MessageSending : MessageEventArgs
    {
        public MessageSending()
        {
            EventType = NotificationEventType.Sending;
        }
    }
}