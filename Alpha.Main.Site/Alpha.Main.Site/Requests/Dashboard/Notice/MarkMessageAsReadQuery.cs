namespace Alpha.Main.Site.Requests.Notice
{
    using MediatR;

    public class MarkMessageAsReadQuery : IRequest<bool>
    {
        public MarkMessageAsReadQuery(int custId, int messageId)
        {
            CustId = custId;
            MessageId = messageId;
        }

        public int CustId { get; private set; }

        public int MessageId { get; private set; }
    }
}