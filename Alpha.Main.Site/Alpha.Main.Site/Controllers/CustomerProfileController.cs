#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerProfileController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public CustomerProfileController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        [AllowProtectedMode]
        public ActionResult BriefProfile()
        {
            var query = new BriefProfileQuery(userSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}