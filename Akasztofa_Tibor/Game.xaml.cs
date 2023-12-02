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
        private string randomszo;
        public Game()
        {
            InitializeComponent();
            eddigieredmenyekszoveg.Content = $"{MainWindow.nev} eddigi eredményei: ";
            Random random = new Random();
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
            megfejtendopelda.Text = randomszo;
        }
    }
}
