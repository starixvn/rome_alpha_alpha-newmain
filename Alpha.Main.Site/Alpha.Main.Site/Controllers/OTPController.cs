namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Filters;
    using AlphaPlus.User.Models;
    using AppResources;
    using Main.Models;
    using MediatR;
    using Requests.Otp;

    public class OtpController : ManualCheckProtectedModeController
    {
        private readonly IMediator mediator;

        public OtpController(IMediator mediator, IUserSession userSession)
            : base(userSession)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> SetUpOtp(SetUpOtpQuery query)
        {
            var customerOtp = await mediator.SendAsync(query);

            return View(customerOtp);
        }

        [HttpGet]
        public ActionResult VerifyOtp(VerifyOtpQuery query)
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Redirect("~/SecurityCode/VerifySecurityCode");
            }

            var verifyOtp = mediator.Send(query);

            return View(verifyOtp);
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<JsonResult> VerifyOtp(VerifyOtpCommand command)
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var result = await mediator.SendAsync(command);

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

        [HttpGet, AllowProtectedMode]
        public ActionResult ResetOtp(ResetOtpQuery query)
        {
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAction(HttpContext);
            }

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public async Task<ActionResult> ResetOtp(ResetOtpCommand command)
        {
            if (IsNotProtectedModeInternal())
            {
                return UnauthorizedAction(HttpContext);
            }

            var result = await mediator.SendAsync(command);

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

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> GenerateOTP()
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(new GenerateOTPCommand(userSession)));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateOtpFunction(UpdateOtpTelegramCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }
    }
}