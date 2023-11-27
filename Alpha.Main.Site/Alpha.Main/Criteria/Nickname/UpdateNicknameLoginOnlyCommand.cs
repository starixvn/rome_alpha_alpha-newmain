namespace Alpha.Main.Criteria.Nickname
{
    public class UpdateNicknameLoginOnlyCommand : NicknameNonQueryCommand
    {
        public UpdateNicknameLoginOnlyCommand(int custId, bool nickNameLoginOnly, string language, string ip)
            : base(custId, ip, language)
        {
            NickNameLoginOnly = nickNameLoginOnly;
        }

        public bool NickNameLoginOnly { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdNicknameLoginOnly";
    }
}
