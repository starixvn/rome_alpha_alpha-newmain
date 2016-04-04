namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using ViewModels;

    public class HomeController : BaseController
    {
        private IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel();

            return View(viewModel);
        }

        public ActionResult MenuBar()
        {
            var message = new MenuBarQuery();

            var viewModel = _mediator.Send(message);

            return PartialView(viewModel);
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult HeaderNav()
        {
            var viewModel = new HeaderNavViewModel();

            return View(viewModel);
        }

        public ActionResult HeaderMsg()
        {
            var query = new HotMessagesQuery
            {
                HotMessageFilePath = HttpContext.Server.MapPath("~/App_Data/UserMessage/ToolTipMessage.xml")
            };

            return View(_mediator.Send(query));
        }
    }
}