namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.Requests;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Alpha.Web;
    using Alpha.Web.Filters;
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

        public async Task<ActionResult> Setup(SetUpChatBotQuery query)
        {
            var model = await mediator.SendAsync(query);

            return model.IsLinked
                ? View("ChatBotSetting", new ChatBotSettingViewModel(model, query))
                : View("RegisterAccount", new RegisterAccountViewModel(model, query));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Unsubscribe(UnsubscribeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> GenerateQRCode(GenerateQRCodeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var viewModel = await mediator.SendAsync(command);

            return PartialView("VerifyAccount", viewModel);
        }
        
        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> VerifyOTP(VerifyOTPCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var isCaptchaValid = this.IsCaptchaValid(null);
            if (command.IsShowCaptcha && !isCaptchaValid)
            {
                return Json(Result.CreateErrorResult(ChatBot.InvalidCaptcha));
            }

            var result = await mediator.SendAsync(command);
            if (result.IsNotOk && result.Code == -3 && !isCaptchaValid)
            {
                return PartialView("Captcha");
            }

            return Json(result);
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ResendOTP(ResendOTPCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [HttpGet]
        public ActionResult Captcha() => PartialView();

        [ChildActionOnly]
        public ActionResult Language(string langCode)
        {
            var viewModel = mediator.Send(new TelegramBotLanguageQuery(langCode, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateFunction(UpdateFunctionCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateLanguage(UpdateLanguageCommand command)
        {
            if (userSession.IsInternal || string.IsNullOrWhiteSpace(command.Language))
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }
    }
}