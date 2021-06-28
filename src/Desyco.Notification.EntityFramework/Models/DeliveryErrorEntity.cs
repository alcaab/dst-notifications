using System;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public class DeliveryErrorEntity
    {
        public DeliveryErrorEntity()
        {
            Id = Guid.NewGuid().ToString();
            ErrorTime = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public string MessageId { get; set; }
        public int DeliveryAttempts { get; set; }
        
        public DateTime ErrorTime { get; set; }
        public string Message { get; set; }
       
    }

}