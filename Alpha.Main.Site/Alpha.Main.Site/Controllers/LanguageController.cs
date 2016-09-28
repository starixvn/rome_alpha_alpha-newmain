#pragma warning disable 618, 612
namespace Alpha.Main.Site.Controllers
{
    using MediatR;
    using Models.Requests;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;

    public class LanguageController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public LanguageController(IMediator mediator, IUserSession userSession)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException(nameof(userSession));
            }

            this.userSession = userSession;
            this.mediator = mediator;
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(userSession.Language));

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageCommand(userSession, languageCode));

            return Json(viewModel);
        }
    }
}