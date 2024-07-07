namespace Przychodnia
{
    public class Wizyta
    {
        public string PeselPacjenta { get; set; }
        public string ImiePacjenta { get; set; }
        public string NazwiskoPacjenta { get; set; }
        public string LoginLekarza { get; set; }
        public DateTime DataWizyty { get; set; }
        public TimeSpan GodzinaWizyty { get; set; }
        public string StatusWizyty { get; set; }
        public string Wywiad { get; set; }
        public string Rozpoznanie { get; set; }
        public string Zalecenia { get; set; }
    }
}
