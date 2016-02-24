namespace Alpha.NewMain.Site.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;

    public class BalanceInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            if (UserSession.Current.IsAgent)
            {
                return "Age_CustomerSelBalanceInfo_Ag";
            }

            if (UserSession.Current.IsMaster)
            {
                return "Age_CustomerSelBalanceInfo_MAg";
            }

            if (UserSession.Current.IsSuper)
            {
                return "Age_CustomerSelBalanceInfo_SAg";
            }

            return string.Empty;
        }
    }
}