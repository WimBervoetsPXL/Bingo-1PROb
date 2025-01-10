using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _random = new Random();
        private Label[,] _player1Card;
        private Label[,] _player2Card;
        private DispatcherTimer _timer;
        private List<int> _bingoNumbers = new List<int>();

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;

            InitializeGrid(ref player1Grid);
            InitializeGrid(ref player2Grid);
        }

        private void InitializeGrid(ref Grid playerGrid)
        {
            for (int row = 0; row < 5; row++)
            {
                for(int col = 0; col < 5; col++)
                {
                    if(!(row == 2 && col == 2))
                    {
                        Label label = new Label();
                        //label.Content = $"{row} - {col}";
                        label.HorizontalAlignment = HorizontalAlignment.Stretch;
                        label.VerticalAlignment = VerticalAlignment.Stretch;
                        label.HorizontalContentAlignment = HorizontalAlignment.Center;
                        label.VerticalContentAlignment = VerticalAlignment.Center;
                        label.BorderBrush = Brushes.Black;
                        label.BorderThickness = new Thickness(1, 1, 1, 1);

                        Grid.SetColumn(label, col);
                        Grid.SetRow(label, row);

                        playerGrid.Children.Add(label);
                    }
                }
            }
        }

        private void OnStartGameClicked(object sender, RoutedEventArgs e)
        {
            _player1Card = GeneratePlayerCard(ref player1Grid);
            _player2Card = GeneratePlayerCard(ref player2Grid);

            _bingoNumbers.Clear();
            bingoNumbersListBox.Items.Clear();
            _timer.Start();
        }

        private Label[,] GeneratePlayerCard(ref Grid playerGrid)
        {
            Label[,] playerCard = new Label[5,5];

            Label[] gridLabels = playerGrid.Children.OfType<Label>().ToArray();

            List<int> usedNumbers = new List<int>();

            foreach (Label label in gridLabels)
            {
                int row = Grid.GetRow(label);
                int col = Grid.GetColumn(label);

                int randomNumber = 0;

                switch(col)
                {
                    case 0:
                        do
                        {
                            randomNumber = _random.Next(1, 16);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 1:
                        do
                        {
                            randomNumber = _random.Next(16, 31);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 2:
                        do
                        {
                            randomNumber = _random.Next(31, 46);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 3:
                        do
                        {
                            randomNumber = _random.Next(46, 61);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 4:
                        do
                        {
                            randomNumber = _random.Next(61, 76);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                }

                usedNumbers.Add(randomNumber);
                label.Content = randomNumber.ToString();
                label.Background = Brushes.Transparent;
                playerCard[row, col] = label;
            }

            return playerCard;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int randomNumber = 0;

            do
            {
                randomNumber = _random.Next(1, 76);
            } while(_bingoNumbers.Exists(element => element == randomNumber));

            _bingoNumbers.Add(randomNumber);
            ListBoxItem item = new ListBoxItem();
            item.Content = randomNumber.ToString();
            bingoNumbersListBox.Items.Add(item);
            bingoNumbersListBox.ScrollIntoView(item);

            if(CheckIfNumberExistsOnCard(ref _player1Card, randomNumber, out int row1, out int col1))
            {
                if (CheckFullRowOrColumn(ref _player1Card, row1, col1))
                {
                    _timer.Stop();
                    MessageBox.Show("Speler 1 heeft gewonnen!");
                }
            }

            if (CheckIfNumberExistsOnCard(ref _player2Card, randomNumber, out int row2, out int col2))
            {
                if (CheckFullRowOrColumn(ref _player2Card, row2, col2))
                {
                    _timer.Stop();
                    MessageBox.Show("Speler 2 heeft gewonnen!");
                }
            }
        }

        private bool CheckIfNumberExistsOnCard(ref Label[,] playerCard, int numberToCheck, out int foundInRow, out int foundInCol)
        {
            foundInRow = 0;
            foundInCol = 0;
            for (int row = 0; row < playerCard.GetLength(0); row++)
            {
                for (int col = 0; col < playerCard.GetLength(1); col++)
                {

                    if (playerCard[row, col] != null 
                        && playerCard[row, col].Content.ToString().Equals(numberToCheck.ToString()))
                    {
                        playerCard[row, col].Background = Brushes.Red;
                        foundInRow = row;
                        foundInCol = col;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckFullRowOrColumn(ref Label[,] playerCard, int checkRow, int checkCol)
        {
            //Check full row:

            bool fullRow = true;

            for(int col = 0; col < playerCard.GetLength(1);col++)
            {
                if(playerCard[checkRow, col] != null)
                {
                    int content = int.Parse(playerCard[checkRow, col].Content.ToString());
                    if (!(_bingoNumbers.Contains(content)))
                    {
                        fullRow = false;
                        break;
                    }
                }
            }

            if(fullRow)
            {
                return true;
            }
    

            //Check full column
            bool fullCol = true;

            for (int row = 0; row < playerCard.GetLength(0); row++)
            {
                if (playerCard[row, checkCol] != null)
                {
                    int content = int.Parse(playerCard[row, checkCol].Content.ToString());
                    if (!(_bingoNumbers.Contains(content)))
                    {
                        fullCol = false;
                        break;
                    }
                }
            }

            if (fullCol)
            {
                return true;
            }
            

            return false;
        }

    }
}
