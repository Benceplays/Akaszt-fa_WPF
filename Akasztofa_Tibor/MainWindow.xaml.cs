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

namespace Akasztofa_Tibor
{
    public partial class MainWindow : Window
    {
        public static int tipusSzam;
        public static string nev;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void GameClick(object sender, RoutedEventArgs e)
        {
            nev = nevbemenet.Text;
            if (Type0.IsChecked == true) tipusSzam = 0;
            else if (Type1.IsChecked == true) tipusSzam = 1;
            else if (Type2.IsChecked == true) tipusSzam = 2;
            MainFrame.Content = new Game();
        }
    }
}
