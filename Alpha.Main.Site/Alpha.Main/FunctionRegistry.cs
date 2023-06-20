#pragma warning disable 612, 618

namespace Alpha.Main
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Services.Impl;
    using Configurations;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Products;
    using Helpers;
    using Models;
    using Models.Menus;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;
    using AuthAppSettings = Alpha.Auth.Configurations.AppSettings;

    public static class FunctionRegistry
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["CreateAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount)),
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["BetStatisticsMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),

            ["TransferMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && !TransferToDownlineHistoryMenuItemVisible),
            ["TransferMenuItemWithDownlineHistory"] = () => new MenuStatus(TransferMenuItemVisible && TransferToDownlineHistoryMenuItemVisible),

            ["TransferMenuItemBySite"] = () => new MenuStatus(TransferMenuItemBySiteVisible && !TransferToDownlineHistoryMenuItemVisible),
            ["TransferMenuItemBySiteWithDownlineHistory"] = () => new MenuStatus(TransferMenuItemBySiteVisible && TransferToDownlineHistoryMenuItemVisible),

            ["BetListsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.ViewBetLists)),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsEnableProduct(ProductIdConstants.NumberGame)),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(
                null,
                () =>
                {
                    if (Current.IsInternal || AppSettings.EnableBetsListMonitoring)
                    {
                        return false;
                    }

                    var usersAlwaysToAccessThisFunction = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
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

                    if (AppSettings.NewTBFAllowedSites.Any(site => string.Equals(site, "*") || string.Equals(site, Current.Site, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }

                    return false;
                }),
            ["BetChoiceLimitMenuItem"] = () => new MenuStatus(
                () => Current.IsShowBetChoiceLimit),
            ["ProgressiveContributionMenuItem"] = () => new MenuStatus(() => new User().IsEnabledJackpot),
            ["ProgressiveJackpotMenuItem"] = () => new MenuStatus(() => new User().ShowProgressiveJackpotGames),
            ["IndirectLoginAccountMenuItem"] = () => new MenuStatus(
                () => AppSettings.IsAllowedIndirectLogin(Current.SRecommendId) && !Current.IsIndirectLoggedIn && (Current.IsSuper || Current.IsMaster)),
            ["DormantAccountListMenuItem"] = () => new MenuStatus(() => Current.IsSuper || Current.IsMaster),
            ["ReportsMenuItem"] = () => new MenuStatus(
                () => !AppSettings.IsUsedNewReports(Current.Site, Current.SRecommendId)),
            ["NewReportsMenuItem"] = () => new MenuStatus(
                () => AppSettings.IsUsedNewReports(Current.Site, Current.SRecommendId)),
            ["BonusDetailMenuItem"] = () => new MenuStatus(
                () => AppSettings.AllowedSitesToViewBonusDetail.Contains(Current.Site.ToUpperInvariant()))
        };

        private static bool TransferMenuItemVisible
            => (!Current.IsSubAcc || Current.SubPageAccess.Transfer)
            && !AuthAppSettings.OfferClearGivenCreditSites
            .Any(s => string.Equals(s, Current.Site, StringComparison.OrdinalIgnoreCase) || string.Equals(s, "*", StringComparison.OrdinalIgnoreCase));

        private static bool TransferMenuItemBySiteVisible
            => (!Current.IsSubAcc || Current.SubPageAccess.Transfer)
            && AuthAppSettings.OfferClearGivenCreditSites
            .Any(s => string.Equals(s, Current.Site, StringComparison.OrdinalIgnoreCase) || string.Equals(s, "*", StringComparison.OrdinalIgnoreCase));

        private static bool TransferToDownlineHistoryMenuItemVisible
            => AppSettings.IsAllowedTransferToDownlineHistorySite(Current.Site)
            && new TransferServiceImpl().IsAllDayBalanceTransfer(Current.CustId);

        private static Func<bool> showAccountInfo = () =>
        {
            return Current == null ? false : Current.SubPageAccess.ViewMemberInfo && !Current.IsProtectedMode;
        };

        private static Func<string> getDefaultUrl = () =>
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var defaultMainPage = HttpContext.Current.Response.ApplyAppPathModifier("~/Dashboard/Index");
            var currentUrlCookieName = CookieHelper.GetCookieNameOfCurrentPage(Current.CustId, Current.SubAccId, Encryption.PearsonEncode);

            var currentURLCookie = HttpContext.Current.Request.Cookies[currentUrlCookieName];
            var ignoreUrls = new List<string>
            {
                "Error.aspx",
                "SignIn.aspx",
                "ChangePassword.aspx",
                "SecurityCode",
                "Dashboard",
                "PartialUM"
            };

            if (currentURLCookie != null &&
                !string.IsNullOrWhiteSpace(currentURLCookie.Value) &&
                !Current.IsInternal &&
                !ignoreUrls.Any(str => currentURLCookie.Value.Contains(str)))
            {
                if (!string.IsNullOrWhiteSpace(currentURLCookie.Value)
                && (currentURLCookie.Value.IndexOf("http", StringComparison.OrdinalIgnoreCase) >= 0 || currentURLCookie.Value.IndexOf("https", StringComparison.OrdinalIgnoreCase) >= 0))
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
                        ExceptionHandler.Handle(ex, TraceEventType.Information);
                    }
                }
            }

            return defaultMainPage;
        };

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

        public static Func<bool> IsProtectedModeFunc => () => Current.IsProtectedMode;

        public static Func<bool> IsInternalFunc => () => Current.IsInternal;

        public static Func<string, string> BuildInvalidSessionUniqueKey => (string url) =>
        {
            if (url.IndexOf("/winlossdetail/", StringComparison.OrdinalIgnoreCase) > -1
            && AppSettings.EnableLogDifferentSessionBetweenMainAndReports)
            {
                return $"{Current.CustId}-{Current.SubAccId}";
            }

            return string.Empty;
        };
    }
}