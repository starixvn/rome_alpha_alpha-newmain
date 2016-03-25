namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Requests.Home;
    using Sunplus.Agent.Model.Entities;

    public class HomeController : BaseController
    {
        private IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        public ActionResult HeaderMsg()
        {
            return View();
        }

    }
}