namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;

    public class CustomerProfileController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public CustomerProfileController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult BriefProfile()
        {
            var query = new BriefProfileQuery(this.userSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}