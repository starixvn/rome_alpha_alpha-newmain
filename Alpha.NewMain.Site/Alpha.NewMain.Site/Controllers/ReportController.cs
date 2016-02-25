namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report;
    using Utils;

    public class ReportController : Controller
    {
        private IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult PortalPage(PortalPageQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }

        public JsonResult GetMoreAnnouncements(MoreAnnouncementsQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}