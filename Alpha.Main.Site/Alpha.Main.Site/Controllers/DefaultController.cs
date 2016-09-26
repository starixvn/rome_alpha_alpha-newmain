namespace Alpha.Main.Site.Controllers
{
    using System.Web.SessionState;
    using Main.Models.SecurityCode;
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
            var browser = this.mediator.Send(new BrowserQuery(this.userSession));

            return RedirectToAction("ChangeSecurityCodeByForcing", "SecurityCode");

            if (browser.IsUnsupportedBrowser)
            {
                return View("UnsupportedBrowsers", browser);
            }

            if (!this.userSession.IsLogged
                || this.userSession.IsPasswordExpired)
            {
                return Redirect("~/Password/ForceChangePassword");
            }

            if (SecurityCodeChecker.IsForcedChangeSecurityCode())
            {
                ISecurityCodeSession securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
                var status = (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;

                switch (status)
                {
                    case SecurityCodeStatus.CHANGE_SECURITYCODE_BY_FORCING:
                        return RedirectToAction("ChangeSecurityCodeByForcing", "SecurityCode");

                    case SecurityCodeStatus.CHANGE_SECURITYCODE_FIRSTLOGIN:
                        return RedirectToAction("ChangeSecurityCodeFirstLogin", "SecurityCode");
                }
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