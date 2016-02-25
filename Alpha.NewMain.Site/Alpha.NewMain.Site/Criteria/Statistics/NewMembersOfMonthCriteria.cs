namespace Alpha.NewMain.Site.Criteria.Statistics
{
    using Fanex.Data.Repository;

    public class NewMembersOfMonthCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelNewMembers";
        }
    }
}