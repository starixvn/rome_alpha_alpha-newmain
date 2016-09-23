namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Auth.Web.Validators;
    using Fanex.Razorbill.Security.Sessions;
    using Main.Helpers;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

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
            try
            {
                var browser = this.mediator.Send(new BrowserQuery(this.userSession));

                if (browser.IsUnsupportedBrowser)
                {
                    return View("UnsupportedBrowsers", browser);
                }

                var query = new IndexQuery(userSession, securityCodeSession, UserPermission.IsShowSecurityVerify);

                var viewModel = mediator.Send(query);

                return View(viewModel);
            }
            catch(Exception ex)
            {
                ValidationLogger.WriteInfo(ex.ToString());

                return Redirect(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));
            }
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