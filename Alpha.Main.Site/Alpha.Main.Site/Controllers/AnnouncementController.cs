namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Announcement;

    public class AnnouncementController : BaseController
    {
        private IMediator mediator;

        public AnnouncementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult HotAnnouncement(HotAnnouncementQuery message)
        {
            var viewModel = mediator.Send(message);
            return View(viewModel);
        }
    }
}