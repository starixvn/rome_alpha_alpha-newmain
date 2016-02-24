namespace Alpha.NewMain.Site.Handlers
{
    using MediatR;
    using Models.Requests.Report.Statistics;
    using Models.Views.Report.Statistics;
    using Services;
    public class StatisticsHandler :
        IRequestHandler<StatisticsQuery, StatisticsViewModel>,
        IRequestHandler<Top10WinningOfMonthQuery, Top10WinningOfMonthViewModel>,
        IRequestHandler<HighestTurnoverOfMonthQuery, HighestTurnoverOfMonthViewModel>,
        IRequestHandler<NewMembersOfMonthQuery, NewMembersOfMonthViewModel>
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsHandler(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public StatisticsViewModel Handle(StatisticsQuery message)
        {
            return _statisticsService.GetStatistics();
        }

        public Top10WinningOfMonthViewModel Handle(Top10WinningOfMonthQuery message)
        {
            return _statisticsService.Top10WinningOfMonth();
        }

        public HighestTurnoverOfMonthViewModel Handle(HighestTurnoverOfMonthQuery message)
        {
            return _statisticsService.HighestTurnoverOfMonth();
        }

        public NewMembersOfMonthViewModel Handle(NewMembersOfMonthQuery message)
        {
            return _statisticsService.NewMembersOfMonth();
        }
    }
}