using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Przychodnia
{
    public partial class WyswietlPacjentowWindow : Window
    {
        private string FilePathPacjenci = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pacjenci.txt");
        private List<Pacjent> wszyscyPacjenci = new List<Pacjent>();

        public WyswietlPacjentowWindow()
        {
            InitializeComponent();
            LoadPacjenci();
        }

        private void LoadPacjenci()
        {
            wszyscyPacjenci = PobierzPacjentow();
            dataGridPacjenci.ItemsSource = wszyscyPacjenci;
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

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterImie = txtFilterImie.Text.ToLower();
            string filterNazwisko = txtFilterNazwisko.Text.ToLower();
            string filterPesel = txtFilterPesel.Text;

            var filteredPacjenci = wszyscyPacjenci.Where(p =>
                (string.IsNullOrWhiteSpace(filterImie) || p.Imie.ToLower().Contains(filterImie)) &&
                (string.IsNullOrWhiteSpace(filterNazwisko) || p.Nazwisko.ToLower().Contains(filterNazwisko)) &&
                (string.IsNullOrWhiteSpace(filterPesel) || p.Pesel.Contains(filterPesel))
            ).ToList();

            dataGridPacjenci.ItemsSource = filteredPacjenci;
        }
    }
}
