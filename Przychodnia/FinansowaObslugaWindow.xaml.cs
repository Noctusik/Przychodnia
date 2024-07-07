using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32; // Dodaj to
using iText = iTextSharp.text;
using iTextSharp.text.pdf;

namespace Przychodnia
{
    public partial class FinansowaObslugaWindow : Window
    {
        private Wizyta wizyta;

        public FinansowaObslugaWindow(Wizyta wizyta)
        {
            InitializeComponent();
            this.wizyta = wizyta;

            if (wizyta.StatusWizyty != "Zrealizowana")
            {
                MessageBox.Show("Tylko wizyty ze statusem 'Zrealizowana' mogą być obsługiwane finansowo.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnGenerujRachunek_Click(object sender, RoutedEventArgs e)
        {
            decimal kwota;
            if (decimal.TryParse(txtKwota.Text, out kwota))
            {
                wizyta.StatusWizyty = "Opłacona";
                UpdateWizytaInFile();
                GenerujRachunekPdf(wizyta, kwota);
                MessageBox.Show("Rachunek został wygenerowany i zapisany jako PDF.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Proszę wprowadzić poprawną kwotę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateWizytaInFile()
        {
            string FilePathWizyty = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wizyty.txt");
            var wizyty = File.ReadAllLines(FilePathWizyty, Encoding.UTF8);
            for (int i = 0; i < wizyty.Length; i++)
            {
                var parts = wizyty[i].Split(',');
                if (parts.Length >= 5 && parts[0] == wizyta.PeselPacjenta && parts[1] == wizyta.LoginLekarza && DateTime.Parse(parts[2]) == wizyta.DataWizyty && TimeSpan.Parse(parts[3]) == wizyta.GodzinaWizyty)
                {
                    wizyty[i] = $"{wizyta.PeselPacjenta},{wizyta.LoginLekarza},{wizyta.DataWizyty:yyyy-MM-dd},{wizyta.GodzinaWizyty},{wizyta.StatusWizyty},{wizyta.Wywiad},{wizyta.Rozpoznanie},{wizyta.Zalecenia}";
                    break;
                }
            }
            File.WriteAllLines(FilePathWizyty, wizyty, Encoding.UTF8);
        }

        private void GenerujRachunekPdf(Wizyta wizyta, decimal kwota)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Zapisz rachunek jako";
            saveFileDialog.FileName = "rachunek.pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                iText.Document document = new iText.Document();
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                document.Open();

                document.Add(new iText.Paragraph("Rachunek z dnia " + DateTime.Now.ToString("yyyy-MM-dd")));
                document.Add(new iText.Paragraph($"Data Wizyty: {wizyta.DataWizyty:yyyy-MM-dd}"));
                document.Add(new iText.Paragraph($"Godzina Wizyty: {wizyta.GodzinaWizyty}"));
                document.Add(new iText.Paragraph($"Pacjent: {wizyta.ImiePacjenta} {wizyta.NazwiskoPacjenta}"));
                document.Add(new iText.Paragraph($"Pesel: {wizyta.PeselPacjenta}"));
                document.Add(new iText.Paragraph($"Lekarz: {wizyta.LoginLekarza}"));
                document.Add(new iText.Paragraph($"Kwota: {kwota.ToString("C", new CultureInfo("pl-PL"))}"));
                document.Add(new iText.Paragraph($"Status: {wizyta.StatusWizyty}"));

                document.Close();
            }
        }
    }
}