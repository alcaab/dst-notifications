using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification.Extensions;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public abstract class InternalNotificationProvider : IInternalNotificationProvider
    {
        private readonly IStorageProvider _storageProvider;
        private readonly INotificationEventHub _eventHub;
        private readonly NotificationOptions _options;
        private readonly ITemplateContentProvider _templateContent;

        protected InternalNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent)
        {
            _storageProvider = storageProvider;
            _eventHub = eventHub;
            _options = options;
            _templateContent = templateContent;
            Connections = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public Dictionary<string, string> Connections { get; }



        protected IEnumerable<NotificationMessage> GetInternalNotifications(NotificationMessage m)
        {

            foreach (var to in m.To)
            {

                var msg = m.JsonClone();

                if (string.IsNullOrEmpty(msg.Id))
                    msg.Id = Guid.NewGuid().ToString();

                msg.To.Clear();
                msg.To.Add(to);

                msg.NotificationMethod = NotificationMethod.Internal;

                if (!string.IsNullOrEmpty(to.Body))
                    msg.Body = to.Body;

                if (!string.IsNullOrEmpty(to.TemplateKey))
                    msg.TemplateKey = to.TemplateKey;

                yield return msg;

            }

        }

        public async Task Notify(NotificationMessage m)
        {
            if (m.To.Count == 0)
                throw new Exception("System user not specify");

            foreach (var msg in GetInternalNotifications(m))
            {

                msg.NotificationMethod = NotificationMethod.Internal;
                msg.AllowSend = false;
                msg.DeliveryAttempts++;
                msg.Status = MessageStatus.Pending;

                try
                {

                    if (msg.To.Count == 0)
                        throw new Exception("Email address not specify");

                    if (!string.IsNullOrEmpty(msg.TemplateKey) && string.IsNullOrEmpty(msg.Body))
                    {
                        try
                        {
                            await _templateContent.CreateMessageBody(msg);
                        }
                        catch (Exception)
                        {
                            msg.DeliveryAttempts = _options.MaxDeliveryAttempts;
                            throw;
                        }
                    }

                    await _eventHub.Emit(new MessageSending
                    {
                        Message = msg
                    });

                    await SendNotification(msg);

                    msg.DeliveryDate = DateTime.UtcNow;
                    msg.Status = MessageStatus.Delivered;

                    await _eventHub.Emit(new MessageDelivered
                    {
                        Message = msg
                    });
                }
                catch (Exception e)
                {
                    msg.Status = MessageStatus.Error;
                    msg.Errors.Add(new NotificationDeliveryError
                    {
                        Message = e.Message,
                        DeliveryAttempts = msg.Errors.Count + 1,
                        MessageId = msg.Id
                    });

                    await _eventHub.Emit(new MessageDelivered
                    {
                        Message = msg
                    });
                }

                await _storageProvider.AddOrUpdateAsync(msg);

            }


        }

        protected abstract Task SendNotification(NotificationMessage m);


    }
}