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
        private Random random = new Random();


        public MainWindow()
        {
            InitializeComponent();

            InitializeGrid(ref player1Grid);
            //InitializeGrid(ref player2Grid);
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
                        label.Content = $"{row} - {col}";
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
            GeneratePlayerCard(ref player1Grid);
            //GeneratePlayerCard(ref player2Grid);
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
                            randomNumber = random.Next(1, 16);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 1:
                        do
                        {
                            randomNumber = random.Next(16, 31);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 2:
                        do
                        {
                            randomNumber = random.Next(31, 46);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 3:
                        do
                        {
                            randomNumber = random.Next(46, 61);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                    case 4:
                        do
                        {
                            randomNumber = random.Next(61, 76);
                        } while (usedNumbers.Contains(randomNumber));
                        break;
                }

                usedNumbers.Add(randomNumber);
            }

            return playerCard;
        }



    }
}
