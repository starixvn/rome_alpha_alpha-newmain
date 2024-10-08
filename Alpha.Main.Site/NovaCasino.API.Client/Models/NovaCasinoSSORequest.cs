using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaCasino.API.Client.Models
{
    public class NovaCasinoSSORequest
    {
        public string Seq => Guid.NewGuid().ToString();

        public string TimeStamp => DateTime.UtcNow.ToString();

        public UserInfo UserInfo { get; set; }
    }
}