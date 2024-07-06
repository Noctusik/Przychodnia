using System.Windows;

namespace Przychodnia
{
    public partial class MainWindow : Window
    {
        private string _login;
        private string _rola;

        public MainWindow(string login, string rola)
        {
            InitializeComponent();
            _login = login;
            _rola = rola;
            SetupUI();
        }
        
        private void SetupUI()
        {
            switch (_rola)
            {
                case "administrator":
                    btnDodajPacjenta.Visibility = Visibility.Visible;
                    btnDodajPracownika.Visibility = Visibility.Visible;
                    btnUmowWizyte.Visibility = Visibility.Visible;
                    btnWyswietlPacjentow.Visibility = Visibility.Visible;
                    btnWyswietlPracownikow.Visibility = Visibility.Visible;
                    btnWyswietlWizyty.Visibility = Visibility.Visible;
                    btnWyswietlRaporty.Visibility = Visibility.Visible;
                    break;
                case "rejestrator":
                    btnDodajPacjenta.Visibility = Visibility.Visible;
                    btnUmowWizyte.Visibility = Visibility.Visible;
                    btnWyswietlPacjentow.Visibility = Visibility.Visible;
                    btnWyswietlPracownikow.Visibility = Visibility.Visible;
                    btnWyswietlWizyty.Visibility = Visibility.Visible;
                    break;
                case "lekarz":
                    btnWyswietlPacjentow.Visibility = Visibility.Visible;
                    btnWyswietlPracownikow.Visibility = Visibility.Visible;
                    btnWyswietlWizyty.Visibility = Visibility.Visible;
                    break;
                case "pacjent":
                    btnWyswietlWizyty.Visibility = Visibility.Visible;
                    break;
                case "dyrektor":
                    btnWyswietlRaporty.Visibility = Visibility.Visible;
                    break;
                default:
                    MessageBox.Show("Nieznana rola użytkownika.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    break;
            }
        }
        private void btnDodajPacjenta_Click(object sender, RoutedEventArgs e)
        {
            AddPacjentWindow addPacjentWindow = new AddPacjentWindow();
            addPacjentWindow.ShowDialog();
        }
        private void btnWyswietlPacjentow_Click(object sender, RoutedEventArgs e)
        {
            WyswietlPacjentowWindow wyswietlPacjentowWindow = new WyswietlPacjentowWindow();
            wyswietlPacjentowWindow.ShowDialog();
        }
        private void btnDodajPracownika_Click(object sender, RoutedEventArgs e)
        {
            AddPracownikWindow addPracownikWindow = new AddPracownikWindow();
            addPracownikWindow.ShowDialog();
        }
        private void btnWyswietlPracownikow_Click(object sender, RoutedEventArgs e)
        {
            WyswietlPracownikowWindow wyswietlPracownikowWindow = new WyswietlPracownikowWindow();
            wyswietlPracownikowWindow.ShowDialog();
        }
        private void btnUmowWizyte_Click(object sender, RoutedEventArgs e)
        {
            UmowWizyteWindow umowWizyteWindow = new UmowWizyteWindow();
            umowWizyteWindow.ShowDialog();
        }



    }

}
