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
    using Web;
    using Web.Filters;

    [SessionState(SessionStateBehavior.Default)]
    public class PasswordController : BaseController
    {
        private readonly IMediator mediator;

        public PasswordController(IMediator mediator)
        {
            this.mediator = mediator;
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
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
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
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter]
        public ActionResult SkipForceChangePassword(SkipForceChangePasswordCommand command)
        {
            if(command.IsSkipForceChangePassword && command.ConfirmMessage.ToUpper() == "OK")
            {
                return Json(new Result { Code = 0 });
            }
            
            return Json(new Result { Code = 999, Message= "Can not skip force change Password" });
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