using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xaml;
using WpfApp1.Annotations;
using WpfApp1.Commands;
using WpfApp1.Models;
using WpfApp1.Windows;

namespace WpfApp1
{
    public class ViewModel : INotifyPropertyChanged
    {
        public MediaPlayer MediaPlayer { get; }

        private bool _isSoundOn;

        public bool IsSoundOn
        {
            get => _isSoundOn;
            set
            {
                _isSoundOn = value;
                if (!value) MediaPlayer.Stop();
                OnPropertyChanged();
            }
        }

        public Config Config { get; }

        public MainWindow MainWindow { get; }

        /// <summary>
        /// Объект для генерации случайных чисел
        /// </summary>
        private static readonly Random RandObj = new Random();

        private ObservableCollection<ScoreRecord> _scoreList;
        /// <summary>
        /// Список результатов
        /// </summary>
        /// <value>
        /// The score list.
        /// </value>
        public ObservableCollection<ScoreRecord> ScoreList { get=>_scoreList;
            set
            {
                _scoreList = value;
                OnPropertyChanged();
            }
        }

        private int _currentTab;
        /// <summary>
        /// Текущая вкладка ("Меню" или "Игра")
        /// </summary>
        /// <value>
        /// The current tab.
        /// </value>
        public int CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Рыбы, которые уже были пройдены в текущей игре
        /// </summary>
        /// <value>
        /// The past fishes.
        /// </value>
        public ObservableCollection<Fish> PastFishes { get; set; }

        private ObservableCollection<Fish> _fishes;
        /// <summary>
        /// Коллекция рыб, включающая только те рыбы, которые выводятся на экран
        /// </summary>
        /// <value>
        /// The fishes.
        /// </value>
        public ObservableCollection<Fish> Fishes { get=>_fishes;
            set
            {
                _fishes = value;
                OnPropertyChanged();
            } }

        private Fish _wantedFish;
        /// <summary>
        /// Искомая рыба
        /// </summary>
        /// <value>
        /// The wanted fish.
        /// </value>
        public Fish WantedFish { get=>_wantedFish;
            set
            {
                _wantedFish = value;
                OnPropertyChanged();
            } }
        
        private float _score;
        /// <summary>
        /// Счет (выражается в весе улова)
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public float Score { get=>_score;
            set
            {
                _score = value;
                OnPropertyChanged();
            } }

