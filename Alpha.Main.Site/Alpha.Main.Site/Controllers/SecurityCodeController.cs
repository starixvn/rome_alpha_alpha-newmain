namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Filter;
    using MediatR;
    using Models.Request.SecurityCode;
    using Models.Requests;
    using Sessions;
    using Sunplus.Agent.Model.Entities;
    [SessionState(SessionStateBehavior.Default)]
    public class SecurityCodeController : BaseController
    {
        private readonly IUserSession userSession;
        private ISecurityCodeSession securityCodeSession;
        private readonly IMediator mediator;

        public SecurityCodeController(IUserSession userSession, ISecurityCodeSession securityCodeSession,IMediator mediator)
        {
            this.securityCodeSession = securityCodeSession;
            this.userSession = userSession;
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult ChangeSecurityCodeFirstLogin()
        {
            var query = new LoginSecurityCodeQuery(this.userSession);

            var viewModel = mediator.Send(query);

            viewModel.Language = mediator.Send(new AllLanguagesQuery(userSession.Language));

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeSecurityCodeFirstLogin(string securityCode, string confirmSecurityCode)
        {
            var query = new ChangeSecurityCodeFirstLoginCommand(securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ChangeSecurityCodeByForcing()
        {
            var query = new LoginSecurityCodeQuery(this.userSession);

            var viewModel = mediator.Send(query);

            viewModel.Language = mediator.Send(new AllLanguagesQuery(userSession.Language));

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeSecurityCodeByForcing(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            var query = new ChangeSecurityCodeByForcingCommand(oldSecurityCode, securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ChangeUserOwnSecurityCode()
        {
            var query = new BaseSecurityCodeQuery(this.userSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeUserOwnSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            var query = new ChangeUserOwnSecurityCodeCommand(oldSecurityCode, securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ResetSecurityCode()
        {
            var query = new BaseSecurityCodeQuery(this.userSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ResetSecurityCode(int? custId, int? subAccId, string securityCode, string confirmSecurityCode)
        {
            var query = new ResetSecurityCodeCommand(custId.HasValue ? custId.Value : 0, subAccId.HasValue ? subAccId.Value : 0, securityCode, confirmSecurityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult VerifySecurityCode()
        {
            var query = new BaseSecurityCodeQuery(this.userSession);

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult VerifySecurityCode(string securityCode)
        {
            var query = new VerifySecurityCodeCommand(securityCode, this.securityCodeSession);

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }
    }
}