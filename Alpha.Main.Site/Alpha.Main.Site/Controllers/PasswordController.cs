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
            return View();
        }

        public ActionResult ForceChangePassword()
        {
            return View();
        }

        public ActionResult ResetMainAccountPassword()
        {
            return View();
        }

        public ActionResult ResetSubAccountPassword()
        {
            return View();
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