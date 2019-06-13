namespace Alpha.Main.Criteria.Nickname
{
    using Params.Nickname;

    public class CreateSubAccountNicknameCommand : CreateNicknameCommand
    {
        public CreateSubAccountNicknameCommand(CreateNicknameParam param)
            : base(param)
        {
            SubAccId = param.SubAccountId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustSubInsNickname";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
