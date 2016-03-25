namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using App_Start;
    using MediatR;
    using Models.Requests;

    public class MainController : BaseController
    {
        private IMediator _mediator;

        public MainController(IMediator mediator)
        {            
            _mediator = mediator;
        }

        public ActionResult Index(MainIndexQuery query)
        {
            return RedirectToAction("Index", "Home");
        }

        public JsonResult ChangeLanguage(ChangeLanguageQuery query)
        {
            var viewModel = _mediator.Send(query);
            return Json(viewModel);
        }
    }
}