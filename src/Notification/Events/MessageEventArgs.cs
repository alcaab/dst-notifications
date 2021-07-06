using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public abstract class MessageEventArgs
    {

        [Obsolete]
        public NotificationMessage Message { get; set; }
        public List<NotificationBase> Messages { get; set; }
        public NotificationEventType EventType { get; set; }

    }
}