namespace Alpha.Main.Criteria.Nickname
{
    public class CreateNicknameCommand : NicknameNonQueryCommand
    {
        public CreateNicknameCommand(int custId, string nickname, string language, string ip)
            : base(custId, ip, language)
        {
            Nickname = nickname;
        }

        public string Nickname { get; }

        public override string GetSettingKey()
            => "Age_CustomerInsNickName";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(Nickname);
    }
}
