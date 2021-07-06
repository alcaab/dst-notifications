using System;
using System.Threading.Tasks;
using Desyco.Notification.Services;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly INotificationEventHub _eventHub;
        private readonly IExternalNotificationProvider _externalNotificationProvider;
        private readonly IDeliveryStrategy _deliveryStrategy;
        private readonly NotificationOptions _options;
        private readonly IInternalNotificationProvider _internalNotificationProvider;
        private readonly ILogger _logger;

        public NotificationProvider(
            NotificationOptions options,
            INotificationEventHub eventHub,
            IInternalNotificationProvider internalNotificationProvider,
            IExternalNotificationProvider externalNotificationProvider,
            ILoggerFactory loggerFactory,
            IDeliveryStrategy deliveryStrategy
        )
        {
            _options = options;
            _eventHub = eventHub;
            _internalNotificationProvider = internalNotificationProvider;
            _externalNotificationProvider = externalNotificationProvider;
            _deliveryStrategy = deliveryStrategy;
            _logger = loggerFactory.CreateLogger<NotificationProvider>();

        }


        public async Task Notify(NotificationMessage m)
        {
            m.Status = MessageStatus.None;

            try
            {
                if (!m.AllowSend)
                {
                    throw new Exception("Invalid operation, use HostService to send messages");
                }

                await _eventHub.Emit(new MessageStart
                {
                    Message = m
                });

                //Get all notifications
                var container = _deliveryStrategy.GetNotifications(m);

                //envia la notificacion por websocket
                if (m.NotificationMethod != NotificationMethod.External)
                    await _internalNotificationProvider.Notify(container.GetNotifications(NotificationMethod.External));

                //await _internalNotificationProvider.Notify(m);

                //Enviar mensaje fuera del sistema via email o cualquier otro medio. 
                if (m.NotificationMethod != NotificationMethod.Internal)
                    await _externalNotificationProvider.Notify(container.GetNotifications(NotificationMethod.Internal));

                    //await _externalNotificationProvider.Notify(m);
                    

            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                //error no manejado
                await _eventHub.Emit(new MessageFailed
                {
                    Message = m,
                    Exception = e
                });

            }

        }


        protected virtual bool SupportExternalAttachment()
        {
            return true;
        }


    }
}