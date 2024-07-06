using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Przychodnia
{
    public partial class WyswietlPracownikowWindow : Window
    {
        private string FilePathUsers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
        private List<Pracownik> wszyscyPracownicy = new List<Pracownik>();

        public WyswietlPracownikowWindow()
        {
            InitializeComponent();
            LoadPracownicy();
        }

        private void LoadPracownicy()
        {
            wszyscyPracownicy = PobierzPracownikow();
            dataGridPracownicy.ItemsSource = wszyscyPracownicy;
        }

        private List<Pracownik> PobierzPracownikow()
        {
            var pracownicy = new List<Pracownik>();
            if (File.Exists(FilePathUsers))
            {
                var lines = File.ReadAllLines(FilePathUsers);
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

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterImie = txtFilterImie.Text.ToLower();
            string filterNazwisko = txtFilterNazwisko.Text.ToLower();
            string filterRola = txtFilterRola.Text.ToLower();

            var filteredPracownicy = wszyscyPracownicy.Where(p =>
                (string.IsNullOrWhiteSpace(filterImie) || p.Imie.ToLower().Contains(filterImie)) &&
                (string.IsNullOrWhiteSpace(filterNazwisko) || p.Nazwisko.ToLower().Contains(filterNazwisko)) &&
                (string.IsNullOrWhiteSpace(filterRola) || p.Rola.ToLower().Contains(filterRola))
            ).ToList();

            dataGridPracownicy.ItemsSource = filteredPracownicy;
        }
    }

    public class Pracownik
    {
        public string Login { get; set; }
        public string Haslo { get; set; }
        public string Rola { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }
    }
}
