using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MessageEntity 
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };

        private string _to { get; set; }
        private string _from { get; set; }
        private string _data { get; set; }
        private string _replyTo { get; set; }

        public string TemplateKey { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.Normal;

        public NotificationMethod NotificationMethod { get; set; }

        public ExternalTextFormat TextFormat { get; set; }

        public string Id { get; set; }
        public string Group { get; set; }

        public string Subject { get; set; }
        

        //TODO: use converter insted
        public List<NotificationAddress> To
        {
            get => JsonConvert.DeserializeObject<List<NotificationAddress>>(_to ?? "", SerializerSettings);
            set => _to = JsonConvert.SerializeObject(value);
        }

        //TODO: use converter insted
        public NotificationAddress From
        {
            get => JsonConvert.DeserializeObject<NotificationAddress>(_from ?? "", SerializerSettings);
            set => _from = JsonConvert.SerializeObject(value);
        }

        //TODO: use converter insted
        public List<NotificationAddress> ReplyTo
        {
            get => JsonConvert.DeserializeObject<List<NotificationAddress>>(_replyTo ?? "", SerializerSettings);
            set => _replyTo = JsonConvert.SerializeObject(value);
        }

        public string Body { get; set; }

        public MessageStatus Status { get; set; }

        public int DeliveryAttempts { get; set; }

        public DateTime CreatedDate { get; set; }

        //TODO: use converter insted
        public Dictionary<string, object> Data
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, object>>(_data ?? "", SerializerSettings);
            set => _data = JsonConvert.SerializeObject(value);
        }

        public ICollection<DeliveryErrorEntity> Errors { get; set; }
        public ICollection<AttachmentEntity> Attachments { get; set; }

    }
}