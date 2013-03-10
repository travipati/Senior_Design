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
        int score;

        public ScoreWindow(double screenHeight, double screenWidth, int gameTime)
        {
            winHeight = screenHeight;
            winWidth = screenWidth;
            time = gameTime;
            InitializeComponent();
            WindowState = System.Windows.WindowState.Maximized;
            score = 100 * 10 / time;
            this.numTime.Inlines.Clear();
            this.numTime.Inlines.Add(new Bold(new Run(time.ToString())));
            this.numScores.Inlines.Clear();
            this.numScores.Inlines.Add(new Bold(new Run(score.ToString())));
            this.Height = winHeight;
            this.Width = winWidth;
        }

        private void menuClicked(object sender, EventArgs e)
        {
            //numScores.Inlines.Clear();
            //numScores.Inlines.Add(new Bold(new Run(" m_clicked")));
            //MessageBox.Show("Button Clicked");
            MainWindow window = new MainWindow();
            this.Visibility = Visibility.Collapsed;
            window.Show();
        }

        private void nextLevelClicked(object sender, EventArgs e)
        {

            Level_0 window = new Level_0();
            this.Visibility = Visibility.Collapsed;
            window.Show();
        }

    }
}
