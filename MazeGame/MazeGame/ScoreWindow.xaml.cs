using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGame
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        double winHeight;
        double winWidth;
        int time;

        public ScoreWindow(double screenHeight, double screenWidth, int gameTime)
        {
            winHeight = screenHeight;
            winWidth = screenWidth;
            time = gameTime;
            InitializeComponent();

            this.numTime.Inlines.Clear();
            this.numTime.Inlines.Add(new Bold(new Run(time.ToString())));
            this.Height = winHeight;
            this.Width = winWidth;
        }

        private void menuClicked(object sender, EventArgs e)
        {
            numScores.Inlines.Clear();
            numScores.Inlines.Add(new Bold(new Run("updated")));
            MessageBox.Show("Button Clicked");
            //ScoreWindow window = new ScoreWindow();
            //this.Visibility = Visibility.Collapsed;
            //window.Show();
        }

        private void nextLevelClicked(object sender, EventArgs e)
        {

            MainWindow window = new MainWindow();
            this.Visibility = Visibility.Collapsed;
            window.Show();
        }

    }
}
