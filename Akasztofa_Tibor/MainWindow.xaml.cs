using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            foreach (string sor in File.ReadAllLines(@"jatekosok.txt"))
            {
                string[] s = sor.Split(';');
                nevbemenet.Items.Add(s[0]);
            }
        }
        private void ComboBoxName_TextChanged(object sender, EventArgs e)
        {
            if (nevbemenet.Text.Length > 0 && Regex.IsMatch(nevbemenet.Text, @"^[a-záéúőóüö A-ZZÁÉÚŐÓÜÖÍ]+$"))
            {
                GameButton.IsEnabled = true;
            }
            else
            {
                GameButton.IsEnabled = false;
            }
        }
        public void GameClick(object sender, RoutedEventArgs e)
        {
            nev = nevbemenet.Text;
            if (gym.IsChecked == true) tipusSzam = 0;
            else if (porno.IsChecked == true) tipusSzam = 1;
            else if (drug.IsChecked == true) tipusSzam = 2;
            MainFrame.Content = new Game();
        }
    }
}
