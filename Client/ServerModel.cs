using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public Action OnConnected { private get; set; }
        public Action<string,string> OnReceiveMessage { private get; set; }
        public Action<List<string>> OnGetUsers { private get; set; }
        public Action<bool> OnSetName { private get; set; }

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

        public async void Connect()
        {
            
            _connection.On(Api.Connected, () => { OnConnected(); });
            _connection.On<string, string>(Api.ReceiveMessage, (user, message) => OnReceiveMessage(user, message));
            _connection.On<List<string>>(Api.GetUsers, (users) => OnGetUsers(users));
            _connection.On<bool>(Api.SetName, (isGood) => OnSetName(isGood));
            
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                OnReceiveMessage("Server", ex.ToString()); //messagesList.Items.Add(ex.Message);
            }
        }

        public async void GetUsers()
        {
            await _connection.InvokeAsync(Api.GetUsers);
        }

        public async void SetNick(string name)
        {
            try
            {
                await _connection.InvokeCoreAsync<string>(Api.SetName, new[] { name });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public async void Disconnect()
        {
            await _connection.DisposeAsync();
        }

        public async void SendMessage(string name, string text)
        {
            await _connection.SendCoreAsync(Api.SendMessage, new[] {name, text});
        }
    }

}