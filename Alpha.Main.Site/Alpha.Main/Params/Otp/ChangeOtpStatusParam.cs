namespace Alpha.Main.Params.Otp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChangeOtpStatusParam
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public int ActorId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public bool IsEnable { get; set; }
    }
}
