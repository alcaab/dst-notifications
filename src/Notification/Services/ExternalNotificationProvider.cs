using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification.Extensions;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public abstract class ExternalNotificationProvider : IExternalNotificationProvider
    {
        private readonly IStorageProvider _storageProvider;
        private readonly INotificationEventHub _eventHub;
        private readonly NotificationOptions _options;
        private readonly ITemplateContentProvider _templateContent;
        private IExternalNotificationProvider _externalNotificationProviderImplementation;

        protected ExternalNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent)
        {
            _storageProvider = storageProvider;
            _eventHub = eventHub;
            _options = options;
            _templateContent = templateContent;
        }


        public virtual async Task Notify(NotificationMessage m)
        {
            if (m.To.Count == 0)
                throw new Exception("System user not specify");

            var msg = m.JsonClone();
            /*
             TODO:Buscar una manera mas efectiva para serializar ya que los tipos se pierden
             pasando a convertirse en JObject
            */
            msg.Data = m.Data;
            if (string.IsNullOrEmpty(msg.Id))
                msg.Id = Guid.NewGuid().ToString();

            msg.NotificationMethod = NotificationMethod.External;
            msg.AllowSend = false;
            msg.DeliveryAttempts++;
            msg.Status = MessageStatus.Pending;

            try
            {
                if (!string.IsNullOrEmpty(msg.TemplateKey) && string.IsNullOrEmpty(msg.Body))
                {
                    try
                    {
                        await _templateContent.CreateMessageBody(msg);
                    }
                    catch 
                    {
                        msg.DeliveryAttempts = _options.MaxDeliveryAttempts;
                        throw;
                    }
                }

                await _eventHub.Emit(new MessageSending
                {
                    Message = msg
                });

                await SendNotificationAsync(msg);

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
                await _eventHub.Emit(new MessageFailed
                {
                    Message = msg
                });
            }

            await _storageProvider.AddOrUpdateAsync(msg);

        }

        public Task Notify(List<NotificationBase> notifications)
        {
            return  Task.CompletedTask;
        }

        protected abstract Task SendNotificationAsync(NotificationMessage m);

    }


}