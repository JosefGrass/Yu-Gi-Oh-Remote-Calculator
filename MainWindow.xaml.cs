using System;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Timers;
using System.Windows.Media.Animation;
using System.Security.Cryptography.X509Certificates;

namespace YGOCalculator
{
    public partial class MainWindow : Window
    {

        private System.Timers.Timer timer;
        private int timeLeft = 50*60; // Zeit in Sekunden
        string PAUSE = "Pause";
        string STARTTIMER = "Start Timer";
        string RESUMETIMER = "Resume";

        public int Player1HP // Hier wird eine öffentliche (public) Eigenschaft (Property) namens "Player1HP" definiert, die den aktuellen Wert der Lebenspunkte (HP) des ersten Spielers repräsentiert.
                             // Der Datentyp dieser Eigenschaft ist ein ganzzahliger Wert (int).

        {
            get { return Convert.ToInt32(Player1HPLabel.Content); } //Dies definiert den "Getter" für die Eigenschaft "Player1HP".
                                                                    //Wenn du auf "Player1HP" zugreifst, wird dieser Code ausgeführt, um den Wert der Lebenspunkte des ersten Spielers abzurufen.
                                                                    //Es liest den aktuellen Wert aus dem Label namens "Player1HPLabel" aus und konvertiert ihn in einen ganzzahligen Wert.

            set { Player1HPLabel.Content = value; } //Dies definiert den "Setter" für die Eigenschaft "Player1HP".
                                                    //Wenn du versuchst, den Wert von "Player1HP" zu ändern, wird dieser Code ausgeführt.
                                                    //Er setzt den Inhalt des Labels "Player1HPLabel" auf den neuen Wert (angegeben durch "value").
        }
        public int Player2HP
        {
            get { return Convert.ToInt32(Player2HPLabel.Content); }
            set { Player2HPLabel.Content = value; }
        }

        public bool isTimerStopped;
        public bool isTimerStarted;


        public MainWindow()
        {
            InitializeComponent(); // Diese Zeile ruft die Methode InitializeComponent() auf.
                                   // Diese Methode wird automatisch vom Visual Studio generiert und initialisiert die Benutzeroberfläche des MainWindow-Objekts, indem sie XAML-Code in C#-Code umwandelt.
                                   // Dadurch werden alle visuellen Elemente und Ereignishandler des MainWindow initialisiert.

            Player1HP = 8000; // Hier wird der Wert der Lebenspunkte (HP) des Spielers auf 8000 gesetzt.
            Player2HP = 8000;
            timer = new System.Timers.Timer(1000); // Intervall auf 1 Sekunde setzen
            timer.Elapsed += Timer_Elapsed;
            isTimerStopped = false;
            isTimerStarted = false;

        }
        private void PlaySound()
        {
            SoundPlayer player = new SoundPlayer("Resources/timeout.wav");
            player.Play();
        }

        private string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Aktualisierung der UI muss auf dem UI-Thread erfolgen
            Dispatcher.Invoke(() =>
            {
                if (timeLeft > 0)
                {
                    timeLeft--;
                    TimerLabel.Content = FormatTime(timeLeft); // Aktualisiert das Timer-Label
                    
                }
                else
                {
                    timer.Stop();
                    PlaySound(); // Spielt den Sound ab, wenn die Zeit abgelaufen ist
                }
            });
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            isTimerStarted = true;
                        
            if (TimerStartStop.Content == PAUSE)
            {
                TimerStartStop.Content = RESUMETIMER;
                timer.Stop();
            }
            else
            {
                 
                TimerLabel.Content = FormatTime(timeLeft);
                timer.Start();
                TimerStartStop.Content = PAUSE;
            }
            
        }

        


