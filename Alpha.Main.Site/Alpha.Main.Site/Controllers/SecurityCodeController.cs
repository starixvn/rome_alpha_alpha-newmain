namespace Alpha.Main.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Filter;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.Default)]
    public class SecurityCodeController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public SecurityCodeController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (userSession.IsForcedSecurityCode)
            {
                bool hasSC = !string.IsNullOrWhiteSpace(userSession.SecurityCode);

                return hasSC
                    ? RedirectToAction("ChangeSecurityCodeByForcing")
                    : RedirectToAction("ChangeSecurityCodeFirstLogin");
            }

            return Redirect("~/");
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeQuery query)
        {
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return Redirect("~/");
            }

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult ChangeSecurityCodeByForcing(ForceChangeSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult ChangeSecurityCodeByForcing(ChangeSecurityCodeByForcingCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult ChangeUserOwnSecurityCode(SecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult ChangeUserOwnSecurityCode(ChangeUserOwnSecurityCodeCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult ResetSecurityCode(SecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult ResetSecurityCode(ResetSecurityCodeCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult VerifySecurityCode(SecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult VerifySecurityCode(VerifySecurityCodeCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [ChildActionOnly]
        public ActionResult Languages()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery());

            return PartialView(viewModel);
        }

        public ActionResult LearnMore()
        {
            return View($"~/Views/SecurityCode/LearnMore/SecurityCode_{this.userSession.Language}.cshtml");
        }
    }
}