using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Przychodnia
{
    public partial class RaportWindow : Window
    {
        public RaportWindow(List<Wizyta> wizyty)
        {
            InitializeComponent();
            var raportData = GenerujRaport(wizyty);
            dataGridRaport.ItemsSource = raportData;
        }

        private IEnumerable<RaportItem> GenerujRaport(List<Wizyta> wizyty)
        {
            var raport = wizyty
                .GroupBy(w => new { w.LoginLekarza, w.DataWizyty.Date })
                .Select(g => new RaportItem
                {
                    Lekarz = g.Key.LoginLekarza,
                    Data = g.Key.Date,
                    LiczbaWizyt = g.Count()
                }).ToList();

            return raport;
        }
    }

    public class RaportItem
    {
        public string Lekarz { get; set; }
        public DateTime Data { get; set; }
        public int LiczbaWizyt { get; set; }
    }
}
