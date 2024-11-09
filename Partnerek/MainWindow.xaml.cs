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

namespace Partnerek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFelvesz_Click(object sender, RoutedEventArgs e)
        {
            string adatok;
            string paradat;
            bool bennevanE = false;
            string seged;
            if (txtEmail.Text != "" && txtNev.Text != "" && txtSzulinap.Text != "")
            {
                paradat = txtNev.Text + ";" + txtBecenev.Text + ";" + txtEmail.Text + ";" + txtSzulinap.Text;
                adatok = txtNev.Text + ";" + txtBecenev.Text + ";" + txtEmail.Text + ";" + txtSzulinap.Text + ";";

                if (chkKapotte.IsChecked == true)
                {
                    adatok += "irtam emailt";
                }
                else adatok += "nem irtam emailt";

                for (int i = 0; i < lbLista.Items.Count; i++)
                {
                    seged = lbLista.Items[i].ToString();
                    if (seged.Contains(paradat))
                    {
                        bennevanE = true;
                        break;
                    }
                }

                if (!bennevanE)
                {
                    lbLista.Items.Add(adatok);
                    mezoReset();
                }
                else MessageBox.Show("Már egyszer benne van");
            }
            else MessageBox.Show("Valami kimaradt");
        }

        private void btnMentes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter fajlba = new StreamWriter("adatok.csv");
                foreach (var item in lbLista.Items)
                {
                    fajlba.WriteLine(item.ToString());
                }
                fajlba.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Hiba van a fájllal");
            }
        }

        private void btnTorles_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter fajlba = new StreamWriter("adatok.csv");
            if (lbLista.SelectedIndex >= 0)
            {
                lbLista.Items.RemoveAt(lbLista.SelectedIndex);

                foreach (var item in lbLista.Items)
                {
                    fajlba.WriteLine(item.ToString());
                }
                fajlba.Close();

            }
            else MessageBox.Show("Nincs kiválasztott");

        }

        private void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            if (lbLista.SelectedIndex >= 0)
            {
                if (txtEmail.Text != "" && txtNev.Text != "" && txtSzulinap.Text != "")
                {

                    if (chkKapotte.IsChecked == true)
                    {
                        lbLista.Items[lbLista.SelectedIndex] = txtNev.Text + ";" + txtBecenev.Text + ";" + txtEmail.Text + ";" + txtSzulinap.Text + ";" + "irtam emailt";
                    }
                    else lbLista.Items[lbLista.SelectedIndex] = txtNev.Text + ";" + txtBecenev.Text + ";" + txtEmail.Text + ";" + txtSzulinap.Text + ";" + "nem irtam emailt";
                    mezoReset();
                }

                else MessageBox.Show("Valami kimaradt");
            }
            else MessageBox.Show("Nincs kiválasztott");
        }

        private void lbLista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string[] seged = lbLista.SelectedItem.ToString().Split(';');

            txtNev.Text = seged[0];
            txtBecenev.Text = seged[1];
            txtEmail.Text = seged[2];
            txtSzulinap.Text = seged[3];
            if (seged[4].ToLower() == "irtam emailt")
            {
                chkKapotte.IsChecked = true;
            }
            else chkKapotte.IsChecked = false;

        }

        private void btnBetoltes_Click(object sender, RoutedEventArgs e)
        {
            lbLista.Items.Clear();
            mezoReset();

            try
            {
                List<string> sorok = File.ReadAllLines("adatok.csv").ToList();


                for (int i = 0; i < sorok.Count; i++)
                {
                    lbLista.Items.Add(sorok[i]);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nincs is ilyen fájl");
            }
        }
        public void mezoReset()
        {
            txtNev.Text = "";
            txtBecenev.Text = "";
            txtEmail.Text = "";
            txtSzulinap.Text = "";
            chkKapotte.IsChecked = false;
        }

        private void btnListaTorles_Click(object sender, RoutedEventArgs e)
        {
            lb2lista.Items.Clear();
        }

        private void lb2lista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string[] seged = lb2lista.SelectedItem.ToString().Split(';');

            txtNev.Text = seged[0];
            txtBecenev.Text = seged[1];
            txtEmail.Text = seged[2];
            txtSzulinap.Text = seged[3];
            if (seged[4].ToLower() == "irtam emailt")
            {
                chkKapotte.IsChecked = true;
            }
            else chkKapotte.IsChecked = false;

        }

        private void txtSzulinap_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.]");
            e.Handled = reg.IsMatch(e.Text);
            txtSzulinap.MaxLength = 10;
        }

        private void btnNemIrtam_Click(object sender, RoutedEventArgs e)
        {
            List<string> seged;
            for (int i = 0; i < lbLista.Items.Count; i++)
            {
                if (!lb2lista.Items.Contains(lbLista.Items[i].ToString()))
                {
                    seged = lbLista.Items[i].ToString().Split(';').ToList();
                    if (seged[4].ToLower().Contains("nem"))
                    {
                        lb2lista.Items.Add(seged[0] + ";" + seged[1] + ";" + seged[2] + ";" + seged[3] + ";" + seged[4]);
                    }
                }
            }
        }

        private void btnSzulinap_Click(object sender, RoutedEventArgs e)
        {
            string mostaniHonap = DateTime.Now.Month.ToString();
            string mostaniNap = DateTime.Now.Day.ToString();
            string mostaniEv = DateTime.Now.Year.ToString();

            string datum = mostaniEv + "." + mostaniHonap + "." + mostaniNap;
            List<string> datumTores;
            for (int i = 0; i < lbLista.Items.Count; i++)
            {
                if (!lb2lista.Items.Contains(lbLista.Items[i].ToString()))
                {
                    datumTores = lbLista.Items[i].ToString().Split(';')[3].Split('.').ToList();
                    if (datumTores[1][0] == '0')
                    {
                        datumTores[1] = datumTores[1][1].ToString();
                    }
                    if (datumTores[2][0] == '0')
                    {
                        datumTores[2] = datumTores[2][1].ToString();
                    }

                    if (Convert.ToInt32(mostaniHonap) == Convert.ToInt32(datumTores[1]))
                    {
                        lb2lista.Items.Add(lbLista.Items[i]);
                    }
                    else if (Convert.ToInt32(mostaniHonap) + 1 == Convert.ToInt32(datumTores[1]) || Convert.ToInt32(mostaniHonap) == 12 && Convert.ToInt32(datumTores[1]) == 1)
                    {
                        if (Convert.ToInt32(mostaniNap) <= Convert.ToInt32(datumTores[2]))
                        {
                            lb2lista.Items.Add(lbLista.Items[i]);
                        }
                    }
                }

            }
        }
    }
}
