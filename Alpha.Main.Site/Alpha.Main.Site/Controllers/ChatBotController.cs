namespace Alpha.Main.Site.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using CaptchaMvc.HtmlHelpers;
    using Configurations;
    using MediatR;
    using Models.Requests;
    using Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.ChatBot;
    using Web;

    public class ChatBotController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ChatBotController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public async Task<ActionResult> Index()
        {
            var model = await mediator.SendAsync(new SetUpChatBotQuery(userSession));

            if (model.IsAlreadySetup)
            {
                return View("ChatBotSetting", new ChatBotSettingViewModel(model.User));
            }

            return View("RegisterAccount", new RegisterAccountViewModel(model.User));
        }

        [HttpPost]
        public async Task<ActionResult> AddAccount(AddAccountCommand command)
        {
            var result = await mediator.SendAsync(command);

            if (result.IsOk)
            {
                var viewModel = new VerifyAccountViewModel(result.UserId, command.MessengerAccount);
                return PartialView("VerifyAccount", viewModel);
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Unsubscribe(UnsubscribeCommand command)
            => Json(await mediator.SendAsync(command));

        [HttpPost]
        public async Task<ActionResult> VerifyVerificationCode(VerifyVerificationCodeCommand command)
        {
            var isCaptchaValid = this.IsCaptchaValid(null);
            if (command.IsShowCaptcha && !isCaptchaValid)
            {
                return Json(CaptchaErrorResult.CreateInvalidCaptchaResult(BuildCaptchaInfo()));
            }

            var result = await mediator.SendAsync(command);
            if (result.IsNotOk && result.FailedTimes >= 3)
            {
                var viewModel = new VerifyAccountViewModel(command.UserId, command.MessengerAccount, !isCaptchaValid);
                return PartialView("VerifyAccount", viewModel);
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResendVerificationCode(ResendVerificationCodeCommand command)
            => Json(await mediator.SendAsync(command));

        [ChildActionOnly]
        public ActionResult Language(string langCode)
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(langCode, AppSettings.DisabledVietnamese));
            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFunction(UpdateFunctionCommand command)
            => Json(await mediator.SendAsync(command));

        [HttpPost]
        public async Task<ActionResult> ChangeLanguage(UpdateLanguageCommand command)
            => Json(await mediator.SendAsync(command));

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (userSession != null && userSession.IsSubAcc)
            {
                UnAuthentication(filterContext);
            }
        }

        private IDictionary<string, string> BuildCaptchaInfo()
        {
            var buildInfo = this.GenerateCaptchaValue(4);

            return new Dictionary<string, string>
            {
                [buildInfo.ImageElementId] = buildInfo.ImageUrl,
                [buildInfo.TokenElementId] = buildInfo.TokenValue
            };
        }
    }
}