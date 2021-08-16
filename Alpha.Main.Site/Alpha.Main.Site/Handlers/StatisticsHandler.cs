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

            var viewModel = new StatisticsViewModel(
                statistics,
                message.UserLevel,
                message.UserSession.SubPageAccess.ViewReports);

            return viewModel;
        }

        public TopWinningMembersViewModel Handle(TopWinningMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetTopWinningMembers(message.CustId, message.UserLevel);

            var viewModel = new TopWinningMembersViewModel(members);

            return viewModel;
        }

        public TopTurnoverMembersViewModel Handle(TopTurnoverMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetTopTurnoverMembers(message.CustId, message.UserLevel);

            var viewModel = new TopTurnoverMembersViewModel(members);

            return viewModel;
        }

        public NewMembersViewModel Handle(NewMembersQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var members = statisticsService.GetNewMembers(message.CustId);

            var viewModel = new NewMembersViewModel(members);

            return viewModel;
        }
    }
}