using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// Represents a single notification
    /// </summary>
    public class NotificationRecipient
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.Normal;
        public NotificationMethod NotificationMethod { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.None;
        public DateTime CreatedDate { get; set; }
        public int DeliveryAttempts { get; set; }
        public string TemplateKey { get; set; }
        public RecipientInfo Recipient { get; set; } = new RecipientInfo();
        public List<NotificationDeliveryError> Errors { get; set; }

    }

    /// <summary>
    /// Represents a list of notification with determined by  a delivery method type
    /// </summary>
    public class NotificationGroup
    {
        public NotificationMethod Method { get; set; }
        public List<NotificationRecipient> Notifications { get; set; } = new List<NotificationRecipient>();
    }

    /// <summary>
    /// Represent a list of NotificationGroup
    /// </summary>
    public class NotificationGroups : List<NotificationGroup>
    {
        public List<NotificationRecipient> GetNotifications(NotificationMethod method)
       => Find(n => n.Method == method)?.Notifications ?? new List<NotificationRecipient>();
    }



}