namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Utils;

    public class MainController : BaseController
    {
        private IMediator _mediator;

        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index(MainIndexQuery query)
        {
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }

        public JsonResult ChangeLanguage(ChangeLanguageQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}