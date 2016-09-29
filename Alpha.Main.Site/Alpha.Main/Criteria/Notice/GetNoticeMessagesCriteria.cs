namespace Alpha.Main.Criteria.Notice
{
    using System;
    using Fanex.Data.Repository;

    public class GetNoticeMessagesCriteria : CriteriaBase
    {
        public int TargetId { get; set; }

        public string TargetId2 { get; set; }

        public DateTime LastPostTime { get; set; }

        public int TotalMessage { get; set; }

        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelMessages";
        }

        public override bool IsValid()
        {
            return TargetId > 0 && !string.IsNullOrEmpty(TargetId2);
        }
    }
}