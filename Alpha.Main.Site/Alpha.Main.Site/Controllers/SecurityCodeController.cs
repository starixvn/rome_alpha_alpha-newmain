namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Filter;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Models.Requests;

    [SessionState(SessionStateBehavior.Default)]
    public class SecurityCodeController : BaseController
    {
        private readonly IMediator mediator;

        public SecurityCodeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult CheckLoginSecurityCode()
        {
            var status = mediator.Send(new CheckLoginSecurityCodeQuery());

            switch (status)
            {
                case SecurityCodeStatus.ChangeSecurityCodeByForcing:
                    return RedirectToAction("ChangeSecurityCodeByForcing", "SecurityCode");

                case SecurityCodeStatus.ChangeSecurityCodeFirstLogin:
                    return RedirectToAction("ChangeSecurityCodeFirstLogin", "SecurityCode");
            }

            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public ActionResult ChangeSecurityCodeFirstLogin(LoginSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ChangeSecurityCodeByForcing(LoginSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeSecurityCodeByForcing(ChangeSecurityCodeByForcingCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ChangeUserOwnSecurityCode(BaseSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ChangeUserOwnSecurityCode(ChangeUserOwnSecurityCodeCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult ResetSecurityCode(BaseSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
        public JsonResult ResetSecurityCode(ResetSecurityCodeCommand query)
        {
            var viewModel = mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public ActionResult VerifySecurityCode(BaseSecurityCodeQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [AntiXSRFFilter]
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
    }
}