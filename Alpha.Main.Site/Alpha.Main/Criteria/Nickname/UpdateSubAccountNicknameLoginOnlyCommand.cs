namespace Alpha.Main.Criteria.Nickname
{
    public class UpdateSubAccountNicknameLoginOnlyCommand : UpdateNicknameLoginOnlyCommand
    {
        public UpdateSubAccountNicknameLoginOnlyCommand(
            int custId,
            int subAccId,
            bool nickNameLoginOnly,
            string language,
            string ip) : base(custId, nickNameLoginOnly, language, ip)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustSubUpdNicknameLoginOnly";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
