namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using AppResources;
    using Main.Models;
    using MediatR;
    using Requests.Otp;
    using SecurityCode.Web;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Filters;

    public class OtpController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public OtpController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult SetUpOtp(SetUpOtpQuery query)
        {
            var customerOtp = mediator.Send(query);

            return View(customerOtp);
        }

        [HttpGet]
        public ActionResult VerifyOtp()
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Redirect("~/SecurityCode/VerifySecurityCode");
            }

            return View();
        }

        [HttpPost, AntiXsrfFilter]
        public JsonResult VerifyOtp(VerifyOtpCommand command)
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var result = mediator.Send(command);

            return Json(result);
        }

        [HttpGet]
        public ActionResult VerifyOtpDifferenceDevice()
        {
            var viewModel = mediator.Send(new VerifyOtpDifferentDeviceQuery(userSession));

            return View(viewModel);
        }

        [HttpPost, AntiXsrfFilter]
        public JsonResult SetUpOtp(SetUpOtpCommand command)
        {
            var result = mediator.Send(command);

            return Json(result);
        }

        [HttpGet]
        public ActionResult ResetOtp(ResetOtpQuery query)
        {
            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetOtp(ResetOtpCommand command)
        {
            var result = mediator.Send(command);

            return View(result);
        }

        [HttpPost, AntiXsrfFilter]
        public JsonResult RemoveOtp()
        {
            var result = mediator.Send(new RemoveOtpCommand(userSession));

            return Json(result);
        }

        [HttpPost, AntiXsrfFilter]
        public JsonResult ChangeOtpStatus(ChangeOtpStatusCommand command)
        {
            var result = mediator.Send(command);

            return Json(result);
        }
    }
}
