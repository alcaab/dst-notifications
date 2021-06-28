using System;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Message was successfully delivered
    /// </summary>
    public class MessageFailed : MessageEventArgs
    {
        public MessageFailed()
        {
            EventType = NotificationEventType.Failed;
        }

        public Exception Exception { get; set; }
    }
}