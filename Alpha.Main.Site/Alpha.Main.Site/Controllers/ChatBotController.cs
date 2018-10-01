namespace Alpha.Main.Site.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AppResources;
    using CaptchaMvc.HtmlHelpers;
    using Configurations;
    using Filters;
    using Main.Models;
    using MediatR;
    using Models.Requests;
    using Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.ChatBot;
    using Web;
    using Web.Filters;

    public class ChatBotController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ChatBotController(
            IMediator mediator,
            IUserSession userSession)
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

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Link Account")]
        public async Task<ActionResult> AddAccount(AddAccountCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var result = await mediator.SendAsync(command);
            if (result.IsOk)
            {
                var viewModel = new VerifyAccountViewModel();
                return PartialView("VerifyAccount", viewModel);
            }

            return Json(result);
        }

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Unlink Account")]
        public async Task<ActionResult> Unsubscribe(UnsubscribeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Verify Verification Code")]
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

            var result = await mediator.SendAsync(command);
            if (result.IsNotOk && result.FailedTimes >= 3 && !isCaptchaValid)
            {
                return PartialView("Captcha");
            }

            return Json(result);
        }

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Resend Verification Code")]
        public async Task<ActionResult> ResendVerificationCode(ResendVerificationCodeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [ChildActionOnly]
        public ActionResult Language(string langCode)
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(langCode, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Update Function")]
        public async Task<ActionResult> UpdateFunction(UpdateFunctionCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.SendAsync(command));
        }

        [HttpPost, AntiXsrfFilter]
        [ChatbotLoggingFilter("Change Chatbot's Language")]
        public async Task<ActionResult> ChangeLanguage(UpdateLanguageCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

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

            if (userSession != null
                && (userSession.IsSubAcc || !AppSettings.OfferChatbotSites.Contains(userSession.Site)))
            {
                UnAuthentication(filterContext);
            }
        }
    }
}