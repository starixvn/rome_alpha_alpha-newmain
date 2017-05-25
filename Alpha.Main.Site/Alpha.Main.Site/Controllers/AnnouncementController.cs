namespace Alpha.Main.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Configurations;
    using MediatR;
    using Models.Requests.Announcement;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AnnouncementController : BaseController
    {
        private readonly IMediator mediator;

        public AnnouncementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult HotAnnouncement(HotAnnouncementQuery message)
        {
            message.NoticeTypes = AppSettings.NoticeTypes;
            var viewModel = mediator.Send(message);
            return View(viewModel);
        }
    }
}