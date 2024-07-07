using System;
using System.IO;
using System.Text;
using System.Windows;

namespace Przychodnia
{
    public partial class ObsluzWizyteWindow : Window
    {
        private string FilePathWizyty = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wizyty.txt");
        private Wizyta wizyta;

        public ObsluzWizyteWindow(Wizyta wizyta)
        {
            InitializeComponent();
            this.wizyta = wizyta;
            txtWywiad.Text = wizyta.Wywiad;
            txtRozpoznanie.Text = wizyta.Rozpoznanie;
            txtZalecenia.Text = wizyta.Zalecenia;

            if (wizyta.StatusWizyty == "Zrealizowana")
            {
                txtWywiad.IsEnabled = false;
                txtRozpoznanie.IsEnabled = false;
                txtZalecenia.IsEnabled = false;
                btnZapisz.IsEnabled = false;
                btnAnuluj.IsEnabled = false;
            }
        }

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            wizyta.Wywiad = txtWywiad.Text;
            wizyta.Rozpoznanie = txtRozpoznanie.Text;
            wizyta.Zalecenia = txtZalecenia.Text;
            wizyta.StatusWizyty = "Zrealizowana";

            // Aktualizacja pliku wizyt
            UpdateWizytaInFile();

            MessageBox.Show("Wizyta została zrealizowana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz anulować wizytę?", "Anulowanie wizyty", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                wizyta.StatusWizyty = "Anulowana";

                // Aktualizacja pliku wizyt
                UpdateWizytaInFile();

                MessageBox.Show("Wizyta została anulowana.", "Anulowano", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void UpdateWizytaInFile()
        {
            var wizyty = File.ReadAllLines(FilePathWizyty, Encoding.UTF8);
            for (int i = 0; i < wizyty.Length; i++)
            {
                var parts = wizyty[i].Split(',');
                if (parts.Length >= 5 && parts[0] == wizyta.PeselPacjenta && parts[1] == wizyta.LoginLekarza && DateTime.Parse(parts[2]) == wizyta.DataWizyty && TimeSpan.Parse(parts[3]) == wizyta.GodzinaWizyty)
                {
                    wizyty[i] = $"{wizyta.PeselPacjenta},{wizyta.LoginLekarza},{wizyta.DataWizyty:yyyy-MM-dd},{wizyta.GodzinaWizyty},{wizyta.StatusWizyty},{wizyta.Wywiad},{wizyta.Rozpoznanie},{wizyta.Zalecenia}";
                    break;
                }
            }
            File.WriteAllLines(FilePathWizyty, wizyty, Encoding.UTF8);
        }
    }
}
