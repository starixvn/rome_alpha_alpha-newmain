using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Main.Params.Password
{
    public class UpdateMainAccountPasswordParam
    {
        public int CustId { get; set; }

        public int ActorId { get; set; }

        public bool IsDisableCasino { get; set; }

        public string NewPassword { get; set; }

        public string CurrentPassword { get; set; }

        public string LanguageKey { get; set; }

        public string Ip { get; set; }

        public bool IsCasinoSync { get; set; }

        public string ActorName { get; set; }

        public string NickName { get; set; }
    }
}
