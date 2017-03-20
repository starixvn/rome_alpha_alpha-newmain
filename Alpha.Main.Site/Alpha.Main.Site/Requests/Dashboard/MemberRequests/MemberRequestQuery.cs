#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.MemberRequests
{
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.MemberRequests;

    public class MemberRequestsQuery : IRequest<MemberRequestsViewModel>
    {
        public MemberRequestsQuery(IUserSession userSession)
        {
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            CustId = userSession.CustId;
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }
    }
}