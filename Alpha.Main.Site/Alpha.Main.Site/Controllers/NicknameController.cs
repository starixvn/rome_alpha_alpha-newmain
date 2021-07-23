namespace Alpha.Main.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using AppResources;
    using Main.Models;
    using MediatR;
    using Requests.Nickname;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Nickname;
    using Web;

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
            AssignDarkModeViewData(userSession);
            var viewModel = new CreateNicknameViewModel(userSession);

            return viewModel.IsValid() ? View(viewModel) : ToDefaultPage();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public ActionResult Index(string nickname)
        {
            AssignDarkModeViewData(userSession);
            var message = new CreateNicknameCommand(userSession)
            {
                Nickname = nickname
            };

            var result = message.IsValid()
                ? mediator.Send(message)
                : Result.CreateErrorResult(Security.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccessful
                ? ToDefaultPage()
                : View(new CreateNicknameViewModel(userSession, nickname) { Result = result });
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}