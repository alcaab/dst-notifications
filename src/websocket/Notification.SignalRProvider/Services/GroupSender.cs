using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Desyco.Notification.SignalRProvider.Services
{
    public class GroupSender
    {
        private readonly IGroupManager _manager;

        public GroupSender(IGroupManager manager)
        {
            _manager = manager;
        }

        public async Task Send(List<string> connections, Action<string> senderAction)
        {
            var groupName = Guid.NewGuid().ToString();
            foreach (var id in connections) await _manager.AddToGroupAsync(id, groupName);

            if (connections.Any())
                senderAction?.Invoke(groupName);


            foreach (var id in connections) await _manager.RemoveFromGroupAsync(id, groupName);
        }
    }
}