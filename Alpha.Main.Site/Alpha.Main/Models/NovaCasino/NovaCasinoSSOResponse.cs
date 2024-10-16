namespace Alpha.Main.Models.NovaCasino
{
    public class NovaCasinoSSOResponse<T> where T : class
    {
        public string Seq { get; set; }

        public string TimeStamp { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }
    }
}