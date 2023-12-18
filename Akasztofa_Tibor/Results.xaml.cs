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
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Page
    {
        public Results()
        {
            InitializeComponent();
            eddigieredmenyekszoveg.Content = $"{MainWindow.nev} eddigi eredményei: ";
            foreach (string sor in File.ReadAllLines(@"jatekosok.txt"))
            {
                string[] s = sor.Split(';');
                string szo = "";
                if (s[0] == MainWindow.nev)
                {
                    szo += $"Edzőterem témakörben nyert {s[1]}, vesztett {s[2]} játékot. \n";
                    szo += $"Vadászat témakörben nyert {s[3]}, vesztett {s[4]} játékot. \n";
                    szo += $"Tudatmodósítószerek témakörben nyert {s[5]}, vesztett {s[6]} játékot.";
                }
                eredmenyek.Text = szo;
            }
        }
    }
}
