namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using AppResources;
    using Auth.Filters;
    using Main.Models;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Filters;

    [SessionState(SessionStateBehavior.Default)]
    public class PasswordController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public PasswordController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        // GET: Password
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult ChangePassword(ChangePasswordQuery query)
        {
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult ForceChangePassword(ForceChangePasswordQuery query)
        {
            if (userSession.IsPasswordExpired || !userSession.IsLogged)
            {
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

            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordQuery query)
        {
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
        [HttpPost, AntiXsrfFilter]
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
        [HttpPost, AntiXsrfFilter, CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }
    }
}