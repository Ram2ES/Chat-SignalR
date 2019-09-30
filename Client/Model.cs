using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR.Client;
using Model;


namespace Client
{
    public class ServerModel
    {
        private HubConnection _connection;
        private string _uri;
        public Action<List<User>> OnConnected;
        public Action<string,string> OnReceiveMessage;
        public ServerModel()
        {
            _uri = "http://localhost:63847" + "/chathub";
            _connection = new HubConnectionBuilder().WithUrl(_uri).Build();
            _connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                await _connection.StartAsync();
            };
        }

        public async void Connect(string name)
        {
            
            _connection.On<List<User>>(Api.Connected, (users) => { OnConnected(users); });
            _connection.On<string, string>(Api.ReceiveMessage, (user, message) => OnReceiveMessage(user, message));
            //_connection.On<string>("ReceiveMessage", (user, message) => OnReceiveMessage(user, message));


            try
            {
                await _connection.StartAsync();
                await _connection.InvokeCoreAsync<string>(Api.SetName, new []{name});

            }
            catch (Exception ex)
            {
                OnReceiveMessage("Server", ex.ToString()); //messagesList.Items.Add(ex.Message);
            }
        }

        public async List<User> GetUsers()
        {
            return await _connection.InvokeCoreAsync(Api.)
        }
    }
    public class ClientModel
    {
        public List<User> Users;
        public List<Message> Messages = new List<Message>();
        public ServerModel Server;
        public ClientModel(string xmlPath)
        {
            Server = new ServerModel();
            Users = Server.GetUsers();
        }
    }
}