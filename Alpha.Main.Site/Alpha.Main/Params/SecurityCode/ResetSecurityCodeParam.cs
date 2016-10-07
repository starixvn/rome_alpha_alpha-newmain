namespace Alpha.Main.Params.SecurityCode
{
    public class ResetSecurityCodeParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public int UserId { get; set; }

        public string NewSecurityCode { get; set; }

        public bool IsResetMember { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }
    }
}