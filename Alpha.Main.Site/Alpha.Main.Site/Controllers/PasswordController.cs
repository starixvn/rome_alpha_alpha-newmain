namespace Alpha.Main.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using AppResources;
    using Auth.Filters;
    using Main.Models;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Model.Entities;
    using Web.Filters;

    [SessionState(SessionStateBehavior.Default)]
    public class PasswordController : ManualCheckProtectedModeController
    {
        private readonly IMediator mediator;

        public PasswordController(IMediator mediator, IUserSession userSession)
            : base(userSession)
        {
            this.mediator = mediator;
        }

        // GET: Password
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult ChangePassword(ChangePasswordQuery query)
        {
            AssignDarkModeViewData(userSession);
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [AllowProtectedMode]
        public ActionResult ForceChangePassword(ForceChangePasswordQuery query)
        {
            if (userSession.IsPasswordExpired || !userSession.IsLogged)
            {
                AssignDarkModeViewData(userSession);
                var viewModel = this.mediator.Send(query);
                return View(viewModel);
            }

            return Redirect("~/");
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [CheckBelongingFilter]
        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordQuery query)
        {
            if (!query.IsValid())
            {
                return RedirectToAction("Index", "Error");
            }

            AssignDarkModeViewData(userSession);
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [CheckBelongingFilter, AllowProtectedMode]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordQuery query)
        {
            AssignDarkModeViewData(userSession);
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAction(HttpContext);
            }

            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter]
        public ActionResult ChangePassword(ChangePasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter, AllowProtectedMode]
        public ActionResult ForceChangePassword(ChangePasswordCommand command)
        {
            if (userSession.IsPasswordExpired || !userSession.IsLogged)
            {
                var result = this.mediator.Send(command);
                return Json(result);
            }

            return Json(Result.CreateErrorResult(Security.NotAuthorized));
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SkipForceChangePassword(SkipForceChangePasswordCommand command)
        {
            var result = mediator.Send(command);
            return Redirect(result.IsSuccessful ?
                Helpers.UrlHelper.MapMainPath("~/") :
                Helpers.UrlHelper.MapOldMainPath("~/_Authorization/Handlers/SignOut.ashx"));
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter, CheckBelongingFilter]
        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        {
            if (!command.IsValid())
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var result = this.mediator.Send(command);
            return Json(result);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter, CheckBelongingFilter, AllowProtectedMode]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        {
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAjaxAction(HttpContext, ControllerContext);
            }

            var result = this.mediator.Send(command);
            return Json(result);
        }
    }
}