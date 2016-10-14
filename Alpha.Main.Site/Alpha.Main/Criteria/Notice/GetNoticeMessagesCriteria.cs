namespace Alpha.Main.Criteria.Notice
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetNoticeMessagesCriteria : CriteriaBase
    {
        public GetNoticeMessagesCriteria(
            int custId,
            string parentSites,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            CustId = custId;
            ParentSites = parentSites;
            LastPostTime = lastPostTime;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        [SpParam(SqlParamName = "targetId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "targetId2")]
        public string ParentSites { get; }

        public DateTime LastPostTime { get; }

        public int TotalMessage { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "Age_CustomerSelMessages";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrEmpty(ParentSites);
    }
}