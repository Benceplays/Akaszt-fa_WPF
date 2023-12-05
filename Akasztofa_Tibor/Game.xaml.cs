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
    public partial class Game : Page
    {
        private List<string> gym = new List<string>();
        private List<string> porno= new List<string>();
        private List<string> drug = new List<string>();
        private string eltalaltbetuk = "";
        private List<char> titkositottszo = new List<char>();
        private string randomszo;
        private int korszam = 1;
        private int maxszam = 6;
        private int probalkozas = 1;
        private int hiba = 0;
        static string IgenNem(bool val) { return val ? "igen" : "nem"; }
        public void Ellenorzo(char betu)
        {
            for (int i = 0; i < randomszo.Length; i++)
            {
                if (betu == randomszo[i])
                {
                    titkositottszo[i] = betu;
                    megfejtendopelda.Text = "";
                    titkositottszo.ForEach(n => megfejtendopelda.Text += n);
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (eltalaltbetuk!= "")
            {
                Regex regex = new Regex($"(?![{eltalaltbetuk}])[a-z]");
                var futureText = $"{(sender as TextBox).Text}{e.Text}";
                e.Handled = !regex.IsMatch(futureText);
            }
            else
            {
                Regex regex = new Regex("[a-z]");
                var futureText = $"{(sender as TextBox).Text}{e.Text}";
                e.Handled = !regex.IsMatch(futureText);
            }

        }
        public Game()
        {
            InitializeComponent();
            eddigieredmenyekszoveg.Content = $"{MainWindow.nev} eddigi eredményei: ";
            Random random = new Random();
            foreach (string sor in File.ReadAllLines(@"jatekosok.txt"))
            {
                string[] s = sor.Split(';');
                string szo = "";
                if (s[0] == MainWindow.nev)
                {
                    szo += $"Edzőterem témakörben nyert {s[1]}, vesztett {s[2]} játékot. \n" ;
                    szo += $"Pornógráfia témakörben nyert {s[3]}, vesztett {s[4]} játékot. \n";
                    szo += $"Tudatmodósítószerek témakörben nyert {s[5]}, vesztett {s[6]} játékot.";
                }
                else
                {
                    szo += $"Edzőterem témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Pornógráfia témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Tudatmodósítószerek témakörben nyert 0, vesztett 0 játékot.";
                }
                eredmenyek.Text = szo;
            }
            foreach (string sor in File.ReadAllLines(@"szavak.txt"))
            {
                string[] s = sor.Split(';');
                if (s[1] == "b") gym.Add(s[0]);
                if (s[1] == "m") porno.Add(s[0]);
                if (s[1] == "i") drug.Add(s[0]);
            }
            switch (MainWindow.tipusSzam) {
                case 0:
                    temakor.Text = "Edzőterem";
                    randomszo = gym[random.Next(gym.Count)];
                    break;
                case 1:
                    temakor.Text = "Pornógráfia";
                    randomszo = porno[random.Next(porno.Count)];
                    break;
                case 2:
                    temakor.Text = "Tudatmodósítószerek";
                    randomszo = drug[random.Next(drug.Count)];
                    break;
            }
            for (int i = 0; i < randomszo.Length; i++)
            {
                titkositottszo.Add('*');
            }
            titkositottszo.ForEach(n => megfejtendopelda.Text += n);
            tipButton.Content = $"{probalkozas}/{maxszam}";
        }
        public void TippClick(object sender, RoutedEventArgs e)
        {
            if (probalkozas <= maxszam && bemenet.Text != "")
            {
                tipButton.Content = $"Tipp {probalkozas++}/{maxszam}";
                if (randomszo.Contains(bemenet.Text))
                { 
                    Ellenorzo(Convert.ToChar(bemenet.Text));
                    eltalaltbetuk+=(Convert.ToChar(bemenet.Text));

                }
                else
                {
                    hiba++;
                    gibbet.Source = new BitmapImage(new Uri($"hangman_{hiba}.png",UriKind.RelativeOrAbsolute));
                }
                esemenytabla.Text += $"{korszam}.kör. Tippelt betű: {bemenet.Text} Találat: {IgenNem(randomszo.Contains(bemenet.Text))} \n";
                korszam++;
                bemenet.Text = "";
            }
        }
        public void eredmenyekClick(object sender, RoutedEventArgs e) { MainFrame.Content = new Results(); }
        public void megfejtesClick(object sender, RoutedEventArgs e) { megfejtesout.Text = randomszo.ToString(); }
    }
}
