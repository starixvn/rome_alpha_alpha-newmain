namespace Alpha.NewMain.Site.Criteria.Report.Statistics
{
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;
    public class GetStatisticsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            if (UserSession.Current.IsAgent)
            {
                return "Age_CustomerSelStatisticsInfo_Ag";
            }

            if (UserSession.Current.IsMaster)
            {
                return "Age_CustomerSelStatisticsInfo_MAg";
            }

            if (UserSession.Current.IsSuper)
            {
                return "Age_CustomerSelStatisticsInfo_SAg";
            }

            return string.Empty;
        }
    }
}