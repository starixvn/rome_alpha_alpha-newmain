namespace Alpha.NewMain.Site.Criteria.Report.Statistics
{
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;

    public class HighestTurnoverOfMonthCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string BalanceUpDay { get; set; }

        public string MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            if (UserSession.Current.IsAgent)
            {
                return "Age_CustomerSelTopTurnoverMembers_Ag";
            }

            if (UserSession.Current.IsMaster)
            {
                return "Age_CustomerSelTopTurnoverMembers_MAg";
            }

            if (UserSession.Current.IsSuper)
            {
                return "Age_CustomerSelTopTurnoverMembers_SAg";
            }

            return string.Empty;
        }
    }
}