        private DispatcherTimer _timer;
        private TimeSpan _time;
        /// <summary>
        /// Оставшееся время, отображающееся на экране
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public TimeSpan Time { get=>_time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public Fish CurrentFish { get; set; }
        /// <summary>
        /// Кнопка, в данный находящаяся в фокусе (в UI)
        /// </summary>
        /// <value>
        /// The current button.
        /// </value>
        public Button CurrentButton { get; set; }
        /// <summary>
        /// Кнопка, на которой изображена желаемая рыба
        /// </summary>
        /// <value>
        /// The wanted button.
        /// </value>
        public Button WantedButton { get; set; }

        public StartGameCommand StartGameCommand { get; }
        public SelectImageCommand SelectImageCommand { get; }
        public EndGameCommand EndGameCommand { get; }
        public SaveScoreCommand SaveScoreCommand { get; }

        /// <summary>
        /// Конструктор
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        public ViewModel(MainWindow mainWindow)
        {
            //
            MediaPlayer = new MediaPlayer();
            Config = new Config();
            MainWindow = mainWindow;
            //
            IsSoundOn = Config.SoundOn;
            //
            StartGameCommand = new StartGameCommand(this);
            SelectImageCommand = new SelectImageCommand(this);
            EndGameCommand = new EndGameCommand(this);
            SaveScoreCommand = new SaveScoreCommand(this);
            //Вкладка "Меню"
            CurrentTab = 0;
            //
            LoadScoreList();
        }

        /// <summary>
        /// Перезапускает таймер
        /// </summary>
        public void RestartTimer()
        {
           StopTimer();
           Time = TimeSpan.FromSeconds(Config.SecondsForGame);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {

                if (Time == TimeSpan.Zero)
                {
                    EndGameCommand.Execute(null);
                    StopTimer();
                }
                Time = Time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            _timer.Start();
        }

        /// <summary>
        /// Останавливает таймер
        /// </summary>
        public void StopTimer()
        {
            _timer?.Stop();
        }

        /// <summary>
        /// Заполняет коллекцию рыб (для экрана)
        /// </summary>
        /// <remarks>
        /// Все касты long в int и обратно из-за того, что импортированный метод случайной выборки 
        /// работает с long, а здесь у нас все в int
        /// </remarks>
        public void FillFishes()
        {
            //Формируем массив порядковых номеров архива рыб
            long[] serialNumbers = new long[Config.ArchiveFishes.Count];
            for (int i = 0; i < serialNumbers.Length; i++)
            {
                serialNumbers[i] = i;
            }
            //Делаем случайную выборку порядковых номеров
            var selectedSerialNumbers = RandomUniqueSelect(serialNumbers, Config.NumberOfFishesOnScreen);
            //По случайной выборке формируем коллекцию рыб на экран
            Fishes = new ObservableCollection<Fish>();
            foreach (var l in selectedSerialNumbers)
            {
                var serialNumber = (int) l;
                Fishes.Add(Config.ArchiveFishes[serialNumber]);
            }
        }

        /// <summary>
        /// Добавляет одну рыбу к коллекции <see cref="Fishes"/> из коллекции <see cref="Config.ArchiveFishes"/>, 
        /// которая еще не была задействована в текущей игре
        /// </summary>
        public void AddFishToFishes(int place = 0)
        {
            long[] fishIds = Config.ArchiveFishes.Where(f => !Fishes.Contains(f) && !PastFishes.Contains(f))
                .Select(f => long.Parse(f.Id.ToString())).ToArray();
            //Если больше нет рыб для добавления
            if (fishIds.Length == 0) return;
            //
            var newFishId = (int) fishIds[RandObj.Next(0, fishIds.Length)];
            if (place == 0)
            {
                Fishes.Add(Config.ArchiveFishes.First(f => f.Id == newFishId));
            }
            else
            {
                Fishes.Insert(place, Config.ArchiveFishes.First(f => f.Id == newFishId));
            }
        }

        /// <summary>
        /// Задает искомую рыбу
        /// </summary>
        public void SetWantedFish()
        {
            var serialNumber = RandObj.Next(0, Fishes.Count);
            WantedFish = Fishes[serialNumber];
        }

        /// <summary>
        /// Случайный выбор m несовпадающих значений массива
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="m"></param>
        /// <remarks>
        /// На вход подается целочисленный массив arr.
        /// На выходе получаем - массив из m случайно 
        /// выбранных значений массива arr, при этом одно и тоже значение массива не выбирается повторно.
        /// Взято с http://codelab.ru/t/random:multiple_choice/?print=1
        /// </remarks>
        /// <returns></returns>
        private static long[] RandomUniqueSelect(long[] arr, int m)
        {
            long[] res = new long[m];
            if (m > arr.Length) return res;

            for (int i = 0; i < arr.Length; i++)
            {
                /* выбор m из оставшихся n-i */
                if ((RandObj.Next() % (arr.Length - i)) < m)
                {
                    res[--m] = arr[i];
                }
            }
            return res;
        }

        /// <summary>
        /// Сохраняет список лучших результатов
        /// </summary>
        public void SaveScoreList(string username)
        {
            ScoreList.Add(new ScoreRecord(){Username = username, Score = Score});
            ScoreList = new ObservableCollection<ScoreRecord>(from item in ScoreList orderby item.Score descending select item);
            WriteToFile();
            
        }

        public void LoadScoreList()
        {
            if (!File.Exists(@"SavedScores.txt"))
            {
                ScoreList = new ObservableCollection<ScoreRecord>();
                WriteToFile();
            }
            ReadFromFile();
        }

        /// <summary>
        /// Шифрует список результатов и записывает его в файл
        /// </summary>
        private void WriteToFile()
        {
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes("12345678901234567890123456789012");
                rijAlg.IV = Encoding.ASCII.GetBytes("1234567890123456");

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        //Записали в криптопоток
                        XamlServices.Save(csEncrypt, ScoreList);
                    }
                    //Записали в файл
                    File.WriteAllBytes(@"SavedScores.txt", msEncrypt.ToArray());
                }
            }
        }

        /// <summary>
        /// Чтение зашифрованного текста из файла, расшифровка и построение списка результатов
        /// </summary>
        private void ReadFromFile()
        {
            string text;
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes("12345678901234567890123456789012");
                rijAlg.IV = Encoding.ASCII.GetBytes("1234567890123456");

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(File.ReadAllBytes(@"SavedScores.txt")))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            text = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            var fromFile = XamlServices.Load(memStream);
            memStream.Close();
            ScoreList = (ObservableCollection<ScoreRecord>)fromFile;
        }

        //public FileInfo ReadResource(UnmanagedMemoryStream resource)
        //{
        //    byte[] b = new byte[resource.Length];
        //    resource.Read(b, 0, (int)resource.Length);
        //    FileInfo fileInfo = new FileInfo("Your_Song.mp3");
        //    FileStream fs = fileInfo.OpenWrite();
        //    fs.Write(b, 0, b.Length);
        //    fs.Close();
        //    _player = new WindowsMediaPlayer();
        //    _player.URL = fileInfo.Name;
        //    _player.settings.setMode("loop", true);
        //    _player.controls.stop();
        //}

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
