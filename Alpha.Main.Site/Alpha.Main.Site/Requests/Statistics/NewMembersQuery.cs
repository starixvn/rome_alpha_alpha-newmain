namespace Alpha.Main.Site.Requests.Statistics
{
    using MediatR;
    using ViewModels.Home.Statistics;

    public class NewMembersQuery : IRequest<NewMembersViewModel>
    {
        private int custId;

        public NewMembersQuery(int custId)
        {
            this.custId = custId;
        }

        public int CustId { get { return this.custId; } }
    }
}