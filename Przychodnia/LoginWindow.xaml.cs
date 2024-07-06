using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Przychodnia
{
    public partial class LoginWindow : Window
    {
        private const string FilePath = "users.txt";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string haslo = txtHaslo.Password;

            var user = SprawdzLogowanie(login, haslo);
            if (user != null)
            {
                MainWindow mainWindow = new MainWindow(user.Item1, user.Item2);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login lub hasło", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Tuple<string, string> SprawdzLogowanie(string login, string haslo)
        {
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Plik użytkowników nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            var users = File.ReadAllLines(FilePath)
                            .Select(line => line.Split(','))
                            .Select(parts => new { Login = parts[0], Haslo = parts[1], Rola = parts[2] })
                            .ToList();

            var user = users.FirstOrDefault(u => u.Login == login && u.Haslo == haslo);
            return user != null ? new Tuple<string, string>(user.Login, user.Rola) : null;
        }
    }
}
