namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Announcement;
    using Utils;

    public class AnnouncementController : BaseController
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