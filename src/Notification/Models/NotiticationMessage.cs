using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    /// <summary>
    /// This library was intended to be use as a mechanism to send automatic notification to user. For that reason
    /// some properties like ReplayTo, Cc, Bcc and ResentCc,  maybe won't be available in the future.
    /// </summary>
    public class PlainMessage
    {

        public string TemplateKey { get; set; }
        public string Id { get; set; }
        public string Group { get; set; }
        public string Subject { get; set; }
        public List<NotificationAddress> To { get; set; } = new List<NotificationAddress>();
        public NotificationAddress From { get; set; }

        public List<NotificationAddress> ReplyTo { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> Cc { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> Bcc { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> ResentCc { get; set; } = new List<NotificationAddress>();
        public string Body { get; set; } 
        public DateTime? DeliveryDate { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.Normal;
        public NotificationMethod NotificationMethod { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.None;
        public DateTime CreatedDate { get; set; }
        public int DeliveryAttempts { get; set; }

    }


    /*esta clase contiene los datos necesarios para los envios de notificaciones external (hasta ahora
     solo se ha provado con correos electronicos pero puede ser cualquier otro*/

    public class NotificationMessage : PlainMessage
    {
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public List<NotificationDeliveryError> Errors { get; set; } = new List<NotificationDeliveryError>();
        public ExternalTextFormat TextFormat { get; set; }
        public List<NotificationAttachment> Attachments { get; set; } = new List<NotificationAttachment>();

        internal bool AllowSend { get; set; } = false;

    }
}