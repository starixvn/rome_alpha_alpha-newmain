namespace Alpha.Main.Criteria.Announcement
{
    using System;
    using Fanex.Data.Repository;

    public class InsertPersonalMessageTargetCriteria : CriteriaBase
    {
        public long MessageId { get; set; }

        public int TargetId { get; set; }

        public string TargetName { get; set; }

        public bool IsRead { get; set; }

        public string BetTransId { get; set; }

        public override string GetSettingKey()
        {
            return "Dot_Msg_SetUsrMessage";
        }

        public override bool IsValid()
        {
            return MessageId > 0;
        }
    }
}