using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Przychodnia
{
    public partial class AddPracownikWindow : Window
    {
        private string FilePathUsers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");

        public AddPracownikWindow()
        {
            InitializeComponent();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            string imie = txtImie.Text;
            string nazwisko = txtNazwisko.Text;
            string telefon = txtTelefon.Text;
            string login = txtLogin.Text;
            string haslo = txtHaslo.Password;
            string rola = ((ComboBoxItem)comboRola.SelectedItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) ||
                string.IsNullOrWhiteSpace(telefon) || string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(haslo) || string.IsNullOrWhiteSpace(rola))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidTelefon(telefon))
            {
                MessageBox.Show("Numer telefonu musi zawierać dokładnie 9 cyfr.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsUniqueLogin(login))
            {
                MessageBox.Show("Login użytkownika już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (StreamWriter swUsers = new StreamWriter(FilePathUsers, true))
                {
                    swUsers.WriteLine($"{login},{haslo},{rola},{imie},{nazwisko},{telefon}");
                }

                MessageBox.Show("Pracownik został dodany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }

        private bool IsValidTelefon(string telefon)
        {
            string pattern = @"^[0-9]{9}$";
            return Regex.IsMatch(telefon, pattern);
        }

        private bool IsUniqueLogin(string login)
        {
            if (File.Exists(FilePathUsers))
            {
                var users = File.ReadAllLines(FilePathUsers);
                return !users.Any(u => u.Split(',')[0] == login);
            }
            return true;
        }
    }
}
