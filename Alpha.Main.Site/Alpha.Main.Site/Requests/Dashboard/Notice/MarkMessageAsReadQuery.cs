namespace Alpha.Main.Site.Requests.Dashboard.Notice
{
    using MediatR;

    public class MarkMessageAsReadQuery : IRequest<bool>
    {
        public MarkMessageAsReadQuery(int custId, int messageId)
        {
            CustId = custId;
            MessageId = messageId;
        }

        public int CustId { get; }

        public int MessageId { get; }
    }
}