namespace Alpha.Main.Params.Password
{
    public class ResetSubAccountPasswordParam
    {
        public int SubAccountId { get; set; }

        public int CustId { get; set; }

        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public bool IsCasinoDisabled { get; set; }

        public string NewPassword { get; set; }

        public string LanguageKey { get; set; }

        public string Ip { get; set; }

        public string HashKey { get; set; }
    }
}