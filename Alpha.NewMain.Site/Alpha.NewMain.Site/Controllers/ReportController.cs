namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report;
    public class ReportController : Controller
    {
        private IMediator _mediator;
        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult PortalPage(PortalPageQuery query)
        {
            return View("~/Views/Report/PortalPage/Index.cshtml", _mediator.Send(query));
        }

        public JsonResult GetMoreAnnouncements(GetMoreAnnouncementsQuery query)
        {
            return Json(_mediator.Send(query));
        }
    }
}