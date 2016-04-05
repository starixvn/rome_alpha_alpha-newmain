namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
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
        public async Task<ActionResult> MenuBar()
        {
            var viewModel = await mediator.SendAsync(new MenuBarQuery(userSession.RoleId));

            return PartialView(viewModel);
        }

        public ActionResult HeaderNav()
        {
            var viewModel = new HeaderNavViewModel(userSession.Language);

            return View(viewModel);
        }

        public ActionResult HeaderMsg()
        {
            var query = new HotMessagesQuery
            {
                HotMessageFilePath = HttpContext.Server.MapPath("~/App_Data/UserMessage/ToolTipMessage.xml")
            };

            return View(mediator.Send(query));
        }
    }
}