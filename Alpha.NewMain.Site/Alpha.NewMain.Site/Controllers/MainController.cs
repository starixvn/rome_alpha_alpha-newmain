namespace Alpha.NewMain.Site.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Extensions;
    using MediatR;
    using Models.Requests;
    using Utils;
    public class MainController : Controller
    {
        private IMediator _mediator;

        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index(MainIndexQuery query)
        {
            LanguageUtil.SetLanguage();
            return View(_mediator.Send(query));
        }

        public JsonResult ChangeLanguage(ChangeLanguageQuery query)
        {
            return Json(_mediator.Send(query));
        }
    }
}