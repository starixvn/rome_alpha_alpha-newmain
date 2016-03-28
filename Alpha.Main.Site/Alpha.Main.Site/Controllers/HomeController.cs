namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
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

            return View(viewModel);
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
            return View();
        }
    }
}