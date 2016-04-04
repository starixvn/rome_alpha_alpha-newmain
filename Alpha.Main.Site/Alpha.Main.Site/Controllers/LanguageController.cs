namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using ViewModels;

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
            var viewModel = mediator.Send(new EmptyQuery() as IRequest<LanguageViewModel>);

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