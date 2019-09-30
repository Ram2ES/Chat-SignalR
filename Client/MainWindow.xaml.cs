using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public ServerModel Server;
        public Model Model;
        public MainWindow()
        {
            InitializeComponent();
            Model = new Model("asdasd");
        }

        public void SetMethods()
        {
//            Server.OnConnected = connectionId =>
//            {
//                Dispatcher?.BeginInvoke(new Action(() => { tbMain.Content = connectionId; }));
//            };

            //Server.OnReceiveMessage = (user, message) => { Messages.Add($"{user} сказал {message}"); };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SetMethods();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.InvokeCoreAsync("SendMessage", new []{NickBox.Text, MessageBox.Text});

                Console.WriteLine(MessageBox.Text);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
