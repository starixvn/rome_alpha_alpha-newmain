#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Statistics
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
        }

        public int CustId { get; private set; }

        public UserLevel UserLevel { get; private set; }
    }
}