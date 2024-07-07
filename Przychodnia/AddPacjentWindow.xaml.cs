using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Przychodnia
{
    public partial class AddPacjentWindow : Window
    {
        private string FilePathPacjenci = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pacjenci.txt");
        private string FilePathUsers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");

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
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(pesel, @"^[0-9]{2}([02468]1|[13579][012])(0[1-9]|1[0-9]|2[0-9]|3[01])[0-9]{5}$"))
            {
                MessageBox.Show("PESEL musi zawierać 11 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(telefon, @"^\d{9}$"))
            {
                MessageBox.Show("Telefon musi zawierać 9 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź, czy pacjent z tym PESELem już istnieje
            if (File.Exists(FilePathPacjenci))
            {
                var lines = File.ReadAllLines(FilePathPacjenci, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 3 && parts[2] == pesel)
                    {
                        MessageBox.Show("Pacjent z tym numerem PESEL już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            // Dodaj pacjenta do pliku
            using (var writer = new StreamWriter(FilePathPacjenci, true, Encoding.UTF8))
            {
                writer.WriteLine($"{imie},{nazwisko},{pesel},{telefon}");
            }

            // Dodaj konto użytkownika
            string login = $"{imie}{nazwisko}".ToLower();
            string haslo = pesel;
            using (var writer = new StreamWriter(FilePathUsers, true, Encoding.UTF8))
            {
                writer.WriteLine($"{login},{haslo},pacjent,{imie},{nazwisko},{telefon}");
            }

            MessageBox.Show("Pacjent został dodany, a konto użytkownika zostało utworzone.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
