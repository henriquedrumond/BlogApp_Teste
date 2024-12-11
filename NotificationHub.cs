﻿using Microsoft.AspNetCore.SignalR;

namespace BlogApp
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
