namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class DefaultController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public DefaultController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            
            var viewModel = mediator.Send(new EmptyQuery() as IRequest<IndexViewModel>);

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(userSession.RoleId));

            return PartialView(viewModel);
        }

        public ActionResult HeaderNav()
        {
            var query = new HeaderNavQuery
            {
                CustId = this.userSession.CustId
            };

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        public ActionResult HeaderMsg()
        {
            var query = new HotMessagesQuery
            {
                Language = this.userSession.Language,
                RoleId = this.userSession.RoleId
            };

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}