namespace Alpha.Main.Params.SecurityCode
{
    public class SecurityCodeParams
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public int UserId { get; set; }

        public bool IsValidSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public bool IsResetMember { get; set; }

        public string HashSecurityCode { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }
    }
}