        private void ResetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timeLeft = 50 * 60; // 50 Minuten zurücksetzen
            TimerLabel.Content = FormatTime(timeLeft);
            isTimerStopped = false;
            isTimerStarted = false;
            if (TimerStartStop.Content == PAUSE)
            {
                TimerStartStop.Content = STARTTIMER;
                
            }
            if (TimerStartStop.Content == RESUMETIMER)
            {
                TimerStartStop.Content = STARTTIMER;
            }
        }

            private void Player1AddHPButton_Click(object sender, RoutedEventArgs e) //Diese Methode wird aufgerufen, wenn der Benutzer auf den Button klickt, um die Lebenspunkte des Spielers zu erhöhen.
                                                                                //Zuerst wird der Inhalt des Textfelds "Player1EditHPTextBox" in eine ganze Zahl (int) konvertiert und in der Variablen "player1HPInput" gespeichert.
                                                                                //Dann wird dieser Wert zu den aktuellen Lebenspunkten des ersten Spielers hinzugefügt, indem die Eigenschaft "Player1HP" verwendet wird.
        {
            int player1HPInput = Convert.ToInt32(Player1EditHPTextBox.Text);
            Player1HP += player1HPInput;
            AddToHistory($"Player 1: {player1HPInput} HP added. Total: {Player1HP} HP", Brushes.Green);
        }

        private void Player1LoseHPButton_Click(object sender, RoutedEventArgs e)
        {
            int player1HPInput = Convert.ToInt32(Player1EditHPTextBox.Text);
            Player1HP -= player1HPInput;
            AddToHistory($"Player 1: {player1HPInput} HP lost. Total: {Player1HP} HP", Brushes.Green);
        }

        private void Player2AddHPButton_Click(object sender, RoutedEventArgs e)
        {
            int player2HPInput = Convert.ToInt32(Player2EditHPTextBox.Text);
            Player2HP += player2HPInput;
            AddToHistory($"Player 2: {player2HPInput} HP added. Total: {Player2HP} HP", Brushes.Blue);
        }

        private void Player2LoseHPButton_Click(object sender, RoutedEventArgs e)
        {
            int player2HPInput = Convert.ToInt32(Player2EditHPTextBox.Text);
            Player2HP -= player2HPInput;
            AddToHistory($"Player 2: {player2HPInput} HP lost. Total: {Player2HP} HP", Brushes.Blue);
        }

        private void ChangeTextBox1000Button_Click(object sender, RoutedEventArgs e) //Dies ist der Event-Handler für das Klickereignis des Buttons "ChangeTextBox1000Button".
                                                                                     //Er wird ausgeführt, wenn der Benutzer auf diesen Button klickt.
        {
            Player1EditHPTextBox.Text = 1000.ToString(); //In dieser Zeile wird der Inhalt des Textfelds "Player1EditHPTextBox" geändert.
                                                         //"1000" ist eine ganze Zahl (int), die in eine Zeichenfolge (String) konvertiert wird, damit sie dem Textfeld zugewiesen werden kann.
                                                         //Die Methode ToString() wird verwendet, um die ganze Zahl in eine Zeichenfolge umzuwandeln, damit sie im Textfeld angezeigt werden kann.
        }

        private void ChangeTextBox500Button_Click(object sender, RoutedEventArgs e)
        {
            Player1EditHPTextBox.Text = 500.ToString();
        }

        private void ChangeTextBox100Button_Click(object sender, RoutedEventArgs e)
        {
            Player1EditHPTextBox.Text = 100.ToString();
        }

        private void ChangeTextBoxDivideBy2Button_Click(object sender, RoutedEventArgs e)
        {
            Player1EditHPTextBox.Text = (Player1HP /2).ToString();
        }

        private void Player2ChangeTextBox1000Button_Click(object sender, RoutedEventArgs e)
        {
            Player2EditHPTextBox.Text = 1000.ToString();
        }

        private void Player2ChangeTextBox500Button_Click(object sender, RoutedEventArgs e)
        {
            Player2EditHPTextBox.Text = 500.ToString();
        }

        private void Player2ChangeTextBox100Button_Click(object sender, RoutedEventArgs e)
        {
            Player2EditHPTextBox.Text = 100.ToString();
        }

        private void Player2ChangeTextBoxDivideBy2Button_Click(object sender, RoutedEventArgs e)
        {
            Player2EditHPTextBox.Text = (Player2HP /2).ToString();
        }

        private void ResetPlayer1_Click(object sender, RoutedEventArgs e) //ResetPlayer1_Click: Diese Methode wird aufgerufen, wenn der Benutzer auf den Button "ResetPlayer1" klickt.
                                                                          //Ihr Zweck ist es, die Lebenspunkte des ersten Spielers auf den Standardwert von 8000 zurückzusetzen.
                                                                          //Dafür wird der Inhalt des Labels "Player1HPLabel" auf 8000 gesetzt.
        {
            Player1HPLabel.Content = 8000;
            AddToHistory("Player 1 HP reset to 8000", Brushes.Red);
        }

        private void ResetPlayer2_Click(object sender, RoutedEventArgs e)
        {
            Player2HPLabel.Content = 8000;
            AddToHistory("Player 2 HP reset to 8000", Brushes.Red);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) //Dies ist der Event-Handler, der aufgerufen wird, wenn der Benutzer Text in ein Textfeld eingibt,
                                                                                         //bevor dieser Text tatsächlich im Textfeld erscheint.
        {
            foreach (char c in e.Text) //Diese Schleife durchläuft jeden einzelnen Buchstaben (char) im eingegebenen Text, der durch das Event-Argument e repräsentiert wird.
                                       //Das bedeutet, dass der eingegebene Text zeichenweise überprüft wird.
            {
                if (!char.IsDigit(c)) // In jeder Iteration der Schleife wird überprüft, ob das aktuelle Zeichen c keine Ziffer ist.
                                      // Die Methode char.IsDigit() prüft, ob das Zeichen eine Ziffer von 0 bis 9 ist.
                                      // Das Ausrufezeichen vor char.IsDigit(c) bewirkt, dass die Bedingung negiert wird, sodass der Code im Inneren der Bedingung ausgeführt wird, wenn c keine Ziffer ist.
                {
                    e.Handled = true; // Wenn ein nicht-numerisches Zeichen gefunden wird, wird das Ereignis behandelt (e.Handled wird auf true gesetzt), was bedeutet, dass das nicht-numerische Zeichen nicht im Textfeld angezeigt wird.

                    break; //Sobald ein nicht-numerisches Zeichen gefunden wurde, wird die Schleife mit break; abgebrochen, da wir bereits wissen, dass der Text ungültig ist und weitere Überprüfungen nicht notwendig sind.
                }
            }
        }

        private void PerformSearch()
        {
            string searchText = Kartensuche.Text;
            searchText = searchText.Replace(" ", "+");
            string url = GetURL() + searchText;
            System.Diagnostics.Process.Start("cmd", $"/c start {url}");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }
        
        private string GetURL()
        {
            return "https://cardcluster.de/search/";
        }

        private void Kartensuche_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
                
        }

        private void AddToHistory(string message,Brush color)
        {
            var paragraph = new Paragraph(new Run(message))
            {
                Foreground = color,
            };
            HistoryRichTextBox.Document.Blocks.InsertBefore(HistoryRichTextBox.Document.Blocks.FirstBlock, paragraph);
        }
        private void UpdatePlayer1HP(string message)
        {
            int player1HPInput = Convert.ToInt32(Player1EditHPTextBox.Text);
            Player1HP += player1HPInput;
            Player1HPLabel.Content= Player1HP.ToString();
            
        }


    }
}
        