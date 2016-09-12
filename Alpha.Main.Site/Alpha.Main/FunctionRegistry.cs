namespace Alpha.Main
{
    using Configurations;
    using Helpers;
    using Models.Menus;
    using Models.SecurityCode;
    using Sessions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;
    public static class FunctionRegistry
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),
            ["TransferMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.Transfer),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["RacingMenuItem"] = () => new MenuStatus(IsShowRacing),
            ["CasinoMenuItem"] = () => new MenuStatus(IsShowCasino),
            ["BingoMenuItem"] = () => new MenuStatus(IsShowBingoReport),
            ["BingoPositionTakingMenuItem"] = () => new MenuStatus(IsShowBingo),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsShowNumberGame),
            ["LiveCasinoMenuItem"] = () => new MenuStatus(IsShowLiveCasino),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["MemberWinLimitMenuItem"] = () => new MenuStatus(Current.IsLiveCasinoSync || !Current.IsHideGoldDeluxe),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(
                null,
                () =>
                {
                    if (Current.IsInternal || Configurations.AppSettings.EnableBetsListMonitoring)
                    {
                        return false;
                    }

                    var usersAlwaysToAccessThisFunction = Configurations.AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
                    var disabled = string.IsNullOrWhiteSpace(usersAlwaysToAccessThisFunction) ||
                        !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .Contains(Current.CustId.ToString());

                    return disabled;
                }),
            ["TotalBetsForecast"] = () => new MenuStatus(
                () =>
                {
                    if (Current.IsSubAcc && !Current.SubPageAccess.ViewTotalBets)
                    {
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(Configurations.AppSettings.AllowedSitesToViewTBF)
                        && !Configurations.AppSettings.AllowedSitesToViewTBF.ToLower().Contains(Current.Site.ToLower()))
                    {
                        return false;
                    }

                    return true;
                }),
            ["BetChoiceLimitMenuItem"] = () => new MenuStatus(
                () =>
                {
                    return Current.IsShowBetChoiceLimit;
                })
        };

        private static Func<bool> showMenuBar = () =>
        {
            const int ForceChangeSecurityCode = 4;
            const int ForceChangeSecurityCodeWithoutValidation = 3;

            if (!Current.IsLogged || Current.IsPasswordExpired)
            {
                return false;
            }

            var securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            var newFlowOfSC = SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode;
            if (newFlowOfSC)
            {
                return false;
            }

            var oldStatus = Convert.ToInt32(Current.SecurityCodeResult);
            if (!securityCodeSession.IsInternal &&
                (oldStatus == ForceChangeSecurityCode || oldStatus == ForceChangeSecurityCodeWithoutValidation))
            {
                return false;
            }

            return true;
        };

        private static Func<bool> showAccountInfo = () =>
        {
            return Current.SubPageAccess.ViewMemberInfo;
        };

        private static Func<string> getDefaultUrl = () =>
        {
            var _defaultMainPage = MicroSites.OldMainUrl("~/_Reports/PortalPage/PortalPage.aspx");

            if (!Current.IsLogged)
            {
                return MicroSites.OldMainUrl("~/_MemberInfo/ChangePassword/ChangePassword.aspx?firstLogin=true");
            }

            if (Current.IsPasswordExpired)
            {
                return MicroSites.OldMainUrl("~/_MemberInfo/ChangePassword/ChangePassword.aspx?expired=true");
            }

            ISecurityCodeSession securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;

            var status = (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;


            //return "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/UpdateForceChangeSecurityCodeTheFirstLogin");
            //return "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/ForceChangeSecurityCodeByUpline");
            return "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/VerifySecurityCode");
            //return "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/ResetSecurityCode");
            //return "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/ChangeSecurityCode");

            //switch (status)
            //    {
            //        case SecurityCodeStatus.FORCED_CHANGE_SC:
            //        securityCodeUrl = "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/ForceChangeSecurityCode");
            //        // return MicroSites.OldMainUrl("~/Security/SecurityCode/ForceChangeSecurityCode");
            //        break;

            //        case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
            //        securityCodeUrl = "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/ForceChangeSecurityCodeWithoutValidation");
            //        //return MicroSites.OldMainUrl("~/Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation");
            //        break;

            //    default:
            //        securityCodeUrl = "/" + AppPathHelper.GetAppPath(Alpha.Main.Configurations.AppSettings.VirtualDirectory, "SecurityCodeNewMain/VerifySecurityCode");
            //        return securityCodeUrl;
            //}




            var currentURLCookie = HttpContext.Current.Request.Cookies[CookieHelper.GetCurrentUrlCookieName()];
            var ignoreUrls = new List<string>
            {
                "Error.aspx",
                "SignIn.aspx",
                "ChangePassword.aspx",
                "SecurityCode"
            };

            if (currentURLCookie != null &&
                !string.IsNullOrEmpty(currentURLCookie.Value) &&
                !Current.IsInternal &&
                !ignoreUrls.Where(str => currentURLCookie.Value.Contains(str)).Any())
            {
                if (!string.IsNullOrWhiteSpace(currentURLCookie.Value)
                && (currentURLCookie.Value.ToLower().Contains("http") || currentURLCookie.Value.ToLower().Contains("https")))
                {
                    try
                    {
                        Uri currentUrl = new Uri(HttpUtility.UrlDecode(currentURLCookie.Value));

                        if (HttpContext.Current.Request.Url.Host == currentUrl.Host)
                        {
                            return currentUrl.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Agent.Utilities.Logs.WriteInfo(ex);
                    }
                }
            }

            return _defaultMainPage;
        };

        public static Func<bool> ShowMenuBar
        {
            get
            {
                return showMenuBar;
            }
        }

        public static Func<bool> ShowIntroduction
        {
            get
            {
                return showMenuBar;
            }
        }

        public static Func<bool> ShowAccountInfo
        {
            get
            {
                return showAccountInfo;
            }
        }

        public static Func<string> GetDefaultUrl
        {
            get
            {
                return getDefaultUrl;
            }
        }
    }
}