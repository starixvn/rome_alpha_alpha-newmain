namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.BalanceInfo;
    using Sunplus.Agent.Model.Entities;

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
        public ActionResult Index()
        {
            var viewModel = mediator.Send(new BalanceInfoQuery(this.userSession.CustId, this.userSession.RoleId, this.userSession.Currency));

            return View(viewModel);
        }
    }
}