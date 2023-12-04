using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Game : Page
    {
        private List<string> biologia = new List<string>();
        private List<string> matematika = new List<string>();
        private List<string> informatika = new List<string>();
        private List<string> tippek = new List<string>();
        private List<string> jotippek = new List<string>();
        private List<char> titkositottszo = new List<char>();
        private string randomszo;
        private int korszam = 1;
        private int maxszam = MainWindow.maxszam;
        private int probalkozas = 1;
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
                    szo += $"Biológia témakörben nyert {s[1]}, vesztett {s[2]} játékot. \n" ;
                    szo += $"Matematika témakörben nyert {s[3]}, vesztett {s[4]} játékot. \n";
                    szo += $"Informatika témakörben nyert {s[5]}, vesztett {s[6]} játékot.";
                }
                else
                {
                    szo += $"Biológia témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Matematika témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Informatika témakörben nyert 0, vesztett 0 játékot.";
                }
                eredmenyek.Text = szo;
            }
            foreach (string sor in File.ReadAllLines(@"szavak.txt"))
            {
                string[] s = sor.Split(';');
                if (s[1] == "b") biologia.Add(s[0]);
                if (s[1] == "m") matematika.Add(s[0]);
                if (s[1] == "i") informatika.Add(s[0]);
            }
            switch (MainWindow.tipusSzam) {
                case 0:
                    temakor.Text = "Biológia";
                    randomszo = biologia[random.Next(biologia.Count)];
                    break;
                case 1:
                    temakor.Text = "Matematika";
                    randomszo = matematika[random.Next(matematika.Count)];
                    break;
                case 2:
                    temakor.Text = "Informatika";
                    randomszo = informatika[random.Next(informatika.Count)];
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
            if (probalkozas <= maxszam)
            {
                tipButton.Content = $"Tipp {probalkozas++}/{maxszam}";
                tippek.Add(bemenet.Text);
                if (randomszo.Contains(bemenet.Text))
                { 
                    jotippek.Add(bemenet.Text); 
                    Ellenorzo(Convert.ToChar(bemenet.Text));
                }
                esemenytabla.Text += $"{korszam}.kör. Tippelt betű: {bemenet.Text} Találat: {IgenNem(randomszo.Contains(bemenet.Text))} \n";
                korszam++;
            }
        }
        public void eredmenyekClick(object sender, RoutedEventArgs e) { MainFrame.Content = new Results(); }
        public void megfejtesClick(object sender, RoutedEventArgs e) { megfejtesout.Text = randomszo.ToString(); }
    }
}
