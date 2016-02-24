namespace Alpha.NewMain.Site.Handlers
{
    using System.Web;
    using Fanex.Common;
    using MediatR;
    using Models.Requests.Report;
    using Models.Views.Report;
    using Services;
    public class ReportHandler :
        IRequestHandler<PortalPageQuery, PortalPageViewModel>,
        IRequestHandler<GetMoreAnnouncementsQuery, AnnouncementViewModel>
    {
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
            var result = new PortalPageViewModel
            {
                BalanceInfo = _reportService.GetBalanceInfo(),
                Announcements = _announcementService.GetAnnouncements(message.LastPostTime)
            };

            return result;
        }

        public AnnouncementViewModel Handle(GetMoreAnnouncementsQuery message)
        {
            return _announcementService.GetAnnouncements(message.LastPostTime);
        }
    }
}