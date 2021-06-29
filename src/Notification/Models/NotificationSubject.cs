using System;
using System.Collections.Generic;
using System.Text;

namespace Desyco.Notification.Models
{


    /// <summary>
    /// One message can contains many recipients all related to the same notification.
    /// for example when customer submit a service request one notification is sent to him
    /// and other different one to the agent who was automatically assigned work the request.
    /// Templatekey allows that each address can contains its own custom body but all related to the same context.
    /// </summary>
    public class NotificationSubject
    {
        public NotificationSubject()
        {

        }

        public NotificationSubject(string subject, string templateKey):this()
        {
            Subject = subject;
            TemplateKey = templateKey;
        }

        public NotificationSubject(string subject):this(subject,null)
        {

        }

        /// <summary>
        /// Indicates that each recipient message body will be assigned from template insted of Subject Body.
        /// It is only supported when TemplateKey is present
        /// </summary>
        public bool PersonalizedEachRecipient { get; set; } 
        public string Subject { get; set; }
        public string TemplateKey { get; set; }
        public string Body { get; set; }
        public NotificationMethod NotificationMethod { get; set; }
        public List<RecipientInfo> Recipients { get; set; } = new List<RecipientInfo>();

    }
}
