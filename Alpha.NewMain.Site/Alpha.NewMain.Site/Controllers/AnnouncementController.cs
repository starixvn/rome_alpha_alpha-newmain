namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Announcement;

    public class AnnouncementController : Controller
    {
        private IMediator _mediator;

        public AnnouncementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult HotAnnouncement(HotAnnouncementQuery query)
        {
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }
    }
}