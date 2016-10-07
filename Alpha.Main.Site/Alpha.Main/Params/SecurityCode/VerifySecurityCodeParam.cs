namespace Alpha.Main.Params.SecurityCode
{
    public class VerifySecurityCodeParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public bool IsSubAcc { get; set; }

        public string SecurityCode { get; set; }

        public string Salt { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }
    }
}