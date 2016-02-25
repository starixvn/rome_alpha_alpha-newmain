namespace Alpha.NewMain.Site.Criteria.Statistics
{
    using System;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;

    public class Top10WinningOfMonthCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            if (UserSession.Current.IsAgent)
            {
                return "Age_CustomerSelTopWinningMembers_Ag";
            }

            if (UserSession.Current.IsMaster)
            {
                return "Age_CustomerSelTopWinningMembers_MAg";
            }

            if (UserSession.Current.IsSuper)
            {
                return "Age_CustomerSelTopWinningMembers_SAg";
            }

            return string.Empty;
        }
    }
}