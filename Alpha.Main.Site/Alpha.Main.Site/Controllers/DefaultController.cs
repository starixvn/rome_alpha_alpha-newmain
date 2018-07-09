#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using AppResources;
    using Configurations;
    using Constants;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Requests;
    using Requests.Otp;
    using SecurityCode.Services;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class DefaultController : BaseController
    {
        private const string ForceChangePasswordPath = "~/Password/ForceChangePassword";
        private const string SecurityCodePath = "~/SecurityCode/Index";
        private const string OtpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";

        private readonly IUserSession userSession;
        private readonly IMediator mediator;
        private readonly SecurityCodeChecker securityCodeChecker;

        public DefaultController(
            IUserSession userSession,
            IMediator mediator,
            SecurityCodeChecker securityCodeChecker)
        {
            this.userSession = userSession;
            this.mediator = mediator;
            this.securityCodeChecker = securityCodeChecker;
        }

        public ActionResult Index()
        {
            var browser = mediator.Send(new BrowserQuery(userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
            }

            if (!userSession.IsLogged
                || userSession.IsPasswordExpired)
            {
                return Redirect(ForceChangePasswordPath);
            }

            if (mediator.Send(new CheckDifferenceDeviceCommand(this.userSession)))
            {
                return Redirect(OtpVerifyDifferenceDevicePath);
            }

            if (securityCodeChecker.IsForcedChangeSecurityCode())
            {
                return Redirect(SecurityCodePath);
            }

            var query = new IndexQuery(userSession, this.securityCodeChecker.IsEnableSecurityCode(), null);

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
                Language = userSession.Language,
                RoleId = userSession.RoleId
            };

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        public ActionResult Kickout(string reason)
        {
            string message = Resources.NoPermissionAccessPage;

            if (!string.IsNullOrEmpty(reason))
            {
                if (reason == "DuplicationLogin")
                {
                    message = Security.stringSessionLogged;
                }
                else if (reason == "AccountClosed")
                {
                    message = Security.lblConfirmClosed;
                }
            }

            return View(message as object);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSession.Current.Language;

            if (AppSettings.DisabledVietnamese && cultureName == Language.Vietnamese)
            {
                cultureName = English;
                UserSession.Current.Language = cultureName;
            }

            base.Initialize(requestContext);
        }
    }
}