namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;

    public class MainController : Controller
    {
        private IMediator _mediator;

        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public ActionResult Index(MainIndexQuery query)
        {
            return View(_mediator.Send(query));
        }
    }
}