using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desyco.Notification.Models;
using Newtonsoft.Json;

namespace Desyco.Notification.Extensions
{
    public static class Extensions
    {

        public static async Task CreateMessageBody(this ITemplateContentProvider source, NotificationMessage m)
        {
            /*
             * Serialization is causing an issue because property Data which is a Dictionary<string,object> object
             * that is loosing its type and insted all objects types are turned to JObject.
             * 
             */
            var data = m.Data/*.JsonClone()*/;
            data.Add("message", m);
            m.Body = await source.GetTemplateContent(m.TemplateKey, data);
        }

        public static T JsonClone<T>(this T source) where T : class
        {
            if (source == null) return default;
            var data = JsonConvert.SerializeObject(source, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return JsonConvert.DeserializeObject<T>(data);

        }

        /// <summary>
        /// Deep clone notifications into container
        /// </summary>
        /// <param name="message"></param>
        /// <param name="container"></param>
        public static void CopyTo(this PlainMessage message, NotificationContainer container)
        {

            foreach (var notification in from subject in message.Subjects from recipient in subject.Recipients select new NotificationBase
            {
                Id = Guid.NewGuid().ToString(),
                Group = message.Group,
                Status = MessageStatus.Pending,
                Recipient = new RecipientInfo
                {
                    //Todo:Inherit method logic here
                    NotificationMethod = recipient.NotificationMethod,
                    Address = recipient.Address,
                    Name = recipient.Name,
                    UserName = recipient.UserName
                },
                //Todo:Inherit method logic here
                NotificationMethod = subject.NotificationMethod,
                TemplateKey = subject.TemplateKey,
                CreatedDate = DateTime.UtcNow,
                Subject = subject.Subject,
                //Todo: body logic here
                Body = subject.Body,
                DeliveryAttempts = 0,
                UrgencyLevel = UrgencyLevel.Normal,
            })
            {
                container.AddNotification(notification);
            }
        }
    }
}