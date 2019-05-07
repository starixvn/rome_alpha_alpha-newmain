namespace Alpha.Main.Params.Nickname
{
    public class UpdateNicknameLoginOnlyParam
    {
        public UpdateNicknameLoginOnlyParam(
            int custId,
            bool nickNameLoginOnly,
            string ip,
            string languageKey)
        {
            CustId = custId;
            NickNameLoginOnly = nickNameLoginOnly;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public bool NickNameLoginOnly { get; }

        public string IP { get; }

        public string LanguageKey { get; }
    }
}
