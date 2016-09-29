namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.Statistics;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
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
            var viewModel = mediator.Send(new StatisticsQuery(this.userSession));

            return PartialView(viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult TopWinningMembers()
        {
            var viewModel = mediator.Send(new TopWinningMembersQuery(this.userSession));

            return PartialView("_TopWinningMembers", viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult TopTurnoverMembers()
        {
            var viewModel = mediator.Send(new TopTurnoverMembersQuery(this.userSession));

            return PartialView("_TopTurnoverMembers", viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult NewMembers()
        {
            var viewModel = mediator.Send(new NewMembersQuery(this.userSession.CustId));

            return PartialView("_NewMembers", viewModel);
        }
    }
}