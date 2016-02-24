namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Report.Statistics;
    public class StatisticsController : Controller
    {
        private IMediator _mediator;
        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index(StatisticsQuery query)
        {
            return View("~/Views/Report/Statistics/Index.cshtml", _mediator.Send(query));
        }

        public JsonResult Top10WinningOfMonth(Top10WinningOfMonthQuery query)
        {
            return Json(_mediator.Send(query));
        }

        public JsonResult HighestTurnoverOfMonth(HighestTurnoverOfMonthQuery query)
        {
            return Json(_mediator.Send(query));
        }

        public JsonResult NewMembersOfMonth(NewMembersOfMonthQuery query)
        {
            return Json(_mediator.Send(query));
        }
    }
}