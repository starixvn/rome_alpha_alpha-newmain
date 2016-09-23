namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.Statistics;
    using Sunplus.Agent.Model.Entities;

    public class StatisticsController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public StatisticsController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = mediator.Send(new StatisticsQuery(this.userSession.CustId, this.userSession.RoleId));

            return View(viewModel);
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