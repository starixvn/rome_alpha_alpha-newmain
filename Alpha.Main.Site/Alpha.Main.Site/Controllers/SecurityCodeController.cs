namespace Alpha.Main.Site.Controllers
{
    using Filter;
    using MediatR;
    using Models.Request.SecurityCode;
    using Sessions;
    using System.Web.Mvc;
    using System.Web.SessionState;
    [SessionState(SessionStateBehavior.Default)]
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
        public ActionResult ForceChangeSecurityCodeByUpline()
        {
            var query = new SecurityCodeQuery(this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ForceChangeSecurityCodeByTheFirstLogin()
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

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ForceChangeSecurityCodeByUpline(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            var scSession = new SecurityCodeSession(Session.SessionID);
            var query = new UpdateForceChangeSecurityCodeByUpline(oldSecurityCode, securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ForceChangeSecurityCodeByTheFirstLogin(string securityCode, string confirmSecurityCode)
        {
            var query = new UpdateForceChangeSecurityCodeTheFirstLogin(securityCode, confirmSecurityCode);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }


        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            var query = new UpdateChangeSecurityCodeCommand(oldSecurityCode, securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpPost]
        //[AntiXSRFFilter]
        public JsonResult VerifySecurityCode(string securityCode)
        {
            var query = new VerifySecurityCodeCommand(securityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ResetSecurityCode(int? custId, int? subAccId, string securityCode, string confirmSecurityCode)
        {
            var query = new ResetSecurityCodeCommand(custId.HasValue ? custId.Value : 0, subAccId.HasValue ? subAccId.Value : 0, securityCode, confirmSecurityCode);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }
    }
}