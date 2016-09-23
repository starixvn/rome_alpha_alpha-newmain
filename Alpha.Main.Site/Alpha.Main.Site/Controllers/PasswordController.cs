namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Password;

    public class PasswordController : BaseController
    {
        private readonly IMediator mediator;

        public PasswordController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: Password
        public ActionResult ChangePassword()
        {
            var viewModel = this.mediator.Send(new PasswordQuery());
            return View(viewModel);
        }

        public ActionResult ForceChangePassword()
        {
            var viewModel = this.mediator.Send(new PasswordQuery());
            return View(viewModel);
        }

        public ActionResult ResetMainAccountPassword()
        {
            var viewModel = this.mediator.Send(new PasswordQuery());
            return View(viewModel);
        }

        public ActionResult ResetSubAccountPassword()
        {
            var viewModel = this.mediator.Send(new PasswordQuery());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        {
            var result = this.mediator.Send(command);
            return Json(result);
        }
    }
}