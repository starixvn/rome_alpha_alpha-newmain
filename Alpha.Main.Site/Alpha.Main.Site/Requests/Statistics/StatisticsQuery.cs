#pragma warning disable 618, 612
namespace Alpha.Main.Site.Requests.Statistics
{
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Home.Statistics;

    public class StatisticsQuery : IRequest<StatisticsViewModel>
    {
        private IUserSession userSession;

        public StatisticsQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            }
        }
    }
}