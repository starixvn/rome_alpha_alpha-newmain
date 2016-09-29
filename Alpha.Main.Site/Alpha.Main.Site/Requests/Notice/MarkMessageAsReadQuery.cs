namespace Alpha.Main.Site.Requests.Notice
{
    using MediatR;

    public class MarkMessageAsReadQuery : IRequest<bool>
    {
        public MarkMessageAsReadQuery(int custId, string messageIds)
        {
            this.CustId = custId;
            this.MessageIds = messageIds;
        }

        public int CustId { get; private set; }

        public string MessageIds { get; private set; }
    }
}