namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Razorbill.Security.Sessions;
    using Helpers;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    [SessionState(SessionStateBehavior.Default)]
    public class DefaultController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly ISecurityCodeSession securityCodeSession;
        private readonly IMediator mediator;

        public DefaultController(
            IUserSession userSession,
            ISecurityCodeSession securityCodeSession,
            IMediator mediator)
        {
            this.userSession = userSession;
            this.securityCodeSession = securityCodeSession;
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            var isBrowserIELower10 = BrowserHelper.IsUnsupportedBrowsers();

            if (isBrowserIELower10)
            {
                return RedirectToAction("IndexIELower10");
            }

            var viewModel = mediator.Send(new IndexQuery(userSession, securityCodeSession, UserPermission.IsShowSecurityVerify));

            return View(viewModel);
        }

        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(userSession.RoleId, Request.UserAgent));

            return PartialView(viewModel);
        }

        public ActionResult HeaderHotMsg()
        {
            var query = new HotMessagesQuery
            {
                Language = this.userSession.Language,
                RoleId = this.userSession.RoleId
            };

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        public ActionResult IndexIELower10()
        {
            return View();
        }
    }
}