namespace Alpha.NewMain.Site.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class AnnouncementsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "targetid")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "targetid2")]
        public string ParentSites { get; set; }

        public DateTime LastPostTime { get; set; }

        public int TotalMessage { get; set; }

        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelMessages";
        }
    }
}