namespace Alpha.Main.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Auth.Filters;
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
        [HttpPost, AntiXsrfFilter, CheckBelongingFilter]
        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        {
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