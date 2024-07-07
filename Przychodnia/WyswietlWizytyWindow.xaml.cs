using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Przychodnia
{
    public partial class WyswietlWizytyWindow : Window
    {
        private string FilePathWizyty = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wizyty.txt");
        private string FilePathPacjenci = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pacjenci.txt");
        private List<Wizyta> wszystkieWizyty = new List<Wizyta>();
        private List<Pacjent> wszyscyPacjenci = new List<Pacjent>();
        private string userLogin;
        private string userRole;

        public WyswietlWizytyWindow(string login, string role)
        {
            InitializeComponent();
            userLogin = login;
            userRole = role;
            LoadData();
            PopulateDataGrid();
        }

        private void LoadData()
        {
            wszyscyPacjenci = PobierzPacjentow();
            wszystkieWizyty = PobierzWizyty();
            PolaczWizytyZPacjentami();
        }

        private void PopulateDataGrid()
        {
            dataGridWizyty.ItemsSource = wszystkieWizyty;
        }

        private List<Pacjent> PobierzPacjentow()
        {
            var pacjenci = new List<Pacjent>();
            if (File.Exists(FilePathPacjenci))
            {
                var lines = File.ReadAllLines(FilePathPacjenci, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        var pacjent = new Pacjent
                        {
                            Imie = parts[0],
                            Nazwisko = parts[1],
                            Pesel = parts[2],
                            Telefon = parts[3]
                        };
                        pacjenci.Add(pacjent);
                    }
                }
            }
            return pacjenci;
        }

        private List<Wizyta> PobierzWizyty()
        {
            var wizyty = new List<Wizyta>();
            if (File.Exists(FilePathWizyty))
            {
                var lines = File.ReadAllLines(FilePathWizyty, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 5)
                    {
                        var wizyta = new Wizyta
                        {
                            PeselPacjenta = parts[0],
                            LoginLekarza = parts[1],
                            DataWizyty = DateTime.Parse(parts[2]),
                            GodzinaWizyty = TimeSpan.Parse(parts[3]),
                            StatusWizyty = parts[4],
                            Wywiad = parts.Length > 5 ? parts[5] : string.Empty,
                            Rozpoznanie = parts.Length > 6 ? parts[6] : string.Empty,
                            Zalecenia = parts.Length > 7 ? parts[7] : string.Empty
                        };
                        wizyty.Add(wizyta);
                    }
                }
            }
            return wizyty;
        }

        private void PolaczWizytyZPacjentami()
        {
            foreach (var wizyta in wszystkieWizyty)
            {
                var pacjent = wszyscyPacjenci.FirstOrDefault(p => p.Pesel == wizyta.PeselPacjenta);
                if (pacjent != null)
                {
                    wizyta.ImiePacjenta = pacjent.Imie;
                    wizyta.NazwiskoPacjenta = pacjent.Nazwisko;
                }
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterPesel = txtFilterPesel.Text;
            string filterLoginLekarza = txtFilterLoginLekarza.Text;
            string filterStatus = (comboStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            var filteredWizyty = wszystkieWizyty.Where(w =>
                (string.IsNullOrWhiteSpace(filterPesel) || w.PeselPacjenta.Contains(filterPesel)) &&
                (string.IsNullOrWhiteSpace(filterLoginLekarza) || w.LoginLekarza.Contains(filterLoginLekarza)) &&
                (filterStatus == "Wszystkie" || string.IsNullOrWhiteSpace(filterStatus) || w.StatusWizyty == filterStatus)
            ).ToList();

            dataGridWizyty.ItemsSource = filteredWizyty;
        }

        private void dataGridWizyty_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridWizyty.SelectedItem is Wizyta wybranaWizyta)
            {
                if (userRole == "lekarz")
                {
                    ObsluzWizyteWindow obsluzWizyteWindow = new ObsluzWizyteWindow(wybranaWizyta);
                    obsluzWizyteWindow.ShowDialog();
                }
                else if (userRole == "rejestrator")
                {
                    if (wybranaWizyta.StatusWizyty == "Zrealizowana")
                    {
                        FinansowaObslugaWindow finansowaObslugaWindow = new FinansowaObslugaWindow(wybranaWizyta);
                        finansowaObslugaWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Tylko wizyty ze statusem 'Zrealizowana' mogą być obsługiwane finansowo.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                LoadData(); // Odświeżenie listy wizyt po obsłudze wizyty
                PopulateDataGrid();
            }
            else
            {
                MessageBox.Show("Proszę wybrać wizytę do obsłużenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}