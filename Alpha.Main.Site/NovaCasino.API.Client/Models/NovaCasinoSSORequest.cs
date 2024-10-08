using System;

namespace NovaCasino.API.Client.Models
{
    public class NovaCasinoSSORequest
    {
        public string Seq => Guid.NewGuid().ToString();

        public string TimeStamp => DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

        public UserInfo UserInfo { get; set; }
    }
}