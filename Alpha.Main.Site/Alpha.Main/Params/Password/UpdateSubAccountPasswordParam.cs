namespace Alpha.Main.Params.Password
{
    public class UpdateSubAccountPasswordParam
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string NewPassword { get; set; }

        public string CurrentPassword { get; set; }

        public string LanguageKey { get; set; }

        public string Username { get; set; }

        public string SubUsername { get; set; }

        public string Nickname { get; set; }

        public string Ip { get; set; }
    }
}