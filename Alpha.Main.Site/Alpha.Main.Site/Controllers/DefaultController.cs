#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Helpers;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Filters;
    using AppResources;
    using Configurations;
    using Constants;
    using Fanex.Cryptography.Aes;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Newtonsoft.Json;
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
        private const string fpsScriptKey = "fpsScript";
        private const string SecretKey = "!@#$";
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

        [HttpGet, AllowProtectedMode]
        public string Auth()
        {
            var param = AesService.Default.Encrypt($"{userSession.UserName}&{GetSessionKeyValue()}&{GetOutputValuesAfterLogin()}&{DateTime.Now}", SecretKey);
            var loginUsername = AesService.Default.Encrypt($"{userSession.UserName}", SecretKey);
            var sessionKeyValue = AesService.Default.Encrypt($"{GetSessionKeyValue()}", SecretKey);
            var outputValue= AesService.Default.Encrypt($"{GetOutputValuesAfterLogin()}", SecretKey);
            Response.Cookies.Add(new HttpCookie("auth_1", loginUsername));
            Response.Cookies.Add(new HttpCookie("auth_2", sessionKeyValue));
            Response.Cookies.Add(new HttpCookie("auth_3", outputValue));

            return $"{AppSettings.ReportDomain}/Auth?param={param}";
        }

        [AllowProtectedMode]
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

            var query = new IndexQuery(userSession, this.securityCodeChecker.IsEnableSecurityCode(), null, nicknameChecker);

            var viewModel = mediator.Send(query);

            // FPS Scritp ==================

            var fpsScript = this.userSession.GetProperty<string>(fpsScriptKey);

            if (!string.IsNullOrWhiteSpace(fpsScript))
            {
                TempData[fpsScriptKey] = fpsScript;

                this.userSession.SetProperty(fpsScriptKey, string.Empty);
            }

            //================

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

        private string GetSessionKeyValue()
        {
            var sessionKeyValue = string.Empty;
            var enableJackpotGameIds = userSession.GetProperty<IList<int>>(SessionConstants.GameEnable + SessionConstants.Jackpot);
            var enableJackpotGames = $"{string.Join(",", enableJackpotGameIds ?? Enumerable.Empty<int>())}";
            var pageAccess = userSession.SubPageAccess;
            var pageAccessPermissions = $"{pageAccess.CreateAccount}${pageAccess.Transfer}${pageAccess.UpdateAccount}$" +
                $"{pageAccess.ViewLog}${pageAccess.ViewMemberInfo}${pageAccess.ViewReports}${pageAccess.ViewTotalBets}${pageAccess.GetPermission("[L]")}";

            var sessions = new List<object>
            {
                userSession.CustId,
                userSession.UserName,
                userSession.Site,
                userSession.Language,
                userSession.LangKey,
                userSession.RoleId,
                userSession.Currency,
                userSession.CurrencyId,
                userSession.IsShowConsolidatedReport,
                userSession.GetProperty<DateTime>(SessionConstants.CustomerDatelimit),
                userSession.IsAdminCanViewVNIP,
                userSession.SubAccId,
                userSession.UserLoginIp,
                userSession.IsProtectedMode,
                userSession.DarkModeName,
                userSession.GetProperty<string>(SessionConstants.MainDomainSessionKey),
                userSession.GetProperty<string>(SessionConstants.LoginIP_Browser),
                userSession.AssetAppPath,
                enableJackpotGames,
                true,
                pageAccessPermissions,
                userSession.IsEnabledSecurityCodeInStatement,
                userSession.SecurityCode,
                SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp,
                SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode,
                AppSettings.ReportRouting,
                userSession.AdminId,
                userSession.AdminName,
                userSession.IndirectCustId,
                userSession.IndirectSubAccId,
                userSession.IndirectUsername,
                userSession.IsPasswordExpired,
                userSession.NickName
            };

            foreach (var item in sessions)
            {
                sessionKeyValue += $"{item};";
            }

            return sessionKeyValue.Substring(0, sessionKeyValue.Length - 1);
        }

        private string GetOutputValuesAfterLogin()
        {
            return JsonConvert.SerializeObject(userSession.GetProperty<Hashtable>(SessionConstants.Outputs));
        }
    }
}