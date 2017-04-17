#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using MediatR;
    using Models.Requests;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Web;

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
            var viewModel = mediator.Send(new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageCommand(userSession, languageCode, AppSettings.DisabledVietnamese));

            return Json(viewModel);
        }
    }
}