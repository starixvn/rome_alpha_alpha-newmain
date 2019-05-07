#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
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
    using Web.Helpers;

    public class DefaultController : BaseController
    {
        private const string OtpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";

        private readonly IUserSession userSession;
        private readonly IMediator mediator;
        private readonly SecurityCodeChecker securityCodeChecker;
        private readonly NicknameChecker nicknameChecker;

        public DefaultController(
            IUserSession userSession,
            IMediator mediator,
            SecurityCodeChecker securityCodeChecker,
            NicknameChecker nicknameChecker)
        {
            this.userSession = userSession;
            this.mediator = mediator;
            this.securityCodeChecker = securityCodeChecker;
            this.nicknameChecker = nicknameChecker;
        }

        public ActionResult Index(string action = null)
        {
            if (IsRedirectToNicknamePage(action))
            {
                return RedirectToAction("Index", "Nickname");
            }

            if (mediator.Send(new CheckDifferenceDeviceCommand(this.userSession)))
            {
                return Redirect(OtpVerifyDifferenceDevicePath);
            }

            var browser = mediator.Send(new BrowserQuery(userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
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

            mediator.Send(new KickoutQuery(userSession));
            ClearSession();

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

        private void ClearSession()
        {
            Session.RemoveAll();
            Session.Abandon();

            if (!Session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }

        private bool IsRedirectToNicknamePage(string action)
        {
            if (nicknameChecker.IsRecommendLoginByNickname) {
                return Request.UrlReferrer?.AbsolutePath != null
                    && Request.UrlReferrer?.AbsolutePath.IndexOf("nickname", StringComparison.OrdinalIgnoreCase) == -1
                    && Request.UrlReferrer?.AbsolutePath.IndexOf("default", StringComparison.OrdinalIgnoreCase) == -1
                    || string.IsNullOrWhiteSpace(action);
            }

            return false;
        }
    }
}