using System;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public class NotificationDeliveryError
    {

        public NotificationDeliveryError() 
        {
            ErrorTime = DateTime.UtcNow;
            Id = Guid.NewGuid().ToString();
        }

        public string MessageId { get; set; }
        public string Id { get; set; }
        public DateTime ErrorTime { get; set; }
        public string Message { get; set; }
        public int DeliveryAttempts { get; set; }
    }
}