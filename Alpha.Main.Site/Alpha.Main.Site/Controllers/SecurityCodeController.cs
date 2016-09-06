namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;
    using Models.Request.SecurityCode;
    public class SecurityCodeNewMainController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public SecurityCodeNewMainController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult ForceChangeSecurityCode()
        {
            var query = new ForceChangeSecurityCodeQuery();

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ForceChangeSecurityCodeWithoutValidation()
        {
            var query = new ForceChangeSecurityCodeWithoutValidationQuery();

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult ChangeSecurityCode()
        {
            var query = new ChangeSecurityCodeQuery();

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult VerifySecurityCode()
        {
            var query = new VerifySecurityCodeQuery();

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

      
        [HttpGet]
        public ActionResult ResetSecurityCode()
        {
            var query = new ResetSecurityCodeQuery();

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}