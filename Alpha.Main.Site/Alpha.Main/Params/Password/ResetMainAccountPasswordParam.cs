namespace Alpha.Main.Params.Password
{
    public class ResetMainAccountPasswordParam
    {
        public int CustId { get; set; }

        public int ActorId { get; set; }

        public int SubAccountId { get; set; }

        public bool IsDisableCasino { get; set; }

        public string NewPassword { get; set; }

        public string LanguageKey { get; set; }

        public string Ip { get; set; }
    }
}