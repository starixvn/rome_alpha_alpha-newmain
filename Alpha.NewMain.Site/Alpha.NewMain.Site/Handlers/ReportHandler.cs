namespace Alpha.NewMain.Site.Handlers
{
    using System;
    using System.Linq;
    using System.Web;
    using Alpha.NewMain.Site.Extensions;
    using Fanex.Common;
    using MediatR;
    using Models.Requests.Report;
    using Models.Views.Report;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class ReportHandler :
        IRequestHandler<PortalPageQuery, PortalPageViewModel>,
        IRequestHandler<MoreAnnouncementsQuery, AnnouncementViewModel>
    {
        private readonly DateTime _maxDateTime = new DateTime(9999, 12, 31);
        private readonly DateTime _minDateTime = new DateTime(1900, 1, 1);
        private const int _totalAnnouncements = 10;

        private readonly IReportService _reportService;

        private readonly IAnnouncementService _announcementService;

        public ReportHandler(
            IReportService reportService,
            IAnnouncementService announcementService)
        {
            _reportService = reportService;
            _announcementService = announcementService;
        }

        public PortalPageViewModel Handle(PortalPageQuery message)
        {
            message.LastPostTime = Converter.ToString2(HttpContext.Current.Request["lastPostTime"]);
            var announcements = Announcements(message.LastPostTime, _totalAnnouncements);
            var result = new PortalPageViewModel
            {
                BalanceInfo = BalanceInfo(),
                Announcements = announcements
            };

            return result;
        }

        public AnnouncementViewModel Handle(MoreAnnouncementsQuery message)
        {
            return Announcements(message.LastPostTime, _totalAnnouncements);
        }

        private BalanceInfoViewModel BalanceInfo()
        {
            var balanceModel = _reportService.GetBalanceInfo();
            var result = new BalanceInfoViewModel
            {
                Today = DateTime.Now.ToShortFormatedDateWithSlash(),
                Yesterday = DateTime.Now.AddDays(-1).ToShortFormatedDateWithSlash()
            };

            if (UserSession.Current.IsSuper)
            {
                result.RoleName = App_GlobalResources.Resources.super;
                result.GivenCreditText = App_GlobalResources.Resources.superGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalMasterCredit;
            }
            else if (UserSession.Current.IsMaster)
            {
                result.RoleName = App_GlobalResources.Resources.master;
                result.GivenCreditText = App_GlobalResources.Resources.masterGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalAgentCredit;
            }
            else if (UserSession.Current.IsAgent)
            {
                result.RoleName = App_GlobalResources.Resources.agent;
                result.GivenCreditText = App_GlobalResources.Resources.agentGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalMemberCredit;
            }

            if (UserSession.Current.Language == "ko-KR" &&
                UserSession.Current.Currency == "Won")
            {
                result.Currency = "원 ";
            }
            else
            {
                result.Currency = UserSession.Current.Currency + " ";
            }

            if (balanceModel != null)
            {
                result.HasData = true;
                result.CashBalance = FormatDecimal_BalanceInfo(balanceModel.CashBalance);
                result.YesterdayCashBalance = FormatDecimal_BalanceInfo(balanceModel.YesterdayCashBalance);
                result.TotalBalance = FormatDecimal_BalanceInfo(balanceModel.TotalBalance);
                result.YesterdayTotalBalance = FormatDecimal_BalanceInfo(balanceModel.YesterdayTotalBalance);
                result.TodayWinLoss = FormatDecimal_BalanceInfo(balanceModel.TodayWinLoss);
                result.YesterdayWinLoss = FormatDecimal_BalanceInfo(balanceModel.YesterdayWinLoss);
                result.GivenCredit = FormatDecimal_BalanceInfo(balanceModel.GivenCredit);
                result.TotalDownlineCredit = FormatDecimal_BalanceInfo(balanceModel.MemberCredit);
            }

            return result;
        }

        private AnnouncementViewModel Announcements(string lastPostTimeMessage, int totalAnnouncements)
        {
            var lastPostTime = string.IsNullOrEmpty(lastPostTimeMessage) ? _maxDateTime : Convert.ToDateTime(lastPostTimeMessage);
            if (lastPostTime == _minDateTime)
            {
                lastPostTime = _maxDateTime;
            }

            var report = _announcementService.GetAnnouncements(lastPostTimeMessage, _totalAnnouncements);
            var result = new AnnouncementViewModel
            {
                IsShowAnnouncementTitle = IsShowAnnouncementTitle(lastPostTime),
                AnnouncementGroup = report.AnnouncementGroups,
                LastPostTime = report.LastPostTime
            };

            if (report.AnnouncementGroups.Count() == totalAnnouncements)
            {
                result.IsShowMoreLink = true;
            }

            HttpContext.Current.Response.SetCookie(new HttpCookie("moreIsVisible", result.IsShowMoreLink.ToString()));
            return result;
        }

        private string FormatDecimal_BalanceInfo(object value)
        {
            var format = "<span class='{0}'>{1}</span>";
            var num = decimal.Round(Convert.ToDecimal(value == DBNull.Value ? 0 : value), 2);
            var style = num < .00m ? "note-text" : "underdog-text";
            var ret = string.Format(format, style, num.Format(2));
            return ret;
        }

        private bool IsShowAnnouncementTitle(DateTime postTime)
        {
            return postTime == _maxDateTime;
        }
    }
}