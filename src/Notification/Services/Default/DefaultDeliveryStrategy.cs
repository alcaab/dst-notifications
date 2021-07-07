using System;
using System.Collections.Generic;
using System.Linq;

namespace Desyco.Notification.Services.Default
{
    public class DefaultDeliveryStrategy : IDeliveryStrategy
    {
        private readonly NotificationOptions _options;

        public DefaultDeliveryStrategy(NotificationOptions options)
        {
            _options = options;
        }

        public MessageContainer GetNotifications(PlainMessage msg)
        {
            var notifications = msg.Subjects.SelectMany(subject => subject.Recipients.Select(recipient =>
                new NotificationBase
                {
                    Id = msg.Id ?? Guid.NewGuid().ToString(),
                    Group = msg.Group,
                    Status = MessageStatus.Pending,
                    Recipient = new RecipientInfo
                    {
                        //Todo:Inherit method logic here
                        NotificationMethod = recipient.NotificationMethod,
                        Address = recipient.Address,
                        Name = recipient.Name,
                        UserName = recipient.UserName
                    },
                    Data = msg.Data,
                    //Todo:Inherit method logic here
                    NotificationMethod = subject.NotificationMethod,
                    TemplateKey = subject.TemplateKey,
                    CreatedDate = DateTime.UtcNow,
                    Subject = subject.Subject,
                    //Todo: body logic here
                    Body = subject.Body,
                    DeliveryAttempts = +msg.DeliveryAttempts,
                    UrgencyLevel = UrgencyLevel.Normal
                })).ToList();

            return new MessageContainer
            {
                {
                    NotificationMethod.External,
                    notifications.Where(w => w.NotificationMethod == NotificationMethod.External).ToList()
                },
                {
                    NotificationMethod.Internal,
                    notifications.Where(w => w.NotificationMethod == NotificationMethod.Internal).ToList()
                }
            };

        }


    }
}
