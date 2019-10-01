using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR.Client;
using Model;

namespace Client
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public HubConnection Connection;
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        private ObservableCollection<string> _users;
        public ObservableCollection<string> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public string Nickname { get; set; }

        private string _text = "";
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private bool _isOffline = true;
        public bool IsOffline
        {
            get => _isOffline;
            set
            {
                _isOffline = value;
                OnPropertyChanged(nameof(IsOffline));
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        public bool IsOnline
        {
            get => !_isOffline;
        }

        private string _status = "Offline";
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private Brush _statusColor = Brushes.Black;
        public Brush StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        private string _connectionButtonText = "Подключиться";

        public string ConnectionButtonText
        {
            get => _connectionButtonText;
            private set
            {
                _connectionButtonText = value;
                OnPropertyChanged(nameof(ConnectionButtonText));
            }
        }



        public async void OnConnectionButton()
        {
            if (IsOnline) Disconnect();
            else Connect();
        }

        public async void Connect()
        {
            Connection = new HubConnectionBuilder().WithUrl(SelectedServer).Build();
            Connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                await Connection.StartAsync();
            };
            Connection.On(Api.Connected, OnConnected);
            Connection.On<string, string>(Api.ReceiveMessage, OnReceiveMessage);
            Connection.On<List<string>>(Api.GetUsers, OnGetUsers);
            Connection.On<bool>(Api.SetName, OnSetName);

            try
            {
                await Connection.StartAsync();
            }
            catch (Exception ex)
            {
                OnReceiveMessage("Server", ex.ToString()); //messagesList.Items.Add(ex.Message);
            }
        }

        public async void Disconnect()
        {
            ConnectionButtonText = "Подключиться";
            IsOffline = true;
            Status = "Offline";
            StatusColor = Brushes.Black;
            Messages.Clear();
            try
            {
                await Connection.DisposeAsync();
            }
            catch (Exception e)
            {
                OnReceiveMessage("Server", e.Message);
            }

        }

        private List<string> _serverList;
        public List<string> ServerList
        {
            get { return _serverList; }
            set
            {
                _serverList = value;
                OnPropertyChanged(nameof(ServerList));
            }
        }
        public string SelectedServer { get; set; }

        public ClientViewModel()
        {
            XmlWorker qwe = new XmlWorker();
            qwe.Execute();
            ServerList = qwe.ReadDocument();
        }

        private void OnReceiveMessage(string name, string message)
        {
            Messages.Add(new Message(name, message));
        }

        private void OnGetUsers(List<string> users)
        {
            Users = new ObservableCollection<string>(users);
        }

        private async void OnSetName(bool isSet)
        {
            if (isSet)
            {
                Status = "Online";
                StatusColor = Brushes.LimeGreen;
                IsOffline = false;
                await Connection.InvokeAsync(Api.GetUsers);
            }
            else
            {
                Status = "Человек с таким именем уже есть!";
                StatusColor = Brushes.Red;
                IsOffline = true;
                Disconnect();
            }
        }

        private async void OnConnected()
        {
            ConnectionButtonText = "Отключиться";
            try
            {
                await Connection.InvokeCoreAsync<string>(Api.SetName, new[] { Nickname });
            }
            catch (Exception e)
            {
                OnReceiveMessage("Server", e.Message);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void SendMessage()
        {
            try
            {
                var txt = string.Copy(Text);
                await Connection.SendCoreAsync(Api.SendMessage, new[] { Nickname, txt });
                Text = "";
            }
            catch (Exception e)
            {
                OnReceiveMessage("Server", e.Message);
            }
        }
    }
}