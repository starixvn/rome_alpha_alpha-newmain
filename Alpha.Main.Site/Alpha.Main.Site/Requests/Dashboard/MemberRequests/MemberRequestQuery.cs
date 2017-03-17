#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.MemberRequests
{
    using MediatR;
    using ViewModels.Dashboard.MemberRequests;

    public class MemberRequestsQuery : IRequest<MemberRequestsViewModel>
    {
        public MemberRequestsQuery()
        {
        }
    }
}