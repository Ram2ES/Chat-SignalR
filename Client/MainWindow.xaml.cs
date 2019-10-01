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
using Model;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientViewModel _clientVM = new ClientViewModel(); 
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _clientVM;

        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _clientVM.Connect();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           _clientVM.SendMessage();
        }
    }
}
