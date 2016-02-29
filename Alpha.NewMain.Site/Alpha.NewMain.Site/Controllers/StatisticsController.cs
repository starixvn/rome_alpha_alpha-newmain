namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report.Statistics;
    using Utils;

    public class StatisticsController : BaseController
    {
        private IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index(StatisticsQuery query)
        {
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }

        public JsonResult Top10WinningOfMonth(Top10WinningOfMonthQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }

        public JsonResult HighestTurnoverOfMonth(HighestTurnoverOfMonthQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }

        public JsonResult NewMembersOfMonth(NewMembersOfMonthQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}