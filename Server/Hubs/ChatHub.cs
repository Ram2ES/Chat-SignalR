using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Model;

namespace Server.Hubs
{
    public class User
    {
        public string Id;
        public string Name;

        public User(string id)
        {
            Id = id;
        }
    }

    public class ChatHub : Hub
    {
        //private int i = 1;

        private static List<User> _users = new List<User>();
        public override Task OnConnectedAsync()
        {
            _users.Add(new User(Context.ConnectionId));
            Clients.Caller.SendAsync(Api.Connected);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync(Api.ReceiveMessage, user, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _users.Remove(_users.FirstOrDefault(u => u.Id == Context.ConnectionId));
            GetUsers();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetUsers()
        {
            var result = new List<string>();
            _users.ForEach(u => result.Add(u.Name));
            await Clients.All.SendAsync(Api.GetUsers, result);
        }


        public async Task SetName(string username)
        {
            if (_users.FirstOrDefault(u => u.Name == username) != null)
                await Clients.Caller.SendAsync(Api.SetName, false);
            else
            {
                var user = _users.FirstOrDefault(u => u.Id == Context.ConnectionId);
                if (user != null)
                {
                    user.Name = username;
                    await Clients.Caller.SendAsync(Api.SetName, true);
                    await Clients.All.SendAsync(Api.GetUsers, _users);
                }
            }
        }

    }
}
