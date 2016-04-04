namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;

    public class LanguageController : BaseController
    {
        private IMediator mediator;

        public LanguageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery());

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageQuery(languageCode));

            return Json(languageCode);
        }
    }
}