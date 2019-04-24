namespace Alpha.Main.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
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

        public ActionResult Index()
        {
            var viewModel = new CreateNicknameViewModel(userSession);

            return viewModel.IsValid() ? View(viewModel) : ToDefaultPage();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string nickname)
        {
            var message = new CreateNicknameCommand(userSession);

            var result = message.IsValid()
                ? mediator.Send(message)
                : Result.CreateErrorResult(Security.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccessful
                ? ToDefaultPage()
                : View(new CreateNicknameViewModel(userSession) { Result = result });
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}