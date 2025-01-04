using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MemoryGameWPF
{
    public partial class MainWindow : Window
    {
        private List<string> sequence;
        private List<string> wordsFromFile;
        private int currentLevel;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            sequence = new List<string>();
            currentLevel = 0;
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += DisplaySequence;

            StatusText.Text = "Odaberite opciju igranja.";

            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            RestartButton.Visibility = Visibility.Hidden;
        }

        private void StartGame()
        {
            int highestScore = GetHighestScore();
            StatusText.Text = $"Najviši rezultat: {highestScore}. Zapamtite sekvencu i unesite je ispravno.";

            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;
            sequence.Clear();
            StartNewLevel();
        }

        private void StartNewLevel()
        {
            currentLevel++;
            sequence = GetRandomSequence();

            StatusText.Text = $"Nivo {currentLevel}: Pazi na sekvencu!";

            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;

            timer.Start();

            Dispatcher.Invoke(() => InputTextBox.Focus());
        }

        private void DisplaySequence(object sender, EventArgs e)
        {
            if (sequence.Count > 0)
            {
                StatusText.Text = string.Join(" ", sequence);
                timer.Stop();
                Task.Delay(1500).ContinueWith(_ => Dispatcher.Invoke(() =>
                {
                    StatusText.Text = "Tvoj red! Ponovi sekvencu.";

                    InputTextBox.Visibility = Visibility.Visible;
                    SubmitButton.Visibility = Visibility.Visible;

                    InputTextBox.IsEnabled = true;
                    SubmitButton.IsEnabled = true;

                    InputTextBox.Focus();
                }));
            }
        }

        private List<string> GetRandomSequence()
        {
            Random random = new Random();
            return wordsFromFile.OrderBy(x => random.Next()).Take(currentLevel).ToList();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputTextBox.Text.Trim();
            int highestScore = GetHighestScore();

            if (userInput == string.Join(" ", sequence))
            {
                StatusText.Text = "Tačno! Prelazimo na sledeći nivo.";
                InputTextBox.Clear();
                InputTextBox.IsEnabled = false;
                SubmitButton.IsEnabled = false;
                Task.Delay(700).ContinueWith(_ => Dispatcher.Invoke(StartNewLevel));
            }
            else
            {
                if (currentLevel > highestScore)
                {
                    SaveHighestScore(currentLevel);
                    highestScore = currentLevel;
                }

                StatusText.Text = $"Kraj igre! Došli ste do nivoa {currentLevel}. \n Najviši rezultat: {highestScore}.";

                InputTextBox.Visibility = Visibility.Hidden;
                SubmitButton.Visibility = Visibility.Hidden;
                InputTextBox.IsEnabled = false;
                SubmitButton.IsEnabled = false;
                SubmitButton.Visibility = Visibility.Hidden;
                RestartButton.Visibility = Visibility.Visible;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
            StatusText.Text = "Igra je resetovana. Pritisnite dugme za pokretanje nove igre.";
            RestartButton.Visibility = Visibility.Hidden;
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitButton_Click(sender, e);
            }
        }

        private void AnimalsButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateAnimalWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void CitiesButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateCityWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void ObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateObjectWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void NumbersButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateNumberWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void FruitsButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateFruitAndVegetableWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            wordsFromFile = GenerateRandomWords();
            ResetGame();
            StartGameButton.Visibility = Visibility.Visible;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
            StartGameButton.Visibility = Visibility.Hidden;
        }

        private void ResetGame()
        {
            sequence.Clear();
            currentLevel = 0;
            InputTextBox.Clear();
            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            RestartButton.Visibility = Visibility.Hidden;

            StatusText.Text = "Novi izbor aktiviran. Pritisnite 'Pokreni igru' za početak.";

            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;
        }

        private List<string> GenerateAnimalWords()
        {
            return new List<string>
            {
                "pas", "mačka", "tigar", "lav", "slon", "žirafa", "zebra", "krava", "ovca", "koza",
                "konj", "pčela", "skakavac", "medved", "vuk", "lisica", "sova", "vrapac", "orlov", "kengur",
                "panda", "koala", "delfin", "kit", "poni", "kornjača", "zmija", "žaba", "kokoška", "konj",
                "slon", "majmun", "riba", "pingvin", "foka", "jednorog", "jak", "aligator", "krokodil", "sova",
                "jelen", "tapir", "nojev", "lemur", "crna pantera", "jak", "rakun", "vol", "bizon", "tigrasti pas"
            };
        }

        private List<string> GenerateCityWords()
        {
            return new List<string>
            {
                "Beograd", "Zagreb", "London", "Pariz", "Berlin", "Njujork", "Tokio", "Sidnej", "Madrid", "Rim",
                "Amsterdam", "Venecija", "Beč", "Kopenhagen", "Oslo", "Prag", "Budimpešta", "Ljubljana", "San Francisco", "Los Angeles",
                "Boston", "Moskva", "Roma", "Helsinki", "Lisabon", "Kopenhagen", "Varšava", "Seul", "Delhi", "Rio de Janeiro",
                "Šangaj", "Istanbul", "Barselona", "Buenos Aires", "Dubai", "Sidnej", "Minhen", "Stokholm", "Toronto", "Vancouver",
                "Hong Kong", "Singapur", "San Diego", "Majami", "Čikago", "Atlanta", "Sijetl", "Montreal", "Tbilisi", "Kijev"
            };
        }

        private List<string> GenerateObjectWords()
        {
            return new List<string>
            {
                "stolica", "sto", "lampa", "telefon", "laptop", "računar", "knjiga", "banka", "šolja", "krevet",
                "papir", "flomaster", "rukavica", "sat", "kofer", "torba", "patike", "tanjir", "čaša", "olovka",
                "daljinski", "sveska", "čajnik", "televizor", "radijator", "frižider", "mikrofon", "slika", "bure", "svetiljka",
                "prozor", "ventilator", "tepih", "mali sto", "uredjaj", "peškir", "stolica za računare", "stolica za trpezariju", "lampa", "kutija",
            };
        }

        private List<string> GenerateNumberWords()
        {
            var random = new Random();
            var singleDigitNumbers = Enumerable.Range(0, 10).Select(x => x.ToString()).ToList();
            var doubleDigitNumbers = Enumerable.Range(10, 90).OrderBy(x => random.Next()).Take(20).Select(x => x.ToString()).ToList();
            var tripleDigitNumbers = Enumerable.Range(100, 900).OrderBy(x => random.Next()).Take(20).Select(x => x.ToString()).ToList();

            return singleDigitNumbers.Concat(doubleDigitNumbers).Concat(tripleDigitNumbers).ToList();
        }

        private List<string> GenerateFruitAndVegetableWords()
        {
            return new List<string>
            {
                "jabuka", "kruška", "šljiva", "breskva", "trešnja", "limun", "narandža", "dinja",
                "lubenica", "jagoda", "krastavac", "mrkva", "paradajz", "krompir", "brokoli", "kelj",
                "spinat", "tikvica", "patlidžan", "karfiol",
            };
        }

        private List<string> GenerateRandomWords()
        {
            var allWords = GenerateAnimalWords()
                .Concat(GenerateCityWords())
                .Concat(GenerateObjectWords())
                .Concat(GenerateNumberWords())
                .Concat(GenerateFruitAndVegetableWords())
                .ToList();

            var random = new Random();
            return allWords.OrderBy(x => random.Next()).Take(20).ToList();
        }

        private int GetHighestScore()
        {
            string filePath = "highscore.txt"; 

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose(); 
                return 0; 
            }

            string content = File.ReadAllText(filePath);
            if (int.TryParse(content, out int highestScore))
            {
                return highestScore;
            }

            return 0;
        }

        private void SaveHighestScore(int score)
        {
            string filePath = "highscore.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            File.WriteAllText(filePath, score.ToString());
        }
    }
}
