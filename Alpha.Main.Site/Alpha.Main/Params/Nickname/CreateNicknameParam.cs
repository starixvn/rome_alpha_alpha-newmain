namespace Alpha.Main.Params.Nickname
{
    public class CreateNicknameParam
    {
        public CreateNicknameParam(
            int custId,
            string nickname,
            string ip,
            string languageKey,
            bool isEnableUpdateNickname)
        {
            CustId = custId;
            Nickname = nickname;
            IP = ip;
            LanguageKey = languageKey;
            IsEnableUpdateNickname = isEnableUpdateNickname;
        }

        public int CustId { get; }

        public string Nickname { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsEnableUpdateNickname { get; }
    }
}
