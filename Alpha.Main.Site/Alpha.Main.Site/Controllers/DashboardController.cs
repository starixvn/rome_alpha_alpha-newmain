#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.Dashboard.BalanceInfo;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard;

    [SessionState(SessionStateBehavior.ReadOnly)]
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
            var viewModel = new DashboardViewModel(userSession.RoleId);

            return View(viewModel);
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