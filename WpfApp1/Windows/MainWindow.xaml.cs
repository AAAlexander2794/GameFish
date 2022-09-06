using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using WpfApp1.Models;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ViewModel(this);
            DataContext = ViewModel;
            ViewModel.StartGameCommand.Execute(null);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ListBoxFishesItem_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var b = (FrameworkElement)sender;
            var item = b.DataContext;
            ViewModel.CurrentButton = (Button) b;
            ViewModel.CurrentFish = (Fish) item;
            Debug.WriteLine($"Mouse entered: {item}");
        }

        private void ListBoxFishesItem_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var b = (FrameworkElement)sender;
            var item = b.DataContext;
            ViewModel.CurrentButton = null;
            Debug.WriteLine($"Mouse left: {item}");
        }
    }
}
