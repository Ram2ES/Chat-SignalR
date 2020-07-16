using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Model;

namespace Server.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        private static readonly List<User> _users = new List<User>();

        public override async Task OnConnectedAsync()
        {
            _users.Add(new User(Context.ConnectionId));
            await Clients.Caller.Connected();
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.TransferMessage(user, message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var disconnectedUser = _users.FirstOrDefault(u => u.Id == Context.ConnectionId);

            if (disconnectedUser != null)
            {
                await Clients.All.UserDisconnected(disconnectedUser.Name);
                _users.Remove(disconnectedUser);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendUsersToCaller()
        {
            await Clients.Caller.SendAllUsers(new List<string>(_users.Select(e => e.Name)));
        }

        public async Task SetName(string username)
        {
            if (_users.Any(u => u.Name == username) || username == Constants.ServerName)
                await Clients.Caller.SetNameResult(false);
            else
            {
                var user = _users.FirstOrDefault(u => u.Id == Context.ConnectionId);
                if (user != null)
                {
                    user.Name = username;
                    await Clients.Caller.SetNameResult(true);
                    await Clients.Others.UserConnected(username);
                    await SendUsersToCaller();
                }
            }
        }

    }
}
