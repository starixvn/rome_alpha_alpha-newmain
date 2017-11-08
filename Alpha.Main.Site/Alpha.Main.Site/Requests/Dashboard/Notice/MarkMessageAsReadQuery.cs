namespace Alpha.Main.Site.Requests.Dashboard.Notice
{
    using MediatR;

    public class MarkMessageAsReadQuery : IRequest<bool>
    {
        public MarkMessageAsReadQuery(int custId, int messageId, bool isInternal)
        {
            CustId = custId;
            MessageId = messageId;
            IsInternal = isInternal;
        }

        public int CustId { get; }

        public int MessageId { get; }

        public bool IsInternal { get; }
    }
}