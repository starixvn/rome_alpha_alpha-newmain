namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Auth.Filters;
    using AppResources;
    using Configurations;
    using Main.Models;
    using MediatR;
    using Models.Request.SecurityCode;
    using Models.Requests;
    using SecurityCode.Web;
    using Sunplus.Agent.Model.Entities;
    using Web.Filters;

    [SessionState(SessionStateBehavior.Default)]
    public class SecurityCodeController : ManualCheckProtectedModeController
    {
        private readonly IMediator mediator;

        public SecurityCodeController(IMediator mediator, IUserSession userSession)
            : base(userSession)
        {
            this.mediator = mediator;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        [AllowProtectedMode]
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
        [AllowProtectedMode]
        public JsonResult ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand query)
        {
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult ChangeSecurityCodeByForcing(ForceChangeSecurityCodeQuery query)
        {
            if (!userSession.IsForcedSecurityCode)
            {
                return Redirect("~/");
            }

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult ChangeSecurityCodeByForcing(ChangeSecurityCodeByForcingCommand query)
        {
            if (!userSession.IsForcedSecurityCode)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

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
        [HttpGet, CheckBelongingFilter, AllowProtectedMode]
        public ActionResult ResetSecurityCode(SecurityCodeQuery query)
        {
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAction(HttpContext);
            }

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter, CheckBelongingFilter, AllowProtectedMode]
        public ActionResult ResetSecurityCode(ResetSecurityCodeCommand query)
        {
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAjaxAction(HttpContext, ControllerContext);
            }

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpGet]
        public ActionResult VerifySecurityCode(SecurityCodeQuery query)
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Redirect("~/Otp/VerifyOtp");
            }

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult VerifySecurityCode(VerifySecurityCodeCommand query)
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [ChildActionOnly, AllowProtectedMode]
        public ActionResult Languages()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese));
            var domain = Request.Headers["Host"];
            var subDomain = domain.Split('.')[0];
            var mainDomain = domain.Replace(subDomain, string.Empty);

            Response.AppendCookie(new HttpCookie("LANGUAGE", viewModel.CurrentLanguage) { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false });
            Response.AppendCookie(new HttpCookie("hidLanguage", viewModel.CurrentLanguage) { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false });

            return PartialView(viewModel);
        }

        public ActionResult LearnMore()
            => View($"~/Views/SecurityCode/LearnMore/SecurityCode_{this.userSession.Language}.cshtml");

        [HttpPost]
        [AntiXsrfFilter]
        public JsonResult UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionCommand query)
            => Json(mediator.Send(query));
    }
}
