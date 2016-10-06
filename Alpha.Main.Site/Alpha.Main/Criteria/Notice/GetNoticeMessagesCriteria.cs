namespace Alpha.Main.Criteria.Notice
{
    using System;
    using Fanex.Data.Repository;

    public class GetNoticeMessagesCriteria : CriteriaBase
    {
        public GetNoticeMessagesCriteria(
            int targetId,
            string targetId2,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            TargetId = targetId;
            TargetId2 = targetId2;
            LastPostTime = lastPostTime;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        public int TargetId { get; }

        public string TargetId2 { get; }

        public DateTime LastPostTime { get; }

        public int TotalMessage { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "Age_CustomerSelMessages";

        public override bool IsValid() => TargetId > 0 && !string.IsNullOrEmpty(TargetId2);
    }
}