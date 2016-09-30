namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Filter;
    using MediatR;
    using Requests.Password;

    [SessionState(SessionStateBehavior.Default)]
    public class PasswordController : BaseController
    {
        private readonly IMediator mediator;

        public PasswordController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: Password
        public ActionResult ChangePassword(ChangePasswordQuery query)
        {
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        public ActionResult ForceChangePassword(ForceChangePasswordQuery query)
        {
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordQuery query)
        {
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordQuery query)
        {
            var viewModel = this.mediator.Send(query);
            return View(viewModel);
        }

        [HttpPost, AntiXSRFFilter]
        public ActionResult ChangePassword(ChangePasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [HttpPost, AntiXSRFFilter]
        public ActionResult ForceChangePassword(ChangePasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [HttpPost, AntiXSRFFilter]
        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [HttpPost, AntiXSRFFilter]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }
    }
}