namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AppResources;
    using CaptchaMvc.HtmlHelpers;
    using Configurations;
    using Fanex.Logging;
    using Main.Models;
    using MediatR;
    using Models.Requests;
    using Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.ChatBot;
    using Web;

    public class ChatBotController : BaseController
    {
        private const string ChatbotLogCategory = "Chatbot";
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly ILogger logger;

        public ChatBotController(
            IMediator mediator,
            IUserSession userSession,
            ILogger logger)
        {
            this.mediator = mediator;
            this.userSession = userSession;
            this.logger = logger;
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
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            logger.Info($"Link messenger account {command.MessengerAccount} to {command.CustName}", ChatbotLogCategory);

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
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            logger.Info($"Unlink message {command.MessengerUserName} with {userSession.UserName}", ChatbotLogCategory);

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost]
        public async Task<ActionResult> VerifyVerificationCode(VerifyVerificationCodeCommand command)
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

            logger.Info($"Verify messenger account {command.MessengerAccount} by {userSession.UserName}", ChatbotLogCategory);

            var result = await mediator.SendAsync(command);
            if (result.IsNotOk && result.FailedTimes >= 3 && !isCaptchaValid)
            {
                return PartialView("Captcha");
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResendVerificationCode(ResendVerificationCodeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            logger.Info($"Resend verification code to messenger account {command.MessengerUsername}", ChatbotLogCategory);

            return Json(await mediator.SendAsync(command));
        }

        [ChildActionOnly]
        public ActionResult Language(string langCode)
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(langCode, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFunction(UpdateFunctionCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            logger.Info($"Update function: {command.FunctionId}, enable: {command.IsEnable}", ChatbotLogCategory);

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost]
        public async Task<ActionResult> ChangeLanguage(UpdateLanguageCommand command)
        {
            logger.Info($"Change Chatbot's language to {command.Language}", ChatbotLogCategory);

            return Json(await mediator.SendAsync(command));
        }

        [HttpGet]
        public ActionResult Captcha()
            => PartialView();

        [HttpGet]
        public ActionResult ReportExample(string language)
            => View("ReportExample", (object)language);

        [HttpGet]
        public ActionResult NotificationExample(string language)
            => View("NotificationExample", (object)language);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (userSession != null && userSession.IsSubAcc)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}