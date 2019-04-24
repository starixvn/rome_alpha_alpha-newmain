namespace Alpha.Main.Criteria.Nickname
{
    using Params.Nickname;

    public class UpdateNicknameLoginOnlyCommand : NicknameNonQueryCommand
    {
        public UpdateNicknameLoginOnlyCommand(UpdateNicknameLoginOnlyParam param)
            : base(param.CustId, param.IP, param.LanguageKey)
        {
            NickNameLoginOnly = param.NickNameLoginOnly;
        }

        public bool NickNameLoginOnly { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdNicknameLoginOnly";
    }
}
