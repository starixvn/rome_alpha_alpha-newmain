namespace Alpha.Main.Params.Nickname
{
    public class CreateNicknameParam
    {
        public CreateNicknameParam(
            int custId,
            string nickname,
            string ip,
            string languageKey)
        {
            CustId = custId;
            Nickname = nickname;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public string Nickname { get; }

        public string IP { get; }

        public string LanguageKey { get; }
    }
}
