namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Requests.BalanceInfo;
    using Requests.Statistics;
    using Services;
    using System;
    using ViewModels.Home.BalanceInfo;
    using ViewModels.Home.Statistics;

    public class HomeHandler 
        : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
        , IRequestHandler<StatisticsQuery, StatisticsViewModel>
        , IRequestHandler<TopWinningMembersQuery, TopWinningMembersViewModel>
        , IRequestHandler<TopTurnoverMembersQuery, TopTurnoverMembersViewModel>
        , IRequestHandler<NewMembersQuery, NewMembersViewModel>
    {
        private BalanceInfoService balanceInfoService;
        private StatisticsService statisticsService;

        public HomeHandler(
            BalanceInfoService balanceInfoService
            , StatisticsService statisticsService)
        {
            this.balanceInfoService = balanceInfoService;
            this.statisticsService = statisticsService;
        }

        public BalanceInfoViewModel Handle(BalanceInfoQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var balanceInfo = balanceInfoService.GetBalanceInfo(message.UserId, message.UserLevel);

            var viewModel = new BalanceInfoViewModel(balanceInfo, message.UserLevel, message.Currency);

            return viewModel;
        }


        public StatisticsViewModel Handle(StatisticsQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var statistics = statisticsService.GetStatistics(message.UserId, message.UserLevel);

            var viewModel = new StatisticsViewModel(statistics, message.UserLevel);

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