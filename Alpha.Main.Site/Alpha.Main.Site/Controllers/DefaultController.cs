namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Requests;
    using SecurityCode.Services;
    using Sessions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class DefaultController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly SecurityCodeSession securityCodeSession;
        private readonly IMediator mediator;
        private readonly SecurityCodeChecker securityCodeChecker;

        public DefaultController(
            IUserSession userSession,
            SecurityCodeSession securityCodeSession,
            IMediator mediator,
            SecurityCodeChecker securityCodeChecker)
        {
            this.userSession = userSession;
            this.securityCodeSession = securityCodeSession;
            this.mediator = mediator;
            this.securityCodeChecker = securityCodeChecker;
        }

        public ActionResult Index()
        {
            var browser = this.mediator.Send(new BrowserQuery(this.userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View("UnsupportedBrowsers", browser);
            }

                if (!this.userSession.IsLogged
                    || this.userSession.IsPasswordExpired)
                {
                    return Redirect("~/Password/ForceChangePassword");
                }

                if (this.securityCodeChecker.IsForcedChangeSecurityCode())
                {
                    return Redirect("~/SecurityCode/CheckLoginSecurityCode");
                }

                var query = new IndexQuery(userSession, securityCodeSession, UserPermission.IsShowSecurityVerify);

                var viewModel = mediator.Send(query);

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

        public ActionResult UnsupportedBrowsers()
        {
            var browser = this.mediator.Send(new BrowserQuery(userSession));

            return View(browser);
        }
    }
}