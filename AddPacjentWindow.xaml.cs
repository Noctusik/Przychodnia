using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Przychodnia
{
    public partial class AddPacjentWindow : Window
    {
        private string FilePathPacjenci = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pacjenci.txt");

        public AddPacjentWindow()
        {
            InitializeComponent();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            string imie = txtImie.Text;
            string nazwisko = txtNazwisko.Text;
            string pesel = txtPesel.Text;
            string telefon = txtTelefon.Text;

            if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) || string.IsNullOrWhiteSpace(pesel) || string.IsNullOrWhiteSpace(telefon))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPesel(pesel))
            {
                MessageBox.Show("Nieprawidłowy numer PESEL.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidTelefon(telefon))
            {
                MessageBox.Show("Numer telefonu musi zawierać dokładnie 9 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pacjenci = PobierzPacjentow();
            if (pacjenci.Any(p => p.Pesel == pesel))
            {
                MessageBox.Show("Pacjent o podanym numerze PESEL już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(FilePathPacjenci, true))
                {
                    sw.WriteLine($"{imie},{nazwisko},{pesel},{telefon}");
                }
                MessageBox.Show("Pacjent został dodany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }

        private bool IsValidPesel(string pesel)
        {
            string pattern = @"^[0-9]{2}([02468][1]|[13579][012])(0[1-9]|1[0-9]|2[0-9]|3[01])[0-9]{5}$";
            return Regex.IsMatch(pesel, pattern);
        }

        private bool IsValidTelefon(string telefon)
        {
            string pattern = @"^[0-9]{9}$";
            return Regex.IsMatch(telefon, pattern);
        }

        private List<Pacjent> PobierzPacjentow()
        {
            var pacjenci = new List<Pacjent>();
            if (File.Exists(FilePathPacjenci))
            {
                var lines = File.ReadAllLines(FilePathPacjenci);
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
    }
}
