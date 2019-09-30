using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Model;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        //private int i = 1;

        private List<User> _users = new List<User>();
        public override Task OnConnectedAsync()
        {
            _users.Add(new User(Context.ConnectionId));
            Clients.Caller.SendAsync(Api.Connected, _users);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync(Api.ReceiveMessage, user, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetUsers()
        {
            await Clients.All.SendAsync(Api.GetUsers, _users);
        }

    }
}
