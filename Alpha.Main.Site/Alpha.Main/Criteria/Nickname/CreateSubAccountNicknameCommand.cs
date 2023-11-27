namespace Alpha.Main.Criteria.Nickname
{
    public class CreateSubAccountNicknameCommand : CreateNicknameCommand
    {
        public CreateSubAccountNicknameCommand(
            int custId,
            int subAccId,
            string nickname,
            string language,
            string ip) : base(custId, nickname, language, ip)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustSubInsNickname";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
