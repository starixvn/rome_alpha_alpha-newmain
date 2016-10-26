namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.Otp;

    public class OtpController : Controller
    {
        private readonly IMediator mediator;

        public OtpController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index(SetUpOtpQuery query)
        {
            var customerOtp = mediator.Send(new SetUpOtpQuery());

            return View(customerOtp);
        }

        [HttpGet]
        public ActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
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

        [HttpPost]
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

        [HttpPost]
        public ActionResult ResetOtp(ResetOtpCommand command)
        {
            var result = mediator.Send(command);

            return View(result);
        }

        [HttpPost]
        public JsonResult RemoveOtp()
        {
            var result = mediator.Send(new RemoveOtpCommand());

            return Json(result);
        }

        [HttpPost]
        public JsonResult ChangeOtpStatus(ChangeOtpStatusCommand command)
        {
            var result = mediator.Send(command);

            return Json(result);
        }
    }
}