using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Hubs
{
    public class NotificationHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var userRole = Context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId != null && userRole != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userRole);
            }
            await base.OnConnectedAsync();
        }
    }
}
