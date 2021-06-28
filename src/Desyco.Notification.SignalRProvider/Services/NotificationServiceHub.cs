using System;
using System.Linq;
using System.Threading.Tasks;
using Desyco.Notification.SignalRProvider.Hub;
using Desyco.Notification.SignalRProvider.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Desyco.Notification.SignalRProvider.Services
{
    public class SignalRNotificationProvider : InternalNotificationProvider
    {

        private readonly GroupSender _groupSender;
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;

        public SignalRNotificationProvider(
            IStorageProvider storageProvider, 
            INotificationEventHub eventHub, 
            NotificationOptions options, 
            ITemplateContentProvider templateContent,
            IHubContext<NotificationHub, INotificationHub> notificationHub) : base(storageProvider, eventHub, options, templateContent)
        {
            _notificationHub = notificationHub;
            _groupSender = new GroupSender(_notificationHub.Groups);
        }

        protected override Task SendNotification(NotificationMessage m)
        {
            try
            {
                var connections = Connections
                    .Where(pair => pair.Value == m.To[0].UserName)
                    .Select(pair => pair.Key)
                    .ToList();

                if (!connections.Any())
                    throw new Exception("Disconnected user");

                Task.Run(async () =>
                {
                    await _groupSender.Send(connections,
                        async groupName => { await _notificationHub.Clients.Group(groupName).Notify(m); });
                });

                return Task.CompletedTask;

            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }





    }
}