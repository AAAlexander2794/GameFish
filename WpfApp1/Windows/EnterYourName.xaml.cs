using System.Windows;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для EnterYourName.xaml
    /// </summary>
    public partial class EnterYourName : Window
    {
        private ViewModel ViewModel { get; set; }

        public EnterYourName(ViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveScoreList(Username.Text);
            Close();
        }
    }
}
