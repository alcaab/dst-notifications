using System;
using System.Threading.Tasks;
using Desyco.Notification.SignalRProvider.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Desyco.Notification.SignalRProvider.Hub
{
    //[Authorize(Policy = "Notification")]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly IInternalNotificationProvider _socketNotificationProvider;

        public NotificationHub(IInternalNotificationProvider socketNotificationProvider)
        {
            _socketNotificationProvider = socketNotificationProvider;
        }

        public override Task OnConnectedAsync()
        {
            lock (_socketNotificationProvider.Connections)
            {
                var userName = Context.GetHttpContext().Request.Query["access_token"].ToString();
                _socketNotificationProvider.Connections
                    .Add(Context.ConnectionId, userName);
            }

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {

            lock (_socketNotificationProvider.Connections)
            {
                if (_socketNotificationProvider.Connections.ContainsKey(Context.ConnectionId))
                    _socketNotificationProvider.Connections.Remove(Context.ConnectionId);
            }

            return Task.CompletedTask;
        }

        public Task Update(NotificationMessage m)
        {
            return Task.CompletedTask;
        }

    }
}