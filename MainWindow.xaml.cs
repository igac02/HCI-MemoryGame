using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MemoryGameWPF
{
    public partial class MainWindow : Window
    {
        private List<string> sequence;
        private List<string> wordsFromFile;
        private int currentLevel;
        private DispatcherTimer timer;
        private DispatcherTimer counterTimer;
        private int displayTimeSeconds = 3;
        private int displayTimeRemaining;
        private Button lastClickedThemeButton;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            UpdateHighScoreDisplay();

            // Set initial focus
            Loaded += (s, e) => AnimalsButton.Focus();
        }

        private void InitializeGame()
        {
            sequence = new List<string>();
            currentLevel = 0;

            // Setup timer for displaying sequence
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += DisplaySequence;

            // Setup counter timer for countdown animation
            counterTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            counterTimer.Tick += CountdownTick;

            StatusText.Text = "Dobrodošli u Memory Game!\nOdaberite temu igre za početak.";

            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            RestartButton.Visibility = Visibility.Hidden;
            LevelIndicator.Visibility = Visibility.Collapsed;
        }

        private void StartGame()
        {
            // Reset UI elements
            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;
            StartGameButton.Visibility = Visibility.Hidden;

            StatusText.Text = "Pripremite se!";

            // Show level indicator with animation
            LevelIndicator.Visibility = Visibility.Visible;

            // Clear previous sequence
            sequence.Clear();

            // Start at level 1
            currentLevel = 0;
            StartNewLevel();
        }

        private void StartNewLevel()
        {
            currentLevel++;

            // Update level display
            LevelText.Text = currentLevel.ToString();

            // Generate new sequence for this level
            sequence = GetRandomSequence();

            // Update status
            StatusText.Text = $"Nivo {currentLevel}: Zapamtite sekvencu!";

            // Hide input controls
            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;

            // Start countdown before showing sequence
            displayTimeRemaining = 3;
            AnimateCountdown();
            counterTimer.Start();
        }

        private void CountdownTick(object sender, EventArgs e)
        {
            displayTimeRemaining--;

            if (displayTimeRemaining <= 0)
            {
                counterTimer.Stop();
                // After countdown, show the sequence
                timer.Start();
            }
            else
            {
                AnimateCountdown();
            }
        }

        private void AnimateCountdown()
        {
            StatusText.Text = $"Prikazivanje sekvence za {displayTimeRemaining}...";
        }

        private void DisplaySequence(object sender, EventArgs e)
        {
            timer.Stop();

            // Display sequence with styled text
            StatusText.Text = string.Join(" ", sequence);

            // After showing sequence for a moment, switch to input mode
            int displayDuration = Math.Max(1500, sequence.Count * 800); // Longer display for longer sequences

            Task.Delay(displayDuration).ContinueWith(_ => Dispatcher.Invoke(() =>
            {
                // Prompt for user input
                StatusText.Text = "Vaš red! Unesite zapamćenu sekvencu.";

                // Show and enable input controls
                InputTextBox.Visibility = Visibility.Visible;
                SubmitButton.Visibility = Visibility.Visible;
                InputTextBox.IsEnabled = true;
                SubmitButton.IsEnabled = true;

                // Clear previous input and focus
                InputTextBox.Clear();
                InputTextBox.Focus();
            }));
        }

        private List<string> GetRandomSequence()
        {
            Random random = new Random();
            return wordsFromFile.OrderBy(x => random.Next()).Take(currentLevel).ToList();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputTextBox.Text.Trim();

            // Check if user's answer matches the sequence
            if (String.Equals(userInput, string.Join(" ", sequence), StringComparison.OrdinalIgnoreCase))
            {
                HandleCorrectAnswer();
            }
            else
            {
                HandleIncorrectAnswer();
            }
        }

        private void HandleCorrectAnswer()
        {
            // Visual feedback for correct answer
            StatusText.Text = "Odlično! Idemo na sledeći nivo.";

            // Success animation could be added here

            // Prepare for next level
            InputTextBox.Clear();
            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;

            // Start next level after a short delay
            Task.Delay(1000).ContinueWith(_ => Dispatcher.Invoke(StartNewLevel));
        }

        private void HandleIncorrectAnswer()
        {
            // Update high score if needed
            int highestScore = GetHighestScore();
            if (currentLevel > highestScore)
            {
                SaveHighestScore(currentLevel);
                UpdateHighScoreDisplay();
            }

            // Show game over message
            StatusText.Text = $"Kraj igre! Dostigli ste nivo {currentLevel}.";

            // Hide input controls and show restart button
            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;
            RestartButton.Visibility = Visibility.Visible;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
            StatusText.Text = "Igra je resetovana. Odaberite temu i pritisnite 'POKRENI IGRU'.";
            RestartButton.Visibility = Visibility.Hidden;
            StartGameButton.Visibility = Visibility.Visible;
            LevelIndicator.Visibility = Visibility.Collapsed;
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitButton_Click(sender, e);
            }
        }

        #region Theme Selection Handlers
        private void AnimalsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateAnimalWords(), "Životinje");
        }

        private void CitiesButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateCityWords(), "Gradovi");
        }

        private void ObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateObjectWords(), "Predmeti");
        }

        private void NumbersButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateNumberWords(), "Brojevi");
        }

        private void FruitsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateFruitAndVegetableWords(), "Voće i povrće");
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTheme(sender as Button, GenerateRandomWords(), "Razno");
        }

        private void SelectTheme(Button themeButton, List<string> words, string themeName)
        {
            // Reset previous button's style if exists
            if (lastClickedThemeButton != null)
            {
                lastClickedThemeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6200EE"));
            }

            // Highlight selected button
            themeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03DAC6"));
            lastClickedThemeButton = themeButton;

            // Set the word list for the theme
            wordsFromFile = words;

            // Reset game state
            ResetGame();

            // Update UI
            StatusText.Text = $"Odabrana tema: {themeName}. Pritisnite 'POKRENI IGRU' za početak.";
            StartGameButton.Visibility = Visibility.Visible;
        }
        #endregion

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void ResetGame()
        {
            // Reset game variables
            sequence.Clear();
            currentLevel = 0;

            // Reset UI
            InputTextBox.Clear();
            InputTextBox.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            RestartButton.Visibility = Visibility.Hidden;
            InputTextBox.Visibility = Visibility.Hidden;
            SubmitButton.Visibility = Visibility.Hidden;
        }

        #region Word Lists
        private List<string> GenerateAnimalWords()
        {
            return new List<string>
            {
                "pas", "mačka", "tigar", "lav", "slon", "žirafa", "zebra", "krava", "ovca", "koza",
                "konj", "pčela", "skakavac", "medved", "vuk", "lisica", "sova", "vrapac", "orao", "kengur",
                "panda", "koala", "delfin", "kit", "poni", "kornjača", "zmija", "žaba", "kokoška", "konj",
                "majmun", "riba", "pingvin", "foka", "jelen", "aligator", "krokodil", "rakun", "bizon", "vepar"
            };
        }

        private List<string> GenerateCityWords()
        {
            return new List<string>
            {
                "Beograd", "Zagreb", "London", "Pariz", "Berlin", "Njujork", "Tokio", "Sidnej", "Madrid", "Rim",
                "Amsterdam", "Venecija", "Beč", "Kopenhagen", "Oslo", "Prag", "Budimpešta", "Ljubljana", "San Francisco", "Los Angeles",
                "Boston", "Moskva", "Helsinki", "Lisabon", "Varšava", "Seul", "Delhi", "Rio", "Šangaj", "Istanbul",
                "Barselona", "Dubai", "Minhen", "Stokholm", "Toronto", "Majami", "Čikago", "Atlanta", "Sijetl", "Montreal"
            };
        }

        private List<string> GenerateObjectWords()
        {
            return new List<string>
            {
                "stolica", "sto", "lampa", "telefon", "laptop", "računar", "knjiga", "banka", "šolja", "krevet",
                "papir", "flomaster", "rukavica", "sat", "kofer", "torba", "patike", "tanjir", "čaša", "olovka",
                "daljinski", "sveska", "čajnik", "televizor", "radijator", "frižider", "mikrofon", "slika", "prozor", "ventilator",
                "tepih", "ogledalo", "vaza", "polica", "ormar", "saksija", "kaiš", "fotoaparat", "nož", "viljuška"
            };
        }

        private List<string> GenerateNumberWords()
        {
            var numbers = new List<string>();

            // Add single digit numbers (0-9)
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(i.ToString());
            }

            // Add selected double digit numbers
            var doubleDigits = new[] { 10, 15, 20, 25, 30, 40, 50, 60, 70, 80, 90, 11, 22, 33, 44, 55, 66, 77, 88, 99 };
            foreach (var num in doubleDigits)
            {
                numbers.Add(num.ToString());
            }

            // Add selected triple digit numbers
            var tripleDigits = new[] { 100, 200, 300, 500, 777, 999, 123, 456, 789, 321 };
            foreach (var num in tripleDigits)
            {
                numbers.Add(num.ToString());
            }

            return numbers;
        }

        private List<string> GenerateFruitAndVegetableWords()
        {
            return new List<string>
            {
                "jabuka", "kruška", "šljiva", "breskva", "trešnja", "limun", "narandža", "dinja",
                "lubenica", "jagoda", "krastavac", "mrkva", "paradajz", "krompir", "brokoli", "kelj",
                "spinat", "tikvica", "patlidžan", "karfiol", "grožđe", "malina", "kupina", "banana",
                "ananas", "kivi", "paprika", "luk", "šargarepa", "kupus", "šampinjon", "grejpfrut",
                "marelica", "višnja", "kajsija", "borovnica", "pasulj", "grašak", "cvekla", "kukuruz"
            };
        }

        private List<string> GenerateRandomWords()
        {
            // Combine all word lists
            var allWords = new List<string>();
            allWords.AddRange(GenerateAnimalWords());
            allWords.AddRange(GenerateCityWords());
            allWords.AddRange(GenerateObjectWords());
            allWords.AddRange(GenerateNumberWords());
            allWords.AddRange(GenerateFruitAndVegetableWords());

            // Shuffle and select words
            var random = new Random();
            return allWords.OrderBy(x => random.Next()).Take(100).ToList();
        }
        #endregion

        #region High Score Management
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

        private void UpdateHighScoreDisplay()
        {
            int highScore = GetHighestScore();
            HighScoreText.Text = $"Najviši rezultat: {highScore}";

            // Show or hide the high score panel based on whether there's a score
            HighScorePanel.Visibility = highScore > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Window Controls
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
        #endregion