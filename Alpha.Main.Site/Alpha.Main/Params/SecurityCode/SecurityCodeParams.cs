namespace Alpha.Main.Params.Announcements
{
    public class SecurityCodeParams
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string RandomKey { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int SubAccountId { get; set; }

        public bool IsResetMember { get; set; }
    }
}