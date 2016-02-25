namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report.Statistics;
    using Utils;

    public class StatisticsController : Controller
    {
        private IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index(StatisticsQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }

        public JsonResult Top10WinningOfMonth(Top10WinningOfMonthQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }

        public JsonResult HighestTurnoverOfMonth(HighestTurnoverOfMonthQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }

        public JsonResult NewMembersOfMonth(NewMembersOfMonthQuery query)
        {
            LanguageUtil.SetLanguage();
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}