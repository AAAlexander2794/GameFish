using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xaml;
using WpfApp1.Annotations;
using WpfApp1.Models;

namespace WpfApp1
{
    public class Config : INotifyPropertyChanged
    {
        /// <summary>
        /// Количество секунд для игры
        /// </summary>
        public int SecondsForGame { get; private set; }

        /// <summary>
        /// Число рыб на экране
        /// </summary>
        public int NumberOfFishesOnScreen { get; private set; }

        //public static string SavedScoresBasedText { get; }

        /// <summary>
        /// Коллекция всех рыб
        /// </summary>
        /// <value>
        /// The archive fishes.
        /// </value>
        public ObservableCollection<Fish> ArchiveFishes { get; private set; }

        public bool SoundOn { get; set; }

        public Config()
        {
            //
            SoundOn = true;
            //
            SecondsForGame = 30;
            //
            NumberOfFishesOnScreen = 9;
            //Создаем архив рыб
            LoadArchiveFishes();
        }

        /// <summary>
        /// Создает архив рыб
        /// </summary>
        private void LoadArchiveFishes()
        {
            var configInfo = new ConfigInfo();
            
            if (!File.Exists(@"ArchiveFishes.txt"))
            {
                XamlServices.Save(@"ArchiveFishes.txt", configInfo);
            }

            configInfo = (ConfigInfo) XamlServices.Load(@"ArchiveFishes.txt");

            if (configInfo.ArchiveFishes.Count == 0)
            {
                configInfo.ArchiveFishes = GetFishInfosTemplate();
                XamlServices.Save(@"ArchiveFishes.txt", configInfo);
            }

            ArchiveFishes = new ObservableCollection<Fish>();
            foreach (var fishInfo in configInfo.ArchiveFishes)
            {
                ArchiveFishes.Add(new Fish(fishInfo));
            }

            SecondsForGame = configInfo.SecondsForGame;
        }

        private ObservableCollection<FishInfo> GetFishInfosTemplate()
        {
            return new ObservableCollection<FishInfo>()
            {
                new FishInfo()
                {
                    Id = 1,
                    Name = "Рыба 1",
                    Weight = 1.5f,
                    ImageSourceUriString = @"/Resources/images/fish_01.jpg",
                    Description = "Это число 1"
                },
                new FishInfo()
                {
                    Id = 2,
                    Name = "Рыба 2",
                    Weight = 2f,
                    ImageSourceUriString = @"/Resources/images/fish_02.jpg",
                    Description = "Это число 2"
                },
                new FishInfo()
                {
                    Id = 3,
                    Name = "Рыба 3",
                    Weight = 0.7f,
                    ImageSourceUriString = @"/Resources/images/fish_03.jpg",
                    Description = "Это число 3"
                },
                new FishInfo()
                {
                    Id = 4,
                    Name = "Рыба 4",
                    Weight = 1.1f,
                    ImageSourceUriString = @"/Resources/images/fish_04.jpg",
                    Description = "Это число 4"
                },
                new FishInfo()
                {
                    Id = 5,
                    Name = "Рыба 5",
                    Weight = 0.9f,
                    ImageSourceUriString = @"/Resources/images/fish_05.jpg",
                    Description = "Это число 5"
                },
                new FishInfo()
                {
                    Id = 6,
                    Name = "Рыба 6",
                    Weight = 0.5f,
                    ImageSourceUriString = @"/Resources/images/fish_06.jpg",
                    Description = "Это число 6"
                },
                new FishInfo()
                {
                    Id = 7,
                    Name = "Рыба 7",
                    Weight = 1.2f,
                    ImageSourceUriString = @"/Resources/images/fish_07.jpg",
                    Description = "Это число 7"
                },
                new FishInfo()
                {
                    Id = 8,
                    Name = "Рыба 8",
                    Weight = 0.8f,
                    ImageSourceUriString = @"/Resources/images/fish_08.jpg",
                    Description = "Это число 8"
                },
                new FishInfo()
                {
                    Id = 9,
                    Name = "Рыба 9",
                    Weight = 1f,
                    ImageSourceUriString = @"/Resources/images/fish_09.jpg",
                    Description = "Это число 9"
                },
                new FishInfo()
                {
                    Id = 10,
                    Name = "Рыба 10",
                    Weight = 1.5f,
                    ImageSourceUriString = @"/Resources/images/fish_10.jpg",
                    Description = "Это число 10"
                },
                new FishInfo()
                {
                    Id = 11,
                    Name = "Рыба 11",
                    Weight = 2f,
                    ImageSourceUriString = @"/Resources/images/fish_11.jpg",
                    Description = "Это число 11"
                },
                new FishInfo()
                {
                    Id = 12,
                    Name = "Рыба 12",
                    Weight = 0.7f,
                    ImageSourceUriString = @"/Resources/images/fish_12.jpg",
                    Description = "Это число 12"
                },
                new FishInfo()
                {
                    Id = 13,
                    Name = "Рыба 13",
                    Weight = 1.1f,
                    ImageSourceUriString = @"/Resources/images/fish_13.jpg",
                    Description = "Это число 13"
                },
                new FishInfo()
                {
                    Id = 14,
                    Name = "Рыба 14",
                    Weight = 0.9f,
                    ImageSourceUriString = @"/Resources/images/fish_14.jpg",
                    Description = "Это число 14"
                },
                new FishInfo()
                {
                    Id = 15,
                    Name = "Рыба 15",
                    Weight = 0.5f,
                    ImageSourceUriString = @"/Resources/images/fish_15.jpg",
                    Description = "Это число 15"
                },
                new FishInfo()
                {
                    Id = 16,
                    Name = "Рыба 16",
                    Weight = 1.2f,
                    ImageSourceUriString = @"/Resources/images/fish_16.jpg",
                    Description = "Это число 16"
                },
                new FishInfo()
                {
                    Id = 17,
                    Name = "Рыба 17",
                    Weight = 0.8f,
                    ImageSourceUriString = @"/Resources/images/fish_17.jpg",
                    Description = "Это число 17"
                },
                new FishInfo()
                {
                    Id = 18,
                    Name = "Рыба 18",
                    Weight = 1f,
                    ImageSourceUriString = @"/Resources/images/fish_18.jpg",
                    Description = "Это число 18"
                },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
