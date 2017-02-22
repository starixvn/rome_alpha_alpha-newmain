#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.Statistics
{
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.Statistics;

    public class StatisticsQuery : IRequest<StatisticsViewModel>
    {
        public StatisticsQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            UserSession = userSession;
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }

        public IUserSession UserSession { get; }
    }
}