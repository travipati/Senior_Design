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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using System.Windows.Forms;



namespace MazeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
       

        public MenuWindow()
        {
            //Download Image

            //Uri uri = new Uri("http://maps.google.com/staticmap?center=45.728220,4.830321&zoom=8&size=200x200&maptype=roadmap&key=ABQIAAAAaHAby4XeLCIadFkAUW4vmRSkJGe9mG57rOapogjk9M-sm4TzXxR2I7bi2Qkj-opZe16CdmDs7_dNrQ");
            //BitmapImage newImage = new BitmapImage(uri);

            //Load Image
            InitializeComponent();
            WindowState = System.Windows.WindowState.Maximized;
            //mainMenu.Source = newImage;
        }

        private void myButtonClicked(object sender, EventArgs e)
        {
            //numScores.Inlines.Clear();
            //numScores.Inlines.Add(new Bold(new Run("updated")));
            System.Windows.MessageBox.Show("Button Clicked");
            MenuWindow window = new MenuWindow();
            this.Visibility = Visibility.Collapsed;
            window.Show();
        }





    }
}
