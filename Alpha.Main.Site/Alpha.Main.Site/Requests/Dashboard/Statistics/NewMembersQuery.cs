namespace Alpha.Main.Site.Requests.Dashboard.Statistics
{
    using MediatR;
    using ViewModels.Dashboard.Statistics;

    public class NewMembersQuery : IRequest<NewMembersViewModel>
    {
        public NewMembersQuery(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}