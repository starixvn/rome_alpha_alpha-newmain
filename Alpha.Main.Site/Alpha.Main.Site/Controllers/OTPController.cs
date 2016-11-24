namespace Alpha.Main.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Otp;
    using Web;
    using Web.Filters;

    public class OtpController : BaseController
    {
        private readonly IMediator mediator;

        public OtpController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        public ActionResult SetUpOtp(SetUpOtpQuery query)
        {
            var customerOtp = mediator.Send(query);

            return View(customerOtp);
        }

        [HttpGet]
        public ActionResult VerifyOtp()
        {
            return View();
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter]
        public JsonResult VerifyOtp(VerifyOtpCommand command)
        {
            var result = mediator.Send(command);

            return Json(result);
        }

        [HttpGet]
        public ActionResult VerifyOtpDifferenceDevice()
        {
            var viewModel = mediator.Send(new VerifyOtpDifferentDeviceQuery());

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
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

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetOtp(ResetOtpCommand command)
        {
            var result = mediator.Send(command);

            return View(result);
        }

        [HttpPost, AntiXsrfFilter]
        public JsonResult RemoveOtp()
        {
            var result = mediator.Send(new RemoveOtpCommand());

            return Json(result);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed!")]
        [HttpPost, AntiXsrfFilter]
        public JsonResult ChangeOtpStatus(ChangeOtpStatusCommand command)
        {
            var result = mediator.Send(command);

            return Json(result);
        }
    }
}