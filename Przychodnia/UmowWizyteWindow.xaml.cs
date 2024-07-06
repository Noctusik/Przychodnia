using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Przychodnia
{
    public partial class UmowWizyteWindow : Window
    {
        private string FilePathWizyty = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wizyty.txt");
        private string FilePathUsers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
        private List<Pracownik> wszyscyLekarze = new List<Pracownik>();
        private List<Pacjent> wszyscyPacjenci = new List<Pacjent>();

        public UmowWizyteWindow()
        {
            InitializeComponent();
            LoadData();
            PopulateComboBoxes();
        }

        private void LoadData()
        {
            wszyscyLekarze = PobierzPracownikow().Where(p => p.Rola == "lekarz").ToList();
            wszyscyPacjenci = PobierzPacjentow();
        }

        private void PopulateComboBoxes()
        {
            comboLekarz.ItemsSource = wszyscyLekarze.Select(l => $"{l.Imie} {l.Nazwisko}").ToList();
            datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
            comboLekarz.SelectionChanged += ComboLekarz_SelectionChanged;
        }

        private void ComboLekarz_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAvailableHours();
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAvailableHours();
        }

        private void UpdateAvailableHours()
        {
            if (comboLekarz.SelectedIndex < 0 || !datePicker.SelectedDate.HasValue)
            {
                comboGodzina.IsEnabled = false;
                return;
            }

            comboGodzina.IsEnabled = true;

            var wybranyLekarz = wszyscyLekarze[comboLekarz.SelectedIndex];
            var dataWizyty = datePicker.SelectedDate.Value;

            var godzinyPracy = new List<string>();
            for (int godzina = 10; godzina < 20; godzina++)
            {
                godzinyPracy.Add($"{godzina}:00");
                godzinyPracy.Add($"{godzina}:30");
            }

            var zajeteGodziny = PobierzZajeteGodziny(wybranyLekarz, dataWizyty);
            var dostepneGodziny = godzinyPracy.Except(zajeteGodziny).ToList();

            if (dataWizyty == DateTime.Today)
            {
                // Usuń godziny z przeszłości względem obecnej godziny
                var aktualnaGodzina = DateTime.Now.TimeOfDay;
                dostepneGodziny = dostepneGodziny.Where(g => TimeSpan.Parse(g) > aktualnaGodzina).ToList();
            }

            comboGodzina.ItemsSource = dostepneGodziny;
        }

        private List<string> PobierzZajeteGodziny(Pracownik lekarz, DateTime data)
        {
            var zajeteGodziny = new List<string>();
            if (File.Exists(FilePathWizyty))
            {
                var lines = File.ReadAllLines(FilePathWizyty, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        var wizytaLekarz = parts[1];
                        var wizytaData = DateTime.Parse(parts[2]);
                        var wizytaGodzina = TimeSpan.Parse(parts[3]);

                        if (lekarz.Login == wizytaLekarz && data == wizytaData)
                        {
                            zajeteGodziny.Add(wizytaGodzina.ToString(@"hh\:mm"));
                        }
                    }
                }
            }
            return zajeteGodziny;
        }

        private List<Pracownik> PobierzPracownikow()
        {
            var pracownicy = new List<Pracownik>();
            if (File.Exists(FilePathUsers))
            {
                var lines = File.ReadAllLines(FilePathUsers, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 6) // Sprawdź, czy liczba elementów jest odpowiednia
                    {
                        var pracownik = new Pracownik
                        {
                            Login = parts[0],
                            Haslo = parts[1],
                            Rola = parts[2],
                            Imie = parts[3],
                            Nazwisko = parts[4],
                            Telefon = parts[5]
                        };
                        pracownicy.Add(pracownik);
                    }
                }
            }
            return pracownicy;
        }

        private List<Pacjent> PobierzPacjentow()
        {
            var pacjenci = new List<Pacjent>();
            if (File.Exists("pacjenci.txt"))
            {
                var lines = File.ReadAllLines("pacjenci.txt", Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4) // Sprawdź, czy liczba elementów jest odpowiednia
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

        private void btnUmowWizyte_Click(object sender, RoutedEventArgs e)
        {
            var pesel = txtPesel.Text;

            if (!IsValidPesel(pesel))
            {
                MessageBox.Show("Nieprawidłowy numer PESEL.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var wybranyPacjent = wszyscyPacjenci.FirstOrDefault(p => p.Pesel == pesel);
            if (wybranyPacjent == null)
            {
                MessageBox.Show("Pacjent o podanym numerze PESEL nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var wybranyLekarz = wszyscyLekarze[comboLekarz.SelectedIndex];
            var dataWizyty = datePicker.SelectedDate.Value;
            var godzinaWizyty = TimeSpan.Parse(comboGodzina.SelectedItem.ToString());

            if (!CzyLekarzDostepny(wybranyLekarz, dataWizyty, godzinaWizyty))
            {
                MessageBox.Show("Wybrany lekarz jest zajęty w tym czasie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (StreamWriter swWizyty = new StreamWriter(FilePathWizyty, true, Encoding.UTF8))
                {
                    swWizyty.WriteLine($"{wybranyPacjent.Pesel},{wybranyLekarz.Login},{dataWizyty:yyyy-MM-dd},{godzinaWizyty},Oczekująca");
                }

                MessageBox.Show("Wizyta została umówiona.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }

        private bool IsValidPesel(string pesel)
        {
            string pattern = @"^[0-9]{11}$";
            return Regex.IsMatch(pesel, pattern);
        }

        private bool CzyLekarzDostepny(Pracownik lekarz, DateTime data, TimeSpan godzina)
        {
            if (File.Exists(FilePathWizyty))
            {
                var lines = File.ReadAllLines(FilePathWizyty, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        var wizytaLekarz = parts[1];
                        var wizytaData = DateTime.Parse(parts[2]);
                        var wizytaGodzina = TimeSpan.Parse(parts[3]);

                        if (lekarz.Login == wizytaLekarz && data == wizytaData && godzina == wizytaGodzina)
                        {
                            return false; // Lekarz jest zajęty
                        }
                    }
                }
            }
            return true; // Lekarz jest dostępny
        }
    }

    
}
