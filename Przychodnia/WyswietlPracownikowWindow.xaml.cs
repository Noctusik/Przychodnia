using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            LoadData();
            PopulateDataGrid();
        }

        private void LoadData()
        {
            wszyscyPracownicy = PobierzPracownikow();
        }

        private void PopulateDataGrid()
        {
            dataGridPracownicy.ItemsSource = wszyscyPracownicy;
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
                    if (parts.Length == 6)
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
            string filterLogin = txtFilterLogin.Text;
            string filterImie = txtFilterImie.Text;
            string filterNazwisko = txtFilterNazwisko.Text;
            string filterRola = txtFilterRola.Text;
           
            var filteredPracownicy = wszyscyPracownicy.Where(p =>
                (string.IsNullOrWhiteSpace(filterLogin) || p.Login.Contains(filterLogin)) &&
                (string.IsNullOrWhiteSpace(filterImie) || p.Imie.Contains(filterImie)) &&
                (string.IsNullOrWhiteSpace(filterNazwisko) || p.Nazwisko.Contains(filterNazwisko)) &&
                (string.IsNullOrWhiteSpace(filterRola) || p.Rola.Contains(filterRola))                
            ).ToList();

            dataGridPracownicy.ItemsSource = filteredPracownicy;
        }
    }


public class Pracownik
    {
        public string Login { get; set; }
        public string Haslo { get; set; } // To pole jest wczytywane, ale nie będzie wyświetlane
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Rola { get; set; }
        public string Telefon { get; set; }
    }
}
