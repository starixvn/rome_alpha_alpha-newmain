namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using CaptchaMvc.HtmlHelpers;
    using Configurations;
    using Main.Models;
    using MediatR;
    using Models.Requests;
    using Requests.ChatBot;
    using ViewModels.ChatBot;
    using Web;

    public class ChatBotController : BaseController
    {
        private readonly IMediator mediator;

        public ChatBotController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index()
            => View(await mediator.SendAsync(new SetUpChatBotQuery()));

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
            if (command.IsShowCaptcha && !this.IsCaptchaValid(null))
            {
                return Json(Result.CreateErrorResult(AppResources.ChatBot.InvalidCaptcha));
            }

            var result = await mediator.SendAsync(command);
            if (result.IsNotOk && result.FailedTimes == 3)
            {
                var viewModel = new VerifyAccountViewModel(command.UserId, command.MessengerAccount, true);
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
        public async Task<ActionResult> UpdateLanguage(UpdateLanguageCommand command)
            => Json(await mediator.SendAsync(command));
    }
}