namespace Alpha.Main.Site.Requests.Notice
{
    using MediatR;

    public class MarkMessageAsReadQuery : IRequest<bool>
    {
        private int custId;
        private string messageIds = string.Empty;

        public MarkMessageAsReadQuery(int custId, string messageIds)
        {
            this.custId = custId;
            this.messageIds = messageIds;
        }

        public int CustId
        {
            get
            {
                return this.custId;
            }
        }

        public string MessageIds
        {
            get
            {
                return this.messageIds;
            }
        }
    }
}