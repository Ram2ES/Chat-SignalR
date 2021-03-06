﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public MainWindow()
        {
            InitializeComponent();
            if (DataContext is ClientViewModel vm)
                vm.MessageCollectionChanged += ScrollGrid;
        }

        private void ScrollGrid()
        {
            if (VisualTreeHelper.GetChild(MessagesList, 0) is Decorator border)
            {
                if (border.Child is ScrollViewer scroll) scroll.ScrollToEnd();
            }
        }
    }
}
