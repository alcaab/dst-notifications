// ReSharper disable once CheckNamespace

using System;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public class NotificationAttachment
    {
        public NotificationAttachment()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;

        }

        public string Id { get; set; }
        public string MessageId { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }
        public DateTime CreatedDate { get; set; }


        public override string ToString()
        {
            return $"{MediaType}/{MediaSubType}";
        }
    }
}