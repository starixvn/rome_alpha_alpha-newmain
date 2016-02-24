namespace Alpha.NewMain.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using Alpha.NewMain.Site.Extensions;
    using MediatR;
    using Models.Requests.Report.Statistics;
    using Models.Views;
    using Models.Views.Report.Statistics;
    using Services;
    using Sunplus.Agent.Model.Entities;

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
            var result = new StatisticsViewModel
            {
                MemberStatus = new List<MemberStatusViewModel>()
            };

            var statistics = _statisticsService.GetStatistics();
            if (statistics != null)
            {
                result.CasinoTransfer = FormatDecimal_Statistics(statistics.CasinoTransfer);
                result.CasinoOutstanding = FormatDecimal_Statistics(statistics.CasinoOutstanding);
                result.BingoTransfer = FormatDecimal_Statistics(statistics.BingoTransfer);
                result.BingoOutstanding = FormatDecimal_Statistics(statistics.BingoOutstanding);
                result.P2PTransfer = FormatDecimal_Statistics(statistics.P2PTransfer);
                result.P2POutstanding = FormatDecimal_Statistics(statistics.P2POutstanding);
                result.Outstanding = FormatDecimal_Statistics(statistics.Outstanding);
                result.TotalOutstanding = FormatDecimal_Statistics(statistics.TotalOutstanding);
                result.NumberNewMembers = statistics.NumberNewMembers.Format(0);

                var masterStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.master,
                    TotalActive = statistics.TotalMasterActive.Format(0),
                    TotalClosed = statistics.TotalMasterClosed.Format(0),
                    TotalSuspended = statistics.TotalMasterSuspended.Format(0)
                };
                var agentStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.agent,
                    TotalActive = statistics.TotalAgentActive.Format(0),
                    TotalClosed = statistics.TotalAgentClosed.Format(0),
                    TotalSuspended = statistics.TotalAgentSuspended.Format(0)
                };
                var memberStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.member,
                    TotalActive = statistics.TotalMemberActive.Format(0),
                    TotalClosed = statistics.TotalMemberClosed.Format(0),
                    TotalSuspended = statistics.TotalMemberSuspended.Format(0)
                };

                if (UserSession.Current.IsSuper)
                {
                    result.MemberStatus.Add(masterStatus);
                    result.MemberStatus.Add(agentStatus);
                    result.MemberStatus.Add(memberStatus);

                    result.Label_Outstanding = App_GlobalResources.Resources.superOutstanding;
                }
                else if (UserSession.Current.IsMaster)
                {
                    result.MemberStatus.Add(agentStatus);
                    result.MemberStatus.Add(memberStatus);
                    result.Label_Outstanding = App_GlobalResources.Resources.masterOutstanding;
                }
                else if (UserSession.Current.IsAgent)
                {
                    result.MemberStatus.Add(memberStatus);
                    result.Label_Outstanding = App_GlobalResources.Resources.masterOutstanding;
                }
            }

            return result;
        }

        public Top10WinningOfMonthViewModel Handle(Top10WinningOfMonthQuery message)
        {
            var custInfo = _statisticsService.Top10WinningOfMonth();
            var result = new Top10WinningOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in custInfo)
            {
                result.CustomerInfo.Add(new CustomerInfoViewModel
                {
                    Username = data.Username,
                    WinLoss = FormatDecimal_Statistics(data.WinLoss)
                });
            }

            return result;
        }

        public HighestTurnoverOfMonthViewModel Handle(HighestTurnoverOfMonthQuery message)
        {
            var custInfo = _statisticsService.HighestTurnoverOfMonth();
            var result = new HighestTurnoverOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in custInfo)
            {
                result.CustomerInfo.Add(new CustomerInfoViewModel
                {
                    Username = data.Username,
                    Turnover = FormatDecimal_Statistics(data.Turnover)
                });
            }

            return result;
        }

        public NewMembersOfMonthViewModel Handle(NewMembersOfMonthQuery message)
        {
            var custInfo = _statisticsService.NewMembersOfMonth();
            var result = new NewMembersOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in custInfo)
            {
                result.CustomerInfo.Add(new CustomerInfoViewModel
                {
                    Username = data.Username,
                    CreatedDate = data.CreatedDate.ToShortLongFormatedDateWithSlash(),
                    GivenCredit = FormatDecimal_Statistics(data.GivenCredit)
                });
            }

            return result;
        }

        private string FormatDecimal_Statistics(object value)
        {
            var format = "<span class='{0}'>{1}</span>";
            var num = decimal.Round(Convert.ToDecimal(value == DBNull.Value ? 0 : value), 2);
            var style = num < .00m ? "note-text" : "underdog-text";
            var ret = string.Format(format, style, num.Format(2));
            return ret;
        }
    }
}