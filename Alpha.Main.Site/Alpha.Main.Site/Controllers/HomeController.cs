namespace Alpha.Main.Site.Controllers
{
    using MediatR;
    using Requests.Home;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
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
            var viewModel = mediator.Send(new BalanceInfoQuery(this.userSession));

            return PartialView("BalanceInfo", viewModel);
        }

        [HttpGet]
        public ActionResult Statistics()
        {
            return PartialView("Statistics");
        }
    }
}