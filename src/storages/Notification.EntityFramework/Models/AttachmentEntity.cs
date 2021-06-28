
using System;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public class AttachmentEntity
    {
        public AttachmentEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        public string MessageId { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }

        public override string ToString()
        {
            return $"{MediaType}/{MediaSubType}";
        }
    }
}