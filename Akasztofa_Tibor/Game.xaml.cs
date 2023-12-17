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
    class Jatekosok
    {
        public string Nev { get; private set; }
        public int Category1WonNumber { get; private set; }
        public int Category1LostNumber { get; private set; }
        public int Category2WonNumber { get; private set; }
        public int Category2LostNumber { get; private set; }
        public int Category3WonNumber { get; private set; }
        public int Category3LostNumber { get; private set; }
        public Jatekosok(string sor)
        {
            string[] s = sor.Split(';');
            Nev = s[0];
            Category1WonNumber = Convert.ToInt32(s[1]);
            Category1LostNumber = Convert.ToInt32(s[2]);
            Category2WonNumber = Convert.ToInt32(s[3]);
            Category2LostNumber = Convert.ToInt32(s[4]);
            Category3WonNumber = Convert.ToInt32(s[5]);
            Category3LostNumber = Convert.ToInt32(s[6]);
        }
    }
    public partial class Game : Page
    {
        private List<string> gym = new List<string>();
        private List<string> porno = new List<string>();
        private List<string> drug = new List<string>();
        private string eltalaltbetuk = "";
        private List<char> titkositottszo = new List<char>();
        private string randomszo;
        private int korszam = 1;
        private int maxszam = 6;
        private int hiba = 0;
        private int iswin;
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
            if (eltalaltbetuk != "")
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
                    szo += $"Edzőterem témakörben nyert {s[1]}, vesztett {s[2]} játékot. \n";
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
            switch (MainWindow.tipusSzam)
            {
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
            tipButton.Content = $"{hiba}/{maxszam}";
        }
        public void CheckClick(object sender, RoutedEventArgs e)
        {
            tipButton.IsEnabled = false;
            megfejtesButton.IsEnabled = false;
            okButton.IsEnabled = false;
            megfejtes.IsReadOnly = true;
            bemenet.IsReadOnly = true;
            megfejtendopelda.Text = randomszo;
            if (megfejtes.Text == randomszo) 
            { 
                iswin = 1;
                iswinlabel.Content = "Siker tesomsz.";
                WinWriteToFile();
            }
            else 
            { 
                iswin = 0;
                iswinlabel.Content = "Not siker tesomsz.";
                LoseWriteToFile();
            }
        }
        public void TippClick(object sender, RoutedEventArgs e)
        {
            if (hiba < maxszam && bemenet.Text != "")
            {
                if (randomszo.Contains(bemenet.Text))
                {
                    Ellenorzo(Convert.ToChar(bemenet.Text));
                    eltalaltbetuk += (Convert.ToChar(bemenet.Text));
                    tipButton.Content = $"Hiba {hiba}/{maxszam}";
                }
                else
                {
                    hiba++;
                    gibbet.Source = new BitmapImage(new Uri($"hangman_{hiba}.png", UriKind.RelativeOrAbsolute));
                    tipButton.Content = $"Hiba {hiba}/{maxszam}";
                }
                esemenytabla.Text += $"{korszam}.kör. Tippelt betű: {bemenet.Text} Találat: {IgenNem(randomszo.Contains(bemenet.Text))} \n";
                korszam++;
                bemenet.Text = "";
            }
        }
        public void eredmenyekClick(object sender, RoutedEventArgs e) { MainFrame.Content = new Results(); }
        public void megfejtesClick(object sender, RoutedEventArgs e)
        {
            LoseWriteToFile();
            megfejtesout.Text = randomszo.ToString();
            tipButton.IsEnabled = false;
            okButton.IsEnabled = false;
            megfejtesButton.IsEnabled = false;
        }
        public void LoseWriteToFile()
        {
            List<Jatekosok> jatekosoklist = new List<Jatekosok>();
            string fullPath = $"jatekosok.txt";
            foreach (string sor in File.ReadAllLines(@"jatekosok.txt"))
            {
                jatekosoklist.Add(new Jatekosok(sor));
            }
            if (Convert.ToInt32(jatekosoklist.Where(j => j.Nev == MainWindow.nev).Count()) >= 1)
            {
                List<string> sorok = new List<string>();
                jatekosoklist.Where(j => j.Nev != MainWindow.nev).ToList().ForEach(j => { sorok.Add($"{j.Nev};{j.Category1WonNumber};{j.Category1LostNumber};{j.Category2WonNumber};{j.Category2LostNumber};{j.Category3WonNumber};{j.Category3LostNumber}"); });
                switch (MainWindow.tipusSzam)
                {
                    case 0:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault() + 1};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault()}");
                        break;
                    case 1:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault() + 1};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault()}");
                        break;
                    case 2:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault() + 1}");
                        break;
                }
                File.WriteAllLines(fullPath, sorok);
            }
            else
            {
                List<string> sorok = new List<string>();
                jatekosoklist.Where(j => j.Nev != MainWindow.nev).ToList().ForEach(j => { sorok.Add($"{j.Nev};{j.Category1WonNumber};{j.Category1LostNumber};{j.Category2WonNumber};{j.Category2LostNumber};{j.Category3WonNumber};{j.Category3LostNumber}"); });
                switch (MainWindow.tipusSzam)
                {
                    case 0:
                        sorok.Add($"{MainWindow.nev};{0};{1};{0};{0};{0};{0}");
                        break;
                    case 1:
                        sorok.Add($"{MainWindow.nev};{0};{0};{0};{1};{0};{0}");
                        break;
                    case 2:
                        sorok.Add($"{MainWindow.nev};{0};{0};{0};{0};{0};{1}");
                        break;
                }
                File.WriteAllLines(fullPath, sorok);
            }
        }
        public void WinWriteToFile()
        {
            List<Jatekosok> jatekosoklist = new List<Jatekosok>();
            string fullPath = $"jatekosok.txt";
            foreach (string sor in File.ReadAllLines(@"jatekosok.txt"))
            {
                jatekosoklist.Add(new Jatekosok(sor));
            }
            if (Convert.ToInt32(jatekosoklist.Where(j => j.Nev == MainWindow.nev).Count()) >= 1)
            {
                List<string> sorok = new List<string>();
                jatekosoklist.Where(j => j.Nev != MainWindow.nev).ToList().ForEach(j => { sorok.Add($"{j.Nev};{j.Category1WonNumber};{j.Category1LostNumber};{j.Category2WonNumber};{j.Category2LostNumber};{j.Category3WonNumber};{j.Category3LostNumber}"); });
                switch (MainWindow.tipusSzam)
                {
                    case 0:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault() + 1};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault()}");
                        break;
                    case 1:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault() + 1};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault()}");
                        break;
                    case 2:
                        sorok.Add($"{MainWindow.nev};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category1LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2WonNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category2LostNumber).SingleOrDefault()};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3WonNumber).SingleOrDefault() + 1};{jatekosoklist.Where(j => j.Nev == MainWindow.nev).Select(j => j.Category3LostNumber).SingleOrDefault()}");
                        break;
                }
                File.WriteAllLines(fullPath, sorok);
            }
            else
            {
                List<string> sorok = new List<string>();
                jatekosoklist.Where(j => j.Nev != MainWindow.nev).ToList().ForEach(j => { sorok.Add($"{j.Nev};{j.Category1WonNumber};{j.Category1LostNumber};{j.Category2WonNumber};{j.Category2LostNumber};{j.Category3WonNumber};{j.Category3LostNumber}"); });
                switch (MainWindow.tipusSzam)
                {
                    case 0:
                        sorok.Add($"{MainWindow.nev};{1};{0};{0};{0};{0};{0}");
                        break;
                    case 1:
                        sorok.Add($"{MainWindow.nev};{0};{0};{1};{0};{0};{0}");
                        break;
                    case 2:
                        sorok.Add($"{MainWindow.nev};{0};{0};{0};{0};{1};{0}");
                        break;
                }
                File.WriteAllLines(fullPath, sorok);
            }
        }
    }
}
