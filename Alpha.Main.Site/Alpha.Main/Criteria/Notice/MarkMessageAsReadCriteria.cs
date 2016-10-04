namespace Alpha.Main.Criteria.Notice
{
    using Fanex.Data.Repository;

    public class MarkMessageAsReadCriteria : NonQueryCommand
    {
        public int UserId { get; set; }

        public string MessageIds { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerUpdMessagesRead";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}