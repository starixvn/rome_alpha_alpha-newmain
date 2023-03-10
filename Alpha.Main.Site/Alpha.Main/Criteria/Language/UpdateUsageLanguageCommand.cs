namespace Alpha.Main.Criteria.Language
{
    using Fanex.Data.Repository;

    public class UpdateUsageLanguageCommand : NonQueryCommand
    {
        public UpdateUsageLanguageCommand(int custId, int subAccId, string languageKey)
        {
            CustId = custId;
            SubAccId = subAccId;
            LanguageUsage = languageKey;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string LanguageUsage { get; }

        public override string GetSettingKey()
            => "Age_CustLanguageUsageUpd";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}
