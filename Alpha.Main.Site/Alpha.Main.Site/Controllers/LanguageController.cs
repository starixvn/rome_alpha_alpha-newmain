namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

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