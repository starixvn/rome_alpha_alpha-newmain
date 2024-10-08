namespace NovaCasino.API.Client.Models
{
    public class NovaCasinoSSOResponse
    {
        public string Seq { get; set; }

        public string TimeStamp { get; set; }

        public string LaunchUrl { get; set; }

        public string[] CustIds { get; set; }
    }
}