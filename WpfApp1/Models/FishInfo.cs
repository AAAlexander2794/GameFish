using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    /// <summary>
    /// Информация о рыбе, которую можно хранить в текстовом файле
    /// </summary>
    public class FishInfo
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

        /// <summary>
        /// Путь к картинке
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public string ImageSourceUriString { get; set; }

        /// <summary>
        /// Описание рыбы
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        public FishInfo()
        {
            
        }

        public FishInfo(Fish fish)
        {
            Id = fish.Id;
            Name = fish.Name;
            Weight = fish.Weight;
            ImageSourceUriString = ((System.Windows.Media.Imaging.BitmapImage)fish.ImageSource).UriSource.ToString();
            Description = fish.Description;
        }
    }
}
