using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    /// <summary>
    /// Класс, который хранится в txt-файле
    /// </summary>
    public class ConfigInfo
    {
        public ObservableCollection<FishInfo> ArchiveFishes { get; set; }

        public int SecondsForGame { get; set; }

        public ConfigInfo()
        {
            ArchiveFishes = new ObservableCollection<FishInfo>();
            //
            SecondsForGame = 30;
        }
    }
}
