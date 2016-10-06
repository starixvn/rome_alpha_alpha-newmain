namespace Alpha.Main.Criteria.Notice
{
    using Fanex.Data.Repository;

    public class MarkMessageAsReadCriteria : NonQueryCommand
    {
        public MarkMessageAsReadCriteria(int userId, string messageIds)
        {
            UserId = userId;
            MessageIds = messageIds;
        }

        public int UserId { get; }

        public string MessageIds { get; }

        public override string GetSettingKey() => "Age_CustomerUpdMessagesRead";

        public override bool IsValid() => UserId > 0;
    }
}