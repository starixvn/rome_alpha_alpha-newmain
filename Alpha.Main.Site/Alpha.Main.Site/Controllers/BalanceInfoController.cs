#pragma warning disable 618, 612
namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.BalanceInfo;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BalanceInfoController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public BalanceInfoController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult Index()
        {
            var viewModel = mediator.Send(new BalanceInfoQuery(userSession));

            return PartialView(viewModel);
        }
    }
}