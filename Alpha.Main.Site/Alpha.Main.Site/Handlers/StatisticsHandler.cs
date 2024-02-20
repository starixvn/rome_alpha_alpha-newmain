namespace Alpha.Main.Site.Handlers
{
    using System;
    using MediatR;
    using Requests.Dashboard.Statistics;
    using Services;
    using ViewModels.Dashboard.Statistics;

    public class StatisticsHandler :
        IRequestHandler<StatisticsQuery, StatisticsViewModel>,
        IRequestHandler<TopWinningMembersQuery, TopWinningMembersViewModel>,
        IRequestHandler<TopTurnoverMembersQuery, TopTurnoverMembersViewModel>,
        IRequestHandler<NewMembersQuery, NewMembersViewModel>
    {
        private readonly StatisticsService statisticsService;

        public StatisticsHandler(StatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public StatisticsViewModel Handle(StatisticsQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var statistics = statisticsService.GetStatistics(message.CustId, message.UserLevel);

            return new StatisticsViewModel(
                statistics,
                message.UserLevel,
                message.UserSession.SubPageAccess.ViewReports);
        }

        public TopWinningMembersViewModel Handle(TopWinningMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetTopWinningMembers(message.CustId, message.UserLevel);

            return new TopWinningMembersViewModel(members);
        }

        public TopTurnoverMembersViewModel Handle(TopTurnoverMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetTopTurnoverMembers(message.CustId, message.UserLevel);

            return new TopTurnoverMembersViewModel(members);
        }

        public NewMembersViewModel Handle(NewMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetNewMembers(message.CustId);

            return new NewMembersViewModel(members);
        }
    }
}