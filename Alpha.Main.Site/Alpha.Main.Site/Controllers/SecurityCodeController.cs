namespace Alpha.Main.Site.Controllers
{
    using MediatR;
    using Models.Request.SecurityCode;
    using SecurityCode.Sessions;
    using System.Web.Mvc;

    public class SecurityCodeNewMainController : BaseController
    {
        private ISecurityCodeSession securityCodeSession;
        private readonly IMediator mediator;

        public SecurityCodeNewMainController(ISecurityCodeSession securityCodeSession, IMediator mediator)
        {
            this.securityCodeSession = securityCodeSession;
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult ForceChangeSecurityCode()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ForceChangeSecurityCodeWithoutValidation()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult ChangeSecurityCode()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult VerifySecurityCode()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

      
        [HttpGet]
        public ActionResult ResetSecurityCode()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}