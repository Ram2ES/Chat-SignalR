using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using Model;

namespace Client
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>() { new Message("sdf", "qweqweqwe") };
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

        private ServerModel _serverModel = new ServerModel();
        public string Nickname { get; set; }
        public string Text { get; set; }

        private bool _isOffline = true;
        public bool IsOffline
        {
            get => _isOffline;
            set
            {
                _isOffline = value;
                OnPropertyChanged(nameof(IsOffline));
            }
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

        public void Connect()
        {
            _serverModel.Connect();
        }

        public ClientViewModel()
        {
            _serverModel.OnConnected = OnConnected;
            _serverModel.OnSetName = OnSetName;
            _serverModel.OnGetUsers = OnGetUsers;
            _serverModel.OnReceiveMessage = OnReceiveMessage;
        }

        private void OnReceiveMessage(string name, string message)
        {
            Messages.Add(new Message(name, message));
            
        }

        private void OnGetUsers(List<string> users)
        {
            Users = new ObservableCollection<string>(users);
        }

        private void OnSetName(bool isSet)
        {
            if (isSet)
            {
                Status = "Online";
                IsOffline = false;
                _serverModel.GetUsers();
            }
            else
            {
                Status = "Человек с таким именем уже есть!";
                IsOffline = true;
                _serverModel.Disconnect();
            }
        }

        private void OnConnected()
        {
            _serverModel.SetNick(Nickname);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SendMessage()
        {
            _serverModel.SendMessage(Nickname, Text);
        }
    }
}