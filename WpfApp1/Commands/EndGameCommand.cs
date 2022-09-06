using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1.Commands
{
    /// <summary>
    /// Команда окончания игры
    /// </summary>
    public class EndGameCommand : ICommand
    {
        private ViewModel ViewModel { get; }

        public EndGameCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.StopTimer();
            //Вкладка "Меню"
            ViewModel.CurrentTab = 0;
            //Остановить музыку
            ViewModel.MediaPlayer.Stop();
        }

        public event EventHandler CanExecuteChanged;
    }
}
