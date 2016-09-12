namespace Alpha.Main.Params.SecurityCode
{
    public class CheckSecurityCodeParams
    {
        public int CustId { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeToken { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int SubAccountId { get; set; }
    }
}