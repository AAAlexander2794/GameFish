using System;
using System.Media;
using System.Net.Mime;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfApp1.Commands
{
    /// <summary>
    /// Команда выбора определенного изображения из списка на экране (ID передается через параметр)
    /// </summary>
    public class SelectImageCommand : ICommand
    {
        private ViewModel ViewModel { get; }

        public SelectImageCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //Индекс желаемой рыбы в списке на экране
            var indexOfWantedFish = ViewModel.Fishes.IndexOf(ViewModel.WantedFish);
            //Если рыба выбрана верно
            if (ViewModel.WantedFish.Id.ToString() == parameter.ToString())
            {
                //
                ColorAnimation ca = new ColorAnimation()
                {
                    From = Colors.Black,
                    To = Colors.Green,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true
                };
                Storyboard.SetTarget(ca, ViewModel.MainWindow.tblHaul);
                Storyboard.SetTargetProperty(ca, new PropertyPath("Foreground.Color"));
                Storyboard stb = new Storyboard();
                stb.Children.Add(ca);
                stb.Begin(ViewModel.MainWindow);
                //
                ViewModel.Score += ViewModel.WantedFish.Weight;
                //
                if (ViewModel.IsSoundOn)
                {
                    MediaPlayer mp = new MediaPlayer();
                    mp.Open(new Uri(@"Resources/sounds/success.wav", UriKind.Relative));
                    mp.Play();
                }
                //
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 1.0;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                //
                myDoubleAnimation.Completed += (sender, args) =>
                {
                    //Добавляем желаемую рыбу в список пройденных
                    ViewModel.PastFishes.Add(ViewModel.WantedFish);
                    //Удаляем желаемую рыбу из списка на экране
                    ViewModel.Fishes.Remove(ViewModel.WantedFish);
                    //Добавляем новую рыбу (из архива) на место удаленной желаемой рыбы
                    ViewModel.AddFishToFishes(indexOfWantedFish);
                    //Если список рыб на экране опустел
                    if (ViewModel.Fishes.Count == 0)
                    {
                        ViewModel.EndGameCommand.Execute(null);
                        return;
                    }
                    //Задаем новую желаемую рыбу
                    ViewModel.SetWantedFish();
                };
                //
                ViewModel.MainWindow.imgBigOnListBox.BeginInit();
                ViewModel.MainWindow.imgBigOnListBox.Source = ViewModel.CurrentFish.ImageSource;
                ViewModel.MainWindow.imgBigOnListBox.EndInit();
                ViewModel.MainWindow.tblWantedFishName.Text = ViewModel.CurrentFish.Name;
                DoubleAnimation imgAnimation = new DoubleAnimation();
                imgAnimation.From = 0.0;
                imgAnimation.To = 1.0;
                imgAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                
                imgAnimation.Completed += (sender, args) =>
                {
                    ViewModel.MainWindow.panelWantedFish.Opacity = 0.0;
                    ViewModel.MainWindow.imgBigOnListBox.Source = null;
                    ViewModel.MainWindow.tblWantedFishName.Text = null;
                };
                ViewModel.MainWindow.panelWantedFish.BeginAnimation(UIElement.OpacityProperty, imgAnimation);
                //
                ViewModel.CurrentButton?.BeginAnimation(System.Windows.UIElement.OpacityProperty, myDoubleAnimation);
                ViewModel.CurrentButton = null;
            }
            //Если рыба выбрана неверно
            else
            {
                //
                if (ViewModel.IsSoundOn)
                {
                    MediaPlayer mp = new MediaPlayer();
                    mp.Open(new Uri(@"Resources/sounds/fault.wav", UriKind.Relative));
                    mp.Play();
                }
                //Анимация покраснения текста улова
                ColorAnimation ca = new ColorAnimation()
                {
                    From = Colors.Black,
                    To = Colors.Red,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true
                };
                Storyboard.SetTarget(ca, ViewModel.MainWindow.tblHaul);
                Storyboard.SetTargetProperty(ca, new PropertyPath("Foreground.Color"));
                Storyboard stb = new Storyboard();
                stb.Children.Add(ca);
                stb.Begin(ViewModel.MainWindow);
                //Отнимаем от улова вес рыбы
                ViewModel.Score -= ViewModel.WantedFish.Weight;
                if (ViewModel.Score < 0) ViewModel.Score = 0;
            }

        }

        public event EventHandler CanExecuteChanged;
    }
}
