namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.NewMain.Site.Extensions;
    using Criteria.Report.Statistics;
    using Fanex.Data.Repository;
    using Models.Dtos;
    using Models.Dtos.Statistics;
    using Models.Views;
    using Models.Views.Report.Statistics;
    using Sunplus.Agent.Model.Entities;
    using Alpha.NewMain.Site.Extensions;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDynamicRepository _dynamicRepository;

        private readonly IReportService _reportService;

        public StatisticsService(
            IDynamicRepository dynamicRepository,
            IReportService reportService)
        {
            _dynamicRepository = dynamicRepository;
            _reportService = reportService;
        }

        public StatisticsViewModel GetStatistics()
        {
            var result = new StatisticsViewModel
            {
                MemberStatus = new List<MemberStatusViewModel>()
            };

            var rawData = _dynamicRepository.Fetch<StatisticsModel>(new GetStatisticsCriteria
            {
                CustId = UserSession.Current.CustId
            }).FirstOrDefault();

            if (rawData != null)
            {
                result.CasinoTransfer = FormatDecimal_Statistics(rawData.CasinoTransfer);
                result.CasinoOutstanding = FormatDecimal_Statistics(rawData.CasinoOutstanding);
                result.BingoTransfer = FormatDecimal_Statistics(rawData.BingoTransfer);
                result.BingoOutstanding = FormatDecimal_Statistics(rawData.BingoOutstanding);
                result.P2PTransfer = FormatDecimal_Statistics(rawData.P2PTransfer);
                result.P2POutstanding = FormatDecimal_Statistics(rawData.P2POutstanding);
                result.Outstanding = FormatDecimal_Statistics(rawData.Outstanding);
                result.TotalOutstanding = FormatDecimal_Statistics(rawData.TotalOutstanding);
                result.NumberNewMembers = rawData.NumberNewMembers.Format(0);

                var masterStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.master,
                    TotalActive = rawData.TotalMasterActive.Format(0),
                    TotalClosed = rawData.TotalMasterClosed.Format(0),
                    TotalSuspended = rawData.TotalMasterSuspended.Format(0)
                };
                var agentStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.agent,
                    TotalActive = rawData.TotalAgentActive.Format(0),
                    TotalClosed = rawData.TotalAgentClosed.Format(0),
                    TotalSuspended = rawData.TotalAgentSuspended.Format(0)
                };
                var memberStatus = new MemberStatusViewModel
                {
                    Level = App_GlobalResources.Resources.member,
                    TotalActive = rawData.TotalMemberActive.Format(0),
                    TotalClosed = rawData.TotalMemberClosed.Format(0),
                    TotalSuspended = rawData.TotalMemberSuspended.Format(0)
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

        public Top10WinningOfMonthViewModel Top10WinningOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new Top10WinningOfMonthCriteria
            {
                CustId = UserSession.Current.CustId,
                BalanceUpDay = balanceUpDay.BalanceUpDay,
                MoveBetsDay = balanceUpDay.MoveBetsDay
            });

            var result = new Top10WinningOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in rawData)
            {
                result.CustomerInfo.Add(new CustomerInfoViewModel
                {
                    Username = data.Username,
                    WinLoss = FormatDecimal_Statistics(data.WinLoss)
                });
            }

            return result;
        }

        public HighestTurnoverOfMonthViewModel HighestTurnoverOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new HighestTurnoverOfMonthCriteria
            {
                CustId = UserSession.Current.CustId,
                BalanceUpDay = balanceUpDay.BalanceUpDay,
                MoveBetsDay = balanceUpDay.MoveBetsDay
            });

            var result = new HighestTurnoverOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in rawData)
            {
                result.CustomerInfo.Add(new CustomerInfoViewModel
                {
                    Username = data.Username,
                    Turnover = FormatDecimal_Statistics(data.Turnover)
                });
            }

            return result;
        }

        public NewMembersOfMonthViewModel NewMembersOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new NewMembersOfMonthCriteria
            {
                CustId = UserSession.Current.CustId
            });

            var result = new NewMembersOfMonthViewModel
            {
                CustomerInfo = new List<CustomerInfoViewModel>()
            };

            foreach (var data in rawData)
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