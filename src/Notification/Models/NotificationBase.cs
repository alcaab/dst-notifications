using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Represents a single notification
    /// </summary>
    public class NotificationBase 
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DeliveryAttempts { get; set; }
        public string TemplateKey { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public RecipientInfo Recipient { get; set; } = new RecipientInfo();
        public List<NotificationDeliveryError> Errors { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.Normal;
        public NotificationMethod NotificationMethod { get; set; } = NotificationMethod.Inherited;
        public MessageStatus Status { get; set; } = MessageStatus.None;
    }


    public class MessageContainer : Dictionary<NotificationMethod, List<NotificationBase>> 
    {
        internal void AddNotification(NotificationBase notification) 
        {
            if (!ContainsKey(notification.NotificationMethod))
                Add(notification.NotificationMethod, new List<NotificationBase>());

            this[notification.NotificationMethod].Add(notification);
        }

        internal List<NotificationBase> GetNotifications(NotificationMethod method) 
            => ContainsKey(method) ? this[method] : new List<NotificationBase>();
    }


}