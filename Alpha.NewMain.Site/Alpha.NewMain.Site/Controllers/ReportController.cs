namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report;
    using Utils;

    public class ReportController : BaseController
    {
        private IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult PortalPage(PortalPageQuery query)
        {
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }

        public JsonResult GetMoreAnnouncements(MoreAnnouncementsQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}