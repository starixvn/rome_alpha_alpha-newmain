namespace Alpha.Main.Site.Controllers
{
    using MediatR;
    using Requests.BalanceInfo;
    using Requests.Statistics;
    using Sunplus.Agent.Model.Entities;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public HomeController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> BalanceInfo()
        {
            var viewModel = mediator.Send(new BalanceInfoQuery(this.userSession.CustId, this.userSession.RoleId, this.userSession.Currency));

            return PartialView("_BalanceInfo", viewModel);
        }

        [HttpGet]
        public ActionResult Statistics()
        {
            var viewModel = mediator.Send(new StatisticsQuery(this.userSession.CustId, this.userSession.RoleId));

            return PartialView("_Statistics", viewModel);
        }

        [HttpGet]
        public ActionResult TopWinningMembers()
        {
            var viewModel = mediator.Send(new TopWinningMembersQuery(this.userSession.CustId, this.userSession.RoleId));

            return PartialView("_TopWinningMembers", viewModel);
        }

        [HttpGet]
        public ActionResult TopTurnoverMembers()
        {
            var viewModel = mediator.Send(new TopTurnoverMembersQuery(this.userSession.CustId, this.userSession.RoleId));

            return PartialView("_TopTurnoverMembers", viewModel);
        }

        [HttpGet]
        public ActionResult NewMembers()
        {
            var viewModel = mediator.Send(new NewMembersQuery(this.userSession.CustId));

            return PartialView("_NewMembers", viewModel);
        }
    }
}