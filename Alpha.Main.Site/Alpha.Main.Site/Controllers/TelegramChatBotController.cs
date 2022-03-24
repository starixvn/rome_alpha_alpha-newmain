namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Alpha.Web;
    using Alpha.Web.Models;
    using CaptchaMvc.HtmlHelpers;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Result = Main.Models.Result;

    public class TelegramChatBotController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public TelegramChatBotController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index()
        {
            var language = userSession.Language;
            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var model = new ChatBotViewModel("test");

            return View("RegisterAccount", new RegisterAccountViewModel(model, language, custLevel.DisplayName));
            //return View("ChatBotSetting", new ChatBotSettingViewModel(model, language, custLevel.DisplayName));
        }

        [HttpPost]
        public ActionResult Unsubscribe()
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(new { IsOk = true });
        }

        [HttpGet]
        public async Task<ActionResult> GenerateQRCode()
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var viewModel = await mediator.SendAsync(new GenerateQRCodeCommand(userSession.RoleId, userSession.CustId, userSession.SubAccId));
            return PartialView("VerifyAccount", viewModel);
        }
        
        [HttpPost]
        public async Task<ActionResult> VerifyOTP(VerifyOTPCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var isCaptchaValid = this.IsCaptchaValid(null);
            if (command.IsShowCaptcha && !isCaptchaValid)
            {
                return Json(Result.CreateErrorResult("Invalid Captcha"));
            }

            var result = await mediator.SendAsync(command);
            //if (result.IsNotOk && result.FailedTimes >= 3 && !isCaptchaValid)
            //{
            //    return PartialView("Captcha");
            //}

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResendOTP()
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(new ResendOTPCommand()));
        }

        [HttpGet]
        public ActionResult Captcha() => PartialView();

        [ChildActionOnly]
        public ActionResult Language(string langCode)
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(langCode, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult ChangeLanguage(UpdateLanguageCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(new { IsOk = true });
        }
    }
}