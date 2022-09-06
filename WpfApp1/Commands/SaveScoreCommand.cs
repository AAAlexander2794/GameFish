using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1.Commands
{
    /// <summary>
    /// Команда окончания игры
    /// </summary>
    public class SaveScoreCommand : ICommand
    {
        private ViewModel ViewModel { get; }

        public SaveScoreCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Windows.EnterYourName window = new Windows.EnterYourName(ViewModel);
            window.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}
