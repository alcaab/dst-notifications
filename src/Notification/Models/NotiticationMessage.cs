using System.Collections.Generic;
using Desyco.Notification.Models;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    /// <summary>
    /// This library was intended to be use as a mechanism to send automatic notification to user. For that reason
    /// some properties like ReplayTo, Cc, Bcc and ResentCc,  maybe won't be available in the future.
    /// </summary>
    public class PlainMessage : NotificationBase
    {
        public List<NotificationSubject> Subjects { get; set; } = new List<NotificationSubject>();
    }


    /*esta clase contiene los datos necesarios para los envios de notificaciones external (hasta ahora
     solo se ha provado con correos electronicos pero puede ser cualquier otro*/

    public class NotificationMessage : PlainMessage
    {
        public List<NotificationAddress> To { get; set; } = new List<NotificationAddress>();
        public NotificationAddress From { get; set; }
        public List<NotificationAddress> ReplyTo { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> Cc { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> Bcc { get; set; } = new List<NotificationAddress>();
        public List<NotificationAddress> ResentCc { get; set; } = new List<NotificationAddress>();
        //public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        
        public ExternalTextFormat TextFormat { get; set; }
        public List<NotificationAttachment> Attachments { get; set; } = new List<NotificationAttachment>();

        internal bool AllowSend { get; set; } = false;

    }
}