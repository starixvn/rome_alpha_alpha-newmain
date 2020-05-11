#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.Statistics
{
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.Statistics;

    public class TopTurnoverMembersQuery : IRequest<TopTurnoverMembersViewModel>
    {
        public TopTurnoverMembersQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }
    }
}