using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using System.Runtime.Serialization;
using System.Windows.Input;
using WpfApp1.Models;

namespace WpfApp1.Commands
{
    /// <summary>
    /// Команда, начинающая новую игру
    /// </summary>
    public class StartGameCommand : ICommand
    {
        private ViewModel ViewModel { get; }

        public StartGameCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //Обнуляем счет
            ViewModel.Score = 0;
            //
            ViewModel.FillFishes();
            //
            ViewModel.PastFishes = new ObservableCollection<Fish>();
            //
            ViewModel.SetWantedFish();
            //Вкладка "Игра"
            ViewModel.CurrentTab = 1;
            //Таймер
            ViewModel.RestartTimer();
            //
            ViewModel.MediaPlayer.Open(new Uri(@"Resources/sounds/loop_game_01.wav", UriKind.Relative));
            ViewModel.MediaPlayer.MediaEnded += (sender, args) =>
            {
                ViewModel.MediaPlayer.Position = TimeSpan.Zero;
                ViewModel.MediaPlayer.Play();
            };
            ViewModel.MediaPlayer.Play();

        }

        public event EventHandler CanExecuteChanged;
    }
}
