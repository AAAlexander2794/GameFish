using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Annotations;

namespace WpfApp1.Models
{
    public class Fish : INotifyPropertyChanged
    {
        /// <summary>
        /// Идентификатор рыбы
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Название рыбы
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Вес рыбы
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public float Weight { get; set; }

        private ImageSource _imageSource;
        /// <summary>
        /// Источник для показа изображения
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public ImageSource ImageSource { get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            } }

        /// <summary>
        /// Описание рыбы
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        public Fish()
        {
            
        }

        public Fish(FishInfo fishInfo)
        {
            Id = fishInfo.Id;
            Name = fishInfo.Name;
            Weight = fishInfo.Weight;
            var image = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + fishInfo.ImageSourceUriString));
            ImageSource = image;
            Description = fishInfo.Description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }
    }
}
