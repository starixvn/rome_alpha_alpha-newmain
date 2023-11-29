#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.SessionState;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Helpers;
    using Alpha.SecurityCode.Services;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Alpha.Web.Helpers;
    using AlphaPlus.User.Models;
    using AppResources;
    using Configurations;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Requests;
    using Requests.Otp;

    [SessionState(SessionStateBehavior.Required)]
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

        [AllowProtectedMode]
        public async Task<ActionResult> Index(string action = null)
        {
            if (IsRedirectToNicknamePage(action))
            {
                return RedirectToAction("Index", "Nickname");
            }

            if (await mediator.SendAsync(new CheckDifferenceDeviceCommand(this.userSession)))
            {
                return Redirect(OtpVerifyDifferenceDevicePath);
            }

            var browser = mediator.Send(new BrowserQuery(userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
            }

            var query = new IndexQuery(userSession, this.securityCodeChecker.IsEnableSecurityCode(), null, nicknameChecker);

            var viewModel = await mediator.SendAsync(query);

            return View(viewModel);
        }

        [AllowProtectedMode]
        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(
                userSession.RoleId,
                Request.UserAgent,
                darkModeName: DarkModeHelper.GetDarkModeName(userSession),
                isHideBanner: AppSettings.BannerIsHide || userSession.IsProtectedMode));

            return PartialView(viewModel);
        }

        [AllowProtectedMode]
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

        [AllowProtectedMode]
        public ActionResult Kickout(string reason)
        {
            var message = MapKickoutReasonToUserMessage(reason);

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
            if (nicknameChecker.IsRecommendLoginByNickname)
            {
                return Request.UrlReferrer?.AbsolutePath != null
                    && Request.UrlReferrer?.AbsolutePath.IndexOf("nickname", StringComparison.OrdinalIgnoreCase) == -1
                    && Request.UrlReferrer?.AbsolutePath.IndexOf("default", StringComparison.OrdinalIgnoreCase) == -1
                    || string.IsNullOrWhiteSpace(action);
            }

            return false;
        }

        private static string MapKickoutReasonToUserMessage(string reason)
        {
            switch (reason)
            {
                case "DuplicationLogin":
                    return Security.stringSessionLogged;

                case "AccountClosed":
                    return Security.lblConfirmClosed;

                case "AccountLocked":
                    return Security.lblConfirmLocked;

                case "KickoutByForceChangePassword":
                    return Security.KickoutMessageByForceChangePassword;

                case "KickoutByProtectedMode":
                    return Security.KickoutMessageByProtectedMode;

                default:
                    return Resources.NoPermissionAccessPage;
            }
        }
    }
}