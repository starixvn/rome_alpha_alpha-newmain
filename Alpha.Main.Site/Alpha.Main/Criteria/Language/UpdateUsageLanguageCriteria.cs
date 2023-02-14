using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.Language
{
    public class UpdateUsageLanguageCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string LanguageUsage { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustLanguageUsageUpd";
        }

        public override bool IsValid()
        {
            return CustId > 0 
                && SubAccId >= 0;
        }
    }
}
