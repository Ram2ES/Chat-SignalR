using System;
using System.Collections.Generic;
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
        private HubConnection _connection;
        private string uri;
        public MainWindow()
        {
            InitializeComponent();
            uri = "http://localhost:63847" + "/chathub";
            _connection = new HubConnectionBuilder().WithUrl(uri).Build();
            _connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                await _connection.StartAsync();
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _connection.On<string>("Connected", connectionId =>
                {
                    Dispatcher?.BeginInvoke(new Action(() => { tbMain.Content = connectionId; }));
                });
            _connection.On<string>("Posted", value =>
            {
                Dispatcher?.BeginInvoke(new Action(() =>
                {
                    messagesList.Items.Add(value);
                }));
            });

            _connection.On<string, string>("ReceiveMessage", (user, message) => { messagesList.Items.Add($"{user} сказал {message}"); });
            try
            {
                await _connection.StartAsync();
                messagesList.Items.Add("ConnectionStarted");
                //enablebutton = false;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.SendAsync("ReceiveMessage", "TestNick", MessageBox.Text);
                Console.WriteLine(MessageBox.Text);
                //messagesList.Items.Add("ConnectionStarted");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
