#pragma warning disable 618, 612
namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.BalanceInfo;
    using Sunplus.Agent.Model.Entities;

    public class DashboardController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public DashboardController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult BalanceInfo()
        {
            var viewModel = mediator.Send(new BalanceInfoQuery(userSession));

            return PartialView(viewModel);
        }
    }
}
