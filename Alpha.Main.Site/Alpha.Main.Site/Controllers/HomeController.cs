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
        
        public ActionResult Navigation(NavigationQuery query)
        {
            query.SubPageAccess = UserSession.Current.SubPageAccess;
            query.IsSubAcc = UserSession.Current.IsSubAcc;
            query.RoleId = UserSession.Current.RoleId;

            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }
    }
}