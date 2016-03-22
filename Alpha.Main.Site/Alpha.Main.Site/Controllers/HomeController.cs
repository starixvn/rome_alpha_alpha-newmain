namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;

    public class HomeController : Controller
    {
        private IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Profile()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            return View();
        }
    }
}