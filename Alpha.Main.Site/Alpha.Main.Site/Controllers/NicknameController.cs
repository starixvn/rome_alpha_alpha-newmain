namespace Alpha.Main.Site.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using AlphaPlus.User.Models;
    using AppResources;
    using Main.Models;
    using MediatR;
    using Requests.Nickname;
    using ViewModels.Nickname;

    public class NicknameController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public NicknameController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [AllowProtectedMode]
        public ActionResult Index()
        {
            var viewModel = new CreateNicknameViewModel(userSession);

            return viewModel.IsValid() ? View(viewModel) : ToDefaultPage();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public async Task<ActionResult> Index(string nickname)
        {
            var message = new CreateNicknameCommand(userSession)
            {
                Nickname = nickname
            };

            var result = message.IsValid()
                ? await mediator.SendAsync(message)
                : Result.CreateErrorResult(Security.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccessful
                ? ToDefaultPage()
                : View(new CreateNicknameViewModel(userSession, nickname) { Result = result });
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}